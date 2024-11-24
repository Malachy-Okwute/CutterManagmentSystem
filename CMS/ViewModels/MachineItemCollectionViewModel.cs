using System.Collections.ObjectModel;

namespace CMS
{
    /// <summary>
    /// View model for <see cref="MachineItemCollectionControl"/>
    /// </summary>
    public class MachineItemCollectionViewModel : ViewModelBase
    {
        private readonly MachineDataManager _machineData;

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
        public MachineItemCollectionViewModel(MachineDataManager machineData)
        {
            // Set machine items 
            _ringItems = new ObservableCollection<MachineItemViewModel>()
            {
                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },
            };

            // Set machine items 
            _pinItems = new ObservableCollection<MachineItemViewModel>()
            {
                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },

                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    CurrentCutterNumber = "12345-6789X",
                    CurrentRunningPartNumber = "12345678",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-21-2024 ~ 2:19 PM"
                },
            };

            _machineData = machineData;
        }

        public void AddMachineToRings(MachineItemViewModel machine)
        {
            foreach (var item in _machineData.Machines)
            {
                
            }

            if (_ringItems.Contains(machine) is false)
                _ringItems.Add(machine);
        }

        public void AddMachineToPins(MachineItemViewModel machine)
        {
            if (_pinItems.Contains(machine) is false)
                _pinItems.Add(machine);
        }

        public void RemoveMachineFromRings(MachineItemViewModel machine)
        {
            if (_ringItems.Contains(machine))
                _ringItems.Remove(machine);
        }

        public void RemoveMachineFromPins(MachineItemViewModel machine)
        {
            if (_pinItems.Contains(machine))
                _pinItems.Remove(machine);
        }

    }
}
