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

        #endregion

        #region Events

        /// <summary>
        /// Event to close dialog window
        /// </summary>
        public event EventHandler<DialogWindowCloseRequestedEventArgs> DialogWindowCloseRequest;

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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataServiceFactory">Access to database</param>
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

            // Send dialog window close request
            DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess));
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

            // Validate incoming data
            ValidationResult result = DataValidationService.Validate(newUser);

            // Set success flag
            IsSuccess = result.IsValid;

            // If validation passes
            if (result.IsValid)
            {
                // Get users table
                using var userTable = _dataServiceFactory.GetDbTable<UserDataModel>();
                // Listen for when user is created
                userTable.DataChanged += UserTable_DataChanged;

                // See if user name exist
                bool isConflicting = (await userTable.GetAllEntitiesAsync()).Any(x => 
                { 
                    return (string.Equals(x.FirstName, newUser.FirstName, StringComparison.OrdinalIgnoreCase) && 
                            string.Equals(x.LastName, newUser.LastName, StringComparison.OrdinalIgnoreCase)); 
                });

                // If user exist
                if(isConflicting)
                {
                    // Alert user
                    await DialogService.InvokeFeedbackDialog(this, $"Username is not available.");
                    // Do nothing else
                    return;
                }

                // commit the newly created user to the users table
                await userTable.CreateNewEntityAsync(newUser);

                // Unhook event
                userTable.DataChanged -= UserTable_DataChanged;
            }

            // Set message
            string message = result.IsValid ? "User created successfully" : result.ErrorMessage;

            if(result.IsValid)
            {
                // Briefly show message
                await DialogService.InvokeAlertDialog(this, message);
            }
            else
            {
                await DialogService.InvokeFeedbackDialog(this, message);
            }

            // If successful...
            if(IsSuccess)
            {
                // Clear data
                ClearUserCreationDataResidue();

                // Send dialog window close request
                DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess));
            }
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
        /// Update users list with with latest data from database
        /// </summary>
        /// <param name="sender">Origin of this event</param>
        /// <param name="e">The actual data that changed</param>
        private void UserTable_DataChanged(object? sender, object e) => Messenger.MessageSender.SendMessage((UserDataModel)e);
        

        #endregion
    }
}
