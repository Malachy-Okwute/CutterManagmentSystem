namespace CutterManagement.Core
{
    public class DialogWindowCloseRequestedEventArgs : EventArgs
    {
        public bool? DialogResult { get; }
        public DialogWindowCloseRequestedEventArgs(bool? dialogResult)
        {
            DialogResult = dialogResult;
        }
    }
}