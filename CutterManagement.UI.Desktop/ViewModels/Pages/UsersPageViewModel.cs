using CutterManagement.Core;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    public class UsersPageViewModel : ViewModelBase, ISubscribeToMessages
    {
        #region Private Fields

        /// <summary>
        /// Access to database
        /// </summary>
        private IDataAccessServiceFactory _dataServiceFactory;

        /// <summary>
        /// A collection of users
        /// </summary>
        private ObservableCollection<UserItemViewModel> _users;

        /// <summary>
        /// Collection of users shift
        /// </summary>
        private Dictionary<UserShift, string> _userShiftCollection;


        /// <summary>
        /// Selected user current shift
        /// </summary>
        public UserShift _selectedUserShift;

        #endregion

        #region Public Properties

        /// <summary>
        /// True of user's list is empty
        /// otherwise, false
        /// </summary>
        public bool IsUserCollectionEmpty => _users.Any();

        /// <summary>
        /// True if still loading users
        /// </summary>
        public bool IsLoading { get; set; }

        /// <summary>
        /// A collection of users
        /// </summary>
        public ObservableCollection<UserItemViewModel> Users
        {
            get => _users;
            set => _users = value;
        }

        /// <summary>
        /// Collection of users shift
        /// </summary>
        public Dictionary<UserShift, string> UserShiftCollection 
        {
            get => _userShiftCollection; 
            set => _userShiftCollection = value; 
        }

        /// <summary>
        /// Selected user current shift
        /// </summary>
        public UserShift SelectedUserShift 
        { 
            get => _selectedUserShift;
            set => _selectedUserShift = value;
        }

        #endregion

        #region Public Commands

        /// <summary>
        /// Command to show log in form
        /// </summary>
        public ICommand OpenAdminLoginDialogCommand { get; set; }

        /// <summary>
        /// Command to open user creation form
        /// </summary>
        public ICommand AddUserCommand { get; set; }

        /// <summary>
        /// Command to save new user shift
        /// </summary>
        public ICommand ChangeUserShiftCommand { get; set; }

        /// <summary>
        /// Command to deactivate user
        /// </summary>
        public ICommand DeactivateUserCommand { get; set; }

        /// <summary>
        /// Command to open user manager dialog
        /// </summary>
        public ICommand OpenUserManagerDialogCommand { get; set; }

        #endregion

        #region Construction 

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataServiceFactory">Access to database</param>
        public UsersPageViewModel(IDataAccessServiceFactory dataServiceFactory)
        {
            _dataServiceFactory = dataServiceFactory;
            _users = new ObservableCollection<UserItemViewModel>();
           
            _ = LoadUsers();

            // Create commands
            AddUserCommand = new RelayCommand(OpenCreateUserDialog);
            OpenAdminLoginDialogCommand = new RelayCommand(OpenAdminLoginDialog);
            OpenUserManagerDialogCommand = new RelayCommand(async () => await OpenUserManagerDialog());
            ChangeUserShiftCommand = new RelayCommand(async () => await ChangeUserShift());
            DeactivateUserCommand = new RelayCommand(async (userId) => await DeactivateUser(Convert.ToInt32(userId)));

            // Register this object to receive messages from messenger
            Messenger.MessageSender.RegisterMessenger(this);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Opens admin login dialog
        /// </summary>
        private void OpenAdminLoginDialog()
        {
            AdminLoginDialogViewModel adminLoginVM = new AdminLoginDialogViewModel();
            DialogService.InvokeDialog(adminLoginVM);
        }

        /// <summary>
        /// Opens user creation dialog
        /// </summary>
        private void OpenCreateUserDialog()
        {
            CreateUserDialogViewModel createUser = new CreateUserDialogViewModel(_dataServiceFactory);
            DialogService.InvokeDialog(createUser);
        }

        /// <summary>
        /// Changes user shift
        /// </summary>
        private async Task ChangeUserShift()
        {
            IDataAccessService<UserDataModel> usersTable = _dataServiceFactory.GetDbTable<UserDataModel>();

            UserDataModel? user = await usersTable.GetEntityByIdAsync(_users.First(x => x.IsEditMode == true).Id);

            if (user is not null && user.Shift.Equals(SelectedUserShift) is false)
            {
                user.Shift = _selectedUserShift;

                await usersTable.UpdateEntityAsync(user);

                UpdateUsersCollection(user);
            }
        }

        /// <summary>
        /// Deactivates a user 
        /// </summary>
        /// <param name="userId">Unique id of the user to deactivate</param>
        private async Task DeactivateUser(int userId)
        {
            IDataAccessService<UserDataModel> usersTable = _dataServiceFactory.GetDbTable<UserDataModel>();

            UserDataModel? user = await usersTable.GetEntityByIdAsync(userId);

            if (user is null) return;
            {
                user.IsActive = false;

                await usersTable.UpdateEntityAsync(user);

                await ReloadUserCollection();
            }
        }

        private async Task OpenUserManagerDialog()
        {
            UserManagerDialogViewModel userManager = new UserManagerDialogViewModel(_dataServiceFactory);

            await userManager.GetDeactivatedUsers();

            DialogService.InvokeDialog(userManager);

            await ReloadUserCollection();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads users from db
        /// </summary>
        private async Task LoadUsers()
        {
            IsLoading = true;

            // Clear users
            _users.Clear();

            // Get users table
            IDataAccessService<UserDataModel> userTable = _dataServiceFactory.GetDbTable<UserDataModel>();

            _userShiftCollection = new Dictionary<UserShift, string>();

            foreach (UserShift shift in Enum.GetValues<UserShift>())
            {
                if (shift is UserShift.None) continue;

                _userShiftCollection.Add(shift, EnumHelpers.GetDescription(shift));
            }

            foreach (UserDataModel user in await userTable.GetAllEntitiesAsync())
            {
                // If user is admin user 
                if ((user.FirstName is "resource" && user.LastName is "admin") || user.IsActive is false || user.IsArchived)
                    // Do not add it
                    continue;

                // Add users
                AddUserToUserCollection(user);
            }

            IsLoading = false;

            OnPropertyChanged(nameof(IsUserCollectionEmpty));
            OnPropertyChanged(nameof(UserShiftCollection));
        }

        /// <summary>
        /// Reloads user's list
        /// </summary>
        public async Task ReloadUserCollection() => await LoadUsers();

        /// <summary>
        /// Adds a user into <see cref="ObservableCollection{T}"/>
        /// <para> T is <see cref="UserItemViewModel"/></para>
        /// </summary>
        /// <param name="user">The user to add</param>
        private void AddUserToUserCollection(UserDataModel user)
        {
            var userItem = new UserItemViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Shift = user.Shift,
                UserShift = EnumHelpers.GetDescription(user.Shift),
                UserFullName = string.Join("  ", user.FirstName, user.LastName),
                UserInitials = string.Join("", user.FirstName[0], user.LastName[0]),
            };

            userItem.UserItemSelected += (sender, e) => 
            {
                _users.ToList().ForEach(x => x.IsEditMode = false);

                if(sender is UserItemViewModel user && user is not null)
                {
                    _selectedUserShift = user.Shift;

                    OnPropertyChanged(nameof(SelectedUserShift));
                }
            };

            _users.Add(userItem);

            CollectionViewSource.GetDefaultView(Users).SortDescriptions.Add(new SortDescription(nameof(UserItemViewModel.Shift), ListSortDirection.Ascending));
        }

        /// <summary>
        /// Get user's current shift
        /// </summary>
        /// <param name="shift">user shift to get</param>
        //public UserShift GetCurrentShift(string shift)
        //{
        //    UserShift shiftKey = UserShift.None;

        //    foreach (var item in UserShiftCollection) 
        //    {
        //        if (item.Value == shift)
        //            shiftKey = item.Key;
        //    }

        //    return shiftKey;
        //}

        /// <summary>
        /// Update users list with the latest information from database
        /// </summary>
        /// <param name="data">The data to update users list with</param>
        public void UpdateUsersCollection(UserDataModel data)
        {
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

        #region Messages

        /// <summary>
        /// Receive message from <see cref="Messenger"/>
        /// </summary>
        /// <param name="message">The message received</param>
        public void ReceiveMessage(IMessage message)
        {
            if (message.GetType() == typeof(UserDataModel))
            {
                UpdateUsersCollection((UserDataModel)message);
            }
        }

        #endregion
    }
}
