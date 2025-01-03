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
    /// Interaction logic for UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        public UsersPage()
        {
            InitializeComponent();


            Items.SizeChanged += (s, e) =>
            {

                // Get the content present of items control
                ContentPresenter itemsControlContentPresenter = (ContentPresenter)Items.ItemContainerGenerator.ContainerFromIndex(0);

                //TODO: Add a dummy data with a message showing that list is empty if we don't have any item on the list

                // make sure we have content presenter
                if (itemsControlContentPresenter is not null)
                {
                    // Get machine-item-control
                    UserItemControl userItemControl = (UserItemControl)VisualTreeHelper.GetChild(itemsControlContentPresenter, 0);

                    #region Label Width

                    // Set data label width to match the items they are labeling
                    UserItemLabel.UserInitialsLabel.Width = userItemControl.UserInitials.ActualWidth;
                    UserItemLabel.UserFullNameLabel.Width = userItemControl.UserFullName.ActualWidth;
                    UserItemLabel.UserShiftLabel.Width = userItemControl.Shift.ActualWidth;

                    #endregion

                }
            };
        }
    }
}
