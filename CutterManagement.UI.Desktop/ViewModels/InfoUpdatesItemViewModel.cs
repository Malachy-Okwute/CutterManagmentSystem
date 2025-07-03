namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="InfoUpdatesItemControl"/>
    /// </summary>
    public class InfoUpdatesItemViewModel : ViewModelBase
    {
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
    }
}
