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
                return (bool)value ? ColorHelpers.GetColor(ColorKind.BackgroundColor1) : ColorHelpers.GetColor(ColorKind.BackgroundColor6);
            }
            else if(parameter as string is "SessionStatus")
            {
                return (bool)value ? ColorHelpers.GetColor(ColorKind.AccentColor5) : ColorHelpers.GetColor(ColorKind.ForegroundColor5);
            }
            else
            {
                return (bool)value ? ColorHelpers.GetColor(ColorKind.ForegroundColor3) : ColorHelpers.GetColor(ColorKind.ForegroundColor8);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
