using CutterManagement.Core;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    public class UsersPageViewModel : ViewModelBase
    {
        #region Private Fields

        /// <summary>
        /// Access to database
        /// </summary>
        private IDataAccessServiceFactory _dataServiceFactory;

        /// <summary>
        /// Tells whether admin user is currently logged in or not
        /// </summary>
        private string _sessionStatus;

        /// <summary>
        /// A collection of users
        /// </summary>
        private ObservableCollection<UserItemViewModel> _users;

        /// <summary>
        /// Shift assigned to current new user
        /// </summary>
        private UserShift _newUserShift;

        #endregion

        #region Public Properties

        /// <summary>
        /// User's first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// User's last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Available shift options
        /// </summary>
        public Dictionary<UserShift, string> UserShiftCollection { get; set; }

        /// <summary>
        /// Shift assigned to current new user
        /// </summary>
        public UserShift NewUserShift
        {
            get => _newUserShift;
            set => _newUserShift = value; 
        }

        /// <summary>
        /// The user name admin uses to login 
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// True if log in form is to be shown 
        /// Otherwise false
        /// </summary>
        public bool IsLoginFormVisible { get; set; }

        /// <summary>
        /// True of user's list is empty
        /// otherwise, false
        /// </summary>
        public bool IsUserCollectionEmpty => _users.Any();

        /// <summary>
        /// Shows a message to client user in regards to creating a user
        /// </summary>
        public bool ShowMessage { get; set; }

        /// <summary>
        /// True if error message is to be shown 
        /// Otherwise false
        /// </summary>
        public bool ShowErrorMessage { get; set; }
        
        /// <summary>
        /// True if success message is to be shown 
        /// Otherwise false
        /// </summary>
        public bool ShowSuccessMessage { get; set; }

        /// <summary>
        /// Message to display to client user if user creation is successful or not
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// True if add user form is to be shown 
        /// Otherwise false
        /// </summary>
        public bool ShowUserCreationForm { get; set; }

        /// <summary>
        /// Tells whether admin user is currently logged in or not
        /// </summary>
        public string SessionStatus 
        {
            get => _sessionStatus;
            set => _sessionStatus = value;
        }
        
        /// <summary>
        /// A collection of users
        /// </summary>
        public ObservableCollection<UserItemViewModel> Users
        {
            get => _users;
            set => _users = value;
        }

        #endregion

        #region Public Commands

        /// <summary>
        /// Command to log in user
        /// </summary>
        public ICommand LoginCommand { get; set; }

        /// <summary>
        /// Command to cancel log in procedure
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// Command to show log in form
        /// </summary>
        public ICommand ShowLoginFormCommand { get; set; }

        /// <summary>
        /// Command to open user creation form
        /// </summary>
        public ICommand AddUserCommand { get; set; }

        /// <summary>
        /// Command to manage user
        /// </summary>
        public ICommand ManageUserCommand { get; set; }

        /// <summary>
        /// Command to create new user
        /// </summary>
        public ICommand CreateCommand { get; set; }

        /// <summary>
        /// Command to cancel the process of creating a new user
        /// </summary>
        public ICommand CancelUserCreationCommand { get; set; }

        #endregion

        #region Construction 

        public UsersPageViewModel(IDataAccessServiceFactory dataServiceFactory)
        {
            _dataServiceFactory = dataServiceFactory;
            _users = new ObservableCollection<UserItemViewModel>();
            UserShiftCollection = new Dictionary<UserShift, string>();
            _newUserShift = UserShift.None;
            LoadUsers();
            GetCurrentLoginSessionStatus();

            // Event hook up
            //_dataServiceFactory.DataChanged += UpdateUsersCollection;

            foreach (UserShift shift in Enum.GetValues<UserShift>())
            {
                UserShiftCollection.Add(shift, EnumHelpers.GetDescription(shift));
            }

            // Create commands
            AddUserCommand = new RelayCommand(() => ShowUserCreationForm = true);
            ManageUserCommand = new RelayCommand(ManageUser);
            CreateCommand = new RelayCommand(async () => await CreateUser());
            CancelUserCreationCommand = new RelayCommand(CancelUserCreation);
            ShowLoginFormCommand = new RelayCommand(() => IsLoginFormVisible = true);
            LoginCommand = new RelayCommand(async(parameter) => await Login(parameter));
            CancelCommand = new RelayCommand(() =>
            {
                // Reset messages
                ResetLoginProcedure();
                IsLoginFormVisible = false;
            });
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Cancels user creation procedure
        /// </summary>
        private void CancelUserCreation()
        {
            ShowUserCreationForm = false;

            ClearUserCreationDataResidue();
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        private async Task CreateUser()
        {
            // Create a user model
            UserDataModel newUser = new UserDataModel
            {
                FirstName = NormalizeName(FirstName),
                LastName = NormalizeName(LastName),
                Shift = NewUserShift,
            };

            // Register user validation
            DataValidationService.RegisterValidator(new UserValidation());

            // Validate incoming data
            ValidationResult result = DataValidationService.Validate(newUser);

            // If validation passes
            if (result.IsValid)
            {
                // Get users table
                IDataAccessService<UserDataModel> userTable = _dataServiceFactory.GetDbTable<UserDataModel>();
                // Listen for when user is created
                userTable.DataChanged += UserTable_DataChanged;
                // commit the newly created user to the users table
                await userTable.CreateNewEntityAsync(newUser);
                // Set flag
                ShowSuccessMessage = true;
                // Unhook event
                userTable.DataChanged -= UserTable_DataChanged;
            }

            // Show message
            ShowMessage = true;
            // Set message
            Message = result.IsValid ? "User created successfully" : result.ErrorMessage;

            // Briefly show message
            await Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((action) =>
            {
                if (result.IsValid)         // TODO: make sure db transaction is successful
                {
                    ShowMessage = false;
                    ShowSuccessMessage = false;
                    ShowUserCreationForm = false;
                    ClearUserCreationDataResidue();
                }
                else
                {
                    Message = result.ErrorMessage;
                    ShowSuccessMessage = false;
                    ShowMessage = false;
                }
            });
        }

        private void ManageUser()
        {

        }

        /// <summary>
        /// Simulate logging-in
        /// </summary>
        /// <param name="parameter">Command parameter</param>
        private async Task Login(object parameter)
        {
            // NOTE: NEVER DECRYPT PASSWORD VALUE OR STORE PASSWORD VALUE IN A VARIABLE
            //       IN A TRUE IDENTITY USER ENVIRONMENT (PRODUCTION).
            //       ALWAYS KEEP PASSWORD SECURE AND ENCRYPTED TO AND FROM USER TO SERVER
            //
            // REMARK - We are just simulating login functionality in this instance as 
            //          this is not the proper way to handle user login authentication and authorization requests.

            // Get the entered password
            string securePassword = ((ISecurePassword)parameter).Password.Decrypt();

            ((ISecurePassword)parameter).Dispose();

            // Create authentication service
            AuthenticationService loginCredential = new AuthenticationService();
            // Authenticate credential passed in by user
            AuthenticationResult authenticationResult = loginCredential.Authenticate(Username, securePassword);

            // If authentication failed
            if (authenticationResult.Success is false)
            {
                // Display error message to user
                ShowErrorMessage = true;

                // Display success message
                ShowSuccessMessage = false;
            }
            // Otherwise...
            else
            {
                // Keep error message hidden
                ShowErrorMessage = false;

                // Display success message
                ShowSuccessMessage = true;

                // Delay for 2 seconds...
                await Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((action) => 
                {

                    // Reset messages
                    ResetLoginProcedure();

                    // Then close log-in form
                    IsLoginFormVisible = false;
                });
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads users from db
        /// </summary>
        private void LoadUsers()
        {
            // Get users table
            IDataAccessService<UserDataModel> userTable = _dataServiceFactory.GetDbTable<UserDataModel>();

            Task.Run(async () =>
            {
                foreach (UserDataModel user in await userTable.GetAllEntitiesAsync())
                {
                    // If user is admin user 
                    if (user.FirstName is "resource" && user.LastName is "admin")
                        // Do not add it
                        continue;

                    // Add users
                    AddUserToUserCollection(user);
                }
            });
        }

        /// <summary>
        /// Adds a user into <see cref="ObservableCollection{T}"/>
        /// <para> T is <see cref="UserItemViewModel"/></para>
        /// </summary>
        /// <param name="user">The user to add</param>
        private void AddUserToUserCollection(UserDataModel user)
        {
            _users.Add(new UserItemViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserShift = user.Shift,
                UserFullName = string.Join("  ", user.FirstName, user.LastName),
                UserInitials = string.Join("", user.FirstName[0], user.LastName[0]),
            });
        }

        /// <summary>
        /// Resets log in procedure messages
        /// </summary>
        private void ResetLoginProcedure()
        {
            ShowErrorMessage = false;
            ShowSuccessMessage = false;
            Username = string.Empty;

            GetCurrentLoginSessionStatus();
        }

        /// <summary>
        /// Gets information about the current admin login status
        /// </summary>
        private void GetCurrentLoginSessionStatus()
        {
            _sessionStatus = AuthenticationService.IsAdminUserAuthorized ?
                                "Session status: Currently logged in" :
                                       "Session status: Not logged in";
            OnPropertyChanged(nameof(SessionStatus));

            // TODO: Broadcast and listen to live event of when login session starts or ended
            //       So we can get live update and update UI with the latest information
        }

        /// <summary>
        /// Resets properties used in users creation to their default values
        /// </summary>
        private void ClearUserCreationDataResidue()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            NewUserShift = UserShift.None;
        }

        /// <summary>
        /// Normalize names entered 
        /// <para>Capitalizes the first letter in first and last name opf user</para>
        /// </summary>
        /// <param name="name">The name to normalize</param>
        /// <returns><see cref="string"/></returns>
        private string NormalizeName(string name)
        {
            if (name == null)
            {
                return string.Empty;
            }
            return char.ToUpper(name[0]) + name.Substring(1);
        }

        #endregion

        #region Event Methods

        /// <summary>
        /// Update users list with the most current users that exists in the database
        /// </summary>
        /// <param name="sender">Origin of this event</param>
        /// <param name="e">The actual data that changed</param>
        private void UserTable_DataChanged(object? sender, object e)
        {
            // Make sure incoming changes is user data
            if (e is not UserDataModel data) return;

            // store data as user data model
            //UserDataModel data = (UserDataModel)user;

            // Check if user exist in local users list
            UserItemViewModel? existingLocalData = _users.FirstOrDefault(x => x.Id == data.Id);

            // Get users db table
            IDataAccessService<UserDataModel> usersTable = _dataServiceFactory.GetDbTable<UserDataModel>();

            // Jump onto UI thread
            DispatcherService.Invoke(async () =>
            {
                // Check if the user is on database
                UserDataModel? existingDbData = await usersTable.GetEntityByIdAsync(data.Id);

                // If user exists locally
                if (existingLocalData is not null)
                {
                    // Get its position on the list
                    int index = _users.IndexOf(existingLocalData);
                    // Remove the user
                    _users.RemoveAt(index);
                    // Add the most current data of the user from db
                    AddUserToUserCollection(data);
                }
                // Otherwise, if user is in db but not locally
                else if (existingDbData is not null && existingLocalData is null)
                {
                    // Then it must be a new user
                    // Add it to the collection 
                    AddUserToUserCollection(data);
                }
                else
                {
                    // Remove user from local collection
                    _users.RemoveAt(data.Id);
                }
            });

            OnPropertyChanged(nameof(IsUserCollectionEmpty));
        }

        #endregion
    }
}
