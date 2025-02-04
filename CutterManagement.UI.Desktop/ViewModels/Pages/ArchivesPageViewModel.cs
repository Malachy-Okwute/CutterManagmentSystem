using CutterManagement.Core;
using System.Collections.ObjectModel;

namespace CutterManagement.UI.Desktop
{
    public class ArchivesPageViewModel : ViewModelBase
    {
        public ObservableCollection<PartItemViewModel> Parts { get; set; }

        public ArchivesPageViewModel()
        {
            Parts = new ObservableCollection<PartItemViewModel>
            {
                new PartItemViewModel 
                { 
                    PartNumber = "1234567890", 
                    SummaryNumber = "12345", 
                    ModelNumber = "M100", 
                    TeethCount = "10",
                    Kind = PartKind.Pinion
                },
                new PartItemViewModel 
                { 
                    PartNumber = "1234567890", 
                    SummaryNumber = "12345", 
                    ModelNumber = "M100", 
                    TeethCount = "10",
                    Kind = PartKind.Pinion
                },
                new PartItemViewModel 
                { 
                    PartNumber = "1234567890", 
                    SummaryNumber = "12345", 
                    ModelNumber = "M100", 
                    TeethCount = "10",
                    Kind = PartKind.Pinion
                },
                new PartItemViewModel 
                { 
                    PartNumber = "1234567890", 
                    SummaryNumber = "12345", 
                    ModelNumber = "M100", 
                    TeethCount = "10",
                    Kind = PartKind.Pinion
                },
                new PartItemViewModel 
                { 
                    PartNumber = "1234567890", 
                    SummaryNumber = "12345", 
                    ModelNumber = "M100", 
                    TeethCount = "10",
                    Kind = PartKind.Pinion
                },
                new PartItemViewModel 
                { 
                    PartNumber = "1234567890", 
                    SummaryNumber = "12345", 
                    ModelNumber = "M100", 
                    TeethCount = "10",
                    Kind = PartKind.Pinion
                },
            };
        }
    }
}
