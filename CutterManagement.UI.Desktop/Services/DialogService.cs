using CutterManagement.Core;
using CutterManagement.Core.Services;
using System.Windows.Controls;

namespace CutterManagement.UI.Desktop
{
    public class DialogService : IDialogService
    {
        private DialogWindow _dialogWindow;
        private FeedbackDialogWindow _feedbackDialogWindow;
        private static DialogService DialogInstance => new DialogService();
        private static IDictionary<Type, Type> _dialogMappings = new Dictionary<Type, Type>();

        public DialogService()
        {
            _dialogWindow = new DialogWindow();
            _feedbackDialogWindow = new FeedbackDialogWindow();
        }

        public static void RegisterDialog<TViewModel, TView>() where TViewModel : IDialogWindowCloseRequest
        {
            if(typeof(TViewModel).BaseType != typeof(DialogViewModelBase))
            {
                throw new InvalidOperationException($"{typeof(TViewModel)} is not a valid dialog view model");
            }

            if(_dialogMappings.ContainsKey(typeof(TViewModel)))
            {
                throw new ArgumentException($"{typeof(TViewModel)} is already associated with {typeof(TView)}");
            }

            _dialogMappings.Add(typeof(TViewModel), typeof(TView));
        }

        private void ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogWindowCloseRequest
        {
            Type viewType = _dialogMappings[typeof(TViewModel)];

            object? dialog = Activator.CreateInstance(viewType);

            if(dialog == null) return;

            EventHandler<DialogWindowCloseRequestedEventArgs>? dialogCallback = null;
            dialogCallback += (s, e) =>
            {
                viewModel.DialogWindowCloseRequest -= dialogCallback;

                if(e.DialogResult.HasValue)
                {
                    _dialogWindow.DialogResult = e.DialogResult;
                }
                else
                {
                    _dialogWindow.Close();
                }
            };

            viewModel.DialogWindowCloseRequest += dialogCallback;

            ((UserControl)dialog).DataContext = viewModel;
            
            _dialogWindow.ContentControl.Content = dialog;

            _dialogWindow.ShowDialog();
        }

        private async Task ShowFeedback<TViewModel>(TViewModel viewModel) where TViewModel : DialogViewModelBase
        {
            var dataContext = new FeedbackDialogWindowViewModel();

            dataContext.Message = viewModel.Message;

            dataContext.IsMessageSuccess = viewModel.IsMessageSuccess;

            _feedbackDialogWindow.DataContext = dataContext;

            await Task.Run(() => 
            {
                DispatcherService.Invoke(() => _feedbackDialogWindow.ShowDialog());

            }).WaitAsync(TimeSpan.FromSeconds(2)).ContinueWith( _ =>
            {
                DispatcherService.Invoke(_feedbackDialogWindow.Close);
            });
        }

        public static void InvokeDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogWindowCloseRequest => DialogInstance.ShowDialog(viewModel);
        public static async Task InvokeDialogFeedbackMessage<TViewModel>(TViewModel viewModel) where TViewModel : DialogViewModelBase => await DialogInstance.ShowFeedback(viewModel);
        
    }
}
