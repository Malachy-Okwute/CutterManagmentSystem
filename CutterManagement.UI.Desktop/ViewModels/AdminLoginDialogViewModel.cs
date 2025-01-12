using CutterManagement.Core;
using CutterManagement.Core.Services;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="AdminLoginDialog"/>
    /// </summary>
    public class AdminLoginDialogViewModel : DialogViewModelBase, IDialogWindowCloseRequest
    {
        /// <summary>
        /// Tells whether admin user is currently logged in or not
        /// </summary>
        private string _sessionStatus;

        /// <summary>
        /// Message to display if attempt to login failed
        /// </summary>
        public bool ShowErrorMessage { get; set; }

        /// <summary>
        /// Message to display if login is successful
        /// </summary>
        public bool ShowSuccessMessage { get; set; }

        /// <summary>
        /// True if admin is currently logged in
        /// otherwise, false
        /// </summary>
        public bool IsAdminLoggedIn => _sessionStatus is "Currently logged in";

        /// <summary>
        /// The user name admin uses to login 
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Tells whether admin user is currently logged in or not
        /// </summary>
        public string SessionStatus
        {
            get => _sessionStatus;
            set => _sessionStatus = value;
        }

        /// <summary>
        /// Event that is invoked when user cancels or proceed with admin login
        /// </summary>
        public event EventHandler<DialogWindowCloseRequestedEventArgs> DialogWindowCloseRequest;

        /// <summary>
        /// Command to log in user
        /// </summary>
        public ICommand LoginCommand { get; set; }

        /// <summary>
        /// Command to cancel log in procedure
        /// </summary>
        public ICommand CancelLoginCommand { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public AdminLoginDialogViewModel()
        {
            Title = "Admin Login";
            GetCurrentLoginSessionStatus();
            LoginCommand = new RelayCommand(async (parameter) => await Login(parameter));
            CancelLoginCommand = new RelayCommand(() =>
            {
                DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(ShowErrorMessage));
            });
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
                // Update login session
                GetCurrentLoginSessionStatus();

                // Hide error message
                ShowErrorMessage = false;

                // Display success message
                ShowSuccessMessage = true;

                // Clear user name
                Username = string.Empty;

                // Delay for 2 seconds...
                await Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((action) =>
                {
                    // Close dialog window
                    DispatcherService.Invoke(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(ShowSuccessMessage)));
                });
            }
        }

        /// <summary>
        /// Gets information about the current admin login status
        /// </summary>
        private void GetCurrentLoginSessionStatus()
        {
            _sessionStatus = AuthenticationService.IsAdminUserAuthorized ? "Currently logged in" : "Not logged in";
            OnPropertyChanged(nameof(SessionStatus));

            // TODO: Broadcast and listen to live event of when login session starts or ended
            //       So we can get live update and update UI with the latest information
        }

    }
}
