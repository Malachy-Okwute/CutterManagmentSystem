using CutterManagement.Core;
using System.Windows;
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
        private IDataAccessServiceFactory _dataFactory;

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
        public string? Count { get; set; }

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

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MachineItemViewModel(IDataAccessServiceFactory? dataFactory)
        {
            if(dataFactory is not null)
            {
                _dataFactory = dataFactory;
            }

            // Create commands
            OpenPopupCommand = new RelayCommand(OpenPopup);
            OpenStatusSettingDialogCommand = new RelayCommand(OpenStatusSettingDialog);
            OpenMachineConfigurationDialogCommand = new RelayCommand(OpenMachineConfigurationDialog);
            OpenMachineDialogCommand = new RelayCommand(OpenMachineDialog);
        }

        #endregion

        #region Command Method

        /// <summary>
        /// Opens a dialog to set machine status
        /// </summary>
        private void OpenStatusSettingDialog()
        {
            ItemSelected?.Invoke(this, EventArgs.Empty);

            // Machine status view model
            var statusSettingVM = new MachineStatusSettingDialogViewModel(_dataFactory, new MachineStatusSettingService(_dataFactory))
            {
                Id = Id,
                Owner = Owner,
                Label = MachineNumber,
                MachineNumber = MachineNumber,
                MachineSetNumber = MachineSetNumber,
                IsConfigured = IsConfigured,
            };

            // Show dialog
            DialogService.InvokeDialog(statusSettingVM);
        }

        /// <summary>
        /// Opens a dialog to configure machine
        /// </summary>
        private void OpenMachineConfigurationDialog()
        {
            ItemSelected?.Invoke(this, EventArgs.Empty);

            // Machine configuration view model
            var machineConfiguration = new MachineConfigurationDialogViewModel(new MachineConfigurationService(_dataFactory))
            {
                Id = Id,
                Owner = Owner,
                Label = MachineNumber,
            };

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
        /// <para>Setup dialog | Frequency-check dialog</para>
        /// </summary>
        private void OpenMachineDialog()
        {
            // Broadcast that this item was selected
            ItemSelected?.Invoke(this, EventArgs.Empty);

            // Setup mode
            FrequencyCheckResult setupMode = Core.FrequencyCheckResult.Setup;

            // If machine is in setup mode
            if(FrequencyCheckResult == setupMode.ToString())
            {
                var setupDialog = new MachineSetupDialogViewModel(_dataFactory)
                {
                    MachineNumber = MachineNumber
                };

                // Invoke setup dialog
                DialogService.InvokeDialog(setupDialog);
            }
            // Otherwise
            else
            {
                var frequencyCheck = new FrequencyCheckDialogViewModel(_dataFactory)
                {
                    Id = Id,
                    PartNumber = "12345678",
                    MachineNumber = "123",
                    PartCount = "50",
                    PartSize = "10",
                    FrequencyCheckResult = "Pass"
                };

                // Invoke frequency check dialog
                DialogService.InvokeDialog(frequencyCheck);
            }
        }

        #endregion
    }
}