using CutterManagement.Core;
using CutterManagement.Core.Services;
using System.Windows.Data;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="NewInfoUpdateDialog"/>
    /// </summary>
    public class NewInfoUpdateDialogViewModel : DialogViewModelBase, IDialogWindowCloseRequest
    {
        /// <summary>
        /// Data access
        /// </summary>
        private IDataAccessServiceFactory _dataFactory;

        /// <summary>
        /// The author of this information
        /// </summary>
        private UserDataModel _user;

        /// <summary>
        /// Title of this information update
        /// </summary>
        public new string Title { get; set; }

        /// <summary>
        /// Author of this information update
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// The actual information
        /// </summary>
        public string Information { get; set; }

        /// <summary>
        /// Flag indicating that users are currently being fetched
        /// </summary>
        private bool _isFetchingUsers;

        /// <summary>
        /// Collection of users
        /// </summary>
        public Dictionary<UserDataModel, string> UsersCollection { get; set; }

        /// <summary>
        /// The author of this information update
        /// </summary>
        public UserDataModel User
        {
            get => _user;
            set => _user = value;
        }

        /// <summary>
        /// Event that is called to close dialog window
        /// </summary>
        public event EventHandler<DialogWindowCloseRequestedEventArgs> DialogWindowCloseRequest;

        /// <summary>
        /// Command to cancel this operation
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// Command to broadcast new information and update
        /// </summary>
        public ICommand BroadcastCommand { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public NewInfoUpdateDialogViewModel(IDataAccessServiceFactory dataFactory)
        {
            _dataFactory = dataFactory;
            UsersCollection = new Dictionary<UserDataModel, string>();

            _ = GetUsers();

            CancelCommand = new RelayCommand(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess)));
            BroadcastCommand = new RelayCommand(async () => await BroadcastInformation());
        }

        /// <summary>
        /// Broadcast new information   
        /// </summary>
        private async Task BroadcastInformation()
        {
            InfoUpdateDataModel? data = null;

            var infoUpdateTable = _dataFactory.GetDbTable<InfoUpdateDataModel>();
            var userTable = _dataFactory.GetDbTable<UserDataModel>();

            // Get user
            UserDataModel? user = await userTable.GetEntityByIdAsync(_user.Id);

            infoUpdateTable.DataChanged += (ss, e) =>
            {
                data = e as InfoUpdateDataModel;

                if(data is not null)
                {
                    Messenger.MessageSender.SendMessage(data);
                }
            };

            InfoUpdateDataModel newInfo = new InfoUpdateDataModel
            {
                Title = Title,
                Information = Information,
                DateCreated = DateTime.Now,
                PublishDate = DateTime.Now.ToString("MM-dd-yyyy ~ hh:mm tt"),
                LastUpdatedDate = DateTime.Now.ToString("MM-dd-yyyy ~ hh:mm tt"),
            };

            if(user is not null)
            {
                // Set the user performing this operation
                newInfo.InfoUpdateUserRelations.Add(new InfoUpdateUserRelations
                {
                    UserDataModel = user,
                    InfoUpdateDataModel = newInfo
                });

                await infoUpdateTable.CreateNewEntityAsync(newInfo);
            }

            // Mark process as successful
            IsSuccess = true;
            // Close dialog
            DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess));
            // Clear data 
            ClearDataResidue();
        }

        /// <summary>
        /// Load users
        /// </summary>
        private async Task GetUsers()
        {
            // Get user db table
            IDataAccessService<UserDataModel> users = _dataFactory.GetDbTable<UserDataModel>();

            if (_isFetchingUsers) return;
            {
                _isFetchingUsers = true;

                try
                {
                    // Collection of users
                    IReadOnlyList<UserDataModel> collectionOfUsers = await users.GetAllEntitiesAsync();

                    foreach (UserDataModel userData in collectionOfUsers)
                    {
                        // Do not load admin user
                        if (userData.LastName is "admin")
                            continue;

                        UsersCollection.Add(userData, userData.FirstName.PadRight(10) + userData.LastName);
                    }
                }
                finally
                {
                    _isFetchingUsers = false;
                }
            }

            // Set current user
            _user = UsersCollection.FirstOrDefault().Key;

            // Update UI
            OnPropertyChanged(nameof(User));

            // Refresh UI
            CollectionViewSource.GetDefaultView(UsersCollection).Refresh();
        }

        /// <summary>
        /// Clears any data residue
        /// </summary>
        private void ClearDataResidue() => Title = Author = Information = string.Empty;
        

    }
}
