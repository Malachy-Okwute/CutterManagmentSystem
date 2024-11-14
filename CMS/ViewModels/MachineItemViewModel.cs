namespace CMS
{
    /// <summary>
    /// View model for <see cref="MachineItemControl"/>
    /// </summary>
    public class MachineItemViewModel : ViewModelBase
    {
        /// <summary>
        /// Unique ID of this machine
        /// </summary>
        public string MachineNumber { get; set; } = string.Empty;

        /// <summary>
        /// The current status of this machine 
        /// <remark>Indicate whether this machine is running or if this machine is down for any reason</remark>
        /// </summary>
        public bool MachineStatus { get; set; }

        /// <summary>
        /// Comment associated with this machine status
        /// <remark>
        /// Running | Down for maintenance | Just got done with maintenance
        /// </remark>
        /// </summary>
        public string MachineStatusComment { get; set; } = string.Empty;

        /// <summary>
        /// Cutter id number currently setup on this machine
        /// </summary>
        public string CurrentCutterNumber { get; set; } = string.Empty;

        /// <summary>
        /// Part unique id number running on this machine
        /// </summary>
        public string CurrentRunningPartNumber { get; set; } = string.Empty;

        /// <summary>
        /// Number of parts produced by this machine with the current cutter
        /// </summary>
        public string ProducedPartCount { get; set; } = string.Empty;

        /// <summary>
        /// Result of the last part checked 
        /// <remark>PASSED | FAILED</remark>
        /// </summary>
        public string ResultOfLastPartChecked { get; set; } = string.Empty;

        /// <summary>
        /// Date and time of the last checked part on this machine
        /// </summary>
        public string DateAndTimeOfLastCheck { get; set; } = string.Empty;
    }
}
