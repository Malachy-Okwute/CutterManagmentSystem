using CutterManagement.Core;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    public class UsersPageViewModel : ViewModelBase
    {
        /// <summary>
        /// Tells whether admin user is currently logged in or not
        /// </summary>
        private string _sessionStatus;

        private ObservableCollection<UserItemViewModel> _users;

        /// <summary>
        /// The user name 
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// True if log in form is to be shown 
        /// Otherwise false
        /// </summary>
        public bool IsLoginFormVisible { get; set; }

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
        /// Tells whether admin user is currently logged in or not
        /// </summary>
        public string SessionStatus 
        {
            get => _sessionStatus;
            set => _sessionStatus = value;
        }
        
        public ObservableCollection<UserItemViewModel> Users
        {
            get => _users;
            set => _users = value;
        }

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

        public UsersPageViewModel()
        {
            GetCurrentLoginSessionStatus();

            _users = new ObservableCollection<UserItemViewModel>
            {
              new UserItemViewModel{ FirstName= "Heather", LastName = "Okwute", UserFullName = "Heather Okwute", UserInitials = "HO", UserShift = "1st shift" },
              new UserItemViewModel{ FirstName= "Malachy", LastName = "Okwute", UserFullName = "Malachy Okwute", UserInitials = "MO", UserShift = "2nd shift" },
              new UserItemViewModel{ FirstName= "Logan", LastName = "Berger", UserFullName = "Logan Berger", UserInitials = "LB", UserShift = "3rd shift" },
              new UserItemViewModel{ FirstName= "Aidan", LastName = "Berger", UserFullName = "Aidan Berger", UserInitials = "AB", UserShift = "1st shift" },
              new UserItemViewModel{ FirstName= "Heather", LastName = "Okwute", UserFullName = "Heather Okwute", UserInitials = "HO", UserShift = "1st shift" },
              new UserItemViewModel{ FirstName= "Malachy", LastName = "Okwute", UserFullName = "Malachy Okwute", UserInitials = "MO", UserShift = "2nd shift" },
              new UserItemViewModel{ FirstName= "Logan", LastName = "Berger", UserFullName = "Logan Berger", UserInitials = "LB", UserShift = "3rd shift" },
              new UserItemViewModel{ FirstName= "Aidan", LastName = "Berger", UserFullName = "Aidan Berger", UserInitials = "AB", UserShift = "1st shift" },
              new UserItemViewModel{ FirstName= "Heather", LastName = "Okwute", UserFullName = "Heather Okwute", UserInitials = "HO", UserShift = "1st shift" },
              new UserItemViewModel{ FirstName= "Malachy", LastName = "Okwute", UserFullName = "Malachy Okwute", UserInitials = "MO", UserShift = "2nd shift" },
              new UserItemViewModel{ FirstName= "Logan", LastName = "Berger", UserFullName = "Logan Berger", UserInitials = "LB", UserShift = "3rd shift" },
              new UserItemViewModel{ FirstName= "Aidan", LastName = "Berger", UserFullName = "Aidan Berger", UserInitials = "AB", UserShift = "1st shift" },
            };


            OnPropertyChanged(nameof(Users));

            ShowLoginFormCommand = new RelayCommand(() => IsLoginFormVisible = true);
            LoginCommand = new RelayCommand(async(parameter) => await Login(parameter));
            CancelCommand = new RelayCommand(() =>
            {
                // Reset messages
                ResetLoginProcedure();
                IsLoginFormVisible = false;
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

        private void GetCurrentLoginSessionStatus()
        {
            _sessionStatus = AuthenticationService.IsAdminUserAuthorized ?
                                "Session status: Currently logged in" :
                                       "Session status: Not logged in";
            OnPropertyChanged(nameof(SessionStatus));

            // TODO: Broadcast and listen to live event of when login session starts or ended
            //       So we can get live update and update UI with the latest information
        }
    }
}
