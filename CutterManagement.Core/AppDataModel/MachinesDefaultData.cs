namespace CutterManagement.Core
{
    public static class MachinesDefaultData
    {
        public static void CreateDefaultMachineItem()
        {
            new MachineModel
            {
                MachineSetId = 000, 
                MachineNumber = 111,
                EntryCreatedDateTime = DateTime.Now,
                LastModifiedDateTime = DateTime.Now,
                Owner = Department.Gear,
                Status = MachineStatus.IsRunning,
                FrequencyCheckResult = FrequencyCheckResult.SETUP,
                User = new UserDataModel { FirstName = "Admin", EntryCreatedDateTime = DateTime.Now },
            };
        }
    }
}
