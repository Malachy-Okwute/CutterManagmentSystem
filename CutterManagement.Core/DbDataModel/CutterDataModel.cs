namespace CutterManagement.Core
{
    /// <summary>
    /// Cutter data model
    /// </summary>
    public class CutterDataModel : DbDataModelBase
    {
        /// <summary>
        /// Foreign id to <see cref="MachineDataModel"/>
        /// </summary>
        public int MachineDataModelId { get; set; }

        /// <summary>
        /// Navigation property <see cref="MachineDataModel"/>
        /// </summary>
        public MachineDataModel MachineData { get; set; }

        /// <summary>
        /// Unique cutter number
        /// </summary>
        public int CutterNumber { get; set; } 

        /// <summary>
        /// The number of parts produced by this cutter
        /// </summary>
        public int Count { get; set; } 

        /// <summary>
        /// The model this cutter is made for
        /// </summary>
        public string Model { get; set; } = string.Empty;

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
        /// The date this cutter was last used
        /// </summary>
        public DateTime LastUsedDate { get; set; }

        /// <summary>
        /// Date this entry was created
        /// </summary>
        public DateTime EntryCreatedDateTime { get; set; }
    }
}
