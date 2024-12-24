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
        }


        /// <summary>
        /// Pop-up control visibility changed event 
        /// 
        /// <Remarks>
        /// This is used to set to location of where pop-up control should appear.
        /// Ideal location is set to be anywhere within app window 
        /// </Remarks>
        /// </summary>
        /// <param name="sender">Origin of this event</param>
        /// <param name="e">Event args</param>
        private void PopupControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Focus on pop-up control
            PopupControl.Focus();

            // Get mouse location relative to pop up container
            Point mousePointerPosition = Mouse.GetPosition(PopupControlContainer);

            // Account for bottom right corner
            if (ItemsContainer.ActualWidth - mousePointerPosition.X < PopupControl.Width && ItemsContainer.ActualHeight - mousePointerPosition.Y < PopupControl.Height)
            {
                Canvas.SetLeft(PopupControl, (mousePointerPosition.X - 1) - PopupControl.Width);
                Canvas.SetTop(PopupControl, (mousePointerPosition.Y - 1) - PopupControl.Height);
            }
            // Account for right edge
            else if (ItemsContainer.ActualWidth - mousePointerPosition.X < PopupControl.Width)
            {
                Canvas.SetLeft(PopupControl, (mousePointerPosition.X - 1) - PopupControl.Width);
                Canvas.SetTop(PopupControl, (mousePointerPosition.Y - 1));
            }
            // Account for top edge
            else if (ItemsContainer.ActualHeight - mousePointerPosition.Y < PopupControl.Height)
            {
                Canvas.SetLeft(PopupControl, (mousePointerPosition.X + 1));
                Canvas.SetTop(PopupControl, (mousePointerPosition.Y + 1) - PopupControl.Height);
            }
            // If pop up wont overflow, set position
            else
            {
                Canvas.SetLeft(PopupControl, (mousePointerPosition.X + 1));
                Canvas.SetTop(PopupControl, (mousePointerPosition.Y + 1));
            }
        }


    }
}
