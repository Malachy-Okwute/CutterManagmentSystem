using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CutterManagement.UI.Desktop
{
    public class ApplicationPageConverter : IValueConverter
    {

        public static ApplicationPageConverter Instance => new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch((AppPage)value)
            {
                case AppPage.HomePage:
                    return new HomePage();

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
