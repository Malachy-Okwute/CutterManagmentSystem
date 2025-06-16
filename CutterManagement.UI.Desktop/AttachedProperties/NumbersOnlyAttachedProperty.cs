using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    public class NumbersOnlyAttachedProperty 
    {
        public static bool GetAcceptNumberOnly(DependencyObject obj)
        {
            return (bool)obj.GetValue(AcceptNumberOnlyProperty);
        }

        public static void SetAcceptNumberOnly(DependencyObject obj, bool value)
        {
            obj.SetValue(AcceptNumberOnlyProperty, value);
        }

        // Using a DependencyProperty as the backing store for AcceptNumberOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AcceptNumberOnlyProperty =
            DependencyProperty.RegisterAttached("AcceptNumberOnly", typeof(bool), typeof(NumbersOnlyAttachedProperty), new PropertyMetadata(OnPropertyChanged));

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is TextEntryControl control)
            {
                control.PreviewTextInput += (s, e) =>
                {
                    bool isInputValid = Regex.IsMatch(e.Text, @"^[0-9]+$");

                    e.Handled = isInputValid is false;
                };

                control.PreviewKeyDown += (s, e) => e.Handled = e.Key.Equals(Key.Space) ? true : false;
            }
        }
    }
}
