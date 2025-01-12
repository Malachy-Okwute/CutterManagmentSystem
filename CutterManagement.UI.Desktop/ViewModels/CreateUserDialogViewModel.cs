using CutterManagement.Core;
using CutterManagement.Core.Services;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="CreateUserDialog"/>
    /// </summary>
    public class CreateUserDialogViewModel : DialogViewModelBase, IDialogWindowCloseRequest
    {
        #region Private Fields

        /// <summary>
        /// Access to database
        /// </summary>
        private IDataAccessServiceFactory _dataServiceFactory;

        /// <summary>
        /// Shift assigned to current new user
        /// </summary>
        private UserShift _newUserShift;

        #endregion

        #region Events

        /// <summary>
        /// Event to close dialog window
        /// </summary>
        public event EventHandler<DialogWindowCloseRequestedEventArgs> DialogWindowCloseRequest;

        #endregion

        #region Properties

        /// <summary>
        /// User's first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// User's last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Shift assigned to current new user
        /// </summary>
        public UserShift NewUserShift
        {
            get => _newUserShift;
            set => _newUserShift = value;
        }

        /// <summary>
        /// Available shift options
        /// </summary>
        public Dictionary<UserShift, string> UserShiftCollection { get; set; }

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

        #endregion

        #region Commands

        /// <summary>
        /// Command to create new user
        /// </summary>
        public ICommand CreateCommand { get; set; }

        /// <summary>
        /// Command to cancel the process of creating a new user
        /// </summary>
        public ICommand CancelUserCreationCommand { get; set; }

        #endregion

        #region Constructor

        public CreateUserDialogViewModel(IDataAccessServiceFactory dataServiceFactory)
        {
            _dataServiceFactory = dataServiceFactory;
            UserShiftCollection = new Dictionary<UserShift, string>();
            _newUserShift = UserShift.None;

            foreach (UserShift shift in Enum.GetValues<UserShift>())
            {
                UserShiftCollection.Add(shift, EnumHelpers.GetDescription(shift));
            }

            CreateCommand = new RelayCommand(async () => await CreateUser());
            CancelUserCreationCommand = new RelayCommand(CancelUserCreation);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Cancels user creation procedure
        /// </summary>
        private void CancelUserCreation()
        {
            ClearUserCreationDataResidue();
            DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(false));
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
                    ClearUserCreationDataResidue();
                    DispatcherService.Invoke(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(result.IsValid)));
                }
                else
                {
                    Message = result.ErrorMessage;
                    ShowSuccessMessage = false;
                    ShowMessage = false;
                }
            });
        }

        #endregion

        #region Methods

        /// <summary>
        /// Resets properties used in users creation to their default values
        /// </summary>
        private void ClearUserCreationDataResidue()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            NewUserShift = UserShift.None;
            ShowMessage = false;
            ShowSuccessMessage = false;
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
        private void UserTable_DataChanged(object? sender, object e) => Messenger.MessageSender.SendMessage((UserDataModel)e);
        

        #endregion

    }
}
