using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    public class TextInputAttachedProperty
    {
        public static bool GetAllowNumericalValues(DependencyObject obj)
        {
            return (bool)obj.GetValue(AllowNumericalValuesProperty);
        }

        public static void SetAllowNumericalValues(DependencyObject obj, bool value)
        {
            obj.SetValue(AllowNumericalValuesProperty, value);
        }

        // Using a DependencyProperty as the backing store for AllowNumericalValues.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AllowNumericalValuesProperty =
            DependencyProperty.RegisterAttached("AllowNumericalValues", typeof(bool), typeof(TextInputAttachedProperty), new PropertyMetadata(OnPropertyValueChanged));

        private static void OnPropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox && textBox is not null)
            {
                textBox.Loaded += (ss, e) => textBox.PreviewTextInput += TextBox_PreviewTextInput;

                textBox.PreviewKeyDown += (ss, e) => e.Handled = e.Key.Equals(Key.Space) ? true : false;
            }
        }

        private static void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // Predict the resulting text after input
                string newText = textBox.Text.Insert(textBox.SelectionStart, e.Text);

                // Regex: optional +/-, digits, optional decimal, digits
                bool isInputValid = Regex.IsMatch(newText, @"^[-]?(\d+(\.\d*)?|\.\d+)?$");

                e.Handled = !isInputValid;
            }
        }
    }
}
