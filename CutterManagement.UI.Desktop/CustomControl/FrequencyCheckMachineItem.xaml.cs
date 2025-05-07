using Microsoft.IdentityModel.Tokens;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Interaction logic for FrequencyCheckMachineItem.xaml
    /// </summary>
    public partial class FrequencyCheckMachineItem : UserControl
    {
        public FrequencyCheckMachineItem()
        {
            InitializeComponent();
        }

        private void CheckMark_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RadioButton parent = (RadioButton)((Border)sender).TemplatedParent;

            if(parent.IsChecked is true)
            {
                return;
            }
            else
            {
                parent.IsChecked ^= true;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = ((TextBox)sender).Text;

            bool isInputValid = int.TryParse(input, out int result);

            if (isInputValid is false && input.IsNullOrEmpty() is false)
            {
                input = input.Remove(input.Length - 1);
                ((TextBox)sender).Text = input;
                ((TextBox)sender).CaretIndex = input.Length;
            }
        }
    }
}
