using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Interaction logic for CutterHistoryPopupControl.xaml
    /// </summary>
    public partial class CutterHistoryPopupControl : UserControl
    {
        public CutterHistoryPopupControl()
        {
            InitializeComponent();

            LostFocus += (s, e) =>
            {
                if (IsFocused is false)
                {
                    ((Popup)Parent).IsOpen = false;
                }
            };

            ScrollViewer.ScrollChanged += (s, e) =>
            {
                var minTriggerPoint = Header.TranslatePoint(new Point(0d, 0d), this).Y + Header.ActualHeight;

                foreach (var item in Items.Items)
                {
                    if (Items.ItemContainerGenerator.ContainerFromItem(item) is FrameworkElement element)
                    {
                        if (item is CutterHistoryPopupItemViewModel data && data.IsHeader is false)
                            continue;

                        var itemLocation = element.TranslatePoint(new Point(0d, 0d), this);

                        if (itemLocation.Y <= minTriggerPoint && item is CutterHistoryPopupItemViewModel headerData && headerData.IsHeader)
                        {
                            ControlHeader = headerData.DateAndTimeOfCheck;
                        }
                    }
                }

                if (string.IsNullOrEmpty(ControlHeader) && Items.Items.Count > 0)
                {
                    ControlHeader = "Date header unavailable";
                }
                else if (string.IsNullOrEmpty(ControlHeader) && Items.Items.Count == 0)
                {
                    ControlHeader = "No history to show";
                }
            };
        }

        public string ControlHeader
        {
            get { return (string)GetValue(ControlHeaderProperty); }
            set { SetValue(ControlHeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ControlHeader.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ControlHeaderProperty =
            DependencyProperty.Register("ControlHeader", typeof(string), typeof(CutterHistoryPopupControl), new PropertyMetadata(default));
    }
}
