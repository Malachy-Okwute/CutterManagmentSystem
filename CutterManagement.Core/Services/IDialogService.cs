namespace CutterManagement.Core
{
    public interface IDialogService
    {
        abstract static void RegisterDialog<TViewModel, TView>();

        void ShowDialog<TViewModel>(TViewModel viewModel, Action<string?> dialogCallback);
    }
}
