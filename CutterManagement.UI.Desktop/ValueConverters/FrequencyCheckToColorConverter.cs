using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Takes a string and convert it to a <see cref="SolidColorBrush"/>
    /// </summary>
    public class FrequencyCheckToColorConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Provides this class as value
        /// </summary>
        /// <returns><see cref="FrequencyCheckToColorConverter"/></returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty((string)value)) return null;

            switch((string)value)
            {
                case "SETUP":
                    return ColorHelpers.GetColor(ColorKind.AccentColor6);
                    
                case "PASSED":
                    return ColorHelpers.GetColor(ColorKind.AccentColor5);

                case "FAILED":
                    return ColorHelpers.GetColor(ColorKind.AccentColor2);

                default:
                    throw new InvalidOperationException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
