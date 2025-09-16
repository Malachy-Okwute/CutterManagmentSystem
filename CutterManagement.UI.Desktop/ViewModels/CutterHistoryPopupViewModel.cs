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
        /// <summary>
        /// Represents the service used to interact with and manage machine-related operations.
        /// </summary>
        private readonly IMachineService _machineService;

        /// <summary>
        /// The collection of items displayed in the cutter history popup.
        /// </summary>
        public ObservableCollection<CutterHistoryPopupItemViewModel> Items { get; set; }

        /// <summary>
        /// True if history is empty. Otherwise false
        /// </summary>
        public bool IsHistoryEmpty { get; set; }

        /// <summary>
        ///  Default constructor
        /// </summary>
        /// <param name="machineService">Machine service</param>
        public CutterHistoryPopupViewModel(IMachineService machineService)
        {
            _machineService = machineService;

            Initialize();
        }

        /// <summary>
        /// Initialize this class
        /// </summary>
        protected override void Initialize()
        {
            Items = new ObservableCollection<CutterHistoryPopupItemViewModel>();
        }

        /// <summary>
        /// Loads history of cutter number specified
        /// </summary>
        /// <param name="cutterNumber">The cutter number to look up and load the history of</param>
        public async Task LoadCutterHistory(string cutterNumber)
        {
            // Clear any pre-existing items
            Items.Clear();

            // Get log db
            var logTable = await ServerRequest.GetDataCollection<ProductionPartsLogDataModel>(new System.Net.Http.HttpClient(), "https://localhost:7261/ProductionPartsLogDataModel");

            var counter = 0;

            foreach (var cutterLog in logTable ?? throw new Exception())
            {
                // Filter cutter number
                if(cutterLog.CutterNumber.Equals(cutterNumber) is false)
                {
                    continue;
                }

                // Add header
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

                // Add data to items collection
                Items.Add(new CutterHistoryPopupItemViewModel
                {
                    CutterNumber = cutterLog.CutterNumber,
                    MachineNumber = cutterLog.MachineNumber,
                    PartNumber = cutterLog.PartNumber,
                    Count = cutterLog.PieceCount,
                    CheckResult = cutterLog.FrequencyCheckResult,
                    //SizeOfPartTooth = cutterLog.ToothSize,
                    SizeOfPartTooth = "10", // ToDo: Wire up actual data
                    //Shift = cutterLog.CurrentShift,
                    Shift = "1st", // ToDo: Wire up actual data
                    UserName = cutterLog.UserFullName ?? "Unknown user",
                    DateAndTimeOfCheck = cutterLog.DateTimeOfCheck.ToString("g"),
                    UseAlternateBackground = counter % 2 == 0,
                    IsHeader = false
                });
                
                counter++;
            }

            IsHistoryEmpty = Items.IsNullOrEmpty();

            // Update UI
            OnPropertyChanged(nameof(IsHistoryEmpty));
        }

        private string GetFullDate(DateTime date)
        {
            return date.GetDateTimeFormats().First();
        }
    }
}
