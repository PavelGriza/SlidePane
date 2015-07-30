using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace SlidePaneControl.Extensions
{
    public static class AnimationExtensions
    {
        private const double SpeedCoefficient = 0.75;

        public static void Animate(this UIElement element, double from, double to, double fullDistance)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            var transform = new TranslateTransform();
            element.RenderTransform = transform;

            var xAnimation = new DoubleAnimation
            {
                From = from,
                Duration = TimeSpan.FromMilliseconds(GetAnimationSpeed(from, to, fullDistance)),
                To = to,
                EnableDependentAnimation = true,
                EasingFunction = new SineEase() { EasingMode = EasingMode.EaseOut }
            };

            Storyboard.SetTarget(xAnimation, transform);
            Storyboard.SetTargetProperty(xAnimation, "X");

            var storyboard = new Storyboard();
            storyboard.Children.Add(xAnimation);

            storyboard.Begin();
        }

        /// <summary>
        /// Calculate the time for sliding animation
        /// </summary>
        private static double GetAnimationSpeed(double from, double to, double fullDistance)
        {
            if (from.Equals(to)) // from == to
            {
                return 0;
            }

            var speed = (Math.Abs(to - from) / fullDistance) * fullDistance * SpeedCoefficient;

            return speed;
        }
    }
}
