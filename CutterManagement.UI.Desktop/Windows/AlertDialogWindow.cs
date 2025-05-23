using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Interaction logic for AlertDialogWindow.xaml
    /// </summary>
    public partial class AlertDialogWindow : Window
    {
        public AlertDialogWindow()
        {
            InitializeComponent();

            Owner = Application.Current.MainWindow;
            ShowInTaskbar = false;
        }
    }
}
