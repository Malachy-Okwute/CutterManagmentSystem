using CutterManagement.Core;
using CutterManagement.Core.Services;
using System.Windows.Data;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="FrequencyCheckDialog"/>
    /// </summary>
    public class FrequencyCheckDialogViewModel : DialogViewModelBase, IDialogWindowCloseRequest
    {
        #region Private Fields

        /// <summary>
        /// User that is setting this machine status
        /// </summary>
        private UserDataModel _user;

        /// <summary>
        /// Data factory
        /// </summary>
        private IDataAccessServiceFactory _dataFactory;

        /// <summary>
        /// Loads user
        /// </summary>
        private Task _taskLoader;

        #endregion

        #region Public Properties

        /// <summary>
        /// Unique machine id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Machine number
        /// </summary>
        public string MachineNumber { get; set; }

        /// <summary>
        /// Part number
        /// </summary>
        public string PartNumber { get; set; }

        /// <summary>
        /// Part count
        /// </summary>
        public string PartCount { get; set; }

        /// <summary>
        /// Part tooth size
        /// </summary>
        public string? PartToothSize { get; set; }

        /// <summary>
        /// Previous part count
        /// </summary>
        public string PreviousPartCount { get; set; }

        /// <summary>
        /// Previous part tooth size
        /// </summary>
        public string? PreviousPartToothSize { get; set; }

        /// <summary>
        /// True if part size can be entered by user
        /// </summary>
        public bool CanEnterPartSize { get; set; }

        /// <summary>
        /// The result of this check
        /// </summary>
        public string FrequencyCheckResult { get; set; }

        /// <summary>
        /// Collection of users
        /// </summary>
        public Dictionary<UserDataModel, string> UsersCollection { get; set; }

        /// <summary>
        /// User that is setting this machine status
        /// </summary>
        public UserDataModel User
        {
            get => _user;
            set => _user = value;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Close dialog request event
        /// </summary>
        public event EventHandler<DialogWindowCloseRequestedEventArgs> DialogWindowCloseRequest;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FrequencyCheckDialogViewModel(IDataAccessServiceFactory dataFactory)
        {
            UsersCollection = new Dictionary<UserDataModel, string>();
            _dataFactory = dataFactory;

            _taskLoader = GetUsers();

        }

        /// <summary>
        /// Load users
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        private async Task GetUsers()
        {
            // Get user db table
            IDataAccessService<UserDataModel> users = _dataFactory.GetDbTable<UserDataModel>();

            foreach (UserDataModel userData in await users.GetAllEntitiesAsync())
            {
                // Do not load admin user
                if (userData.LastName is "admin")
                    continue;

                UsersCollection.Add(userData, userData.FirstName.PadRight(10) + userData.LastName);
            }

            // Set current user
            _user = UsersCollection.FirstOrDefault().Key;

            // Update UI
            OnPropertyChanged(nameof(User));

            // Refresh UI
            CollectionViewSource.GetDefaultView(UsersCollection).Refresh();
        }


        #endregion    
    }
}
