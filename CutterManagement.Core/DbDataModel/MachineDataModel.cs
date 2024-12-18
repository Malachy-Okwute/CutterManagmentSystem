namespace CutterManagement.Core
{
    /// <summary>
    /// Machine data model
    /// </summary>
    public class MachineDataModel : DbDataModelBase
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
        public CutterChangeInformation CutterChangeInfo{ get; set; }

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
        /// Collection of part
        /// </summary>
        public ICollection<PartDataModel>? Part { get; set; }

        /// <summary>
        /// Collection of cutters
        /// </summary>
        public ICollection<CutterDataModel>? Cutter { get; set; }

        /// <summary>
        /// Collection of user
        /// </summary>
        public ICollection<UserDataModel>? Users { get; set; }
    }
}
