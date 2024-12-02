namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Design model for <see cref="MachineItemControl"/>
    /// </summary>
    public class MachineItemDesignModel : MachineItemViewModel
    {
        /// <summary>
        /// A singleton instance of this design-model
        /// </summary>
        public static readonly MachineItemDesignModel Instance = new();

        /// <summary>
        /// Default constructor
        /// </summary>
        public MachineItemDesignModel() 
        {
            // Dummy data
            MachineNumber = "123";
            MachineStatusComment = "IsRunning";
            CurrentCutterNumber = "X12345-67890";
            CurrentRunningPartNumber = "123456789";
            ProducedPartCount = "1000";
            //ResultOfLastPartChecked = "PASSED";
            //DateAndTimeOfLastCheck = "10-10-2024 ~ 12:36 AM";
        }
    }
}
