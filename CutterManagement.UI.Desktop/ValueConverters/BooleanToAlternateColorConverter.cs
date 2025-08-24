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
    internal class BooleanToAlternateColorConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? ColorHelpers.GetColor(ColorKind.BackgroundColor3) : ColorHelpers.GetColor(ColorKind.BackgroundColor1);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
