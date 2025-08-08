using CutterManagement.Core;

namespace CutterManagement.UI.Desktop
{
    public static class ProductionPartsLogHelper 
    {
        public static void LogProductionProgress(UserDataModel? user, MachineDataModel data, IDataAccessService<ProductionPartsLogDataModel> productionLogTable)
        {
            // Make sure we have cutter at least
            if (data.Cutter is null) return;

            // Log production data progress    
            _ = productionLogTable.CreateNewEntityAsync(new ProductionPartsLogDataModel
            {
                MachineNumber = data.MachineNumber,
                CutterNumber = data.Cutter.CutterNumber,
                PartNumber = data.PartNumber,
                Comment = data.StatusMessage,
                Model = data.Cutter.Model,
                FrequencyCheckResult = data.FrequencyCheckResult.ToString(),
                PieceCount = data.Cutter.Count.ToString(),
                UserFullName = $"{user?.FirstName} {user?.LastName}" ?? "n/a",
                ToothCount = "n/a (coming soon)",
                CurrentShift = "n/a (coming soon)",
                ToothSize = string.IsNullOrEmpty(data.PartToothSize) ? "n/a" : data.PartToothSize,
                CMMData = data.Cutter.CMMData.LastOrDefault() ?? null,
                CutterChangeInfo = data.Cutter.CutterChangeInfo.ToString(),
            });
        }
    }
}
