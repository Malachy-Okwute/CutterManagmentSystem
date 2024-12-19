using System.Collections.ObjectModel;

namespace CutterManagement.UI.Desktop
{
    public class HomePageViewModel : ViewModelBase
    {
        public readonly MachineItemCollectionViewModel MachineItemCollection;

        public HomePageViewModel(MachineItemCollectionViewModel machineItemCollection)
        {
            MachineItemCollection = machineItemCollection;
        }
    }
}
