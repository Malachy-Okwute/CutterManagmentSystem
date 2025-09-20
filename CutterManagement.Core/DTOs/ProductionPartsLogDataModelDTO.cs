namespace CutterManagement.Core
{
    /// <summary>
    /// Data model DTO representing parts produced along with data associated with it
    /// </summary>
    /// <param name="IsArchived"> True if this log is archived </param>
    /// <param name="MachineNumber"> The machine number of where this record is taken from </param>
    /// <param name="CutterNumber"> Cutter number that is set up in the machine </param>
    /// <param name="PartNumber"> The part number being ran </param>
    /// <param name="SummaryNumber"> The summary of the part </param>
    /// <param name="ToothCount"> The tooth count of the part </param>
    /// <param name="PieceCount"> Current piece count </param>
    /// <param name="ToothSize"> Current tooth measured size </param>
    /// <param name="UserFullName"> The full name of user capturing this data </param>
    /// <param name="Comment"> Comment associated with this log if any </param>
    /// <param name="FrequencyCheckResult"> The result of frequency check </param>
    /// <param name="CurrentShift"> Current shift at the time of capturing this log </param>
    /// <param name="CutterChangeInfo"> Reason for removing cutter </param>
    /// <param name="CMMDataId"> Unique identifier for CMM data associated with this log </param>
    /// <param name="CMMData"> CMM data associated with this log if available </param>
    public record ProductionPartsLogDataModelDTO(bool IsArchived, string MachineNumber, string CutterNumber, string PartNumber, string SummaryNumber, string ToothCount, string PieceCount, string ToothSize, string? UserFullName, string Comment, string FrequencyCheckResult, string CurrentShift, string? CutterChangeInfo, int? CMMDataId, CMMDataModel? CMMData)
    {
        /// <summary>
        /// Date and time of last frequency check
        /// </summary>
        public DateTime DateTimeOfCheck { get; set; } = DateTime.Now;
    }
}
