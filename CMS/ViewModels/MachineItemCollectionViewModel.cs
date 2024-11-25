using Serilog;
using Serilog.Core;
using System.Collections.ObjectModel;

namespace CMS
{
    /// <summary>
    /// View model for <see cref="MachineItemCollectionControl"/>
    /// </summary>
    public class MachineItemCollectionViewModel : ViewModelBase
    {
        /// <summary>
        /// Data manager
        /// </summary>
        private readonly IMachineDataService _machineData;

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
        public MachineItemCollectionViewModel(IMachineDataService machineData)
        {
            _ringItems = new ObservableCollection<MachineItemViewModel>();
            _pinItems = new ObservableCollection<MachineItemViewModel>();
            _machineData = machineData;

        }

        public void AddMachineToRings()
        {
            foreach (var machineItem in _machineData.GetMachines())
            {
                if (machineItem.Value.MachineOwner == Department.Gear && !_ringItems.Contains(new MachineItemViewModel(machineItem.Value)))
                {
                    _ringItems.Add(new MachineItemViewModel(machineItem.Value));
                    Log.Logger.Information($"Machine { machineItem.Value.UniqueID } was added");
                }
            }
        }

        public void AddMachineToPins(MachineItemViewModel machine)
        {
            foreach (var machineItem in _machineData.GetMachines())
            {
                if (machineItem.Value.MachineOwner == Department.Pinion && !_pinItems.Contains(new MachineItemViewModel(machineItem.Value)))
                {
                    _pinItems.Add(new MachineItemViewModel(machineItem.Value));
                    Log.Logger.Information($"Machine {machineItem.Value.UniqueID} was added");
                }
            }
        }

        public void RemoveMachineFromRings(MachineItemViewModel machine)
        {
            if (_ringItems.Contains(machine))
            {
                Log.Logger.Information($"Machine {machine.MachineNumber} was removed");
                _ringItems.Remove(machine);
            }
        }

        public void RemoveMachineFromPins(MachineItemViewModel machine)
        {
            if (_pinItems.Contains(machine))
            {
                Log.Logger.Information($"Machine {machine.MachineNumber} was removed");
                _pinItems.Remove(machine);
            }
        }

    }
}
