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

        /// <summary>
        /// True if pop up should show in the view 
        /// otherwise false
        /// </summary>
        public bool IsPopupOpen { get; set; }

        /// <summary>
        /// The kind of command to run
        /// </summary>
        public CommandKind CommandKind { get; set; }

        #endregion

        #region Public Events

        /// <summary>
        /// Event that gets fired when this item gets clicked on
        /// </summary>
        public event EventHandler<CommandKind> ItemSelected;

        #endregion

        #region Public Commands

        /// <summary>
        /// Command to open pop up control 
        /// <para>
        /// <see cref="MachineOptionsPopupControl"/>
        /// </para>
        /// </summary>
        public ICommand OpenPopupCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MachineItemViewModel()
        {
            // Create commands
            OpenPopupCommand = new RelayCommand(() => ItemSelected?.Invoke(this, CommandKind.PopCommand));
        }

        #endregion        
    }
}
