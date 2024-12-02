using Serilog;
using System.Collections.ObjectModel;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="MachineItemCollectionControl"/>
    /// </summary>
    public class MachineItemCollectionViewModel : ViewModelBase
    {
        /// <summary>
        /// Collection of <see cref="MachineItemControl"/> representing rings
        /// </summary>
        private ObservableCollection<MachineItemViewModel> _ringItems;

        /// <summary>
        /// Collection of <see cref="MachineItemControl"/> representing pins
        /// </summary>
        private ObservableCollection<MachineItemViewModel> _pinItems;

        /// <summary>
        /// Collection of <see cref="MachineItemControl"/> representing rings
        /// </summary>
        public ObservableCollection<MachineItemViewModel> RingItems 
        {
            get => _ringItems;
            set => _ringItems = value;
        }

        /// <summary>
        /// Collection of <see cref="MachineItemControl"/> representing pins
        /// </summary>
        public ObservableCollection<MachineItemViewModel> PinItems
        {
            get => _pinItems;
            set => _pinItems = value;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public MachineItemCollectionViewModel()
        {
            _ringItems = new ObservableCollection<MachineItemViewModel>();
            _pinItems = new ObservableCollection<MachineItemViewModel>();
        }

    }
}
