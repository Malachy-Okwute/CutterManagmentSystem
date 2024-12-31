using CutterManagement.Core;
using CutterManagement.DataAccess;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Interaction logic for MachineItemCollectionControl.xaml
    /// </summary>
    public partial class MachineItemCollectionControl : UserControl
    {

        /// <summary>
        /// Default constructor
        /// </summary>
        public MachineItemCollectionControl()
        {
            InitializeComponent();

            // Hook into items-control size changed event
            Items.SizeChanged += (s, e) =>
            {
                // Get the content present of items control
                ContentPresenter itemsControlContentPresenter = (ContentPresenter)Items.ItemContainerGenerator.ContainerFromIndex(0);

                //TODO: Add a dummy data if we don't have any item on the list

                // make sure we have content presenter
                if (itemsControlContentPresenter is not null)
                {
                    // Get machine-item-control
                    MachineItemControl machineItemControl = (MachineItemControl)VisualTreeHelper.GetChild(itemsControlContentPresenter, 0);

                    #region Label Width

                    // Set data label width to match the items they are labeling [Pins]
                    PinsItemLabel.MachineIDLabel.Width = machineItemControl.MachineNumber.ActualWidth;
                    PinsItemLabel.MachineStatusLabel.Width = machineItemControl.MachineStatus.ActualWidth;
                    PinsItemLabel.CutterNumberLabel.Width = machineItemControl.CutterNumber.ActualWidth;
                    PinsItemLabel.PartNumberLabel.Width = machineItemControl.CurrentRunningPartNumber.ActualWidth;
                    PinsItemLabel.CountLabel.Width = machineItemControl.ProducedPartCount.ActualWidth;
                    PinsItemLabel.ResultLabel.Width = machineItemControl.ResultOfLastPartChecked.ActualWidth;
                    PinsItemLabel.DateAndTimeStampLabel.Width = machineItemControl.DateAndTimeOfLastCheck.ActualWidth;
                    // [Gears]
                    GearsItemLabel.MachineIDLabel.Width = machineItemControl.MachineNumber.ActualWidth;
                    GearsItemLabel.MachineStatusLabel.Width = machineItemControl.MachineStatus.ActualWidth;
                    GearsItemLabel.CutterNumberLabel.Width = machineItemControl.CutterNumber.ActualWidth;
                    GearsItemLabel.PartNumberLabel.Width = machineItemControl.CurrentRunningPartNumber.ActualWidth;
                    GearsItemLabel.CountLabel.Width = machineItemControl.ProducedPartCount.ActualWidth;
                    GearsItemLabel.ResultLabel.Width = machineItemControl.ResultOfLastPartChecked.ActualWidth;
                    GearsItemLabel.DateAndTimeStampLabel.Width = machineItemControl.DateAndTimeOfLastCheck.ActualWidth;

                    #endregion

                }
            };

            // Listen out for when pop is visible or not visible
            PopupControl.IsVisibleChanged += PopupControl_IsVisibleChanged;
        }

        /// <summary>
        /// Pop-up control visibility changed event 
        /// 
        /// <para>
        /// This is used to set to location of where pop-up control should appear.
        /// Ideal location is set to be anywhere within app window 
        /// </para>
        /// </summary>
        /// <param name="sender">Origin of this event</param>
        /// <param name="e">Event args</param>
        private void PopupControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Do nothing if we're not visible 
            if (e.NewValue is false)
            { 
                return; 
            }

            // Make sure pop up control size is calculated properly
            if(PopupControl.ActualWidth == 0 || PopupControl.ActualHeight == 0)
            {
                PopupControl.UpdateLayout();
            }

            // Focus on pop-up control
            PopupControl.Focus();

            // Get mouse location relative to pop up container
            Point mousePointerPosition = Mouse.GetPosition(PopupControlContainer);

            // Account for bottom right corner
            if (ItemsContainer.ActualWidth - mousePointerPosition.X < PopupControl.ActualWidth && ItemsContainer.ActualHeight - mousePointerPosition.Y < PopupControl.ActualHeight)
            {
                Canvas.SetLeft(PopupControl, (mousePointerPosition.X - 1) - PopupControl.ActualWidth);
                Canvas.SetTop(PopupControl, (mousePointerPosition.Y - 1) - PopupControl.ActualHeight);
            }
            // Account for right edge
            else if (ItemsContainer.ActualWidth - mousePointerPosition.X < PopupControl.ActualWidth)
            {
                Canvas.SetLeft(PopupControl, (mousePointerPosition.X - 1) - PopupControl.ActualWidth);
                Canvas.SetTop(PopupControl, (mousePointerPosition.Y - 1));
            }
            // Account for top edge
            else if (ItemsContainer.ActualHeight - mousePointerPosition.Y < PopupControl.ActualHeight)
            {
                Canvas.SetLeft(PopupControl, (mousePointerPosition.X + 1));
                Canvas.SetTop(PopupControl, (mousePointerPosition.Y + 1) - PopupControl.ActualHeight);
            }
            // If pop up wont overflow, set position
            else
            {
                Canvas.SetLeft(PopupControl, (mousePointerPosition.X + 1));
                Canvas.SetTop(PopupControl, (mousePointerPosition.Y + 1));
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => ((MachineItemCollectionViewModel)DataContext).IsConfigurationFormOpen = false;
        
    }
}
