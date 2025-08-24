namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="CutterHistoryItemControl"/>
    /// </summary>
    public class CutterHistoryPopupItemViewModel : ViewModelBase
    {
        /// <summary>
        /// Cutter number
        /// </summary>
        public string CutterNumber { get; set; }

        /// <summary>
        /// Machine number
        /// </summary>
        public string MachineNumber { get; set; }

        /// <summary>
        /// Part number
        /// </summary>
        public string PartNumber { get; set; }

        /// <summary>
        /// Produced quantity
        /// </summary>
        public string Count { get; set; }

        /// <summary>
        /// Size of part tooth
        /// </summary>
        public string SizeOfPartTooth { get; set; }

        /// <summary>
        /// The result of the check
        /// </summary>
        public string CheckResult { get; set; }

        /// <summary>
        /// Current shift
        /// </summary>
        public string Shift { get; set; }

        /// <summary>
        /// User that is logging this check
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Date and time of the check
        /// </summary>
        public string DateAndTimeOfCheck { get; set; }

        /// <summary>
        /// True if default background is to be used for this item
        /// </summary>
        public bool UseAlternateBackground { get; set; }

        /// <summary>
        /// True if this it is being used as a header
        /// </summary>
        public bool IsHeader { get; set; }
    }
}
