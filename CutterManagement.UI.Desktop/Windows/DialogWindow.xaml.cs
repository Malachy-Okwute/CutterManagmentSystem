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
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        /// <summary>
        /// The view model of this window
        /// </summary>
        public DialogWindowViewModel ViewModel { get; set; }

        public DialogWindow()
        {
            InitializeComponent();

            // Set data context
            DataContext = ViewModel;
        }
    }
}
