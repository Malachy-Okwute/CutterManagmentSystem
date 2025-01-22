using CutterManagement.Core;
using System.Collections.ObjectModel;
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

        #endregion

        #region Public Properties

        /// <summary>
        /// True of user's list is empty
        /// otherwise, false
        /// </summary>
        public bool IsUserCollectionEmpty => _users.Any();

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
        /// Command to show log in form
        /// </summary>
        public ICommand OpenAdminLoginDialogCommand { get; set; }

        /// <summary>
        /// Command to open user creation form
        /// </summary>
        public ICommand AddUserCommand { get; set; }

        /// <summary>
        /// Command to manage user
        /// </summary>
        public ICommand ManageUserCommand { get; set; }

        #endregion

        #region Construction 

        public UsersPageViewModel(IDataAccessServiceFactory dataServiceFactory)
        {
            _dataServiceFactory = dataServiceFactory;
            _users = new ObservableCollection<UserItemViewModel>();
           
            LoadUsers();
            
            // Create commands
            AddUserCommand = new RelayCommand(OpenCreateUserDialog);
            ManageUserCommand = new RelayCommand(ManageUser);
            OpenAdminLoginDialogCommand = new RelayCommand(OpenAdminLoginDialog);

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

        private void ManageUser()
        {

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

                CollectionViewSource.GetDefaultView(Users).Refresh();

            }).ContinueWith((action) => OnPropertyChanged(nameof(IsUserCollectionEmpty)));
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
