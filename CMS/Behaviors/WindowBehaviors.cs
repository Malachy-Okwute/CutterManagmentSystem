using System.Windows;
using System.Windows.Media.Animation;

namespace CMS
{
    public class WindowBehaviors 
    {
        // Using a DependencyProperty as the backing store for WindowLoaded.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WindowLoadedProperty =
            DependencyProperty.Register("WindowLoaded", typeof(bool), typeof(DependencyObject), new PropertyMetadata(default(bool), OnPropertyChanged));

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

            bool animationCompleted = false;

            window.Loaded += delegate
            {
                if(window is CMSSplashWindow)
                   Animations.Fade(window, AnimationEasingKind.QuinticEase, EasingMode.EaseInOut, 3, 0.8, 1);
                else
                   Animations.Fade(window, AnimationEasingKind.QuinticEase, EasingMode.EaseInOut, 3, 0.4, 1);
            };

            window.Closing += (s, e) =>
            {
                if(!Animations.AnimationCompleted) 
                    e.Cancel = true;

                if (window is CMSSplashWindow)
                    Animations.Fade(window, AnimationEasingKind.QuinticEase, EasingMode.EaseInOut, 3, 0.8, 0, 1);
                else
                    Animations.Fade(window, AnimationEasingKind.QuinticEase, EasingMode.EaseInOut, 3, 0.4, 0, 1);
            };

        }
    }
}
