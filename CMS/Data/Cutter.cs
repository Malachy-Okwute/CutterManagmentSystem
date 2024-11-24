namespace CMS
{
    /// <summary>
    /// Cutter data model
    /// </summary>
    public class Cutter
    {
        /// <summary>
        /// Unique id of this object
        /// </summary>
        public string UniqueID { get; set; } = string.Empty;

        /// <summary>
        /// The count representing the number of parts produced by this cutter
        /// </summary>
        public string? Count { get; set; }

        /// <summary>
        /// The kind of <see cref="PartKind"/> this cutter is made for
        /// </summary>
        public PartKind Kind { get; set; }

        /// <summary>
        /// The owner of this cutter
        /// </summary>
        public Department Department { get; set; }

        /// <summary>
        /// Condition of this cutter
        /// Options = Brand new or used
        /// </summary>
        public CutterCondition Condition { get; set; }

        /// <summary>
        /// The model this cutter is made for
        /// </summary>
        public PartModel Model { get; set; } 

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model">The part model assigned to this cutter</param>
        public Cutter(PartModel model)
        {
            Model = model;
        }
    }
}
