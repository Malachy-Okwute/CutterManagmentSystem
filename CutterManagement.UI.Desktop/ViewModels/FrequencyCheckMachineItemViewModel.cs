namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="FrequencyCheckMachineItem"/>
    /// </summary>
    public class FrequencyCheckMachineItemViewModel : ViewModelBase
    {
        /// <summary>
        /// Unique machine id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Machine number
        /// </summary>
        public string MachineNumber { get; set; }

        /// <summary>
        /// Part number
        /// </summary>
        public string PartNumber { get; set; }

        /// <summary>
        /// Part count
        /// </summary>
        public int PartCount { get; set; }

        /// <summary>
        /// Part tooth size
        /// </summary>
        public string PartSize { get; set; }

        /// <summary>
        /// True if part size can be entered by user
        /// </summary>
        public bool CanEnterPartSize { get; set; }
    }
}
