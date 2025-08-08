using CutterManagement.Core;
using CutterManagement.Core.Services;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.PortableExecutable;
using System.Windows.Data;
using System.Windows.Input;

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
        /// Passed check
        /// </summary>
        private FrequencyCheckResult _passedCheck = FrequencyCheckResult.Passed;

        /// <summary>
        /// Failed check
        /// </summary>
        private FrequencyCheckResult _failedCheck = FrequencyCheckResult.Failed;

        /// <summary>
        /// Provides service to machine
        /// </summary>
        private readonly IMachineService _machineService;

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
        /// Comment
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// True if frequency check passed
        /// </summary>
        public bool PassedCheck { get; set; }

        /// <summary>
        /// True if frequency check failed
        /// </summary>
        public bool FailedCheck { get; set; }

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

        #region Public Commands

        /// <summary>
        /// Command to cancel frequency check process
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// Command to update machine data
        /// </summary>
        public ICommand UpdateCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FrequencyCheckDialogViewModel(IMachineService machineService)
        {
            UsersCollection = new Dictionary<UserDataModel, string>();
            _machineService = machineService;

            _ = GetUsers();

            // Create commands
            CancelCommand = new RelayCommand(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess)));
            UpdateCommand = new RelayCommand(async () => await UpdateMachine());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates machine info
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        private async Task UpdateMachine()
        {
            // New data from database
            MachineDataModel? data = null;

            // Get machine table
            IDataAccessService<MachineDataModel> machineTable = _machineService.DataBaseAccess.GetDbTable<MachineDataModel>();
            // Get user table
            IDataAccessService<UserDataModel> userTable = _machineService.DataBaseAccess.GetDbTable<UserDataModel>();
            // Get part log table
            IDataAccessService<ProductionPartsLogDataModel> productionLogTable = _machineService.DataBaseAccess.GetDbTable<ProductionPartsLogDataModel>();

            EventHandler<object>? handler = null;

            // Listen for changes 
            handler += (s, e) =>
            {
                // Unsubscribe from event to prevent memory leak
                machineTable.DataChanged -= handler;
                // Set new data
                data = e as MachineDataModel;
                // Send out message
                Messenger.MessageSender.SendMessage(data ?? throw new ArgumentNullException("SelectedMachine data cannot be null"));

                // User associated with cmm data entry
                UserDataModel? user = (data.MachineUserInteractions.Single(user => user.Id == _user.Id).UserDataModel);

                // Log cmm data
                ProductionPartsLogHelper.LogProductionProgress(_user, data, productionLogTable);
            };

            // Subscribe to data changed event
            machineTable.DataChanged += handler;

            // Get machine
            MachineDataModel? machine = await machineTable.GetEntityByIdAsync(Id);

            // Get user
            UserDataModel? user = await userTable.GetEntityByIdAsync(_user.Id);

            // If machine is not null...
            if(machine is not null)
            {
                // Make sure piece count is entered
                if(PartCount.IsNullOrEmpty())
                {
                    await DialogService.InvokeFeedbackDialog(this, "Enter part piece-count");

                    return;
                }

                // Make sure new piece count is greater than current count
                if(int.Parse(PartCount) <= machine.Cutter.Count)
                {
                    await DialogService.InvokeFeedbackDialog(this, "Piece-count must be greater than previous-count");

                    return;
                }

                // Make sure either "Pass" or "Fail" is selected
                if (PassedCheck is false && FailedCheck is false)
                {
                    await DialogService.InvokeFeedbackDialog(this, "Choose \" Passed \" or \" Failed \" for this check");

                    return;
                }

                // Prompt users to specify why part failed
                if (FailedCheck is true && string.IsNullOrEmpty(Comment))
                {
                    await DialogService.InvokeFeedbackDialog(this, "Please specify why part failed in the comment section");

                    return;
                }

                // If count is greater than previous count by more than 100
                if ((int.Parse(PartCount) - machine.Cutter.Count) > 100)
                {
                    string errorMessage = $"Current count is {machine.Cutter.Count}. Do you mean to enter {PartCount} ?";

                    // Verify piece count is reasonable
                    bool? response = await DialogService.InvokeFeedbackDialog(this, errorMessage, FeedbackDialogKind.Prompt);

                    // If user did not mean to enter the current part number
                    if(response is false)
                    {
                        // Cancel this process
                        return;
                    }
                }

                // Set new information
                machine.Cutter.Count = int.Parse(PartCount);
                machine.PartToothSize = PartToothSize ?? machine.PartToothSize;
                machine.FrequencyCheckResult = PassedCheck ? _passedCheck : _failedCheck;
                machine.Status = (machine.FrequencyCheckResult == _failedCheck) ? MachineStatus.Warning : MachineStatus.IsRunning;
                machine.StatusMessage = (machine.FrequencyCheckResult == _failedCheck) ? (Comment!) : (Comment ?? "In good condition");
                machine.DateTimeLastModified = DateTime.Now;

                // Set the user performing this operation
                machine.MachineUserInteractions.Add(new MachineUserInteractions
                {
                    UserDataModel = user ?? throw new NullReferenceException($"User with the name {user?.FirstName.PadRight(6)} {user?.LastName} not found"),
                    MachineDataModel = machine
                });

                // Update machine on database
                await machineTable.UpdateEntityAsync(machine);

                // Close dialog
                DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess));
            }
        }

        /// <summary>
        /// Load users
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        private async Task GetUsers()
        {
            // Get user db table
            IDataAccessService<UserDataModel> users = _machineService.DataBaseAccess.GetDbTable<UserDataModel>();

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
