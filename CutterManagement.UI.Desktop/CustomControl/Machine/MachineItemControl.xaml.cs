using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Interaction logic for MachineItemControl.xaml
    /// </summary>
    public partial class MachineItemControl : UserControl
    {
        public MachineItemControl()
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
