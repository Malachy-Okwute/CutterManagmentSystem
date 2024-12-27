using System.Windows;
using System.Windows.Controls;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Password box attached property
    /// </summary>
    public class PasswordBoxAttachedProperty
    {
        public static bool GetPasswordHasContent(DependencyObject obj)
        {
            return (bool)obj.GetValue(PasswordHasContentProperty);
        }

        public static void SetPasswordHasContent(DependencyObject obj, bool value)
        {
            obj.SetValue(PasswordHasContentProperty, value);
        }

        // Using a DependencyProperty as the backing store for PasswordHasContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordHasContentProperty =
            DependencyProperty.RegisterAttached("PasswordHasContent", typeof(bool), typeof(PasswordBoxAttachedProperty), new PropertyMetadata(OnPropertyValueChanged));

        private static void OnPropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBoxControl = (PasswordBox)d;

            if (passwordBoxControl is not null)
            {
                passwordBoxControl.Loaded += PasswordBoxControl_Loaded;
            }
        }

        private static void PasswordBoxControl_Loaded(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBoxControl = (PasswordBox)sender;

            passwordBoxControl.PasswordChanged += PasswordBoxControl_PasswordChanged;

            if (string.IsNullOrEmpty(passwordBoxControl.Password))
            {
                SetPasswordHasContent(passwordBoxControl, false);
            }
            else
            {
                SetPasswordHasContent(passwordBoxControl, true);
            }
        }

        private static void PasswordBoxControl_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = (PasswordBox)sender;

            if (string.IsNullOrEmpty(passwordBox.Password))
            {
                SetPasswordHasContent(passwordBox, false);
            }
            else
            {
                SetPasswordHasContent(passwordBox, true);
            }
        }

    }
}
