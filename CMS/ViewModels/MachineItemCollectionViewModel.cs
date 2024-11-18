using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace CMS
{
    /// <summary>
    /// View model for <see cref="MachineItemCollectionControl"/>
    /// </summary>
    public class MachineItemCollectionViewModel : ViewModelBase
    {
        /// <summary>
        /// Collection of <see cref="MachineItemControl"/>
        /// </summary>
        private ObservableCollection<MachineItemViewModel> _items;

        /// <summary>
        /// Collection of <see cref="MachineItemControl"/>
        /// </summary>
        public ObservableCollection<MachineItemViewModel> Items 
        {
            get => _items;
            set => _items = value;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public MachineItemCollectionViewModel()
        {
            // Set machine items 
            _items = new ObservableCollection<MachineItemViewModel>()
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
        }
    }
}
