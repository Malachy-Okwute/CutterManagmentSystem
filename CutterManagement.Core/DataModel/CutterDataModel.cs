namespace CutterManagement.Core
{
    /// <summary>
    /// Cutter data model
    /// </summary>
    public class CutterDataModel : DataModelBase
    {
        /// <summary>
        /// Unique cutter number
        /// </summary>
        public string CutterNumber { get; set; } 

        /// <summary>
        /// The number of parts produced by this cutter
        /// </summary>
        public int Count { get; set; } 

        /// <summary>
        /// The model this cutter is made for
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Extra information relating to the reason cutter is pulled
        /// </summary>
        public string CutterChangeComment { get; set; }

        /// <summary>
        /// The kind of <see cref="PartKind"/> this cutter is made for
        /// </summary>
        public PartKind Kind { get; set; }

        /// <summary>
        /// The owner of this cutter
        /// </summary>
        public Department Owner { get; set; }

        /// <summary>
        /// Condition of this cutter
        /// Options = Brand new or used
        /// </summary>
        public CutterCondition Condition { get; set; }

        /// <summary>
        /// The reason cutter assigned to this machine was pulled from this machine
        /// </summary>
        public CutterRemovalReason CutterChangeInfo { get; set; }

        /// <summary>
        /// The date this cutter was last used
        /// </summary>
        public DateTime LastUsedDate { get; set; }

        /// <summary>
        /// Machine data model navigation property id
        /// </summary>
        public int? MachineDataModelId { get; set; }

        /// <summary>
        /// Machine data model navigation property
        /// </summary>
        public MachineDataModel MachineDataModel { get; set; }

        /// <summary>
        /// Collection of CMM data model navigation properties
        /// </summary>
        public ICollection<CMMDataModel> CMMData { get; set; } = new List<CMMDataModel>();

    }
}
