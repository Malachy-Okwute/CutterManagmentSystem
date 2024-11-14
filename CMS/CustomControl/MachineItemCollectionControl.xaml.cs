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

namespace CMS
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
            
            // Set data-context
            DataContext = new MachineItemCollectionViewModel();

            // Hook into items-control size changed event
            Items.SizeChanged += (s, e) =>
            {
                // Get the content present of items control
                ContentPresenter itemsControlContentPresenter = (ContentPresenter)Items.ItemContainerGenerator.ContainerFromIndex(0);

                // Get machine-item-control
                MachineItemControl machineItemControl = (MachineItemControl)VisualTreeHelper.GetChild(itemsControlContentPresenter, 0);

                // Set data label width to match the items they are labeling
                MachineIDLabel.Width = machineItemControl.MachineNumber.ActualWidth;
                MachineStatusLabel.Width = machineItemControl.MachineStatus.ActualWidth;
                CutterNumberLabel.Width = machineItemControl.CutterNumber.ActualWidth;
                PartNumberLabel.Width = machineItemControl.CurrentRunningPartNumber.ActualWidth;
                CountLabel.Width = machineItemControl.ProducedPartCount.ActualWidth;
                ResultLabel.Width = machineItemControl.ResultOfLastPartChecked.ActualWidth;
                DateAndTimeStampLabel.Width = machineItemControl.DateAndTimeOfLastCheck.ActualWidth;
            };
        }
    }
}
