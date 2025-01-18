using CutterManagement.Core;
using CutterManagement.Core.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
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
        /// Data factory
        /// </summary>
        private IDataAccessServiceFactory _dataFactory;

        /// <summary>
        /// The current status of the item to configure
        /// </summary>
        private MachineStatus _currentStatus;

        /// <summary>
        /// User that is setting this machine status
        /// </summary>
        private UserDataModel _user;

        /// <summary>
        /// Message to display about the configuration process result
        /// </summary>
        private string _message;

        /// <summary>
        /// Task loader
        /// </summary>
        private Task _taskLoader;

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
        public string MachineStatusMessage { get; set; }

        /// <summary>
        /// True if message is to be displayed to user
        /// Otherwise false
        /// </summary>
        public bool ShowMessage { get; set; }

        /// <summary>
        /// True if attempt to set machine status succeeded
        /// </summary>
        public bool IsStatusSettingSuccessful { get; set; }

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

        /// <summary>
        /// Message to display about the configuration process result
        /// </summary>
        public string Message
        {
            get => _message;
            set => _message = value;
        }

        #endregion

        #region Events

        /// <summary>
        /// When user cancels or proceeds with setting machine status
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
        public MachineStatusSettingDialogViewModel(IDataAccessServiceFactory dataFactory, IMachineService machineService)
        {
            Title = "Set status";
            _machineService = machineService;
            _dataFactory = dataFactory;
            CurrentStatus = MachineStatus.None;
            StatusCollection = new Dictionary<MachineStatus, string>();
            UsersCollection = new Dictionary<UserDataModel, string>();

            _taskLoader = GetUsers();

            foreach (MachineStatus status in Enum.GetValues<MachineStatus>())
            {
                // Add every status
                StatusCollection.Add(status, EnumHelpers.GetDescription(status));
            }

            // Create commands
            CancelCommand = new RelayCommand(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsStatusSettingSuccessful)));
            UpdateStatusCommand = new RelayCommand(UpdateMachineStatus);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Updates machine status
        /// </summary>
        private async void UpdateMachineStatus()
        {
            MachineDataModel newData = new MachineDataModel
            {
                Id = Id,
                Owner = Owner,
                Status = _currentStatus,
                MachineNumber = MachineNumber,
                MachineSetId = MachineSetNumber,
                StatusMessage = MachineStatusMessage,
            };

            await SetMachineStatus(newData);

            // If configuration is successful
            if (IsStatusSettingSuccessful)
            {
                // Send dialog window close request
                DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsStatusSettingSuccessful));
            }

        }

        #endregion

        public async Task SetMachineStatus(MachineDataModel data)
        {
            MachineDataModel? machineDataModel = null;

            try
            {
                ValidationResult result = await _machineService.SetStatus(data, _user.Id, (data) =>
                {
                    machineDataModel = data;
                });

                // Set message
                _message = string.IsNullOrEmpty(result.ErrorMessage) ? "Machine status successfully set " : result.ErrorMessage;

                if(result.IsValid)
                {
                    IsStatusSettingSuccessful = true;
                    Messenger.MessageSender.SendMessage(machineDataModel ?? throw new ArgumentNullException($"{machineDataModel} is null"));
                }

                OnPropertyChanged(nameof(Message));

                ShowMessage = true;

                // Wait for 2 seconds
                await Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith((action) =>
                {
                    // If process is successful...
                    if (result.IsValid)
                    {
                        // Close message
                        ShowMessage = false;
                    }
                    // Otherwise
                    else
                    {
                        // Reset success flag
                        IsStatusSettingSuccessful = false;

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

        #region Methods

        /// <summary>
        /// Load users
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        private async Task GetUsers()
        {
            // Get user db table
            IDataAccessService<UserDataModel> users = _dataFactory.GetDbTable<UserDataModel>();

            foreach (UserDataModel userData in await users.GetAllEntitiesAsync())
            {
                // Do not load admin user
                if(userData.LastName is "admin")
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

        #endregion

        #region Messages

        public void ReceiveMessage(IMessage message)
        {
            // Empty
        }

        #endregion

    }
}

