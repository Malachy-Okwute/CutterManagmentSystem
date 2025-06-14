using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    /// Interaction logic for CutterSwapControl.xaml
    /// </summary>
    public partial class CutterSwapControl : UserControl
    {
        public CutterSwapControl()
        {
            InitializeComponent();
        }

        public string MachineNumber
        {
            get { return (string)GetValue(MachineNumberProperty); }
            set { SetValue(MachineNumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MachineNumber.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MachineNumberProperty =
            DependencyProperty.Register("MachineNumber", typeof(string), typeof(CutterSwapControl), new PropertyMetadata());



        public string CutterNumber
        {
            get { return (string)GetValue(CutterNumberProperty); }
            set { SetValue(CutterNumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CutterNumber.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CutterNumberProperty =
            DependencyProperty.Register("CutterNumber", typeof(string), typeof(CutterSwapControl), new PropertyMetadata());



        public string PartNumber
        {
            get { return (string)GetValue(PartNumberProperty); }
            set { SetValue(PartNumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PartNumber.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PartNumberProperty =
            DependencyProperty.Register("PartNumber", typeof(string), typeof(CutterSwapControl), new PropertyMetadata());



        public string Count
        {
            get { return (string)GetValue(CountProperty); }
            set { SetValue(CountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Count.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CountProperty =
            DependencyProperty.Register("Count", typeof(string), typeof(CutterSwapControl), new PropertyMetadata());

    }
}
