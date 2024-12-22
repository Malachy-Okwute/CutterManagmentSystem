using CutterManagement.Core;
using CutterManagement.DataAccess;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Design model for <see cref="MachineItemCollectionControl"/>
    /// </summary>
    public class MachineItemCollectionDesignModel : MachineItemCollectionViewModel
    {
        /// <summary>
        /// A singleton instance of this design-model
        /// </summary>
        public static readonly MachineItemCollectionDesignModel Instance = new(new DataAccessServiceFactory());

        /// <summary>
        /// Default constructor
        /// </summary>
        public MachineItemCollectionDesignModel(IDataAccessServiceFactory dataFactory) : base(dataFactory) 
        {
            //new PartDataModel { Id = "123456789" };

            //new CutterDataModel { Id = "X12345" };

            // design-time dummy data
            RingItems = new ObservableCollection<MachineItemViewModel>
            {
                new MachineItemViewModel()
                {
                    MachineNumber = "123",
                    StatusMessage = "IsRunning",
                    CutterNumber = "X12345-67890",
                    PartNumber = "123456789",
                    Count = "1000",
                    //ResultOfLastPartChecked = "PASSED",
                    //DateAndTimeOfLastCheck = "10-10-2024 ~ 12:36 AM"
                },
                new MachineItemViewModel()
                {
                    MachineNumber = "123",
                    StatusMessage = "IsRunning",
                    CutterNumber = "X12345-67890",
                    PartNumber = "123456789",
                    Count = "1000",
                    //ResultOfLastPartChecked = "PASSED",
                    //DateAndTimeOfLastCheck = "10-10-2024 ~ 12:36 AM"
                },
                new MachineItemViewModel()
                {
                    MachineNumber = "123",
                    StatusMessage = "IsRunning",
                    CutterNumber = "X12345-67890",
                    PartNumber = "123456789",
                    Count = "1000",
                    //ResultOfLastPartChecked = "PASSED",
                    //DateAndTimeOfLastCheck = "10-10-2024 ~ 12:36 AM"
                },
                new MachineItemViewModel()
                {
                    MachineNumber = "123",
                    StatusMessage = "IsRunning",
                    CutterNumber = "X12345-67890",
                    PartNumber = "123456789",
                    Count = "1000",
                    //ResultOfLastPartChecked = "PASSED",
                    //DateAndTimeOfLastCheck = "10-10-2024 ~ 12:36 AM"
                },
                new MachineItemViewModel()
                {
                    MachineNumber = "123",
                    StatusMessage = "IsRunning",
                    CutterNumber = "X12345-67890",
                    PartNumber = "123456789",
                    Count = "1000",
                    //ResultOfLastPartChecked = "PASSED",
                    //DateAndTimeOfLastCheck = "10-10-2024 ~ 12:36 AM"
                },
                new MachineItemViewModel()
                {
                    MachineNumber = "123",
                    StatusMessage = "IsRunning",
                    CutterNumber = "X12345-67890",
                    PartNumber = "123456789",
                    Count = "1000",
                    //ResultOfLastPartChecked = "PASSED",
                    //DateAndTimeOfLastCheck = "10-10-2024 ~ 12:36 AM"
                },
                new MachineItemViewModel()
                {
                    MachineNumber = "123",
                    StatusMessage = "IsRunning",
                    CutterNumber = "X12345-67890",
                    PartNumber = "123456789",
                    Count = "1000",
                    //ResultOfLastPartChecked = "PASSED",
                    //DateAndTimeOfLastCheck = "10-10-2024 ~ 12:36 AM"
                },
                new MachineItemViewModel()
                {
                    MachineNumber = "123",
                    StatusMessage = "IsRunning",
                    CutterNumber = "X12345-67890",
                    PartNumber = "123456789",
                    Count = "1000",
                    //ResultOfLastPartChecked = "PASSED",
                    //DateAndTimeOfLastCheck = "10-10-2024 ~ 12:36 AM"
                },
                new MachineItemViewModel()
                {
                    MachineNumber = "123",
                    StatusMessage = "IsRunning",
                    CutterNumber = "X12345-67890",
                    PartNumber = "123456789",
                    Count = "1000",
                    //ResultOfLastPartChecked = "PASSED",
                    //DateAndTimeOfLastCheck = "10-10-2024 ~ 12:36 AM"
                },
                new MachineItemViewModel()
                {
                    MachineNumber = "123",
                    StatusMessage = "IsRunning",
                    CutterNumber = "X12345-67890",
                    PartNumber = "123456789",
                    Count = "1000",
                    //ResultOfLastPartChecked = "PASSED",
                    //DateAndTimeOfLastCheck = "10-10-2024 ~ 12:36 AM"
                },
                new MachineItemViewModel()
                {
                    MachineNumber = "123",
                    StatusMessage = "IsRunning",
                    CutterNumber = "X12345-67890",
                    PartNumber = "123456789",
                    Count = "1000",
                    //ResultOfLastPartChecked = "PASSED",
                    //DateAndTimeOfLastCheck = "10-10-2024 ~ 12:36 AM"
                },
                new MachineItemViewModel()
                {
                    MachineNumber = "123",
                    StatusMessage = "IsRunning",
                    CutterNumber = "X12345-67890",
                    PartNumber = "123456789",
                    Count = "1000",
                    //ResultOfLastPartChecked = "PASSED",
                    //DateAndTimeOfLastCheck = "10-10-2024 ~ 12:36 AM"
                },
                new MachineItemViewModel()
                {
                    MachineNumber = "123",
                    StatusMessage = "IsRunning",
                    CutterNumber = "X12345-67890",
                    PartNumber = "123456789",
                    Count = "1000",
                    //ResultOfLastPartChecked = "PASSED",
                    //DateAndTimeOfLastCheck = "10-10-2024 ~ 12:36 AM"
                },
                new MachineItemViewModel()
                {
                    MachineNumber = "123",
                    StatusMessage = "IsRunning",
                    CutterNumber = "X12345-67890",
                    PartNumber = "123456789",
                    Count = "1000",
                    //ResultOfLastPartChecked = "PASSED",
                    //DateAndTimeOfLastCheck = "10-10-2024 ~ 12:36 AM"
                },
                new MachineItemViewModel()
                {
                    MachineNumber = "123",
                    StatusMessage = "IsRunning",
                    CutterNumber = "X12345-67890",
                    PartNumber = "123456789",
                    Count = "1000",
                    //ResultOfLastPartChecked = "PASSED",
                    //DateAndTimeOfLastCheck = "10-10-2024 ~ 12:36 AM"
                },
                new MachineItemViewModel()
                {
                    MachineNumber = "123",
                    StatusMessage = "IsRunning",
                    CutterNumber = "X12345-67890",
                    PartNumber = "123456789",
                    Count = "1000",
                    //ResultOfLastPartChecked = "PASSED",
                    //DateAndTimeOfLastCheck = "10-10-2024 ~ 12:36 AM"
                },
            };
        }
    }
}
