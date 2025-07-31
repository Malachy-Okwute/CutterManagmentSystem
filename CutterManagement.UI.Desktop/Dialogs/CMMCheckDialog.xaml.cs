using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Interaction logic for CMMCheckDialog.xaml
    /// </summary>
    public partial class CMMCheckDialog : UserControl
    {
        public CMMCheckDialog()
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

        #region Old Code

        //private void DecimalTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if(sender is TextBox control)
        //    {
        //        control.Text = ValidateInput(control.Text);

        //        control.CaretIndex = control.Text.Length;
        //    }
        //}

        //public string ValidateInput(string input)
        //{
        //    input = input.Trim();

        //    if (string.IsNullOrEmpty(input))
        //    {
        //        return input = string.Empty;
        //    }

        //    char inputChar = input[input.Length - 1];

        //    if(char.IsDigit(inputChar) is false && (inputChar != '-' && inputChar != '.'))
        //    {
        //        return input = input.Remove(input.Length - 1);
        //    }

        //    if (input == "00" && input.Length > 1 && input[0] == '0')
        //    {
        //        return input = input.Remove(input.Length - 1);
        //    }

        //    if (char.IsLetter(input[input.Length - 1]))
        //    {
        //        return input = input.Remove(input.Length - 1);
        //    }

        //    if (char.IsDigit(inputChar) is false && (inputChar == '-' || inputChar == '.'))
        //    {
        //        if (inputChar == '-' && input.Length > 1)
        //        {
        //            input = input.Remove(input.Length - 1);
        //        }

        //        if (inputChar == '.' && input.Count(i => i == inputChar) > 1)
        //        {
        //            input = input.Remove(input.Length - 1);
        //        }
        //    }

        //    if (input.Contains("."))
        //    {
        //        string decimalPortion = input.Substring(input.IndexOf('.'));

        //        if(input.IndexOf('.') == 0)
        //        {
        //            input = input.Insert(0, "0");
        //        }
        //        else if((input.IndexOf('-') == 0 && input.IndexOf('.') == 1))
        //        {
        //            input = input.Insert(1, "0");
        //        }

        //        //input = decimalPortion.Length > 3 ? input.Remove(input.Length - 1) : input;
        //    }

        //    return input;
        //}

        #endregion
    }
}
