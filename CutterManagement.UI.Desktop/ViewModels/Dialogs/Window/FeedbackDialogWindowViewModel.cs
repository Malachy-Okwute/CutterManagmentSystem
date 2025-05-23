using CutterManagement.Core;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="FeedbackDialogWindow"/>
    /// </summary>
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

        /// <summary>
        /// Type of <see cref="FeedbackDialogWindow"/>
        /// </summary>
        public FeedbackDialogKind FeedbackDialogKind { get; set; }

        /// <summary>
        /// True if feedback dialog kind is a prompt
        /// NOTE: Prompts user to choose Yes or No
        /// </summary>
        public bool IsPrompt { get; set; }
    }
}
