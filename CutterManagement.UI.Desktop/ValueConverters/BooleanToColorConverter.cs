using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace CutterManagement.UI.Desktop
{
    internal class BooleanToColorConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(parameter == null)
            {
                return (bool)value ? ColorHelpers.GetColor(ColorKind.SuccessBackgroundColorBrush) : ColorHelpers.GetColor(ColorKind.WarningBackgroundColorBrush);
            }
            else if(parameter as string is "SessionStatus")
            {
                return (bool)value ? ColorHelpers.GetColor(ColorKind.AccentColor5) : ColorHelpers.GetColor(ColorKind.ForegroundColor5);
            }
            else
            {
                return (bool)value ? ColorHelpers.GetColor(ColorKind.SuccessForegroundColorBrush) : ColorHelpers.GetColor(ColorKind.WarningForegroundColorBrush);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
