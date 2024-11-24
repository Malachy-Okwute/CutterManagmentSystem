using System.Collections.ObjectModel;

namespace CMS
{
    /// <summary>
    /// Design model for <see cref="MachineItemCollectionControl"/>
    /// </summary>
    public class MachineItemCollectionDesignModel : MachineItemCollectionViewModel
    {
        /// <summary>
        /// A singleton instance of this design-model
        /// </summary>
        public static readonly MachineItemCollectionDesignModel Instance = new (new MachineDataManager());

        /// <summary>
        /// Default constructor
        /// </summary>
        public MachineItemCollectionDesignModel(MachineDataManager machineDataManager) : base(machineDataManager)
        {
            // dummy data
            RingItems = new ObservableCollection<MachineItemViewModel>
            {
                new MachineItemViewModel
                {
                    MachineNumber = "123",
                    MachineStatus = true,
                    MachineStatusComment = "Running",
                    CurrentCutterNumber = "X12345-67890",
                    CurrentRunningPartNumber = "123456789",
                    ProducedPartCount = "1000",
                    ResultOfLastPartChecked = "PASSED",
                    DateAndTimeOfLastCheck = "10-10-2024 ~ 12:36 AM"
                }
            };
        }
    }
}
