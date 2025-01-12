using CutterManagement.Core;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="SetStatusControl"/>
    /// </summary>
    public class MachineSetStatusDialogViewModel : ViewModelBase
    {
        /// <summary>
        /// The current status of the item to configure
        /// </summary>
        private object _currentStatus;

        /// <summary>
        /// Message to display about the configuration process result
        /// </summary>
        private string _message;

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

        public MachineSetStatusDialogViewModel()
        {
            // Initialize
            StatusCollection = new Dictionary<MachineStatus, string>();

            foreach (MachineStatus status in Enum.GetValues<MachineStatus>())
            {
                // Add every status
                StatusCollection.Add(status, EnumHelpers.GetDescription(status));
            }
        }
         
        public void ClearDataResidue()
        {
            MachineStatusMessage = string.Empty;
        }
    }
}
