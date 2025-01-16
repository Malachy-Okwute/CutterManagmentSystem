using CutterManagement.Core;
using CutterManagement.Core.Services;
using SQLitePCL;
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

        /// <summary>
        /// Machine data
        /// </summary>
        private MachineDataModel _machineDataModel;

        #endregion

        #region Public Properties

        /// <summary>
        /// Machine data
        /// </summary>
        public MachineDataModel MachineDataModel
        {
            get => _machineDataModel;
            set => _machineDataModel = value;
        }

        /// <summary>
        /// The unique Id of this machine
        /// </summary>
        public int Id
        {
            get => _machineDataModel.Id;
            set => _machineDataModel.Id = value;
        }

        /// <summary>
        /// Unique number assigned to this machine
        /// </summary>
        public string MachineNumber
        {
            get => _machineDataModel.MachineNumber;
            set => _machineDataModel.MachineNumber = value;
        }

        /// <summary>
        /// Unique set number assigned to this machine
        /// </summary>
        public string MachineSetNumber
        {
            get => _machineDataModel.MachineSetId;
            set => _machineDataModel.MachineSetId = value;
        }

        /// <summary>
        /// The current status of this machine 
        /// </summary>
        public MachineStatus Status
        {
            get => _machineDataModel.Status;
            set => _machineDataModel.Status = value;
        }

        /// <summary>
        /// Comment related to the status of this machine
        /// </summary>
        public string StatusMessage
        {
            get => _machineDataModel.StatusMessage;
            set => _machineDataModel.StatusMessage = value;
        }

        /// <summary>
        /// The owner of this machine
        /// </summary>
        public Department Owner
        {
            get => _machineDataModel.Owner;
            set => _machineDataModel.Owner = value;
        }

        /// <summary>
        /// Result of the last part checked 
        /// <para>SETUP | PASSED | FAILED</para>
        /// </summary>
        public string FrequencyCheckResult
        {
            get => _machineDataModel.FrequencyCheckResult.ToString();
            set
            {
                if (Enum.TryParse(value, out FrequencyCheckResult result))
                {
                    _machineDataModel.FrequencyCheckResult = result;
                    FrequencyCheckResult = value;
                }
                else
                {
                    throw new InvalidOperationException("Invalid result, notify developer");
                }
            }
        }

        /// <summary>
        /// Date and time of the last checked part on this machine
        /// </summary>
        public string DateTimeLastModified 
        {
            get => _machineDataModel.DateTimeLastModified.ToString("MM-dd-yyyy ~ hh:mm tt"); 
            set 
            { 
                if (DateTime.TryParse(value, out DateTime result))
                {
                    _machineDataModel.DateTimeLastModified = result;
                    DateTimeLastModified = value; 
                } 
                else
                {
                    throw new InvalidOperationException("Invalid date, notify developer");
                }
            }
        }


        /// <summary>
        /// Part unique id number running on this machine
        /// </summary>
        public string? PartNumber { get; set; }

        /// <summary>
        /// Cutter id number currently setup on this machine
        /// </summary>
        public string? CutterNumber { get; set; }

        /// <summary>
        /// Number of parts produced by this machine with the current cutter
        /// </summary>
        public string? Count { get; set; }

        /// <summary>
        /// True if this machine is running, false if it's sitting idle or down for maintenance
        /// </summary>
        public bool IsRunning { get; set; }

        /// <summary>
        /// True if pop up should show in the view 
        /// otherwise false
        /// </summary>
        public bool IsPopupOpen { get; set; }

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
        }

        #endregion

        private void OpenStatusSettingDialog()
        {
            ItemSelected?.Invoke(this, EventArgs.Empty);
            var statusSettingVM = new MachineStatusSettingDialogViewModel(_dataFactory, new MachineService(_dataFactory))
            {
                Id = Id,
                Owner = Owner,
                Label = MachineNumber,
                MachineNumber = MachineNumber,
                MachineSetNumber = MachineSetNumber
            };

            DialogService.InvokeDialog(statusSettingVM);
        }

        /// <summary>
        /// Opens machine configuration dialog
        /// </summary>
        private void OpenMachineConfigurationDialog()
        {
            ItemSelected?.Invoke(this, EventArgs.Empty);
            var machineConfiguration = new MachineConfigurationDialogViewModel(new MachineService(_dataFactory))
            {
                MachineDataModel = _machineDataModel
            };

            // Show dialog
            DialogService.InvokeDialog(machineConfiguration);
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
