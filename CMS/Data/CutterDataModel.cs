namespace CMS
{
    /// <summary>
    /// Cutter data model
    /// </summary>
    public class CutterDataModel
    {
        /// <summary>
        /// Unique id of this cutter
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The number of parts produced by this cutter
        /// </summary>
        public string Count { get; set; }

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
        /// The model this cutter is made for
        /// </summary>
        public string Model { get; set; } 
    }
}
