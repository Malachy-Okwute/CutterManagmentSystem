using CutterManagement.Core;
using CutterManagement.Core.Services;
using System.Diagnostics;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="MachineConfigurationDialogControl"/>
    /// </summary>
    public class MachineConfigurationDialogViewModel : DialogViewModelBase, IDialogWindowCloseRequest, ISubscribeToMessages
    {
        #region Private Fields

        /// <summary>
        /// Machine service
        /// </summary>
        private IMachineService _machineService;

        /// <summary>
        /// The current status of the item to configure
        /// </summary>
        private MachineStatus _currentStatus;

        /// <summary>
        /// Message to display about the configuration process result
        /// </summary>
        private string _message;

        #endregion

        #region Public Properties

        public int Id { get; set; }

        /// <summary>
        /// Label indicating current machine number
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Owner of machine 
        /// </summary>
        public Department Owner { get; set; }

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
        public MachineStatus CurrentStatus 
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

        #region Public Events

        /// <summary>
        /// When user cancels or proceeds with configuring machine
        /// </summary>
        public event EventHandler<DialogWindowCloseRequestedEventArgs> DialogWindowCloseRequest;

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
        public MachineConfigurationDialogViewModel(IMachineService machineService)
        {
            Title = "Configuration";
            _machineService = machineService;
            CurrentStatus = MachineStatus.None;
            StatusCollection = new Dictionary<MachineStatus, string>();

            foreach (MachineStatus status in Enum.GetValues<MachineStatus>())
            {
                // Add every status
                StatusCollection.Add(status, EnumHelpers.GetDescription(status));
            }

            // Create commands
            UpdateCommand = new RelayCommand(async () => await UpdateData());
            CancelCommand = new RelayCommand(() =>
            {
                DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsConfigurationSuccessful));
                ClearDataResidue();
            });

            // Register this class to receive messages from messenger
            Messenger.MessageSender.RegisterMessenger(this);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Update machine item with new data
        /// </summary>
        private async Task UpdateData()
        {
            // Create a new machine data model with data we care about sending to db
            MachineDataModel newData = new MachineDataModel
            {
                // Set incoming data
                Id = Id,
                Owner = Owner,
                MachineNumber = MachineNumber,
                MachineSetId = MachineSetNumber,
                Status = _currentStatus,
                StatusMessage = MachineStatusMessage.Trim(),
                IsConfigured = true,
            };

            // Configure machine with new data
            await ConfigureMachine(newData);

            // If configuration is successful
            if (IsConfigurationSuccessful)
            {
                // Send dialog window close request
                DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsConfigurationSuccessful));
            }

        }

        #endregion

        #region Methods

        /// <summary>
        /// Configures a machine item
        /// </summary>
        /// <param name="machineItem">The machine to configure</param>
        public async Task ConfigureMachine(MachineDataModel newData)
        {
            try
            {
                // Try configuring machine with new data, get the result of the process
                (ValidationResult, MachineDataModel?) result =  await _machineService.Configure(newData);

                // Set message
                _message = string.IsNullOrEmpty(result.Item1.ErrorMessage) ? "Configuration successful" : result.Item1.ErrorMessage;

                // If process is successful...
                if (result.Item1.IsValid && result.Item2 is not null)
                {
                    // Set configuration success here to update UI faster.
                    // NOTE: This is used in binding to change background / foreground color in UI
                    //
                    // Mark configuration as successful
                    IsConfigurationSuccessful = true; 

                    // Send out message
                    Messenger.MessageSender.SendMessage(result.Item2);
                }

                // Update UI with the current message
                OnPropertyChanged(nameof(Message));

                // Show message
                ShowMessage = true;

                // Wait for 2 seconds
                await Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((action) =>
                {
                    // If process is successful...
                    if (result.Item1.IsValid)
                    {
                        // Close message
                        ShowMessage = false;

                        // Clear data
                        ClearDataResidue();
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
        }

        public void ClearDataResidue()
        {
            Label = string.Empty;
            MachineNumber = string.Empty;
            MachineStatusMessage = string.Empty;
        }

        public void ReceiveMessage(IMessage message)
        {
            // Empty
        }

        #endregion
    }
}
