namespace CutterManagement.Core
{
    /// <summary>
    /// Part data model
    /// </summary>
    public class PartDataModel : DataModelBase, IMessage
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
        /// The summary this part is associated to
        /// </summary>
        public string SummaryNumber { get; set; }

        /// <summary>
        /// The type of this part
        /// Ring or pinion
        /// </summary>
        public PartKind Kind { get; set; }

        /// <summary>
        /// Navigation property id
        /// </summary>
        //public int? MachineDataModelId { get; set; }

        /// <summary>
        /// Machine navigation property collection 
        /// </summary>
        //public ICollection<MachineDataModel> MachineDataModel { get; set; } = new List<MachineDataModel>();
    }
}
