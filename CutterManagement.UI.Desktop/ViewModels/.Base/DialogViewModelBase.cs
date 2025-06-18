namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model base for dialogs 
    /// </summary>
    public class DialogViewModelBase: ViewModelBase
    {
        /// <summary>
        /// The title of dialog 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Feedback message
        /// </summary>
        //public string Message { get; set; }

        /// <summary>
        /// True if operation is successful, Otherwise false
        /// </summary>
        public bool IsSuccess { get; set; }
    }
}
