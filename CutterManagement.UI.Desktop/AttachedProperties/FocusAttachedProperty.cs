using Microsoft.Extensions.Options;
using System.Windows;
using System.Windows.Controls;

namespace CutterManagement.UI.Desktop
{
    public class FocusAttachedProperty
    {


        public static bool GetSetFocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(SetFocusProperty);
        }

        public static void SetSetFocus(DependencyObject obj, bool value)
        {
            obj.SetValue(SetFocusProperty, value);
        }

        // Using a DependencyProperty as the backing store for SetFocus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SetFocusProperty =
            DependencyProperty.RegisterAttached("SetFocus", typeof(bool), typeof(FocusAttachedProperty), new PropertyMetadata(OnPropertyValueChanged));

        private static void OnPropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Control control = (Control)d;

            if(control is not null)
            {
                control.Loaded += Control_Loaded;
            }
        }

        private static void Control_Loaded(object sender, RoutedEventArgs e)
        {
            Control control = (Control)sender;

            control.Focus();
        }
    }
}
