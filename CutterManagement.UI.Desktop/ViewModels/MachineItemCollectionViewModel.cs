using CutterManagement.Core;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="MachineItemCollectionControl"/>
    /// </summary>
    public class MachineItemCollectionViewModel : ViewModelBase
    {
        #region Private Fields

        /// <summary>
        /// Collection of <see cref="MachineItemControl"/> representing rings
        /// </summary>
        private ObservableCollection<MachineItemViewModel> _ringItems;

        /// <summary>
        /// Collection of <see cref="MachineItemControl"/> representing pins
        /// </summary>
        private ObservableCollection<MachineItemViewModel> _pinItems;

        /// <summary>
        /// Data service factory
        /// </summary>
        private IDataAccessServiceFactory _dataAccessService;

        /// <summary>
        /// Load data asynchronously 
        /// </summary>
        private Task _dataLoader;

        #endregion

        #region Public Properties

        /// <summary>
        /// Collection of <see cref="MachineItemControl"/> representing rings
        /// </summary>
        public ObservableCollection<MachineItemViewModel> RingItems 
        {
            get => _ringItems;
            set => _ringItems = value;
        }

        /// <summary>
        /// Collection of <see cref="MachineItemControl"/> representing pins
        /// </summary>
        public ObservableCollection<MachineItemViewModel> PinItems
        {
            get => _pinItems;
            set => _pinItems = value;
        }

        #endregion

        public MachineItemViewModel _machineItemViewModel;

        public MachineItemViewModel MachineItemViewModel
        {
            get => _machineItemViewModel;
            set => _machineItemViewModel = value;
        }

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MachineItemCollectionViewModel(IDataAccessServiceFactory dataAccessService)
        {
            _dataAccessService = dataAccessService;
            _machineItemViewModel = new MachineItemViewModel();

            _ringItems = new ObservableCollection<MachineItemViewModel>();
            _pinItems = new ObservableCollection<MachineItemViewModel>();
            _dataLoader = LoadMachineData();
        }

        private void OnItemSelectionChanged(object? sender, EventArgs e)
        {
            MachineItemViewModel? selectedItem = (sender as MachineItemViewModel);
            _machineItemViewModel.IsPopupOpen = false;

            if (selectedItem is not null)
            {
                _machineItemViewModel.IsPopupOpen = true;
                _machineItemViewModel = selectedItem;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads existing machine data from database, 
        /// or generate default machine data if no data was found in the database
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        private async Task LoadMachineData()
        {
            // Makes sure we have service
            if (_dataAccessService is null) return;

            // Get tables needed
            IDataAccessService<MachineDataModel> machineTable = _dataAccessService.GetDbTable<MachineDataModel>();
            IDataAccessService<UserDataModel> userTable = _dataAccessService.GetDbTable<UserDataModel>();
            IDataAccessService<MachineDataModelUserDataModel> machineUserTable = _dataAccessService.GetDbTable<MachineDataModelUserDataModel>();

            // if we don't have any data in the database
            if ((await machineTable.GetAllEntitiesAsync()).Count < 1)
            {
                // Generate default ring and pins machine
                var defaultPinionMachineData = MachinesDataGenerator.GenerateDefaultMachineItems(Department.Pinion, 14);
                var defaultRingMachineData = MachinesDataGenerator.GenerateDefaultMachineItems(Department.Ring, 14);

                // Get admin user
                UserDataModel? admin = (await userTable.GetAllEntitiesAsync()).ToList().FirstOrDefault(user => user.LastName is "admin");

                // Ensure pin and ring collections are empty
                _pinItems.Clear();
                _ringItems.Clear();

                // Go through generated pinion data
                foreach (MachineDataModel data in defaultPinionMachineData)
                {
                    // Create join table for machine and user
                    // NOTE: Default machine is generated using admin user 
                    MachineDataModelUserDataModel machineUserJoinData = new MachineDataModelUserDataModel 
                    {
                        MachineDataModel = data, 
                        UserDataModel = admin! 
                    };
                    // Create new machine record in the database
                    await machineUserTable.CreateNewEntityAsync(machineUserJoinData);
                    // Populate item list
                    _pinItems.Add(ResolveToMachineItemViewModel(data));
                }

                // Create join table for machine and user
                // NOTE: Default machine is generated using admin user 
                foreach (MachineDataModel data in defaultRingMachineData)
                {
                    MachineDataModelUserDataModel machineUserJoinData = new MachineDataModelUserDataModel
                    {
                        MachineDataModel = data,
                        UserDataModel = admin!
                    };
                    // Create new machine record in the database
                    await machineUserTable.CreateNewEntityAsync(machineUserJoinData);
                    // Populate item list
                    _ringItems.Add(ResolveToMachineItemViewModel(data));
                }
            }

            // Go through data from database
            foreach (MachineDataModel data in await machineTable.GetAllEntitiesAsync())
            {
                // If machine data is owned by pinion
                if (data.Owner is Department.Pinion)
                {
                    // Populate pin item list with data
                    _pinItems.Add(ResolveToMachineItemViewModel(data));
                }
                // Otherwise
                else
                {
                    // Populate ring item list with data
                    _ringItems.Add(ResolveToMachineItemViewModel(data));
                }
            }
        }

        /// <summary>
        /// Resolves <see cref="MachineDataModel"/> to <see cref="MachineItemViewModel"/>
        /// </summary>
        /// <param name="machineData">The data to pass to <see cref="MachineItemViewModel"/></param>
        /// <returns><see cref="MachineItemViewModel"/></returns>
        private MachineItemViewModel ResolveToMachineItemViewModel(MachineDataModel machineData)
        {
            MachineItemViewModel items = new MachineItemViewModel
            {
                MachineSetId = machineData.MachineSetId,
                MachineNumber = machineData.MachineNumber,
                Status = machineData.Status,
                StatusMessage = machineData.StatusMessage,
                FrequencyCheckResult = machineData.FrequencyCheckResult.ToString(),
                DateTimeLastModified = machineData.DateTimeLastModified.ToString("MM-dd-yyyy ~ hh:mm tt"),
            };

            items.ItemSelected += OnItemSelectionChanged;

            return items;
        }

        #endregion
    }
}