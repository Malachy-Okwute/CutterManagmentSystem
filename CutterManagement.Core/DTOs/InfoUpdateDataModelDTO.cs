namespace CutterManagement.Core
{
    /// <summary>
    /// DTO Data model for information updates 
    /// </summary>
    /// <param name="IsArchived"> True if this it is currently archived </param>
    /// <param name="Title"> Title of this information update </param>
    /// <param name="PublishDate"> Date this information was published </param>
    /// <param name="LastUpdatedDate"> Date this information was lasts modified </param>
    /// <param name="Information"> The actual information </param>
    /// <param name="Kind"> The kind of part (Gear / Pinion) </param>
    /// <param name="PartNumberWithMove"> The selected part number  </param>
    /// <param name="PressureAngleCoast"> Pressure angle value on coast </param>
    /// <param name="PressureAngleDrive"> Pressure angle value on drive </param>
    /// <param name="SpiralAngleCoast"> Spiral angle value on coast </param>
    /// <param name="SpiralAngleDrive"> Spiral angle value on drive </param>
    /// <param name="UserDataModelId"> Unique user foreign key </param>
    /// <param name="UserDataModel"> The actual user / author of this update </param>
    public record InfoUpdateDataModelDTO(bool IsArchived, string Title, string PublishDate, string LastUpdatedDate, string Information, PartKind Kind, string PartNumberWithMove, string PressureAngleCoast, string PressureAngleDrive, string SpiralAngleCoast, string SpiralAngleDrive, int UserDataModelId, UserDataModel UserDataModel) : IMessage
    {
        /// <summary>
        /// True if move is attached to this data point
        /// </summary>
        public bool HasAttachedMoves => int.TryParse(PartNumberWithMove, out var result) is true;
    }
}
