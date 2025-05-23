using System.Windows.Media;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Kinds of color 
    /// </summary>
    public enum ColorKind
    {
        BackgroundColor1, BackgroundColor2, BackgroundColor3, BackgroundColor4, BackgroundColor5, BackgroundColor6, BackgroundColor7,
        ForegroundColor1, ForegroundColor2, ForegroundColor3, ForegroundColor4, ForegroundColor5, ForegroundColor6, ForegroundColor7, ForegroundColor8,
        ButtonHoverBackgroundColor1,
        BorderColor1, BorderColor2, BorderColor3, BorderColor4, BorderColor5, BorderColor6,
        AccentColor1, AccentColor2, AccentColor3, AccentColor4, AccentColor5, AccentColor6, AccentColor7,
        SuccessBackgroundColorBrush, WarningBackgroundColorBrush, ErrorBackgroundColorBrush,
        SuccessForegroundColorBrush, SuccessIconForegroundColorBrush, WarningForegroundColorBrush, WarningIconForegroundColorBrush, ErrorForegroundColorBrush, ErrorIconForegroundColorBrush, 
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
                // Background colors
                case ColorKind.BackgroundColor1:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#ffffff")!;

                case ColorKind.BackgroundColor2:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#f6f7f9")!;

                case ColorKind.BackgroundColor3:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#e8ebee")!;
                    
                case ColorKind.BackgroundColor4:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#eaeaea")!;
                   
                case ColorKind.BackgroundColor5:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#3d6cf6")!;
                    
                case ColorKind.BackgroundColor6:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#f7d8d6")!;
                    
                case ColorKind.BackgroundColor7:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#4e4e64")!;
                
                // Foreground colors
                case ColorKind.ForegroundColor1:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#000000")!;

                case ColorKind.ForegroundColor2:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#bbbbbb")!;

                case ColorKind.ForegroundColor3:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#4e4e64")!;
                
                case ColorKind.ForegroundColor4:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#3d6cf6")!;

                case ColorKind.ForegroundColor5:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#f62b4f")!;

                case ColorKind.ForegroundColor6:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#8f8f94")!;
                
                case ColorKind.ForegroundColor7:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#ffffff")!;

                case ColorKind.ForegroundColor8:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#a05859")!;
                
                // Button hover color
                case ColorKind.ButtonHoverBackgroundColor1:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#4e4e64")!;

                // Border colors                
                case ColorKind.BorderColor1:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#e6e8e9")!;

                case ColorKind.BorderColor2:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#edefef")!;

                case ColorKind.BorderColor3:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#3d6cf6")!;

                case ColorKind.BorderColor4:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#f62b4f")!;

                case ColorKind.BorderColor5:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#4e4e64")!;

                case ColorKind.BorderColor6:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#bbbbbb")!;

                // Accent colors                
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

                case ColorKind.AccentColor7:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#dddddd")!;

                // Success | Warning | Error Background
                case ColorKind.SuccessBackgroundColorBrush:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#def2d6")!;

                case ColorKind.WarningBackgroundColorBrush:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#f8f3d6")!;

                case ColorKind.ErrorBackgroundColorBrush:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#ebc8c4")!;

                // Success | Warning | Error Foreground
                case ColorKind.SuccessForegroundColorBrush:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#586c50")!;

                case ColorKind.SuccessIconForegroundColorBrush:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#adc1a5")!;

                case ColorKind.WarningForegroundColorBrush:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#937d4b")!;

                case ColorKind.WarningIconForegroundColorBrush:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#d9d5a5")!;

                case ColorKind.ErrorForegroundColorBrush:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#b63947")!;

                case ColorKind.ErrorIconForegroundColorBrush:
                    return (SolidColorBrush)new BrushConverter().ConvertFromString("#ca9b9b")!;

                default:
                    throw new ArgumentOutOfRangeException("Color not configured yet");
            }
        }
    }
}
