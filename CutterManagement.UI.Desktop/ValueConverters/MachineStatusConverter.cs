using CutterManagement.Core;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace CutterManagement.UI.Desktop
{
    public class MachineStatusConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Provides this converter as value
        /// </summary>
        /// <returns><see cref="MachineStatusConverter"/></returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        /// <summary>
        /// Takes <see cref="MachineStatus"/> and converts it into an indicator in the view
        /// <para>
        /// Indicator such as :- <br/>
        /// <see cref="RippleEffectIndicatorControl"/>  <br/>
        /// <see cref="WarningIndicatorControl"/>  <br/>
        /// <see cref="MaintenanceIndicatorControl"/>
        /// </para>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>
        /// <see cref="RippleEffectIndicatorControl"/>  <br/>
        /// <see cref="WarningIndicatorControl"/>  <br/>
        /// <see cref="MaintenanceIndicatorControl"/>
        /// </returns>
        /// <exception cref="InvalidOperationException">Exception that is thrown when <see cref="MachineStatus"/> that is not converted is used</exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch((MachineStatus)value)
            {
                case MachineStatus.IsRunning:
                    return new RippleEffectIndicatorControl();

                case MachineStatus.Warning:
                    return new WarningIndicatorControl();

                case MachineStatus.IsDownForMaintenance:
                    return new MaintenanceIndicatorControl();

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
