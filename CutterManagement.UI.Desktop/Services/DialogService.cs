using CutterManagement.Core;
using CutterManagement.Core.Services;
using System.Windows;
using System.Windows.Controls;

namespace CutterManagement.UI.Desktop
{
    public class DialogService : IDialogService
    {
        private DialogWindow _dialogWindow;
        private AlertDialogWindow _alertDialogWindow;
        private FeedbackDialogWindow _feedbackDialogWindow;
        private static DialogService DialogInstance => new DialogService();
        private static IDictionary<Type, Type> _dialogMappings = new Dictionary<Type, Type>();

        public DialogService()
        {
            _dialogWindow = new DialogWindow();
            _alertDialogWindow = new AlertDialogWindow();
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

        private async Task ProcessAndShowAlertDialog<TViewModel>(TViewModel viewModel) where TViewModel : DialogViewModelBase
        {
            AlertDialogWindowViewModel dataContext = new AlertDialogWindowViewModel();

            dataContext.Message = viewModel.Message;

            dataContext.IsMessageSuccess = viewModel.IsSuccess;

            _alertDialogWindow.DataContext = dataContext;

            await Task.Run(() =>
            {
                DispatcherService.Invoke(() => _alertDialogWindow.ShowDialog());

            }).WaitAsync(TimeSpan.FromSeconds(3)).ContinueWith( _ =>
            {
                DispatcherService.Invoke(_alertDialogWindow.Close);
            });
        }

        private async Task<bool?> ProcessAndShowFeedbackDialog<TViewModel>(TViewModel viewModel, FeedbackDialogKind dialogKind) where TViewModel : DialogViewModelBase
        {
            FeedbackDialogWindowViewModel dataContext = new FeedbackDialogWindowViewModel();

            EventHandler<DialogWindowCloseRequestedEventArgs>? dialogCallback = null;
            dialogCallback += (s, e) =>
            {
                dataContext.DialogWindowCloseRequest -= dialogCallback;

                if (e.DialogResult.HasValue)
                {
                    dataContext.Response = e.DialogResult;
                    _feedbackDialogWindow.DialogResult = e.DialogResult;
                }
                else
                {
                    DispatcherService.Invoke(_feedbackDialogWindow.Close);
                }
            };

            dataContext.DialogWindowCloseRequest += dialogCallback;

            dataContext.Message = viewModel.Message;

            dataContext.IsSuccess = viewModel.IsSuccess;

            dataContext.IsPrompt = dialogKind == FeedbackDialogKind.Prompt;

            _feedbackDialogWindow.DataContext = dataContext;

            await Task.Run(() => DispatcherService.Invoke(() => _feedbackDialogWindow.ShowDialog()));

            return dataContext.Response;
        }

        public static void InvokeDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogWindowCloseRequest
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            // Apply blur effect to the main window
            BlurHelper.ApplyBlurEffect(mainWindow.MainAppWindow);

            // Show dialog window
            DialogInstance.ShowDialog(viewModel);

            // Remove blur effect from the main window
            BlurHelper.RemoveBlurEffect(mainWindow.MainAppWindow);
        }

        public static async Task InvokeAlertDialog<TViewModel>(TViewModel viewModel) where TViewModel : DialogViewModelBase => await DialogInstance.ProcessAndShowAlertDialog(viewModel);
        public static async Task<bool?> InvokeFeedbackDialog<TViewModel>(TViewModel viewModel, FeedbackDialogKind dialogKind = FeedbackDialogKind.Alert) where TViewModel : DialogViewModelBase => await DialogInstance.ProcessAndShowFeedbackDialog(viewModel, dialogKind);
        
    }
}
