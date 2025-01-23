using System.Windows;
using System.Windows.Media.Animation;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Application animations
    /// </summary>
    public static class Animations
    {
        /// <summary>
        /// Fades the specified element into or out of view as desired
        /// </summary>
        /// <param name="element">The element to fade in</param>
        /// <param name="animationEasingKind">The type of easing associated with this animation </param>
        /// <param name="easingMode">The desired mode of the easing function</param>
        /// <param name="easingFactor">Factor associated with some of the easing functions. eg. <see cref="BackEase.Amplitude"/>, <see cref="BounceEase.Bounciness"/> etc.</param>
        /// <param name="duration">Duration of animation</param>
        /// <param name="to">Animation end point</param>
        /// <param name="from">Animation starting point</param>
        public static void Fade(FrameworkElement element, AnimationEasingKind animationEasingKind, EasingMode easingMode, double easingFactor, double duration, double to, double from = 0)
        {
            // Create a storyboard
            Storyboard storyboard = new Storyboard();

            // The animation to run
            var animation = AnimationHelpers.DoubleAnimation(duration, from, to, animationEasingKind, easingMode, easingFactor);

            // Add animation to storyboard
            storyboard.Children.Add(animation);

            // Set property of element to animate
            Storyboard.SetTargetProperty(storyboard, new PropertyPath("Opacity"));

            // Start animation 
            storyboard.Begin(element);
        }

        /// <summary>
        /// Fades a <see cref="FrameworkElement"/> out of view with animation 
        /// </summary>
        /// <param name="element">The element to fade out of view</param>
        /// <returns><see cref="Task"/></returns>
        public static async Task FadeElementOutOfView(FrameworkElement element)
        {
            // TODO: Simplify
            Fade(element, AnimationEasingKind.QuinticEase, EasingMode.EaseInOut, easingFactor: 3, duration: 0.6, to: 0, from: 1);

            // Allow time for animation to play
            await Task.Delay(TimeSpan.FromSeconds(0.6));

            await Task.CompletedTask;
        }

    }
}
