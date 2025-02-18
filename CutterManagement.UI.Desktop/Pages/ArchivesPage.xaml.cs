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
    /// Interaction logic for ArchivesPage.xaml
    /// </summary>
    public partial class ArchivesPage : Page
    {
        public ArchivesPage()
        {
            InitializeComponent();

            Items.SizeChanged += (s, e) =>
            {
                if (Items.IsLoaded is false)
                {
                    Items.UpdateLayout();
                }

                // Get the content present of items control
                ContentPresenter itemsControlContentPresenter = (ContentPresenter)Items.ItemContainerGenerator.ContainerFromIndex(0);

                // make sure we have content presenter
                if (itemsControlContentPresenter is not null)
                {
                    // Get item-control
                    PartItemControl partItemControl = (PartItemControl)VisualTreeHelper.GetChild(itemsControlContentPresenter, 0);

                    #region Label Width

                    // Set data label width to match the items they are labeling
                    PartsItemLabel.PartNumberLabel.Width = partItemControl.PartNumber.ActualWidth;
                    PartsItemLabel.SummaryNumberLabel.Width = partItemControl.SummaryNumber.ActualWidth;
                    PartsItemLabel.ModelLabel.Width = partItemControl.Model.ActualWidth;
                    PartsItemLabel.TeethCountLabel.Width = partItemControl.TeethCount.ActualWidth;
                    PartsItemLabel.TypeLabel.Width = partItemControl.PartType.ActualWidth;

                    #endregion
                }

                if (Items.Items.Count == 0)
                {
                    PartsItemLabel.PartNumberLabel.Width = 160;
                    PartsItemLabel.SummaryNumberLabel.Width = 140;
                    PartsItemLabel.ModelLabel.Width = 140;
                    PartsItemLabel.TeethCountLabel.Width = 140;
                }
            };

        }
    }
}
