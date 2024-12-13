namespace CutterManagement.Core
{
    /// <summary>
    /// Cutter data model
    /// </summary>
    public class CutterDataModel
    {
        /// <summary>
        /// Unique id of this cutter
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Unique cutter id
        /// </summary>
        public string CutterId { get; set; } = string.Empty;

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

    }
}
