namespace CutterManagement.Core
{
    /// <summary>
    /// CMM data-model
    /// </summary>
    public class CMMDataModel : DataModelBase
    {
        /// <summary>
        /// Before corrections value
        /// </summary>
        public string BeforeCorrections { get; set; }

        /// <summary>
        /// After corrections value
        /// </summary>
        public string AfterCorrections { get; set; }

        /// <summary>
        /// Pressure angle coast value
        /// </summary>
        public string PressureAngleCoast { get; set; }

        /// <summary>
        /// Pressure angle drive value
        /// </summary>
        public string PressureAngleDrive { get; set; }

        /// <summary>
        /// Spiral angle coast value
        /// </summary>
        public string SpiralAngleCoast { get; set; }

        /// <summary>
        /// Spiral angle drive value
        /// </summary>
        public string SpiralAngleDrive { get; set; }

        /// <summary>
        /// Fr value (Runout)
        /// </summary>
        public string Fr { get; set; }

        /// <summary>
        /// Part size according to measurement
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// Piece count
        /// </summary>
        public string Count { get; set; }

        /// <summary>
        /// Cutter data model navigation property id
        /// </summary>
        public int? CutterDataModelId { get; set; }

        /// <summary>
        /// Cutter data model navigation property
        /// </summary>
        public CutterDataModel CutterDataModel { get; set; }

    }
}
