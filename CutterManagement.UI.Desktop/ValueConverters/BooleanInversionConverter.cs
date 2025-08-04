using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace CutterManagement.UI.Desktop
{
    public class BooleanInversionConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => !(bool)value;
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
