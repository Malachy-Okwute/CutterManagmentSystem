namespace CutterManagement.Core
{
    /// <summary>
    /// Part data model
    /// </summary>
    public class PartDataModel
    {
        /// <summary>
        /// The unique id used to identify this data on db
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Unique part id
        /// </summary>
        public string PartId { get; set; } = string.Empty;

        /// <summary>
        /// The number of teeth this part has
        /// </summary>
        public string PartToothCount { get; set; } = string.Empty;

        /// <summary>
        /// The model of this part
        /// </summary>
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// The type of this part
        /// Ring or pinion
        /// </summary>
        public PartKind Kind { get; set; }

    }
}
