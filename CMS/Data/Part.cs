namespace CMS
{
    /// <summary>
    /// Part data model
    /// </summary>
    public class Part
    {
        /// <summary>
        /// The unique id of this part object
        /// </summary>
        public string UniqueID { get; set; } = string.Empty;

        /// <summary>
        /// The number of teeth this part has
        /// </summary>
        public string PartToothCount { get; set; } = string.Empty;

        /// <summary>
        /// The type of this part
        /// Ring or pinion
        /// </summary>
        public PartKind Kind { get; set; }

        /// <summary>
        /// The model of this part
        /// </summary>
        public PartModel Model { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model">Model of this part</param>
        public Part(PartModel model)
        {
            Model = model;
        }
    }
}
