namespace CutterManagement.Core
{
    public class MachineModel
    {
        /// <summary>
        /// The unique number assigned to this machine object
        /// </summary>
        public int MachineNumber { get; set; }

        /// <summary>
        /// The unique set id assigned to this machine object
        /// </summary>
        public int MachineSetId { get; set; }

        /// <summary>
        /// The count representing the number of parts produced by this machine
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Measured tooth size of part
        /// </summary>
        public int PartToothSize { get; set; } 

        /// <summary>
        /// The most recent date and time this table was modified
        /// </summary>
        public DateTime LastModifiedDateTime { get; set; }

        /// <summary>
        /// The date and time when this entity record was created
        /// </summary>
        public DateTime EntryCreatedDateTime { get; set; }

        /// <summary>
        /// The dept. owner of this machine
        /// </summary>
        public Department Owner { get; set; }

        /// <summary>
        /// The status of this machine indicating whether it's running, sitting idle or down for maintenance
        /// </summary>
        public MachineStatus Status { get; set; }

        /// <summary>
        /// The reason cutter assigned to this machine was pulled from this machine
        /// </summary>
        public CutterChangeInformation CutterChangeInfo { get; set; }

        /// <summary>
        /// Extra information relating to the reason cutter is pulled
        /// </summary>
        public string CutterChangeComment { get; set; } = string.Empty;

        /// <summary>
        /// The result of a frequency check
        /// Options = Passed or Failed
        /// </summary>
        public FrequencyCheckResult FrequencyCheckResult { get; set; }

        /// <summary>
        /// The part that is currently setup on this machine or null if machine doesn't have part
        /// </summary>
        public PartDataModel? Part { get; set; }

        /// <summary>
        /// The cutter that is currently setup on this machine or null if machine doesn't have cutter
        /// </summary>
        public CutterDataModel? Cutter { get; set; }

        /// <summary>
        /// User that made the most recent change to this machine
        /// </summary>
        public UserDataModel? User { get; set; }
    }
}
