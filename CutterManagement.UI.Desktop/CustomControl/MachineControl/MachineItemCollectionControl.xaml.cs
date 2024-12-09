using System.Windows.Controls;
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

            // TODO: To be removed 
            DataContext = new MachineItemCollectionDesignModel();

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

    }
}
