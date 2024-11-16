using System.Windows;
using System.Windows.Media.Animation;

namespace CMS
{
    public class WindowBehaviors 
    {
        // Using a DependencyProperty as the backing store for WindowLoadedBehavior.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WindowLoadedBehaviorProperty =
            DependencyProperty.Register("WindowLoadedBehavior", typeof(bool), typeof(DependencyObject), new PropertyMetadata(default(bool), OnPropertyChanged));

        /// <summary>
        /// Get value of this attached property
        /// </summary>
        /// <param name="element">The visual element this property is attached to</param>
        /// <returns></returns>
        public static bool GetWindowLoadedBehavior(DependencyObject element)
        {
            return (bool)element.GetValue(WindowLoadedBehaviorProperty);
        }

        /// <summary>
        /// Sets the value from view to use or not use this attached behavior
        /// </summary>
        /// <param name="element">The visual element this property is attached to</param>
        /// <param name="value">Boolean value determining if this attached behavior is to be used or not</param>
        public static void SetWindowLoadedBehavior(DependencyObject element, bool value)
        {
            element.SetValue(WindowLoadedBehaviorProperty, value);
        }

        /// <summary>
        /// Property changed call back
        /// </summary>
        /// <param name="sender">The source of this event</param>
        /// <param name="e">Event args</param>
        private static void OnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // Make sure this behavior is for windows only
            if(sender is not Window window) return;

            // Hook into window loaded event
            window.Loaded += delegate
            {
                // Animate and fade window into view
                if(window is CMSSplashWindow)
                   Animations.Fade(window, AnimationEasingKind.QuinticEase, EasingMode.EaseInOut, easingFactor: 3, duration: 0.8, to: 1);
                else
                   Animations.Fade(window, AnimationEasingKind.QuinticEase, EasingMode.EaseInOut, easingFactor: 3, duration: 0.6, to: 1);
            };
        }
    }
}
