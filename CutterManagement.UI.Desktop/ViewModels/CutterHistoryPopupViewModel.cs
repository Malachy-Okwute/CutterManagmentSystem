using Accessibility;
using CutterManagement.Core;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="CutterHistoryPopupControl"/>
    /// </summary>
    public class CutterHistoryPopupViewModel : ViewModelBase
    {
        private readonly IMachineService _machineService;

        public ObservableCollection<CutterHistoryPopupItemViewModel> Items { get; set; }

        public bool IsHistoryEmpty { get; set; }

        public CutterHistoryPopupViewModel(IMachineService machineService)
        {
            _machineService = machineService;

            Initialize();
        }

        protected override void Initialize()
        {
            Items = new ObservableCollection<CutterHistoryPopupItemViewModel>();
        }

        public async Task<bool> LoadCutterHistory()
        {
            Items.Clear();

            using var logTable = _machineService.DataBaseAccess.GetDbTable<ProductionPartsLogDataModel>();

            var counter = 0;

            foreach (var cutterLog in await logTable.GetAllEntitiesAsync())
            {
                if (Items.IsNullOrEmpty())
                {
                    Items.Add(new CutterHistoryPopupItemViewModel
                    {
                        DateAndTimeOfCheck = cutterLog.DateTimeOfCheck.ToString("M-dd-yyyy"),
                        UseAlternateBackground = false,
                        IsHeader = true 
                    });
                }
                else if (GetFullDate(DateTime.Parse(Items.Last().DateAndTimeOfCheck)).Equals(GetFullDate(cutterLog.DateTimeOfCheck)) is false)
                {
                    Items.Add(new CutterHistoryPopupItemViewModel
                    {
                        DateAndTimeOfCheck = cutterLog.DateTimeOfCheck.ToString("M-dd-yyyy"),
                        UseAlternateBackground = false,
                        IsHeader = true
                    });

                    counter = 0;
                }

                Items.Add(new CutterHistoryPopupItemViewModel
                {
                    CutterNumber = cutterLog.CutterNumber,
                    MachineNumber = cutterLog.MachineNumber,
                    PartNumber = cutterLog.PartNumber,
                    Count = cutterLog.PieceCount,
                    CheckResult = cutterLog.FrequencyCheckResult,
                    //SizeOfPartTooth = cutterLog.ToothSize,
                    SizeOfPartTooth = "10",
                    //Shift = cutterLog.CurrentShift,
                    Shift = "1st",
                    UserName = cutterLog.UserFullName ?? "Unknown user",
                    DateAndTimeOfCheck = cutterLog.DateTimeOfCheck.ToString("g"),
                    UseAlternateBackground = counter % 2 == 0,
                    IsHeader = false
                });
                
                counter++;
            }

            IsHistoryEmpty = Items.IsNullOrEmpty();

            OnPropertyChanged(nameof(IsHistoryEmpty));

            return true;
        }

        private string GetFullDate(DateTime date)
        {
            return date.GetDateTimeFormats().First();
        }
    }
}
