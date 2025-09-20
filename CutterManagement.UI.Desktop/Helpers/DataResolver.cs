using CutterManagement.Core;
using System.Net.Http;
using System.Threading.Tasks;

namespace CutterManagement.UI.Desktop
{
    public static class DataResolver
    {
        /// <summary>
        /// Resolves <see cref="MachineDataModel"/> to <see cref="MachineItemViewModel"/>
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="machineData">The data to pass to <see cref="MachineItemViewModel"/></param>
        /// <returns><see cref="MachineItemViewModel"/></returns>
        public static async Task<MachineItemViewModel> ResolveToMachineItemViewModel(MachineDataModel machineData, IMachineService machineService, EventHandler eventHandler)
        {
            MachineItemViewModel item = new MachineItemViewModel(machineService)
            {
                Id = machineData.Id,
                MachineSetNumber = machineData.MachineSetId,
                MachineNumber = machineData.MachineNumber,
                Status = machineData.Status,
                StatusMessage = machineData.StatusMessage,
                Owner = machineData.Owner,
                IsConfigured = machineData.IsConfigured,
                FrequencyCheckResult = machineData.FrequencyCheckResult.ToString(),
                DateTimeLastModified = machineData.DateTimeLastModified.ToString("MM-dd-yyyy ~ hh:mm tt"),
            };

            if(machineData.CutterDataModelId is not null)
            {
                HttpClient client = machineService.HttpClientFactory.CreateClient("CutterManagementApi");

                var cutter = await ServerRequest.GetData<CutterDataModel>(client, $"CutterDataModel/{machineData.CutterDataModelId}");

                if(cutter is not null)
                {
                    item.CutterNumber = $"{cutter.CutterNumber}-{cutter.SummaryNumber}";
                    item.PartNumber = machineData.PartNumber;
                    item.Count = cutter.Count.ToString();
                    item.PartPreviousToothSize = machineData.PartToothSize;
                }
            }

            // Hook in selection changed event
            item.ItemSelected += eventHandler;

            return item;
        }

    }
}
