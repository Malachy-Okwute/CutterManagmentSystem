using CutterManagement.Core;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="MachineItemControl"/>
    /// </summary>
    public class MachineItemViewModel : ViewModelBase
    {
        #region Public Properties

        /// <summary>
        /// Unique ID of this machine
        /// </summary>
        public string MachineNumber { get; set; }

        /// <summary>
        /// Unique set ID of this machine
        /// </summary>
        public string MachineSetId { get; set; } 

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
        public string? StatusMessage { get; set; } 

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

        public bool IsPopupOpen { get; set; }

        #endregion

        public event EventHandler ItemSelected;

        public ICommand OpenPopupCommand { get; set; }

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MachineItemViewModel()
        {
            OpenPopupCommand = new RelayCommand(() => OpenPopup(this));
        }

        #endregion
        private void OpenPopup(MachineItemViewModel machineItem)
        {
            ItemSelected?.Invoke(machineItem, EventArgs.Empty);
        }

    }
}
