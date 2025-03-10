using CutterManagement.Core;
using CutterManagement.Core.Services;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="FrequencyCheckDialog"/>
    /// </summary>
    public class FrequencyCheckDialogViewModel : DialogViewModelBase, IDialogWindowCloseRequest
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
        public string PartCount { get; set; }

        /// <summary>
        /// Part tooth size
        /// </summary>
        public string PartSize { get; set; }

        /// <summary>
        /// True if part size can be entered by user
        /// </summary>
        public bool CanEnterPartSize { get; set; }

        /// <summary>
        /// The result of this check
        /// </summary>
        public string FrequencyCheckResult { get; set; }

        /// <summary>
        /// Close dialog request event
        /// </summary>
        public event EventHandler<DialogWindowCloseRequestedEventArgs> DialogWindowCloseRequest;

        /// <summary>
        /// Default constructor
        /// </summary>
        public FrequencyCheckDialogViewModel()
        {
            
        }
    }
}
