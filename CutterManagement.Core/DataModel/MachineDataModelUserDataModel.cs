namespace CutterManagement.Core
{
    /// <summary>
    /// Many to many relationship between machine and part
    /// </summary>
    public class MachineDataModelUserDataModel
    {
        /// <summary>
        /// Machine foreign key id
        /// </summary>
        public int MachineDataModelId { get; set; }

        /// <summary>
        /// Machine navigation property
        /// </summary>
        public MachineDataModel MachineDataModel { get; set; }

        /// <summary>
        /// User foreign key id
        /// </summary>
        public int UserDataModelId { get; set; }

        /// <summary>
        /// User navigation property
        /// </summary>
        public UserDataModel UserDataModel { get; set; }
    }
}
