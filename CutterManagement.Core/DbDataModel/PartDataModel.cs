namespace CutterManagement.Core
{
    /// <summary>
    /// Part data model
    /// </summary>
    public class PartDataModel : DbDataModelBase
    {
        /// <summary>
        /// Foreign id to <see cref="MachineDataModel"/>
        /// </summary>
        public int MachineDataModelId { get; set; }

        /// <summary>
        /// Navigation property <see cref="MachineDataModel"/>
        /// </summary>
        public MachineDataModel MachineData { get; set; }

        /// <summary>
        /// Unique part number
        /// </summary>
        public int PartNumber { get; set; }

        /// <summary>
        /// The number of teeth this part has
        /// </summary>
        public int PartToothCount { get; set; }

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
