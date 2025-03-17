using CutterManagement.Core;
using CutterManagement.Core.Services;
using System.Windows.Data;
using System.Windows.Navigation;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="MachineSetupDialog"/>
    /// </summary>
    public class MachineSetupDialogViewModel : DialogViewModelBase, IDialogWindowCloseRequest
    {
        #region Private Fields

        /// <summary>
        /// Data factory
        /// </summary>
        private IDataAccessServiceFactory _dataFactory;

        /// <summary>
        /// Get available cutters
        /// </summary>
        private readonly Task _fetchAvailableCutters;

        /// <summary>
        /// Cutter number
        /// </summary>
        private string _cutterNumber;

        /// <summary>
        /// Collection of part
        /// </summary>
        private List<PartDataModel> _parts;

        /// <summary>
        /// Collection of cutter
        /// </summary>
        private List<CutterDataModel> _cutters;

        /// <summary>
        /// Collection of part number
        /// </summary>
        private Dictionary<int, string> _partNumberCollection;

        /// <summary>
        /// True if cutter number is valid
        /// Otherwise false
        /// </summary>
        private bool _isCutterNumberValid => _cutterNumber.Count() >= 5;

        #endregion

        #region Public Properties

        /// <summary>
        /// Machine number
        /// </summary>
        public string MachineNumber { get; set; }

        /// <summary>
        /// Collection of cutter
        /// </summary>
        public List<CutterDataModel> Cutters
        {
            get => _cutters;
            set => _cutters = value;
        }

        /// <summary>
        /// Collection of part
        /// </summary>
        public List<PartDataModel> Parts
        {
            get => _parts;
            set => _parts = value;
        }

        /// <summary>
        /// Cutter number
        /// </summary>
        public string CutterNumber
        {
            get => _cutterNumber;
            set
            {
                _cutterNumber = value;

                if (_isCutterNumberValid)
                {
                    GetCorrespondingPartNumbers();
                }
                else if (_partNumberCollection.Count > 1)
                {
                    ClearPartCollection();
                }

                if (_isCutterNumberValid is false)
                {
                    CutterCondition = string.Empty;
                    CutterType = string.Empty;
                }

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Condition of cutter indicating if cutter is a used or new cutter
        /// </summary>
        public string CutterCondition { get; set; }

        /// <summary>
        /// Type of cutter
        /// <para>Pinion | Ring</para>
        /// </summary>
        public string CutterType { get; set; }

        /// <summary>
        /// Part selected
        /// </summary>
        public int SelectedPart { get; set; }

        /// <summary>
        /// Collection of part number
        /// </summary>
        public Dictionary<int, string> PartNumberCollection
        {
            get => _partNumberCollection;
            set => _partNumberCollection = value;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Event to close dialog window
        /// </summary>
        public event EventHandler<DialogWindowCloseRequestedEventArgs> DialogWindowCloseRequest;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dataFactory">data factory</param>
        public MachineSetupDialogViewModel(IDataAccessServiceFactory dataFactory)
        {
            Title = "Setup";
            _dataFactory = dataFactory;
            _cutters = new();
            _parts = new();
            _fetchAvailableCutters = GetCutters();
            _partNumberCollection = new Dictionary<int, string>
            {
                // Default KVP
                { 0, "Select a part" }
            };
            SelectedPart = PartNumberCollection.First().Key;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fetch cutters 
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        private async Task GetCutters()
        {
            // Get cutter table
            IDataAccessService<CutterDataModel> cutterTable = _dataFactory.GetDbTable<CutterDataModel>();

            // Add every cutter to cutter collection
            (await cutterTable.GetAllEntitiesAsync()).ToList().ForEach(_cutters.Add);
        }

        /// <summary>
        /// Fetch parts
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        private async Task GetParts()
        {
            // Get part table
            IDataAccessService<PartDataModel> partTable = _dataFactory.GetDbTable<PartDataModel>();

            // Add every cutter to part collection
            (await partTable.GetAllEntitiesAsync()).ToList().ForEach(_parts.Add);
        }

        /// <summary>
        /// Get part number for a specific cutter
        /// </summary>
        private void GetCorrespondingPartNumbers()
        {
            // Get parts
            Task.Run(GetParts).ContinueWith(_ =>
            {
                // Get cutter
                CutterDataModel? cutter = _cutters.FirstOrDefault(c => c.CutterNumber == CutterNumber);

                if (cutter is null)
                {
                    ClearPartCollection();
                    CutterType = "Unknown";
                    CutterCondition = "Cutter not found";
                    SelectedPart = PartNumberCollection.First().Key;
                    return;
                }

                // Get all part numbers that uses the current cutter number
                _parts.Where(part => (part.Model == cutter.Model) && (part.Kind == cutter.Kind))
                      .ToList().ForEach(part =>
                      {
                          if (_partNumberCollection.ContainsKey(part.Id) is false)
                          {
                              _partNumberCollection.Add(part.Id, part.PartNumber);
                          }
                      });

                CutterCondition = $"{EnumHelpers.GetDescription(cutter.Condition)}: {cutter.Count}";
                CutterType = EnumHelpers.GetDescription(cutter.Kind);

                // Refresh part number collection in view
                DispatcherService.Invoke(() => CollectionViewSource.GetDefaultView(PartNumberCollection).Refresh());
            });

        }

        /// <summary>
        /// Clear part collection
        /// </summary>
        private void ClearPartCollection()
        {
            foreach (var item in _partNumberCollection)
            {
                if (item.Key == 0) continue;

                _partNumberCollection.Remove(item.Key);
            }

            // Refresh part number collection in view
            DispatcherService.Invoke(() => CollectionViewSource.GetDefaultView(PartNumberCollection).Refresh());
        }

        #endregion    
    }
}