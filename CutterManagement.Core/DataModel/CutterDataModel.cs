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
        /// Machine and cutters navigation properties
        /// </summary>
        public ICollection<MachineDataModelCutterDataModel> MachinesAndCutters { get; set; }
    }
}
