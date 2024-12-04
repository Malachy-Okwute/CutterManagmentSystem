namespace CutterManagement.Core
{
    /// <summary>
    /// Machine data model
    /// </summary>
    public class MachineDataModel
    {
        /// <summary>
        /// The unique id assigned to this machine object
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// The unique set id assigned to this machine object
        /// </summary>
        public string SetID { get; set; } = string.Empty;

        /// <summary>
        /// The count representing the number of parts produced by this machine
        /// </summary>
        public string Count { get; set; } = string.Empty;

        /// <summary>
        /// Measured tooth size of part
        /// </summary>
        public string PartToothSize { get; set; } = string.Empty;

        /// <summary>
        /// The last date and time data record was updated
        /// </summary>
        public DateTime DateTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The dept. owner of this machine
        /// </summary>
        public Department Owner { get; set; }

        /// <summary>
        /// The status of this machine indicating whether it's running, sitting idle or down for maintenance
        /// </summary>
        public MachineStatus Status { get; set; }

        /// <summary>
        /// The result of a frequency check
        /// Options = Passed or Failed
        /// </summary>
        public FrequencyCheckResult FrequencyCheckResult { get; set; }

        /// <summary>
        /// The part this machine is currently setup to run/produce or null if not setup
        /// </summary>
        public PartDataModel? Part { get; set; }

        /// <summary>
        /// The cutter this machine is currently set up with to run/produce parts or null if not setup
        /// </summary>
        public CutterDataModel? Cutter { get; set; }
    }
}
