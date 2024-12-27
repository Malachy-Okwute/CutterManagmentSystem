using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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
    /// Interaction logic for PasswordEntryControl.xaml
    /// </summary>
    public partial class PasswordEntryControl : UserControl
    {
        public PasswordEntryControl()
        {
            InitializeComponent();
        }

        public string PlaceHolderIcon
        {
            get { return (string)GetValue(PlaceHolderIconProperty); }
            set { SetValue(PlaceHolderIconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlaceHolderIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceHolderIconProperty =
            DependencyProperty.Register("PlaceHolderIcon", typeof(string), typeof(PasswordEntryControl), new PropertyMetadata(default));


        public string PlaceHolderText
        {
            get { return (string)GetValue(PlaceHolderTextProperty); }
            set { SetValue(PlaceHolderTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlaceHolderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceHolderTextProperty =
            DependencyProperty.Register("PlaceHolderText", typeof(string), typeof(PasswordEntryControl), new PropertyMetadata(default));
    }
}
