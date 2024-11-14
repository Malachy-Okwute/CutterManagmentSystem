using System.Windows;
using System.Windows.Media.Animation;

namespace CMS
{
    public class WindowBehaviors : DependencyObject
    {
        // Using a DependencyProperty as the backing store for WindowLoaded.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WindowLoadedProperty =
            DependencyProperty.Register("WindowLoaded", typeof(bool), typeof(WindowBehaviors), new PropertyMetadata(default(bool), OnPropertyChanged));

        public static bool GetWindowLoaded(DependencyObject element)
        {
            return (bool)element.GetValue(WindowLoadedProperty);
        }

        public static void SetWindowLoaded(DependencyObject element, bool value)
        {
            element.SetValue(WindowLoadedProperty, value);
        }

        private static void OnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if(sender is not Window window) return;

            window.IsVisibleChanged += (s, e) =>
            {
                Animations.Fade(window, AnimationEasingKind.QuinticEase, EasingMode.EaseInOut, 3, 2, 1);
            };
        }
    }
}
