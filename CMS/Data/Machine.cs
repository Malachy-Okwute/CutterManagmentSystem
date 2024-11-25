namespace CMS
{
    /// <summary>
    /// Machine data model
    /// </summary>
    public class Machine
    {
        /// <summary>
        /// The unique id assigned to this machine object
        /// </summary>
        public string UniqueID { get; set; }

        /// <summary>
        /// The unique set id assigned to this machine object
        /// </summary>
        public string UniqueSetID { get; set; }

        /// <summary>
        /// The count representing the number of parts produced by this machine
        /// </summary>
        public string? Count { get; set; }

        /// <summary>
        /// Measured tooth size of part
        /// </summary>
        public string? PartToothSize { get; set; }

        /// <summary>
        /// The last date and time data record was updated
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// The dept. owner of this machine
        /// </summary>
        public Department MachineOwner { get; set; }

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
        /// The part this machine is currently setup to run/produce
        /// </summary>
        public Part? RunningPart { get; set; }

        /// <summary>
        /// The cutter this machine is currently set up with to run/produce parts
        /// </summary>
        public Cutter? Cutter { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="uniqueID">The unique id</param>
        /// <param name="uniqueSetID">The unique set id</param>
        /// <param name="owner">The dept. owner of this machine</param>
        public Machine(string uniqueID, string uniqueSetID, Department owner)
        {
            UniqueID = uniqueID;
            UniqueSetID = uniqueSetID;
            MachineOwner = owner;
        }
    }
}
