namespace CutterManagement.Core
{
    public static class MachinesDefaultData
    {
        public static List<MachineDataModel> GenerateDefaultMachineItems()
        {
            return new List<MachineDataModel> 
            { 
                new MachineDataModel
                {
                    MachineNumber = "000",
                    MachineSetId = "111",
                    Owner = Department.Gear,
                    Status = MachineStatus.IsIdle,
                    FrequencyCheckResult = FrequencyCheckResult.SETUP, 
                    DateTimeLastModified = DateTime.UtcNow,
                    DateCreated = DateTime.UtcNow,
                }
            };
        }
    }
}
