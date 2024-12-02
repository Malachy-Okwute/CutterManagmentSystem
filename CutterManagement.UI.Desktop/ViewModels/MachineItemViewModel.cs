using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="MachineItemControl"/>
    /// </summary>
    public class MachineItemViewModel : ViewModelBase
    {
        #region Private Fields

        #endregion

        #region Public Properties

        /// <summary>
        /// Unique ID of this machine
        /// </summary>
        public string MachineNumber { get; set; }

        /// <summary>
        /// Unique set ID of this machine
        /// </summary>
        public string MachineSetNumber { get; set; }

        /// <summary>
        /// Cutter id number currently setup on this machine
        /// </summary>
        public string CurrentCutterNumber { get; set; }

        /// <summary>
        /// True if this machine is running, false if it's sitting idle or down for maintenance
        /// </summary>
        public bool MachineStatus { get; set; }

        /// <summary>
        /// The current status of this machine 
        /// </summary>
        //public MachineStatus MachineStatusDetails { get; set; }

        /// <summary>
        /// Comment related to the status of this machine
        /// </summary>
        public string? MachineStatusComment { get; set; } 

        /// <summary>
        /// Part unique id number running on this machine
        /// </summary>
        public string CurrentRunningPartNumber { get; set; }

        /// <summary>
        /// Number of parts produced by this machine with the current cutter
        /// </summary>
        public string ProducedPartCount { get; set; }

        /// <summary>
        /// Result of the last part checked 
        /// <remark>PASSED | FAILED</remark>
        /// </summary>
        public string ResultOfLastPartChecked { get; set; }

        /// <summary>
        /// Date and time of the last checked part on this machine
        /// </summary>
        public string DateAndTimeOfLastCheck { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// Command to run when this item is selected
        /// </summary>
        //public ICommand ItemSelectedCommand { get; set; }

        /// <summary>
        /// Command to run when this item's edit button is clicked on
        /// </summary>
        //public ICommand EditItemCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MachineItemViewModel()
        {
            //ItemSelectedCommand = new RelayCommand(SelectItem, (d) =>true);
            //EditItemCommand  = new RelayCommand();
        }

        #endregion
    }
}
