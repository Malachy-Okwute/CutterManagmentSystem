namespace CutterManagement.Core
{
    public class CutterModel
    {
        /// <summary>
        /// Information about the machine this cutter is currently setup on
        /// 
        /// <para> 
        /// NOTE: If cutter is not currently setup and is still re-useable, 
        ///       Information about the previous machine is provided 
        /// </para>
        /// </summary>
        public MachineDataModel MachineData { get; set; }

        /// <summary>
        /// Unique cutter number
        /// </summary>
        public string CutterNumber { get; set; } = string.Empty;

        /// <summary>
        /// The number of parts produced by this cutter
        /// </summary>
        public string Count { get; set; } = string.Empty;

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
