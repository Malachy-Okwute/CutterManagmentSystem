using CutterManagement.Core;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="MachineConfigurationControl"/>
    /// </summary>
    public class MachineConfigurationViewModel : ViewModelBase
    {
        /// <summary>
        /// Machine item
        /// </summary>
        private MachineItemViewModel _machineItemViewModel;

        /// <summary>
        /// Collection of machine items
        /// </summary>
        private MachineItemCollectionViewModel _machineItemCollectionVM;

        /// <summary>
        /// Machine configuration service
        /// </summary>
        private IMachineConfigurationService _machineConfiguration;

        /// <summary>
        /// Data access service
        /// </summary>
        private IDataAccessServiceFactory _dataAccessService;

        /// <summary>
        /// The current status of the item to configure
        /// </summary>
        private object _currentStatus;

        /// <summary>
        /// New machine number
        /// </summary>
        public string MachineNumber { get; set; }

        /// <summary>
        /// New machine set number
        /// </summary>
        public string MachineSetNumber { get; set; }

        /// <summary>
        /// Collection of status options available
        /// </summary>
        public Dictionary<MachineStatus, string> StatusCollection { get; set; }

        /// <summary>
        /// The current status of the item to configure
        /// </summary>
        public object CurrentStatus 
        {
            get => _currentStatus;
            set => _currentStatus = value;
        }

        /// <summary>
        /// New machine status message
        /// </summary>
        public string MachineStatusMessage { get; set; }

        /// <summary>
        /// Command to update a machine item with new data
        /// </summary>
        public ICommand UpdateCommand { get; set; }

        /// <summary>
        /// Command to cancel update operation
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="machineItemViewModel">The machine item to update</param>
        /// <param name="dataAccessService">Access to database tables</param>
        /// <param name="machineItemCollectionVM">Origin of machine item</param>
        public MachineConfigurationViewModel(MachineItemViewModel machineItemViewModel, IDataAccessServiceFactory dataAccessService, MachineItemCollectionViewModel machineItemCollectionVM)
        {
            // Initialize
            _machineItemViewModel = machineItemViewModel;
            _machineItemCollectionVM = machineItemCollectionVM;
            _dataAccessService = dataAccessService;
            CurrentStatus = _machineItemViewModel.Status;
            StatusCollection = new Dictionary<MachineStatus, string>();
            _machineConfiguration = new MachineConfigurationService(_dataAccessService);

            foreach (MachineStatus status in Enum.GetValues<MachineStatus>())
            {
                // Add every status
                StatusCollection.Add(status, EnumHelpers.GetDescription(status));
            }

            // Create commands
            UpdateCommand = new RelayCommand(UpdateData);
            CancelCommand = new RelayCommand(() => _machineItemCollectionVM.IsConfigurationFormOpen = false);
        }

        /// <summary>
        /// Update machine item with new data
        /// </summary>
        private void UpdateData()
        {
            // Set new data
            _machineItemViewModel.MachineNumber = MachineNumber;
            _machineItemViewModel.MachineSetNumber = MachineSetNumber;
            _machineItemViewModel.Status = (MachineStatus)CurrentStatus;
            _machineItemViewModel.StatusMessage = MachineStatusMessage;

            // Configure machine with new data
            ConfigureMachine(_machineItemViewModel);

            // Close configuration form
            _machineItemCollectionVM.IsConfigurationFormOpen = false;
        }

        /// <summary>
        /// Configures a machine item
        /// </summary>
        /// <param name="machineItem">The machine to configure</param>
        public void ConfigureMachine(MachineItemViewModel machineItem) => Task.Run(async () => { await _machineConfiguration.Configure(machineItem); });
        
    }
}
