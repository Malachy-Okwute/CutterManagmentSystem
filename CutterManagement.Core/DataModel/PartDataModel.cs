namespace CutterManagement.Core
{
    /// <summary>
    /// Part data model
    /// </summary>
    public class PartDataModel : DataModelBase
    {
        /// <summary>
        /// Unique part number
        /// </summary>
        public string PartNumber { get; set; }

        /// <summary>
        /// The number of teeth this part has
        /// </summary>
        public string PartToothCount { get; set; }

        /// <summary>
        /// The model of this part
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// The type of this part
        /// Ring or pinion
        /// </summary>
        public PartKind Kind { get; set; }

        /// <summary>
        /// Machine and parts skip navigation properties
        /// </summary>
        public ICollection<MachineDataModelPartDataModel> MachinesAndParts { get; set; }
    }
}
