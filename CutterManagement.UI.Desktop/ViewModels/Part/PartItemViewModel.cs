using CutterManagement.Core;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="PartItemControl"/>
    /// </summary>
    public class PartItemViewModel : ViewModelBase
    {
        /// <summary>
        /// The unique part number
        /// </summary>
        public string PartNumber { get; set; }

        /// <summary>
        /// Part summary number
        /// </summary>
        public string SummaryNumber { get; set; }

        /// <summary>
        /// Part model number
        /// </summary>
        public string ModelNumber { get; set; }

        /// <summary>
        /// Number of teeth on part
        /// </summary>
        public string TeethCount { get; set; }

        /// <summary>
        /// The kind of part (Gear / Pinion)
        /// </summary>
        public PartKind Kind { get; set; }
    }
}
