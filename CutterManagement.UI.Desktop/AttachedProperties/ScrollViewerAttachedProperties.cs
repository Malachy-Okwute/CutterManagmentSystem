using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    public class ScrollViewerAttachedProperties
    {
        public static double GetSetScrollSpeed(DependencyObject obj)
        {
            return (double)obj.GetValue(SetScrollSpeedProperty);
        }

        public static void SetSetScrollSpeed(DependencyObject obj, double value)
        {
            obj.SetValue(SetScrollSpeedProperty, value);
        }

        // Using a DependencyProperty as the backing store for SetScrollSpeed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SetScrollSpeedProperty =
            DependencyProperty.RegisterAttached("SetScrollSpeed", typeof(double), typeof(ScrollViewerAttachedProperties), new PropertyMetadata(OnPropertyValueChanged));

        private static void OnPropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewer scrollViewer && scrollViewer is not null)
            {
                scrollViewer.PreviewMouseWheel += (s, e) =>
                {
                    if (s is ScrollViewer scrollViewer && scrollViewer is not null && scrollViewer.IsMouseOver is true)
                    {
                        scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta / 120 * GetSetScrollSpeed(scrollViewer));
                        e.Handled = true;
                    }
                };
            }
        }
    }
}
