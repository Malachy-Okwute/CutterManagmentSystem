using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    public class DialogBase : UserControl
    {
        private DialogWindow _dialogWindow;

        private TaskCompletionSource<bool> _taskCompletionSource;

        public ICommand CloseCommand { get; set; }

        public DialogBase()
        {
            //_dialogWindow = new DialogWindow();
            //_dialogWindow.ViewModel = new DialogWindowViewModel();
            //_taskCompletionSource = new TaskCompletionSource<bool>();

            CloseCommand = new RelayCommand(()  => _dialogWindow.Close());
        }

        public Task ShowDialog<T>(T viewModel) where T : DialogViewModelBase
        {
            DispatcherService.Invoke(() =>
            {

            });

            return _taskCompletionSource.Task;
        }
    }
}
