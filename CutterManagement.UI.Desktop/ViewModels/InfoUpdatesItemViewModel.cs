namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="InfoUpdatesItemControl"/>
    /// </summary>
    public class InfoUpdatesItemViewModel : ViewModelBase
    {
        /// <summary>
        /// Unique id of this information
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title of this information update
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Author of this information update
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Date this information was published
        /// </summary>
        public string PublishDate { get; set; }

        /// <summary>
        /// Date this information was lasts modified
        /// </summary>
        public string LastUpdatedDate { get; set; }

        /// <summary>
        /// The actual information
        /// </summary>
        public string Information { get; set; }

        /// <summary>
        /// True if move is attached
        /// </summary>
        public bool HasAttachedMoves => int.TryParse(PartNumber, out var result) is true;

        /// <summary>
        /// Part type if a move is attached 
        /// </summary>
        public string? Kind { get; set; }

        /// <summary>
        /// Part number if a move is attached 
        /// </summary>
        public string? PartNumber { get; set; }

        /// <summary>
        /// Pressure angle value on coast
        /// </summary>
        public string? PressureAngleCoast { get; set; }

        /// <summary>
        /// Pressure angle value on drive
        /// </summary>
        public string? PressureAngleDrive { get; set; }

        /// <summary>
        /// Spiral angle value on coast
        /// </summary>
        public string? SpiralAngleCoast { get; set; }

        /// <summary>
        /// Spiral angle value on drive
        /// </summary>
        public string? SpiralAngleDrive { get; set; }

    }
}
