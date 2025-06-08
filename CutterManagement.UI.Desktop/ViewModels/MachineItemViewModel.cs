using CutterManagement.Core;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="MachineItemControl"/>
    /// </summary>
    public class MachineItemViewModel : ViewModelBase
    {
        #region Private Fields

        /// <summary>
        /// Provides services to machine
        /// </summary>
        private readonly IMachineService _machineService;

        /// <summary>
        /// The current count on this machine item
        /// </summary>
        private int _currentCount;

        #endregion

        #region Public Properties

        /// <summary>
        /// The unique Id of this machine
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Unique number assigned to this machine
        /// </summary>
        public string MachineNumber { get; set; }

        /// <summary>
        /// Unique set number assigned to this machine
        /// </summary>
        public string MachineSetNumber { get; set; } 

        /// <summary>
        /// Cutter id number currently setup on this machine
        /// </summary>
        public string? CutterNumber { get; set; } 

        /// <summary>
        /// True if this machine is running, false if it's sitting idle or down for maintenance
        /// </summary>
        public bool IsRunning { get; set; }

        /// <summary>
        /// True if this machine item is configured, 
        /// Otherwise false
        /// </summary>
        public bool IsConfigured { get; set; }

        /// <summary>
        /// The current status of this machine 
        /// </summary>
        public MachineStatus Status { get; set; }

        /// <summary>
        /// Comment related to the status of this machine
        /// </summary>
        public string StatusMessage { get; set; } 

        /// <summary>
        /// Part unique id number running on this machine
        /// </summary>
        public string? PartNumber { get; set; }

        /// <summary>
        /// Number of parts produced by this machine with the current cutter
        /// </summary>
        public string Count { get; set; }

        /// <summary>
        /// Result of the last part checked 
        /// <para>SETUP | PASSED | FAILED</para>
        /// </summary>
        public string FrequencyCheckResult { get; set; } 

        /// <summary>
        /// Date and time of the last checked part on this machine
        /// </summary>
        public string DateTimeLastModified { get; set; }

        /// <summary>
        /// True if pop up should show in the view 
        /// otherwise false
        /// </summary>
        public bool IsPopupOpen { get; set; }

        /// <summary>
        /// True if piece count can be edited
        /// Otherwise false
        /// <para>
        /// DEV-NOTE: Controls Readonly property in UI
        /// </para>
        /// </summary>
        public bool CanEditPieceCount { get; set; }

        /// <summary>
        /// The owner of this machine
        /// </summary>
        public Department Owner { get; set; }

        /// <summary>
        /// True is this machine has cutter, Otherwise false
        /// </summary>
        public bool HasCutter => CutterNumber is not null;

        /// <summary>
        /// Size of part tooth
        /// </summary>
        public string PartPreviousToothSize { get; set; }

        /// <summary>
        /// True if admin user is currently logged in
        /// otherwise false
        /// </summary>
        public bool IsAdminLoggedIn => AuthenticationService.IsAdminUserAuthorized;

        #endregion

        #region Public Events

        /// <summary>
        /// Event that gets broadcasted whenever this item gets selected
        /// </summary>
        public event EventHandler ItemSelected;
 
        #endregion

        #region Public Commands

        /// <summary>
        /// Command to open pop up control 
        /// </summary>
        public ICommand OpenPopupCommand { get; set; }

        /// <summary>
        /// Command to open machine configuration dialog
        /// </summary>
        public ICommand OpenMachineConfigurationDialogCommand { get; set; }

        /// <summary>
        /// Command to open machine set status dialog
        /// </summary>
        public ICommand OpenStatusSettingDialogCommand { get; set; }

        /// <summary>
        /// Command to open machine dialog
        /// </summary>
        public ICommand OpenMachineDialogCommand { get; set; }

        /// <summary>
        /// Command to open cutter removal dialog
        /// </summary>
        public ICommand OpenCutterRemovalDialogCommand { get; set; }

        /// <summary>
        /// Command to adjust piece count 
        /// </summary>
        public ICommand PieceCountAdjustmentCommand { get; set; }

        /// <summary>
        /// Command to save new entered piece count as current
        /// </summary>
        public ICommand UpdatePieceCountCommand { get; set; }

        /// <summary>
        /// Command to cancel piece count editing process
        /// </summary>
        public ICommand CancelPieceCountEditingCommand { get; set; }

        /// <summary>
        /// Command to open cutter relocation dialog
        /// </summary>
        public ICommand OpenCutterRelocationDialogCommand { get; set; }

        /// <summary>
        /// Command to change the current part number
        /// </summary>
        public ICommand ChangePartNumberCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MachineItemViewModel(IMachineService machineService)
        {
            _machineService = machineService;
            CanEditPieceCount = !CanEditPieceCount;

            // Create commands
            OpenPopupCommand = new RelayCommand(OpenPopup);
            PieceCountAdjustmentCommand = new RelayCommand(EditPieceCount);
            UpdatePieceCountCommand = new RelayCommand(async () => await UpdatePieceCount());
            OpenMachineDialogCommand = new RelayCommand(async () => await OpenMachineDialog());
            OpenMachineConfigurationDialogCommand = new RelayCommand(OpenMachineConfigurationDialog);
            ChangePartNumberCommand = new RelayCommand(async () => await OpenPartNumberChangeDialog());
            OpenStatusSettingDialogCommand = new RelayCommand(async () => await OpenStatusSettingDialog());
            OpenCutterRemovalDialogCommand = new RelayCommand(async () => await OpenCutterRemovalDialog());
            OpenCutterRelocationDialogCommand = new RelayCommand(async () => await OpenCutterRelocationDialog());
            CancelPieceCountEditingCommand = new RelayCommand(() =>
            {
                Count = _currentCount.ToString();
                CanEditPieceCount = !CanEditPieceCount;
            });
        }

        #endregion

        #region Command Method

        /// <summary>
        /// Opens a dialog to set machine status
        /// </summary>
        private async Task OpenStatusSettingDialog()
        {
            ItemSelected?.Invoke(this, EventArgs.Empty);

            // Machine status view model
            var statusSettingDialog = _machineService.GetDialogViewModel<MachineStatusSettingDialogViewModel>();

            // Make sure machine is configured
            if (IsConfigured is false)
            {
                // Define a message
                statusSettingDialog.Message = "SelectedMachine need to be configured for production";

                // Show feed back message
                await DialogService.InvokeFeedbackDialog(statusSettingDialog);

                // Do nothing else
                return;
            }

            statusSettingDialog.Id = Id;
            statusSettingDialog.Owner = Owner;
            statusSettingDialog.Label = MachineNumber;
            statusSettingDialog.MachineNumber = MachineNumber;
            statusSettingDialog.MachineSetNumber = MachineSetNumber;
            statusSettingDialog.IsConfigured = IsConfigured;

            // Show dialog
            DialogService.InvokeDialog(statusSettingDialog);
        }

        /// <summary>
        /// Opens a dialog to configure machine
        /// </summary>
        private void OpenMachineConfigurationDialog()
        {
            // Broadcast that this item was selected
            ItemSelected?.Invoke(this, EventArgs.Empty);

            // Machine configuration view model
            var machineConfiguration = _machineService.GetDialogViewModel<MachineConfigurationDialogViewModel>();

            machineConfiguration.Id = Id;
            machineConfiguration.Owner = Owner;
            machineConfiguration.Label = MachineNumber;
            
            // Show dialog
            DialogService.InvokeDialog(machineConfiguration);
        }

        /// <summary>
        /// Opens the popup control
        /// </summary>
        private void OpenPopup()
        {
            // Broadcast that this item was selected
            ItemSelected?.Invoke(this, EventArgs.Empty);

            // Set this item as the selected item
            IsPopupOpen ^= true;
        }

        /// <summary>
        /// Open machine dialog
        /// <para>Setup dialog | CMM-check dialog | Frequency-check dialog</para>
        /// </summary>
        private async Task OpenMachineDialog()
        {
            // Broadcast that this item was selected
            ItemSelected?.Invoke(this, EventArgs.Empty);

            // Setup mode
            FrequencyCheckResult setupMode = Core.FrequencyCheckResult.Setup;

            // If machine is in setup mode
            if (FrequencyCheckResult == setupMode.ToString() && HasCutter is false)
            {
                // Setup dialog view model
                var setupDialog = _machineService.GetDialogViewModel<MachineSetupDialogViewModel>();

                // Get cutters ready
                await setupDialog.ReloadCutters();

                setupDialog.GetMachineItem(this);

                // Make sure machine is configured
                if (IsConfigured is false)
                {
                    // Define a message
                    setupDialog.Message = "This machine need to be configured for production";

                    // Show feed back message
                    await DialogService.InvokeFeedbackDialog(setupDialog);

                    // Do nothing else
                    return;
                }

                // Invoke setup dialog
                DialogService.InvokeDialog(setupDialog);
            }
            else if (FrequencyCheckResult == setupMode.ToString() && HasCutter)
            {
                // Show form to enter CMM data
                var cmmCheckDialog = _machineService.GetDialogViewModel<CMMCheckDialogViewModel>();

                cmmCheckDialog.Id = Id;
                cmmCheckDialog.CurrentCount = int.Parse(Count) == 0 ? "Count" : Count;

                DialogService.InvokeDialog(cmmCheckDialog);
            }
            //Otherwise
            else
            {
                var frequencyCheck =  _machineService.GetDialogViewModel<FrequencyCheckDialogViewModel>();

                frequencyCheck.Id = Id;
                frequencyCheck.PartNumber = PartNumber ?? "Part number unknown";
                frequencyCheck.MachineNumber = MachineNumber;
                frequencyCheck.PreviousPartCount = string.Format("Count: {0}", Count);
                frequencyCheck.PreviousPartToothSize = PartPreviousToothSize.Equals("0") ? string.Format("Size: {0}", "n/a") : string.Format("Size: {0}", PartPreviousToothSize);

                // Invoke frequency check dialog
                DialogService.InvokeDialog(frequencyCheck);
            }
        }

        /// <summary>
        /// Open a dialog to change part number for this machine item
        /// </summary>
        private async Task OpenPartNumberChangeDialog()
        {
            // Broadcast that this item was selected
            ItemSelected?.Invoke(this, EventArgs.Empty);

            // Setup dialog view model
            var setupDialog = _machineService.GetDialogViewModel<MachineSetupDialogViewModel>();

            // Get cutters ready
            await setupDialog.ReloadCutters();

            setupDialog.GetMachineItem(this);

            setupDialog.Title = "Change part number";

            setupDialog.IsChangePartNumber = false;

            // Make sure machine is currently set up with cutter and part number
            if (HasCutter is false)
            {
                // Define a message
                setupDialog.Message = "Machine does not have part number currently set up";

                // Show feed back message
                await DialogService.InvokeFeedbackDialog(setupDialog);

                // Do nothing else
                return;
            }

            // If we have a cutter number
            if (string.IsNullOrEmpty(CutterNumber) is false)
            {
                // Format cutter number 
                string cutterNumber = CutterNumber.Substring(0, CutterNumber.IndexOf("-"));
                // Set cutter number
                setupDialog.CutterNumber = cutterNumber;
                // Show dialog
                DialogService.InvokeDialog(setupDialog);
            }
        }


        /// <summary>
        /// Opens cutter removal dialog
        /// </summary>
        private async Task OpenCutterRemovalDialog()
        {
            // Broadcast that this item was selected
            ItemSelected?.Invoke(this, EventArgs.Empty);

            var cutterRemovalDialog = _machineService.GetDialogViewModel<CutterRemovalDialogViewModel>();

            // If machine doesn't currently have cutter..
            if (HasCutter is false)
            {
                //--- Cancel cutter removal process ---//

                // Error message
                cutterRemovalDialog.Message = $"[{MachineNumber}]    does not currently have any cutter to be removed";

                // Show dialog
                await DialogService.InvokeFeedbackDialog(cutterRemovalDialog);

                // Do nothing else
                return;
            }

            cutterRemovalDialog.Id = Id;
            cutterRemovalDialog.PartNumber = PartNumber ?? "Part number unknown";
            cutterRemovalDialog.CutterNumber = CutterNumber ?? "Cutter number unknown";
            cutterRemovalDialog.PreviousPartCount = string.Format("Count: {0}", Count);
            cutterRemovalDialog.MachineNumber = MachineNumber;
            
            // Show dialog
            DialogService.InvokeDialog(cutterRemovalDialog);
        }

        /// <summary>
        /// Initiates piece count editing process
        /// </summary>
        private void EditPieceCount()
        {
            // Broadcast that this item was selected
            ItemSelected?.Invoke(this, EventArgs.Empty);

            // Make sure this machine has cutter
            if(HasCutter)
            {
                // Store current value
                _currentCount = int.Parse(Count);
                // Start piece count process
                CanEditPieceCount = !CanEditPieceCount;
            }
        }

        /// <summary>
        /// Confirms that user intended to enter the new piece count
        /// </summary>
        public async Task<bool?> VerifyUserIntention()
        {
            // Dummy view model
            var pieceCountAdjustmentVM = new PieceCountAdjustmentDialogViewModel(); // Dummy view model. Used to be able to show prompt

            // Message
            pieceCountAdjustmentVM.Message = $"Current count is {_currentCount}. Do you mean to enter {Count} ?";

            // Prompt user
            var result = await DialogService.InvokeFeedbackDialog(pieceCountAdjustmentVM, FeedbackDialogKind.Prompt);

            // Return prompt result
            return result;
        }

        /// <summary>
        /// Updates count on this machine
        /// </summary>
        private async Task UpdatePieceCount()
        {
            // User's intention result
            bool? userIntentionResult = null;

            await _machineService.AdjustPieceCount(Id, int.Parse(Count), async () =>
            {
                userIntentionResult = await VerifyUserIntention();

                return userIntentionResult;
            })
            .ContinueWith(_ =>
            {
                if(userIntentionResult is not true)
                {
                    Count = _currentCount.ToString();
                }

                CanEditPieceCount = !CanEditPieceCount;
            });
        }

        /// <summary>
        /// Relocates cutter and associated data to a different machine
        /// </summary>
        private async Task OpenCutterRelocationDialog()
        {
            // Broadcast that this item was selected
            ItemSelected?.Invoke(this, EventArgs.Empty);

            // Get cutter relocation view model
            var cutterRelocationDialog = _machineService.GetDialogViewModel<CutterRelocationDialogViewModel>();

            // If machine doesn't currently have cutter..
            if (HasCutter is false)
            {
                //--- Cancel cutter relocation process ---//

                // Error message
                cutterRelocationDialog.Message = $"[{MachineNumber}]  does not currently have any cutter to be relocated";

                // Show dialog
                await DialogService.InvokeFeedbackDialog(cutterRelocationDialog);

                // Do nothing else
                return;
            }

            cutterRelocationDialog.Id = Id;
            cutterRelocationDialog.Count = Count;
            cutterRelocationDialog.MachineNumber = MachineNumber;
            cutterRelocationDialog.PartNumber = PartNumber ?? "Part number unknown";
            cutterRelocationDialog.CutterNumber = CutterNumber ?? "Cutter number unknown";
            cutterRelocationDialog.Owner = Owner;
            
            // Load valid machines 
            await cutterRelocationDialog.ReloadMachines();

            // Load users
            await cutterRelocationDialog.ReloadUsers();

            // Show dialog
            DialogService.InvokeDialog(cutterRelocationDialog);
        }

        #endregion
    }
}