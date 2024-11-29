using System.Windows.Input;

namespace CMS
{
    /// <summary>
    /// View model for <see cref="MachineItemControl"/>
    /// </summary>
    public class MachineItemViewModel : ViewModelBase
    {
        #region Private Fields

        /// <summary>
        /// Data
        /// </summary>
        private MachineDataModel _machine;

        #endregion

        #region Public Properties

        /// <summary>
        /// Unique ID of this machine
        /// </summary>
        public string MachineNumber 
        {
            get => _machine.Id;
            set => _machine.Id = value;
        }

        /// <summary>
        /// Unique set ID of this machine
        /// </summary>
        public string MachineSetNumber 
        {
            get => _machine.SetID;
            set => _machine.SetID = value; 
        } 

        /// <summary>
        /// Cutter id number currently setup on this machine
        /// </summary>
        public string CurrentCutterNumber
        {
            get => _machine.Cutter.Id;
            set 
            {
                if (_machine.Cutter is not null)
                _machine.Cutter.Id = value ?? string.Empty;
            }
        }


        /// <summary>
        /// True if this machine is running, false if it's sitting idle or down for maintenance
        /// </summary>
        public bool MachineStatus => _machine.Status == CMS.MachineStatus.IsRunning;

        /// <summary>
        /// The current status of this machine 
        /// </summary>
        public MachineStatus MachineStatusDetails
        {
            get => _machine.Status;
            set => _machine.Status = value;
        }

        /// <summary>
        /// Comment related to the status of this machine
        /// </summary>
        public string MachineStatusComment { get; set; } 

        /// <summary>
        /// Part unique id number running on this machine
        /// </summary>
        public string CurrentRunningPartNumber
        {
            get => _machine.RunningPart?.Id;
            set
            { 
                if(_machine.RunningPart is not null)
                    _machine.RunningPart.Id = value ?? string.Empty;
            } 
        }


        /// <summary>
        /// Number of parts produced by this machine with the current cutter
        /// </summary>
        public string ProducedPartCount
        {
            get => _machine.Cutter?.Count;
            set
            {
                if (_machine.Cutter is not null)
                    _machine.Cutter.Count = value ?? string.Empty;
            }
        }

        /// <summary>
        /// Result of the last part checked 
        /// <remark>PASSED | FAILED</remark>
        /// </summary>
        public string ResultOfLastPartChecked => _machine.FrequencyCheckResult.ToString();

        /// <summary>
        /// Date and time of the last checked part on this machine
        /// </summary>
        public string DateAndTimeOfLastCheck => _machine.DateTime.ToString("d-MM-yyyy ~ t");

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
        public MachineItemViewModel(MachineDataModel machine)
        {
            _machine = machine;
            //ItemSelectedCommand = new RelayCommand(SelectItem, (d) =>true);
            //EditItemCommand  = new RelayCommand();
        }

        #endregion
    }
}
