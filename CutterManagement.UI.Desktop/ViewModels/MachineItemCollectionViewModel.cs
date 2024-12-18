using CutterManagement.Core;
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

        private IDataAccessService<MachineDataModel> _machineData;

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
        public MachineItemCollectionViewModel(IDataAccessService<MachineDataModel> machineData)
        {
            _machineData = machineData;

            _ringItems = new ObservableCollection<MachineItemViewModel>();
            _pinItems = new ObservableCollection<MachineItemViewModel>();
            LoadMachineData();
        }

        private void LoadMachineData()
        {
            if (_machineData is null) return;

            if(_machineData.GetAllEntitiesAsync().Result.Count is 0)
            {

            }
        }

    }
}
