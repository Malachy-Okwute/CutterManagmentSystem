using System.Windows;
using System.Windows.Controls;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Interaction logic for MachineOptionsPopupControl.xaml
    /// </summary>
    public partial class MachineOptionsPopupControl : UserControl
    {
        public MachineOptionsPopupControl()
        {
            InitializeComponent();

            LostFocus += MachineOptionsPopupControl_LostFocus;
        }

        private void MachineOptionsPopupControl_LostFocus(object sender, RoutedEventArgs e)
        {
            //((MachineItemCollectionViewModel)DataContext).MachineItemViewModel.IsPopupOpen = false;
        }
    }
}
