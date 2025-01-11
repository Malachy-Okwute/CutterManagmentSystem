using CutterManagement.Core;
using CutterManagement.Core.Services;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="MachineItemControl"/>
    /// </summary>
    public class MachineItemViewModel : ViewModelBase
    {
        private IDialogService _dialogService;
        private IDataAccessServiceFactory _dataFactory;
        private MachineConfigurationDialogViewModel _machineConfiguration;

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
        /// Command to open machine configuration form
        /// </summary>
        public ICommand OpenMachineConfigurationFormCommand { get; set; }

        /// <summary>
        /// Command to open machine set status form
        /// </summary>
        public ICommand OpenSetStatusFormCommand { get; set; }

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
            OpenMachineConfigurationFormCommand = new RelayCommand(OpenMachineConfigurationForm);
        }

        #endregion

        private void OpenMachineConfigurationForm()
        {
            ItemSelected?.Invoke(this, EventArgs.Empty);
            _machineConfiguration = new MachineConfigurationDialogViewModel(new MachineService(_dataFactory))
            {
                Id = Id,
                Owner = Owner,
                Label = MachineNumber,
                CurrentStatus = Status
            };

            DialogService.InvokeDialog(_machineConfiguration);
        }

        /// <summary>
        /// Opens the popup control
        /// </summary>
        private void OpenPopup()
        {
            // Broadcast item selected event
            ItemSelected?.Invoke(this, EventArgs.Empty);

            // Set this item as the selected item
            IsPopupOpen ^= true;
        }

    }
}
