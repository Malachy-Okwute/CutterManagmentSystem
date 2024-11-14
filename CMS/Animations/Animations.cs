using System.Windows;
using System.Windows.Media.Animation;

namespace CMS
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

            // Add animation to storyboard
            storyboard.Children.Add(AnimationHelpers.DoubleAnimation(duration, from, to, animationEasingKind, easingMode, easingFactor));

            // Set property of element to animate
            Storyboard.SetTargetProperty(storyboard, new PropertyPath("Opacity"));

            // Start animation 
            storyboard.Begin(element);
        }
    }
}
