namespace CutterManagement.Core
{
    /// <summary>
    /// Many to many relationship between machine and cutter
    /// </summary>
    public class MachineDataModelCutterDataModel
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
        /// Cutter foreign key id
        /// </summary>
        public int CutterDataModelId { get; set; }

        /// <summary>
        /// Cutter navigation property
        /// </summary>
        public CutterDataModel CutterDataModel { get; set; }
    }
}
