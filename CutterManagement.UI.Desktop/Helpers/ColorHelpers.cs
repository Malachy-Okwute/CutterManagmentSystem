using System.Windows.Media;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Kinds of color 
    /// </summary>
    public enum ColorKind
    {
        BackgroundColor1, BackgroundColor2, BackgroundColor3, ForegroundColor1, ForegroundColor2, ForegroundColor3, AccentColor1, AccentColor2, AccentColor3, AccentColor4, AccentColor5, AccentColor6,
    }

    /// <summary>
    /// Color helper class
    /// </summary>
    public static class ColorHelpers
    {
        /// <summary>
        /// Provides a <see cref="SolidColorBrush"/> of the kind of color specified
        /// </summary>
        /// <param name="color">The kind of color to get</param>
        /// <returns><see cref="SolidColorBrush"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Exception that gets thrown when invalid color is specified
        /// </exception>
        public static SolidColorBrush GetColor(ColorKind color)
        {
            switch (color)
            {
                case ColorKind.BackgroundColor1:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#ffffff")!;

                case ColorKind.BackgroundColor2:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#525298")!;

                case ColorKind.BackgroundColor3:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#eeeef2")!;

                case ColorKind.ForegroundColor1:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#ffffff")!;

                case ColorKind.ForegroundColor2:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#525298")!;

                case ColorKind.ForegroundColor3:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#bdbdbd")!;

                case ColorKind.AccentColor1:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#ffff00")!;

                case ColorKind.AccentColor2:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#ff6b6c")!;

                case ColorKind.AccentColor3:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#5cffaa")!;

                case ColorKind.AccentColor4:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#ffb519")!;

                case ColorKind.AccentColor5:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#24b17c")!;

                case ColorKind.AccentColor6:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#a0a0a0")!;

                default:
                    throw new ArgumentOutOfRangeException("Color not configured yet");
            }
        }
    }
}
