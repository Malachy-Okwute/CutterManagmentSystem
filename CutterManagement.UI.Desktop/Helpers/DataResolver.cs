using CutterManagement.Core;

namespace CutterManagement.UI.Desktop
{
    public static class DataResolver
    {
        /// <summary>
        /// Resolves <see cref="MachineDataModel"/> to <see cref="MachineItemViewModel"/>
        /// </summary>
        /// <param name="machineData">The data to pass to <see cref="MachineItemViewModel"/></param>
        /// <returns><see cref="MachineItemViewModel"/></returns>
        public static MachineItemViewModel ResolveToMachineItemViewModel(MachineDataModel machineData, EventHandler eventHandler)
        {
            MachineItemViewModel items = new MachineItemViewModel
            {
                Id = machineData.Id,
                MachineSetNumber = machineData.MachineSetId,
                MachineNumber = machineData.MachineNumber,
                Status = machineData.Status,
                StatusMessage = machineData.StatusMessage,
                Owner = machineData.Owner,
                FrequencyCheckResult = machineData.FrequencyCheckResult.ToString(),
                DateTimeLastModified = machineData.DateTimeLastModified.ToString("MM-dd-yyyy ~ hh:mm tt"),
            };

            // Hook in selection changed event
            items.ItemSelected += eventHandler;

            return items;
        }

    }
}
