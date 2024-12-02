using DocumentFormat.OpenXml.EMMA;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Part data model
    /// </summary>
    public class PartDataModel
    {
        /// <summary>
        /// The model of this part
        /// </summary>
        private string _model = string.Empty;

        /// <summary>
        /// The unique id of this part object
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// The number of teeth this part has
        /// </summary>
        public string PartToothCount { get; set; } = string.Empty;

        /// <summary>
        /// The type of this part
        /// Ring or pinion
        /// </summary>
        public PartKind Kind { get; set; }

        /// <summary>
        /// The model of this part
        /// </summary>
        public string Model
        {
            get => string.Format("M" + _model, _model);
            set => _model = value;
        }
    }
}
