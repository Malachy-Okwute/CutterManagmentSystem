using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace CutterManagement.UI.Desktop
{
    public static class BlurHelper
    {
        public static void ApplyBlurEffect(UIElement element)
        {
            var blurEffect = new BlurEffect { Radius = 10 };
            element.Effect = blurEffect;
        }

        public static void RemoveBlurEffect(UIElement element)
        {
            element.Effect = null;
        }
    }
}
