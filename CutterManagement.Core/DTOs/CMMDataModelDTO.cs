namespace CutterManagement.Core
{
    /// <summary>
    /// CMM data-model DTO
    /// </summary>
    /// <param name="BeforeCorrections"> Before corrections value </param>
    /// <param name="AfterCorrections"> After corrections value </param>
    /// <param name="PressureAngleCoast"> Pressure angle coast value </param>
    /// <param name="PressureAngleDrive"> Pressure angle drive value </param>
    /// <param name="SpiralAngleCoast"> Spiral angle coast value </param>
    /// <param name="SpiralAngleDrive"> Spiral angle drive value </param>
    /// <param name="Fr"> Fr value (Runout) </param>
    /// <param name="Size"> Part size according to measurement </param>
    /// <param name="Count"> Piece count </param>
    /// <param name="CutterDataModelId"> Cutter data model navigation property id </param>
    /// <param name="CutterDataModel"> Cutter data model navigation property </param>
    public record CMMDataModelDTO(string BeforeCorrections, string AfterCorrections, string PressureAngleCoast, string PressureAngleDrive, string SpiralAngleCoast, string SpiralAngleDrive, string Fr, string Size, string Count, int? CutterDataModelId, CutterDataModel CutterDataModel);
}
