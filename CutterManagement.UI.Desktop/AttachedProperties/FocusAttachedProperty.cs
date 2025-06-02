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
            FrameworkElement control = (FrameworkElement)d;

            if (control is null) return;

            control.Loaded += (s, e) =>
            {
                control.Focus();
            };

            if (control.IsLoaded)
            {
                control.Focus();

                if (control is TextBox) ((TextBox)control).SelectAll();

            }
        }
    }
}
