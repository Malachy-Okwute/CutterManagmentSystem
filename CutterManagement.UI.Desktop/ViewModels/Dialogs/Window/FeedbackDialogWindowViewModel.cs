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

        public bool? Response { get; set; }

        #region Public Events

        /// <summary>
        /// Close dialog request event
        /// </summary>
        public event EventHandler<DialogWindowCloseRequestedEventArgs> DialogWindowCloseRequest;

        #endregion

        public ICommand OKButtonCommand { get; set; }
        public ICommand NoButtonCommand { get; set; }
        public ICommand YesButtonCommand { get; set; }

        public FeedbackDialogWindowViewModel()
        {
            OKButtonCommand = new RelayCommand(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(true)));
            YesButtonCommand = new RelayCommand(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(true)));
            NoButtonCommand = new RelayCommand(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(false)));
        }
    }
}
