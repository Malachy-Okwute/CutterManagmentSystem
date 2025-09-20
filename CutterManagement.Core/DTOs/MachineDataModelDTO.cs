namespace CutterManagement.Core
{
    /// <summary>
    /// Machine data model DTO
    /// </summary>
    /// <param name="MachineNumber"> The unique number assigned to this machine object </param>
    /// <param name="MachineSetId"> The unique set id assigned to this machine object </param>
    /// <param name="Count"> The count representing the number of parts produced by this machine </param>
    /// <param name="PartToothSize"> Measured tooth size of part </param>
    /// <param name="PartNumber"> The part number this machine is running </param>
    /// <param name="StatusMessage"> Comment related to the status of this machine </param>
    /// <param name="CutterChangeComment"> Extra information relating to the reason cutter is pulled </param>
    /// <param name="IsConfigured"> Flag indicating if this machine is configured of not </param>
    /// <param name="DateTimeLastModified"> The most recent date and time this table was modified </param>
    /// <param name="Owner"> The dept. owner of this machine </param>
    /// <param name="Status"> The status of this machine indicating whether it's running, sitting idle or down for maintenance </param>
    /// <param name="CutterChangeInfo"> The reason cutter assigned to this machine was pulled from this machine </param>
    /// <param name="FrequencyCheckResult"> The result of a frequency check
    /// Options = Passed or Failed </param>
    /// <param name="CutterDataModelId"> Cutter navigation property id </param>
    /// <param name="Cutter"> Cutter navigation property </param>
    public record MachineDataModelDTO(string MachineNumber, string MachineSetId, int Count, string PartToothSize, string PartNumber, string StatusMessage, string CutterChangeComment, bool IsConfigured, DateTime DateTimeLastModified, Department Owner, MachineStatus Status, CutterRemovalReason CutterChangeInfo, FrequencyCheckResult FrequencyCheckResult, int? CutterDataModelId, CutterDataModel Cutter) : IMessage
    {
        /// <summary>
        /// The most recent date and time this machine was setup with part and cutter
        /// </summary>
        public DateTime DateTimeLastSetup { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Machine and users navigation property collection
        /// </summary>
        public ICollection<MachineUserInteractions> MachineUserInteractions { get; set; } = new List<MachineUserInteractions>();
    }
}
