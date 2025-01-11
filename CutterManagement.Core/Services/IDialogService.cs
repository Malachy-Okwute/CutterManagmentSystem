using CutterManagement.Core.Services;

namespace CutterManagement.Core
{
    public interface IDialogService
    {
        abstract static void RegisterDialog<TViewModel, TView>() where TViewModel : IDialogWindowCloseRequested;

        void ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogWindowCloseRequested;
    }
}
