namespace CutterManagement.Core
{
    /// <summary>
    /// Machine data model
    /// </summary>
    public class MachineDataModel : DataModelBase
    {
        /// <summary>
        /// The unique number assigned to this machine object
        /// </summary>
        public string MachineNumber { get; set; } 

        /// <summary>
        /// The unique set id assigned to this machine object
        /// </summary>
        public string MachineSetId { get; set; } 

        /// <summary>
        /// The count representing the number of parts produced by this machine
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Measured tooth size of part
        /// </summary>
        public string PartToothSize { get; set; }

        /// <summary>
        /// The most recent date and time this table was modified
        /// </summary>
        public DateTime DateTimeLastModified { get; set; } 

        /// <summary>
        /// The dept. owner of this machine
        /// </summary>
        public Department Owner { get; set; }

        /// <summary>
        /// The status of this machine indicating whether it's running, sitting idle or down for maintenance
        /// </summary>
        public MachineStatus Status { get; set; }

        /// <summary>
        /// The reason cutter assigned to this machine was pulled from this machine
        /// </summary>
        public CutterChangeInformation CutterChangeInfo{ get; set; }

        /// <summary>
        /// Extra information relating to the reason cutter is pulled
        /// </summary>
        public string CutterChangeComment { get; set; }

        /// <summary>
        /// The result of a frequency check
        /// Options = Passed or Failed
        /// </summary>
        public FrequencyCheckResult FrequencyCheckResult { get; set; }

        /// <summary>
        /// Machine and parts navigation properties
        /// </summary>
        public ICollection<MachineDataModelPartDataModel> MachinesAndParts { get; set; }

        /// <summary>
        /// Machine and cutters navigation properties
        /// </summary>
        public ICollection<MachineDataModelCutterDataModel> MachinesAndCutters { get; set; }

        /// <summary>
        /// Machine and users navigation properties
        /// </summary>
        public ICollection<MachineDataModelUserDataModel> MachinesAndUsers { get; set; }

    }
}
