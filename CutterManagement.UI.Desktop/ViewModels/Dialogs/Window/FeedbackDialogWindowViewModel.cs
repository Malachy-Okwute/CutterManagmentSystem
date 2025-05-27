using CutterManagement.Core;
using CutterManagement.Core.Services;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="FeedbackDialogWindow"/>
    /// </summary>
    public class FeedbackDialogWindowViewModel : ViewModelBase, IDialogWindowCloseRequest
    {
        /// <summary>
        /// Feedback message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// True if operation is successful, Otherwise false
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Type of <see cref="FeedbackDialogWindow"/>
        /// </summary>
        public FeedbackDialogKind FeedbackDialogKind { get; set; }

        /// <summary>
        /// True if feedback dialog kind is a prompt
        /// NOTE: Prompts user to choose Yes or No
        /// </summary>
        public bool IsPrompt { get; set; }

        /// <summary>
        /// Close dialog request event
        /// </summary>
        public event EventHandler<DialogWindowCloseRequestedEventArgs> DialogWindowCloseRequest;

        /// <summary>
        /// Command to run when ok button is pressed
        /// </summary>
        public ICommand OKButtonCommand { get; set; }

        /// <summary>
        /// Command to run when no button is pressed
        /// </summary>
        public ICommand NoButtonCommand { get; set; }

        /// <summary>
        /// Command to run when yes button is pressed
        /// </summary>
        public ICommand YesButtonCommand { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public FeedbackDialogWindowViewModel()
        {
            // Create commands
            OKButtonCommand = new RelayCommand(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(true)));
            YesButtonCommand = new RelayCommand(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(true)));
            NoButtonCommand = new RelayCommand(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(false)));
        }
    }
}
