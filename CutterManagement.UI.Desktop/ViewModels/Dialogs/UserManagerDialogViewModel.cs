using CutterManagement.Core;
using CutterManagement.Core.Services;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="UserManagerDialog"/>
    /// </summary>
    public class UserManagerDialogViewModel : DialogViewModelBase, IDialogWindowCloseRequest
    {
        #region Private Fields

        /// <summary>
        /// Collection of deactivated users
        /// </summary>
        private ObservableCollection<DeactivatedUserViewModel> _deactivatedUser;

        /// <summary>
        /// Http client factory
        /// </summary>
        private IHttpClientFactory _httpFactory;

        #endregion

        #region Public Properties

        /// <summary>
        /// Collection of deactivated users
        /// </summary>
        public ObservableCollection<DeactivatedUserViewModel> DeactivatedUser
        {
            get => _deactivatedUser;
            set => _deactivatedUser = value;
        }

        /// <summary>
        /// True if deactivated user list is empty
        /// </summary>
        public bool IsDeactivatedUserCollectionEmpty { get; set; }

        /// <summary>
        /// True if Delete and Activate buttons are enabled
        /// </summary>
        public bool IsButtonActive { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// Command to activate users
        /// </summary>
        public ICommand ActivateUserCommand { get; set; }

        /// <summary>
        /// Command to delete user
        /// </summary>
        public ICommand DeleteUserCommand { get; set; }

        /// <summary>
        /// Command to cancel and close this dialog
        /// </summary>
        public ICommand CancelCommand { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Event that gets fired when dialog needs to close
        /// </summary>
        public event EventHandler<DialogWindowCloseRequestedEventArgs> DialogWindowCloseRequest;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserManagerDialogViewModel(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
            _deactivatedUser = new ObservableCollection<DeactivatedUserViewModel>();

            ActivateUserCommand = new RelayCommand(async () => await ActivateUser());
            DeleteUserCommand = new RelayCommand(async () => await DeleteUser());
            CancelCommand = new RelayCommand(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess)));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Archives selected users from collection of deactivated users
        /// </summary>
        private async Task DeleteUser()
        {
            HttpClient client =_httpFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7057");

            var userCollection = await ServerRequest.GetDataCollection<UserDataModel>(client, $"UserDataModel");

            // Archive only users that are selected
            foreach (var user in _deactivatedUser)
            {
                if (user.IsSelected)
                {
                    UserDataModel? actualUser = userCollection?.Single(u => u.Id == user.Id);

                    if (actualUser is not null)
                    {
                        actualUser.IsArchived = true;
                        actualUser.FirstName = $"{actualUser.FirstName}_Archived";
                        actualUser.LastName = $"{actualUser.LastName}_Archived";
                        var postResponse = await ServerRequest.PostData(client, $"UserDataModel", actualUser);
                    }
                }
            }

            IsDeactivatedUserCollectionEmpty = _deactivatedUser.Any() is false;

            IsButtonActive = !IsDeactivatedUserCollectionEmpty;

            // Close dialog
            DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess));
        }

        /// <summary>
        /// Activates selected users from collection of deactivated users
        /// </summary>
        private async Task ActivateUser()
        {
            HttpClient client = _httpFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7057");

            var userCollection = await ServerRequest.GetDataCollection<UserDataModel>(client, $"UserDataModel");

            // Activate only users that are selected
            foreach (var user in _deactivatedUser)
            {
                if (user.IsSelected)
                {
                    UserDataModel? actualUser = userCollection?.Single(u => u.Id == user.Id);

                    if (actualUser is not null)
                    {
                        actualUser.IsActive = true;
                        var postResponse = await ServerRequest.PostData(client, $"UserDataModel", actualUser);
                    }
                }
            }

            IsDeactivatedUserCollectionEmpty = _deactivatedUser.Any() is false;
            IsButtonActive = !IsDeactivatedUserCollectionEmpty;

            // Close dialog
            DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess));
        }

        /// <summary>
        /// Fetch deactivated users
        /// </summary>
        public async Task GetDeactivatedUsers()
        {
            HttpClient client = _httpFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7057");

            var userCollection = await ServerRequest.GetDataCollection<UserDataModel>(client, $"UserDataModel");

            userCollection?.ToList().ForEach(user =>
            {
                if(user.IsActive is false && user.IsArchived is false)
                {
                    _deactivatedUser.Add(new DeactivatedUserViewModel 
                    { 
                        Id= user.Id,
                        UserFullName = $"{user.FirstName} {user.LastName}" 
                    });
                }
            });

            IsDeactivatedUserCollectionEmpty = _deactivatedUser.Any() is false;
            IsButtonActive = !IsDeactivatedUserCollectionEmpty;

        }

        #endregion
    }
}
