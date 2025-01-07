namespace CutterManagement.Core
{
    /// <summary>
    /// A helper class used in generating default machine data
    /// </summary>
    public static class MachinesDataGenerator
    {
        /// <summary>
        /// Generates machine items
        /// <para>
        /// T is <see cref="MachineDataModel"/>
        /// </para>
        /// </summary>
        /// <param name="department">The owner of machine to generate</param>
        /// <param name="amount">The number of machines to generate</param>
        /// <returns><see cref="List{T}"/></returns>
        public static List<MachineDataModel> GenerateDefaultMachineItems(Department department, int amount)
        {
            List<MachineDataModel> machineItems = new List<MachineDataModel>();

            for (int i = 0; i < amount; i++)
            {
                machineItems.Add(new MachineDataModel
                {
                    MachineNumber = "000",
                    MachineSetId = "111",
                    Owner = department,
                    Count = 0,
                    PartToothSize = "0",
                    Status = MachineStatus.Warning,
                    StatusMessage = "Machine need to be configured by admin",
                    FrequencyCheckResult = FrequencyCheckResult.SETUP,
                    DateTimeLastModified = DateTime.Now,
                    DateCreated = DateTime.Now,
                    CutterChangeInfo = CutterChangeInformation.None
                });
            }

            return machineItems;
        }
    }
}
