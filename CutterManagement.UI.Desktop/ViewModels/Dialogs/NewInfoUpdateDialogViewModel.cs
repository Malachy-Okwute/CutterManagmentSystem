using CutterManagement.Core;
using CutterManagement.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutterManagement.UI.Desktop
{
    public class NewInfoUpdateDialogViewModel : DialogViewModelBase, IDialogWindowCloseRequest
    {
        public event EventHandler<DialogWindowCloseRequestedEventArgs> DialogWindowCloseRequest;


    }
}
