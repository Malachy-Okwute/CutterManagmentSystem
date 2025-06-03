using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Interaction logic for CutterRemovalControl.xaml
    /// </summary>
    public partial class CutterRemovalControl : UserControl
    {
        public CutterRemovalControl()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox control)
            {
                bool isInputValid = Regex.IsMatch(e.Text, @"^[0-9]+$");

                e.Handled = isInputValid is false;
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e) => e.Handled = e.Key.Equals(Key.Space) ? true : false;

    }
}
