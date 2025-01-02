using CutterManagement.Core;
using System.Diagnostics;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="MachineConfigurationControl"/>
    /// </summary>
    public class MachineConfigurationViewModel : ViewModelBase, IDisposable
    {
        #region Private Fields

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
        private IMachineService _machineService;

        /// <summary>
        /// Data access service
        /// </summary>
        private IDataAccessServiceFactory _dataAccessService;

        /// <summary>
        /// The current status of the item to configure
        /// </summary>
        private object _currentStatus;

        /// <summary>
        /// Message to display about the configuration process result
        /// </summary>
        private string _message;

        #endregion

        #region Public Properties

        /// <summary>
        /// Label indicating current machine number
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// New machine number
        /// </summary>
        public string MachineNumber { get; set; }

        /// <summary>
        /// New machine set number
        /// </summary>
        public string MachineSetNumber { get; set; }

        /// <summary>
        /// New machine status message
        /// </summary>
        public string MachineStatusMessage { get; set; }

        /// <summary>
        /// True if message should be shown, otherwise false
        /// </summary>
        public bool ShowMessage { get; set; }

        /// <summary>
        /// True if configuration process ran successfully, otherwise false
        /// </summary>
        public bool IsConfigurationSuccessful { get; set; }

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
        /// Message to display about the configuration process result
        /// </summary>
        public string Message
        {
            get => _message;
            set => _message = value;
        }

        #endregion

        #region Public Commands

        /// <summary>
        /// Command to update a machine item with new data
        /// </summary>
        public ICommand UpdateCommand { get; set; }

        /// <summary>
        /// Command to cancel update operation
        /// </summary>
        public ICommand CancelCommand { get; set; }

        #endregion

        #region Constructor

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
            Label = _machineItemViewModel.MachineNumber;
            StatusCollection = new Dictionary<MachineStatus, string>();
            _machineService = new MachineService(_dataAccessService);

            foreach (MachineStatus status in Enum.GetValues<MachineStatus>())
            {
                // Add every status
                StatusCollection.Add(status, EnumHelpers.GetDescription(status));
            }

            // Create commands
            UpdateCommand = new RelayCommand(UpdateData);
            CancelCommand = new RelayCommand(() =>
            {
                _machineItemCollectionVM.IsConfigurationFormOpen = false;
                Dispose();
            });
        }

        #endregion

        #region Methods

        /// <summary>
        /// Update machine item with new data
        /// </summary>
        private void UpdateData()
        {
            // Create a new machine data model
            MachineDataModel newData = new MachineDataModel
            {
                // Set incoming data
                Id = _machineItemViewModel.Id,
                Owner = _machineItemViewModel.Owner,
                MachineNumber = MachineNumber,
                MachineSetId = MachineSetNumber,
                Status = (MachineStatus)CurrentStatus,
                StatusMessage = MachineStatusMessage,
            };

            // Configure machine with new data
            ConfigureMachine(newData);
        }

        /// <summary>
        /// Configures a machine item
        /// </summary>
        /// <param name="machineItem">The machine to configure</param>
        public void ConfigureMachine(MachineDataModel newData)
        {
            Task.Run(async () => 
            {
                try
                {
                    // Try configuring machine with new data, get the result of the process
                    ValidationResult result =  await _machineService.Configure(newData);

                    // Set message
                    _message = string.IsNullOrEmpty(result.ErrorMessage) ? "Configuration successful" : result.ErrorMessage;

                    // If process is successful 
                    if (result.IsValid)
                    {
                        // Set success flag
                        IsConfigurationSuccessful = true;
                    }

                    // Show message
                    ShowMessage = true;
                    // Update UI with the current message
                    OnPropertyChanged(nameof(Message));

                    // Wait for 2 seconds
                    await Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((action) =>
                    {
                        // If process is successful
                        if (result.IsValid)
                        {
                            // Close configuration form
                            _machineItemCollectionVM.IsConfigurationFormOpen = false;

                            // Close message
                            ShowMessage = false;

                            Dispose();
                        }
                        // Otherwise
                        else
                        {
                            // Set success flag
                            IsConfigurationSuccessful = false;

                            // Close message
                            ShowMessage = false;
                        }
                    });
                }
                catch (Exception ex)
                {
                    Debugger.Break();
                    Debug.WriteLine(ex.Message);
                }
            });
        }

        public void Dispose()
        {
            Label = string.Empty;
            MachineNumber = string.Empty;
            MachineStatusMessage = string.Empty;
        }

        #endregion
    }
}
