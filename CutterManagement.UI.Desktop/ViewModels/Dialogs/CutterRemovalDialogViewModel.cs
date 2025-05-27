using CutterManagement.Core;
using CutterManagement.Core.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="CutterRemovalDialog"/>
    /// </summary>
    public class CutterRemovalDialogViewModel : DialogViewModelBase, IDialogWindowCloseRequest
    {
        #region Private Fields

        /// <summary>
        /// Data factory
        /// </summary>
        private IDataAccessServiceFactory _dataFactory;

        /// <summary>
        /// Currently selected 
        /// </summary>
        private CutterRemovalReason _cutterRemovalReason;

        /// <summary>
        /// User that is setting this machine status
        /// </summary>
        private UserDataModel _user;

        /// <summary>
        /// 
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
        /// Current number
        /// </summary>
        public string CutterNumber { get; set; }

        /// <summary>
        /// Part count
        /// </summary>
        public string PartCount { get; set; }

        /// <summary>
        /// Previous part count
        /// </summary>
        public string PreviousPartCount { get; set; }

        /// <summary>
        /// Comment
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// True if cutter is to stay in the department
        /// </summary>
        public bool KeepCutter { get; set; }

        /// <summary>
        /// True if cutter needs to be rebuilt
        /// </summary>
        public bool RebuildCutter { get; set; }

        /// <summary>
        /// Collection of users
        /// </summary>
        public Dictionary<UserDataModel, string> UsersCollection { get; set; }

        /// <summary>
        /// Collection of cutter removal reasons
        /// </summary>
        public Dictionary<CutterRemovalReason, string> CutterRemovalReasonCollection { get; set; }

        /// <summary>
        /// User that is setting this machine status
        /// </summary>
        public UserDataModel User
        {
            get => _user;
            set => _user = value;
        }

        /// <summary>
        /// User that is setting this machine status
        /// </summary>
        public CutterRemovalReason CutterRemovalReason
        {
            get => _cutterRemovalReason;
            set => _cutterRemovalReason = value;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Event to close dialog window
        /// </summary>
        public event EventHandler<DialogWindowCloseRequestedEventArgs> DialogWindowCloseRequest;

        #endregion

        #region Public Commands

        /// <summary>
        /// Command to cancel cutter removal process
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// Command to remove cutter
        /// </summary>
        public ICommand RemoveCutterCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dataFactory">Data factory</param>
        public CutterRemovalDialogViewModel(IDataAccessServiceFactory dataFactory)
        {
            _dataFactory = dataFactory;
            CutterRemovalReasonCollection = new Dictionary<CutterRemovalReason, string>();
            UsersCollection = new Dictionary<UserDataModel, string>();

            _taskLoader = GetUsers();

            foreach (CutterRemovalReason reason in Enum.GetValues<CutterRemovalReason>())
            {
                // Add every reason
                CutterRemovalReasonCollection.Add(reason, EnumHelpers.GetDescription(reason));
            }

            CancelCommand = new RelayCommand(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess)));
            RemoveCutterCommand = new RelayCommand(async () => await RemoveCutter());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load users
        /// </summary>
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

        private async Task RemoveCutter()
        {
            // New data from database
            MachineDataModel? data = null;

            // Get machine table
            IDataAccessService<MachineDataModel> machineTable = _dataFactory.GetDbTable<MachineDataModel>();
            // Get user table
            IDataAccessService<UserDataModel> userTable = _dataFactory.GetDbTable<UserDataModel>();
            // Get cutter table
            IDataAccessService<CutterDataModel> cutterTable = _dataFactory.GetDbTable<CutterDataModel>();

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

            // Get user
            UserDataModel? user = await userTable.GetEntityByIdAsync(_user.Id);

            // If machine is not null...
            if (machine is not null)
            {
                // Current cutter
                CutterDataModel? cutter = await cutterTable.GetEntityByIdAsync(machine.Cutter.Id) ?? throw new ArgumentNullException("Cutter not found");

                // Make sure piece count is entered
                if (PartCount.IsNullOrEmpty())
                {
                    Message = $"Enter part piece-count";

                    await DialogService.InvokeFeedbackDialog(this);

                    return;
                }

                // Make sure new piece count is greater than current count
                if (int.Parse(PartCount) < machine.Cutter.Count)
                {
                    Message = $"Piece-count must be greater or equal to previous-count";

                    await DialogService.InvokeFeedbackDialog(this);

                    return;
                }

                // Make sure either "Keep" or "Rebuild" is selected
                if (KeepCutter is false && RebuildCutter is false)
                {
                    Message = $"Choose \" Keep \" or \" Rebuild \" for this check";

                    await DialogService.InvokeFeedbackDialog(this);

                    return;
                }

                // If count is greater than previous count by more than 100
                if ((int.Parse(PartCount) - machine.Cutter.Count) > 100)
                {
                    Message = $"Previous count is {machine.Cutter.Count}. Do you mean to enter {PartCount} ?";

                    // Verify piece count is reasonable
                    bool? response = await DialogService.InvokeFeedbackDialog(this, FeedbackDialogKind.Prompt);

                    // If user did not mean to enter the current part number
                    if (response is false)
                    {
                        // Cancel this process
                        return;
                    }
                }

                // Make sure we have a reason for removing cutter
                if(CutterRemovalReason == CutterRemovalReason.None)
                {
                    Message = $"Select reason for removing cutter";

                    await DialogService.InvokeFeedbackDialog(this);

                    return;
                }

                // Set new information
                machine.FrequencyCheckResult = FrequencyCheckResult.Setup;
                machine.Status =  MachineStatus.Warning;
                machine.StatusMessage = Comment ?? $"Cutter was removed. {DateTime.Now.ToString("D")}";
                machine.DateTimeLastModified = DateTime.Now;
                machine.Cutter.CutterChangeInfo = CutterRemovalReason;
                machine.Cutter.LastUsedDate = DateTime.Now;
                machine.Cutter.Condition = CutterCondition.Used;
                machine.Cutter.Count = int.Parse(PartCount);
                machine.Cutter.MachineDataModelId = null;
                machine.CutterDataModelId = null;

                if(KeepCutter is false && RebuildCutter is true)
                {
                    await cutterTable.DeleteEntityAsync(cutter);
                }

                // Set the user performing this operation
                machine.MachineUserInteractions.Add(new MachineUserInteractions
                {
                    UserDataModel = user ?? throw new NullReferenceException($"User with the name {user?.FirstName.PadRight(6)} {user?.LastName} not found"),
                    MachineDataModel = machine
                });

                // Update database
                await machineTable.UpdateEntityAsync(machine);
                await cutterTable.UpdateEntityAsync(cutter);

                // Stop listening for changes
                machineTable.DataChanged -= delegate { };

                // Close dialog
                DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess));
            }

            #endregion
        }
    }
}
