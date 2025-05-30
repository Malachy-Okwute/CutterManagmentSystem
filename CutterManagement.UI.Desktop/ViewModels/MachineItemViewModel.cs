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
        /// Data access factory
        /// </summary>
        //private IDataAccessServiceFactory _dataFactory;

        /// <summary>
        /// Provides services to machine
        /// </summary>
        private readonly IMachineService _machineService;

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

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MachineItemViewModel(IMachineService machineService)
        {
            _machineService = machineService;

            // Create commands
            OpenPopupCommand = new RelayCommand(OpenPopup);
            OpenStatusSettingDialogCommand = new RelayCommand(async () => await OpenStatusSettingDialog());
            OpenMachineConfigurationDialogCommand = new RelayCommand(OpenMachineConfigurationDialog);
            OpenMachineDialogCommand = new RelayCommand(async () => await OpenMachineDialog());
            OpenCutterRemovalDialogCommand = new RelayCommand(async () => await OpenCutterRemovalDialog());
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
            var statusSettingVM = _machineService.GetDialogViewModel<MachineStatusSettingDialogViewModel>();

            statusSettingVM.Id = Id;
            statusSettingVM.Owner = Owner;
            statusSettingVM.Label = MachineNumber;
            statusSettingVM.MachineNumber = MachineNumber;
            statusSettingVM.MachineSetNumber = MachineSetNumber;
            statusSettingVM.IsConfigured = IsConfigured;

            // Make sure machine is configured
            if (IsConfigured is false)
            {
                // Define a message
                statusSettingVM.Message = "Machine need to be configured for production";

                // Show feed back message
                await DialogService.InvokeFeedbackDialog(statusSettingVM);

                // Do nothing else
                return;
            }

            // Show dialog
            DialogService.InvokeDialog(statusSettingVM);
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

                setupDialog.GetMachineItem(this);

                // Make sure machine is configured
                if (IsConfigured is false)
                {
                    // Define a message
                    setupDialog.Message = "Machine need to be configured for production";

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
                var cmmCheck = _machineService.GetDialogViewModel<CMMCheckDialogViewModel>();

                cmmCheck.Id = Id;
                cmmCheck.CurrentCount = int.Parse(Count) == 0 ? "Count" : Count;

                DialogService.InvokeDialog(cmmCheck);
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
        /// Opens cutter removal dialog
        /// </summary>
        private async Task OpenCutterRemovalDialog()
        {
            // Broadcast that this item was selected
            ItemSelected?.Invoke(this, EventArgs.Empty);

            var cutterRemoval = _machineService.GetDialogViewModel<CutterRemovalDialogViewModel>();

            cutterRemoval.Id = Id;
            cutterRemoval.PartNumber = PartNumber ?? "Part number unknown";
            cutterRemoval.CutterNumber = CutterNumber ?? "Cutter number unknown";
            cutterRemoval.PreviousPartCount = string.Format("Count: {0}", Count);
            cutterRemoval.MachineNumber = MachineNumber;

            // If machine doesn't currently have cutter..
            if(HasCutter is false)
            {
                //--- Cancel cutter removal process ---//

                // Error message
                cutterRemoval.Message = $"[{MachineNumber}]    does not currently have any cutter";

                // Show dialog
                await DialogService.InvokeFeedbackDialog(cutterRemoval);

                // Do nothing else
                return;
            }

            DialogService.InvokeDialog(cutterRemoval);
        }

        #endregion
    }
}