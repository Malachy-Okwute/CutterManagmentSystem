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
            StatusMessage = "IsRunning";
            CutterNumber = "X12345-67890";
            PartNumber = "123456789";
            Count = "1000";
            FrequencyCheckResult = "PASSED";
            DateTimeLastModified = "10-10-2024 ~ 12:36 AM";
        }
    }
}
