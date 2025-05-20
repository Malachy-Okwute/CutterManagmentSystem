namespace CutterManagement.Core
{
    /// <summary>
    /// Join table between machine and users
    /// </summary>
    public class MachineUserInteractions : DataModelBase
    {
        /// <summary>
        /// Machine foreign key
        /// </summary>
        public int? MachineDataModelId { get; set; }

        /// <summary>
        /// Associated machine object
        /// </summary>
        public MachineDataModel MachineDataModel { get; set; }

        /// <summary>
        /// User foreign key
        /// </summary>
        public int? UserDataModelId { get; set; }

        /// <summary>
        /// Associated user object
        /// </summary>
        public UserDataModel UserDataModel { get; set; }

        /// <summary>
        /// Date and time of last entry
        /// </summary>
        public DateTime LastEntryDateTime { get; set; } = DateTime.Now;
    }
}
