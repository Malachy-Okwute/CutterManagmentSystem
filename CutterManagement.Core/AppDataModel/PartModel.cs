namespace CutterManagement.Core
{
    public class PartModel
    {
        /// <summary>
        /// <summary>
        /// Unique part number
        /// </summary>
        public string PartNumber { get; set; } = string.Empty;

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

        /// <summary>
        /// Date this entry was created
        /// </summary>
        public DateTime EntryCreatedDateTime { get; set; }
    }
}
