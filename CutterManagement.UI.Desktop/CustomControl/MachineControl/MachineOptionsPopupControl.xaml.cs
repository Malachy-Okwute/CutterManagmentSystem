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

            // Listen for when this control loses focus
            LostFocus += MachineOptionsPopupControl_LostFocus;
        }

        /// <summary>
        /// Hide pop up control when it loses focus
        /// </summary>
        /// <param name="sender">The source of this event</param>
        /// <param name="e">Event args</param>
        private void MachineOptionsPopupControl_LostFocus(object sender, RoutedEventArgs e)
        {
            // Unhook the current event 
            LostFocus -= MachineOptionsPopupControl_LostFocus;

            // Close pop up control
            ((MachineItemViewModel)DataContext).IsPopupOpen = false;

            // Continue listening for when this control loses focus
            LostFocus += MachineOptionsPopupControl_LostFocus;
        }
    }
}
