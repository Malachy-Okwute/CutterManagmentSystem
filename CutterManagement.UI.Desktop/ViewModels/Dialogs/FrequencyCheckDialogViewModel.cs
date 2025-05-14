using CutterManagement.Core;
using CutterManagement.Core.Services;
using Microsoft.IdentityModel.Tokens;
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
        /// Comment
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// True if part size can be entered by user
        /// </summary>
        //public bool CanEnterPartSize { get; set; }

        /// <summary>
        /// True if frequency check passed
        /// </summary>
        public bool PassedCheck { get; set; }

        /// <summary>
        /// True if frequency check failed
        /// </summary>
        public bool FailedCheck { get; set; }

        /// <summary>
        /// Passed check
        /// </summary>
        private FrequencyCheckResult _passedCheck = FrequencyCheckResult.Passed;

        /// <summary>
        /// Failed check
        /// </summary>
        private FrequencyCheckResult _failedCheck = FrequencyCheckResult.Failed;

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
        public FrequencyCheckDialogViewModel(IDataAccessServiceFactory dataFactory)
        {
            UsersCollection = new Dictionary<UserDataModel, string>();
            _dataFactory = dataFactory;

            _taskLoader = GetUsers();

            // Create commands
            CancelCommand = new RelayCommand(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsMessageSuccess)));
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
            IDataAccessService<MachineDataModel> machineTable = _dataFactory.GetDbTable<MachineDataModel>();

            // Listen for changes 
            machineTable.DataChanged += (s, e) =>
            {
                // Set new data
                data = e as MachineDataModel;
                // Send out message
                Messenger.MessageSender.SendMessage(data ?? throw new ArgumentNullException("Machine data cannot be null"));
            };

            // Get machine
            MachineDataModel? machine = await machineTable.GetEntityByIdAsync(Id);

            // If machine is not null...
            if(machine is not null)
            {
                // Make sure piece count is entered
                if(PartCount.IsNullOrEmpty())
                {
                    Message = $"Enter part piece-count";

                    await DialogService.InvokeDialogFeedbackMessage(this);

                    return;
                }

                // Make sure new piece count is greater than current count
                if(int.Parse(PartCount) <= machine.Cutter.Count)
                {
                    Message = $"Piece-count must be greater than previous-count";

                    await DialogService.InvokeDialogFeedbackMessage(this);

                    return;
                }

                // Make sure either "Pass" or "Fail" is selected
                if (PassedCheck is false && FailedCheck is false)
                {
                    Message = $"Choose \" Passed \" or \" Failed \" ";

                    await DialogService.InvokeDialogFeedbackMessage(this);

                    return;
                }

                // Set new information
                machine.Cutter.Count = int.Parse(PartCount);
                machine.PartToothSize = PartToothSize ?? machine.PartToothSize;
                machine.Status = MachineStatus.IsRunning;
                machine.FrequencyCheckResult = PassedCheck ? _passedCheck : _failedCheck;
                machine.StatusMessage = Comment ?? "Meets specifications (CMM check)";

                // Update machine on database
                await machineTable.UpdateEntityAsync(machine);

                // Stop listening for changes
                machineTable.DataChanged -= delegate { };

                // Close dialog
                DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsMessageSuccess));
            }
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
