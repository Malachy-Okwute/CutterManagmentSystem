namespace CutterManagement.Core
{
    /// <summary>
    /// Cutter data model DTO
    /// </summary>
    /// <param name="CutterNumber"> Unique cutter number </param>
    /// <param name="Count"> The number of parts produced by this cutter </param>
    /// <param name="SummaryNumber"> The summary this cutter is made for </param>
    /// <param name="CutterChangeComment"> Extra information relating to the reason cutter is pulled </param>
    /// <param name="Kind"> The kind of <see cref="PartKind"/> this cutter is made for </param>
    /// <param name="Owner"> The owner of this cutter </param>
    /// <param name="Condition"> Condition of this cutter
    /// Options = Brand new or used </param>
    /// <param name="CutterChangeInfo"> The reason cutter assigned to this machine was pulled from this machine </param>
    /// <param name="LastUsedDate"> The date this cutter was last used </param>
    /// <param name="MachineDataModelId"> Machine data model navigation property id </param>
    /// <param name="MachineDataModel"> Machine data model navigation property </param>
    public record CutterDataModelDTO(string CutterNumber, int Count, string SummaryNumber, string CutterChangeComment, PartKind Kind, Department Owner, CutterCondition Condition, CutterRemovalReason CutterChangeInfo, DateTime LastUsedDate, int? MachineDataModelId, MachineDataModel MachineDataModel)
    {
        /// <summary>
        /// Collection of CMM data model navigation properties
        /// </summary>
        public ICollection<CMMDataModel> CMMData { get; set; } = new List<CMMDataModel>();

    }
}
