using CutterManagement.Core;
using CutterManagement.Core.Services;
using System.Windows.Controls;

namespace CutterManagement.UI.Desktop
{
    public class DialogService : IDialogService
    {
        private static IDictionary<Type, Type> _dialogMappings = new Dictionary<Type, Type>();
        private DialogWindow _dialogWindow;
        private FeedbackDialogWindow _feedbackDialogWindow;

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

        public void ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogWindowCloseRequest
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

        public void ShowFeedback(object viewModel)
        {
            _feedbackDialogWindow.Owner = _dialogWindow;

            var dataContext = new FeedbackDialogWindowViewModel();

            dataContext.Message = ((DialogViewModelBase)viewModel).Message;

            dataContext.IsMessageSuccess = ((DialogViewModelBase)viewModel).IsMessageSuccess;

            _feedbackDialogWindow.DataContext = dataContext;

            _feedbackDialogWindow.ShowDialog();
        }

        public static void InvokeDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogWindowCloseRequest => new DialogService().ShowDialog(viewModel);
        
    }
}
