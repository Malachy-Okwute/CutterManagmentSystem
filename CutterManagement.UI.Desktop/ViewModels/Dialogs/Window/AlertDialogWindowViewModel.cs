using CutterManagement.Core;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="AlertDialogWindow"/>
    /// </summary>
    public class AlertDialogWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Feedback message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// True if operation is successful, Otherwise false
        /// </summary>
        public bool IsSuccess { get; set; }
    }
}
