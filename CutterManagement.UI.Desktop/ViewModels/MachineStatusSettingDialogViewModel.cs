using CutterManagement.Core;
using CutterManagement.Core.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="StatusSettingDialog"/>
    /// </summary>
    public class MachineStatusSettingDialogViewModel : DialogViewModelBase, IDialogWindowCloseRequest
    {
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

        private Task _taskLoader;

        /// <summary>
        /// The unique id of this machine 
        /// </summary>
        public int Id { get; set; }

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

        public string CurrentUserName { get; set; }

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

        /// <summary>
        /// When user cancels or proceeds with setting machine status
        /// </summary>
        public event EventHandler<DialogWindowCloseRequestedEventArgs> DialogWindowCloseRequest;

        public ICommand UpdateStatusCommand { get; set; }
        public ICommand CancelCommand { get; set; }

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
            CancelCommand = new RelayCommand(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(false)));
            UpdateStatusCommand = new RelayCommand(UpdateMachineStatus);

        }

        private void UpdateMachineStatus()
        {
            MachineDataModel newData = new MachineDataModel
            {
                Status = _currentStatus,
                StatusMessage = MachineStatusMessage,
            };

            var machineUser = new MachineDataModelUserDataModel
            {
                UserDataModel = _user,
                UserDataModelId = _user.Id,
                MachineDataModel = newData,
                MachineDataModelId = Id,
            };
        }

        private async Task GetUsers()
        {
            IDataAccessService<UserDataModel> users = _dataFactory.GetDbTable<UserDataModel>();

            foreach (UserDataModel userData in await users.GetAllEntitiesAsync())
            {
                UsersCollection.Add(userData, userData.FirstName + " " + userData.LastName);
            }

            OnPropertyChanged(nameof(UsersCollection));
        }

        public void ClearDataResidue()
        {
            MachineStatusMessage = string.Empty;
        }
    }
}
