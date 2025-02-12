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

        public string Message { get; set; }
        public bool IsMessageSuccess { get; set; }
    }
}
