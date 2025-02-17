namespace CutterManagement.UI.Desktop
{
    public class FeedbackDialogWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Feedback message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// True if operation is successful, Otherwise false
        /// </summary>
        public bool IsMessageSuccess { get; set; }
    }
}
