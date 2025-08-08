namespace CutterManagement.Core
{
    /// <summary>
    /// Data archive representing parts produced along with data associated with it
    /// </summary>
    public class ProductionPartsLogDataArchive : DataModelBase
    {
        /// <summary>
        /// True if this log is archived
        /// </summary>
        public bool IsArchived { get; set; }

        /// <summary>
        /// The machine number of where this record is taken from
        /// </summary>
        public string MachineNumber { get; set; }

        /// <summary>
        /// Cutter number that is set up in the machine
        /// </summary>
        public string CutterNumber { get; set; }

        /// <summary>
        /// The part number being ran
        /// </summary>
        public string PartNumber { get; set; }

        /// <summary>
        /// The model of the part
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// The tooth count of the part
        /// </summary>
        public string ToothCount { get; set; }

        /// <summary>
        /// Current piece count
        /// </summary>
        public string PieceCount { get; set; }

        /// <summary>
        /// Current tooth measured size
        /// </summary>
        public string ToothSize { get; set; }

        /// <summary>
        /// The full name of user capturing this data
        /// </summary>
        public string UserFullName { get; set; }

        /// <summary>
        /// Comment associated with this log if any
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// The result of frequency check
        /// </summary>
        public string FrequencyCheckResult { get; set; }

        /// <summary>
        /// Current shift at the time of capturing this log
        /// </summary>
        public string CurrentShift { get; set; }

        /// <summary>
        /// Reason for removing cutter
        /// </summary>
        public string? CutterChangeInfo { get; set; }

        /// <summary>
        /// Unique identifier for CMM data associated with this log
        /// </summary>
        public int? CMMDataId { get; set; }

        /// <summary>
        /// CMM data associated with this log if available
        /// </summary>
        public CMMDataModel? CMMData { get; set; }

        /// <summary>
        /// Date and time of last frequency check
        /// </summary>
        public DateTime DateTimeOfCheck { get; set; } = DateTime.Now;
    }
}
