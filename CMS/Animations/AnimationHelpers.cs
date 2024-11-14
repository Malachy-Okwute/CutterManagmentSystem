using System.Windows;
using System.Windows.Media.Animation;

namespace CMS
{
    /// <summary>
    /// A helper class for animation
    /// </summary>
    public class AnimationHelpers
    {
        /// <summary>
        /// Provides a specified easing function type
        /// </summary>
        /// <param name="easingKind">The easing function requested</param>
        /// <param name="easingMode">The desired mode of the easing function</param>
        /// <param name="easingFactor">Factor associated with some of the easing functions. eg. <see cref="BackEase.Amplitude"/>, <see cref="BounceEase.Bounciness"/> etc.</param>
        /// <returns><see cref="IEasingFunction"/></returns>
        public static IEasingFunction GetEasingFunction(AnimationEasingKind easingKind, EasingMode easingMode, double easingFactor = 0)
        {
            // Sort and return requested easing function
            switch (easingKind)
            {
                case AnimationEasingKind.BackEase:
                    return new BackEase { Amplitude = easingFactor, EasingMode = easingMode };

                case AnimationEasingKind.BounceEase:
                    return new BounceEase { Bounciness = easingFactor, EasingMode = easingMode };

                case AnimationEasingKind.CircleEase:
                    return new CircleEase { EasingMode = easingMode };

                case AnimationEasingKind.CubicEase:
                    return new CubicEase { EasingMode = easingMode };

                case AnimationEasingKind.ElasticEase:
                    return new ElasticEase { Springiness = easingFactor, EasingMode = easingMode };

                case AnimationEasingKind.ExponentialEase:
                    return new ExponentialEase { Exponent = easingFactor, EasingMode = easingMode };

                case AnimationEasingKind.PowerEase:
                    return new PowerEase { Power = easingFactor, EasingMode = easingMode };

                case AnimationEasingKind.QuadraticEase:
                    return new QuadraticEase { EasingMode = easingMode };

                case AnimationEasingKind.QuarticEase:
                    return new QuarticEase { EasingMode = easingMode };

                case AnimationEasingKind.QuinticEase:
                    return new QuinticEase { EasingMode = easingMode };

                case AnimationEasingKind.SineEase:
                    return new SineEase { EasingMode = easingMode };

                default:
                    // TODO: Set a default easing function
                    return null!;
            }
        }

        /// <summary>
        /// Basic double animation with easing function 
        /// </summary>
        /// <param name="duration">Duration of animation</param>
        /// <param name="from">Animation starting point</param>
        /// <param name="to">Animation end point</param>
        /// <param name="animationEasingKind">The type of easing associated with this animation </param>
        /// <param name="easingMode">The desired mode of the easing function</param>
        /// <param name="easingFactor">Factor associated with some of the easing functions. eg. <see cref="BackEase.Amplitude"/>, <see cref="BounceEase.Bounciness"/> etc.</param>
        /// <returns><see cref="System.Windows.Media.Animation.DoubleAnimation"/></returns>
        public static DoubleAnimation DoubleAnimation(double duration, double from, double to, AnimationEasingKind animationEasingKind, EasingMode easingMode, double easingFactor)
        {
            // Create animation object
            DoubleAnimation animation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(duration)),
                From = from,
                To = to,
                EasingFunction = GetEasingFunction(animationEasingKind, easingMode, easingFactor)
            };

            // return animation 
            return animation;
        }
    }
}
