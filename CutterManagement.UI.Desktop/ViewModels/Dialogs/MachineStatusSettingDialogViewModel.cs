using CutterManagement.Core;
using CutterManagement.Core.Services;
using System.Diagnostics;
using System.Net.Http;
using System.Windows.Data;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="StatusSettingDialog"/>
    /// </summary>
    public class MachineStatusSettingDialogViewModel : DialogViewModelBase, IDialogWindowCloseRequest, ISubscribeToMessages
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
        /// New machine status message
        /// </summary>
        private string _machineStatusMessage;

        /// <summary>
        /// User that is setting this machine status
        /// </summary>
        private UserDataModel _user;

        #endregion

        #region Properties

        /// <summary>
        /// The unique id of this machine 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Machine number
        /// </summary>
        public string MachineNumber { get; set; }

        /// <summary>
        /// Machine Set number
        /// </summary>
        public string MachineSetNumber { get; set; }

        /// <summary>
        /// The owner of this machine
        /// </summary>
        public Department Owner { get; set; }

        /// <summary>
        /// Label indicating current machine number
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// New machine status message
        /// </summary>
        public string MachineStatusMessage
        {
            get => _machineStatusMessage;
            set => _machineStatusMessage = value;
        }

        /// <summary>
        /// True if this machine item is configured, 
        /// Otherwise false
        /// </summary>
        public bool IsConfigured { get; set; }

        /// <summary>
        /// Collection of status options available
        /// </summary>
        public Dictionary<MachineStatus, string> StatusCollection { get; set; }

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

        /// <summary>
        /// The current status of the item to configure
        /// </summary>
        public MachineStatus CurrentStatus
        {
            get => _currentStatus;
            set => _currentStatus = value;
        }

        #endregion

        #region Events

        /// <summary>
        /// Event that is invoked when user cancels or proceeds with setting machine status
        /// </summary>
        public event EventHandler<DialogWindowCloseRequestedEventArgs> DialogWindowCloseRequest;

        #endregion

        #region Commands

        /// <summary>
        /// Command to update machine status
        /// </summary>
        public ICommand UpdateStatusCommand { get; set; }

        /// <summary>
        /// Command to cancel setting machine status
        /// </summary>
        public ICommand CancelCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="machineService"></param>
        /// <param name="dataFactory"></param>
        public MachineStatusSettingDialogViewModel(IMachineService machineService)
        {
            Title = "Set status";
            _machineService = machineService;
            CurrentStatus = MachineStatus.None;
            StatusCollection = new Dictionary<MachineStatus, string>();
            UsersCollection = new Dictionary<UserDataModel, string>();

            _ = GetUsers();

            foreach (MachineStatus status in Enum.GetValues<MachineStatus>())
            {
                // Add every status
                StatusCollection.Add(status, EnumHelpers.GetDescription(status));
            }

            // Create commands
            CancelCommand = new RelayCommand(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess)));
            UpdateStatusCommand = new RelayCommand(UpdateMachineStatus);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Updates machine status
        /// </summary>
        private async void UpdateMachineStatus()
        {
            // Make sure machine is configured
            if (IsConfigured is false)
            {
                // Show feed back message
                await DialogService.InvokeFeedbackDialog(this, "SelectedMachine need to be configured first by admin");

                // Do nothing else
                return;
            }

            // Make sure we have a user
            if (_user is null)
            {
                // Show feed back message
                await DialogService.InvokeFeedbackDialog(this, "Please add a user to continue");

                // Do nothing else
                return;
            }

            // If we have message...
            if (string.IsNullOrWhiteSpace(_machineStatusMessage) is false)
            {
                // Trim message
                _machineStatusMessage = _machineStatusMessage.Trim(Environment.NewLine.ToCharArray());
            }

            // Create new machine model
            MachineDataModel newData = new MachineDataModel
            {
                Id = Id,
                Owner = Owner,
                Status = _currentStatus,
                MachineNumber = MachineNumber,
                MachineSetId = MachineSetNumber,
                StatusMessage = _machineStatusMessage,
            };

            // Attempt to set machine status
            await SetMachineStatus(newData);

            // If configuration is successful
            if (IsSuccess)
            {
                // Send dialog window close request
                DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets status <see cref="MachineStatus"/> to the specified machine item
        /// </summary>
        /// <param name="data">The machine to set it's status</param>
        /// <returns><see cref="Task"/></returns>
        /// <exception cref="ArgumentNullException">
        /// Exception that gets thrown if attempt to set status fails
        /// </exception>
        public async Task SetMachineStatus(MachineDataModel data)
        {
            MachineDataModel? machineDataModel = null;

            try
            {
                // Try setting status and also grab result coming from the process
                ValidationResult result = await _machineService.SetStatusAsync(data, _user.Id, (callbackData) =>
                {
                    machineDataModel = callbackData;
                });

                // Set message
                string message = string.IsNullOrEmpty(result.ErrorMessage) ? $"{data.MachineNumber} status successfully set " : result.ErrorMessage;

                // Set flag
                IsSuccess = result.IsValid;

                // Update UI message
                //OnPropertyChanged(nameof(Message));

                // If we succeed
                if (result.IsValid)
                {
                    // Send out message to subscriber that needs to know about the data that changed
                    Messenger.MessageSender.SendMessage(machineDataModel ?? throw new ArgumentNullException($"{machineDataModel} is null"));

                    // Show success message
                    await DialogService.InvokeAlertDialog(this, message);
                }
                else
                {
                    await DialogService.InvokeFeedbackDialog(this, message);
                }


            }
            catch (Exception ex)
            {
                Debugger.Break();
                Debug.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// Load users
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        private async Task GetUsers()
        {
            HttpClient client = _machineService.HttpClientFactory.CreateClient();
            client.BaseAddress = new Uri($"https://localhost:7057");

            var userCollection = await ServerRequest.GetDataCollection<UserDataModel>(client, $"UserDataModel");

            userCollection?.ForEach(user => 
            {
                // Do not load admin user
                if (user.LastName is "admin")
                    return;

                UsersCollection.Add(user, user.FirstName.PadRight(10) + user.LastName);

            });

            // Set current user
            _user = UsersCollection.FirstOrDefault().Key;

            // Update UI
            OnPropertyChanged(nameof(User));

            // Refresh UI
            CollectionViewSource.GetDefaultView(UsersCollection).Refresh();
        }

        #endregion

        #region Messages

        public void ReceiveMessage(IMessage message)
        {
            // Empty
        }

        #endregion
    }
}

