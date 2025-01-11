using CutterManagement.Core;
using CutterManagement.Core.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace CutterManagement.UI.Desktop
{
    public class DialogService : IDialogService
    {
        private static IDictionary<Type, Type> _dialogMappings = new Dictionary<Type, Type>();
        private DialogWindow _dialogWindow;

        public DialogService()
        {
            _dialogWindow = new DialogWindow();
        }

        public static void RegisterDialog<TViewModel, TView>() where TViewModel : IDialogWindowCloseRequested
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

        public void ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogWindowCloseRequested
        {
            Type viewType = _dialogMappings[typeof(TViewModel)];

            object? dialog = Activator.CreateInstance(viewType);

            if(dialog == null) return;

            EventHandler<DialogWindowCloseRequestedEventArgs>? dialogCallback = null;
            dialogCallback += (s, e) =>
            {
                viewModel.DialogWindowCloseRequested -= dialogCallback;

                if(e.DialogResult.HasValue)
                {
                    _dialogWindow.DialogResult = e.DialogResult;
                }
                else
                {
                    _dialogWindow.Close();
                }
            };

            viewModel.DialogWindowCloseRequested += dialogCallback;

            ((UserControl)dialog).DataContext = viewModel;
            
            _dialogWindow.ContentControl.Content = dialog;

            _dialogWindow.ShowDialog();
        }

        public static void InvokeDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogWindowCloseRequested
        {
            var test = new DialogService();
            test.ShowDialog(viewModel);
        }
    }
}
