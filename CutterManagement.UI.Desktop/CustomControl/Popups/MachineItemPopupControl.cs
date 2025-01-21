using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Interaction logic for MachineItemPopupControl.xaml
    /// </summary>
    public partial class MachineItemPopupControl : UserControl
    {
        public MachineItemPopupControl()
        {
            InitializeComponent();

            LostFocus += (s, e) =>
            {
                if(IsFocused is false)
                {
                    ((Popup)Parent).IsOpen = false;
                }
            };
        }
    }
}
