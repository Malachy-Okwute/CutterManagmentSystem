namespace CutterManagement.Core
{
    /// <summary>
    /// Machine data model
    /// </summary>
    public class MachineDataModel : DataModelBase, IMessage
    {
        /// <summary>
        /// The unique number assigned to this machine object
        /// </summary>
        public string MachineNumber { get; set; } 

        /// <summary>
        /// The unique set id assigned to this machine object
        /// </summary>
        public string MachineSetId { get; set; } 

        /// <summary>
        /// The count representing the number of parts produced by this machine
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Measured tooth size of part
        /// </summary>
        public string PartToothSize { get; set; }

        /// <summary>
        /// The part number this machine is running
        /// </summary>
        public string PartNumber{ get; set; }

        /// <summary>
        /// Comment related to the status of this machine
        /// </summary>
        public string StatusMessage { get; set; }

        /// <summary>
        /// Extra information relating to the reason cutter is pulled
        /// </summary>
        public string CutterChangeComment { get; set; }

        /// <summary>
        /// Flag indicating if this machine is configured of not
        /// </summary>
        public bool IsConfigured { get; set; }

        /// <summary>
        /// The most recent date and time this table was modified
        /// </summary>
        public DateTime DateTimeLastModified { get; set; }

        /// <summary>
        /// The most recent date and time this machine was setup with part and cutter
        /// </summary>
        public DateTime DateTimeLastSetup { get; set; } = DateTime.MinValue;

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
        /// The result of a frequency check
        /// Options = Passed or Failed
        /// </summary>
        public FrequencyCheckResult FrequencyCheckResult { get; set; }

        /// <summary>
        /// Cutter navigation property id
        /// </summary>
        public int? CutterDataModelId { get; set; }

        /// <summary>
        /// Cutter navigation property
        /// </summary>
        public CutterDataModel Cutter { get; set; }

        /// <summary>
        /// Navigation property id
        /// </summary>
        public int? CMMDataModelId { get; set; }

        /// <summary>
        /// Navigation property
        /// </summary>
        public CMMDataModel CMMData { get; set; }


        ///// <summary>
        ///// Parts navigation property collection
        ///// </summary>
        //public PartDataModel Part { get; set; }

        /// <summary>
        /// Users navigation property collection
        /// </summary>
        public ICollection<UserDataModel> Users { get; set; } = new List<UserDataModel>();
    }
}
