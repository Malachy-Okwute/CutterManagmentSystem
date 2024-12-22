namespace CutterManagement.Core
{
    /// <summary>
    /// Many to many relationship between machine and part
    /// </summary>
    public class MachineDataModelPartDataModel 
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
        /// Part foreign key id
        /// </summary>
        public int PartDataModelId { get; set; }

        /// <summary>
        /// Part navigation property
        /// </summary>
        public PartDataModel PartDataModel { get; set; }
    }
}
