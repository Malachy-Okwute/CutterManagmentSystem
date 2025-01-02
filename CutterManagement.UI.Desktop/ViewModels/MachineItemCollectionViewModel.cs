using CutterManagement.Core;
using CutterManagement.DataAccess;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

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
        /// <see cref="MachineItemViewModel"/>
        /// </summary>
        private MachineItemViewModel _machineItemViewModel;

        /// <summary>
        /// <see cref="MachineConfigurationViewModel"/>
        /// </summary>
        private MachineConfigurationViewModel _machineConfigurationViewModel;

        /// <summary>
        /// <see cref="MachineSetStatusViewModel"/>
        /// </summary>
        private MachineSetStatusViewModel _machineSetStatusViewModel;

        /// <summary>
        /// Data service factory
        /// </summary>
        private IDataAccessServiceFactory _dataAccessService;

        /// <summary>
        /// Load data asynchronously 
        /// </summary>
        private readonly Task _dataLoader;

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

        /// <summary>
        /// <see cref="MachineItemViewModel"/>
        /// </summary>
        public MachineItemViewModel MachineItemViewModel
        {
            get => _machineItemViewModel;
            set => _machineItemViewModel = value;
        }

        /// <summary>
        /// <see cref="MachineConfigurationViewModel"/>
        /// </summary>
        public MachineConfigurationViewModel MachineConfigurationViewModel 
        {
            get => _machineConfigurationViewModel;
            set => _machineConfigurationViewModel = value;
        }

        /// <summary>
        /// <see cref="MachineSetStatusViewModel"/>
        /// </summary>
        public MachineSetStatusViewModel MachineSetStatusViewModel
        {
            get => _machineSetStatusViewModel;
            set => _machineSetStatusViewModel = value;
        }

        /// <summary>
        /// True if configuration form is open
        /// otherwise false
        /// </summary>
        public bool IsConfigurationFormOpen { get; set; }

        /// <summary>
        /// True if set status form is open
        /// otherwise false
        /// </summary>
        public bool IsSetStatusFormOpen { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// Command to open machine configuration form
        /// </summary>
        public ICommand OpenMachineConfigurationFormCommand { get; set; }

        /// <summary>
        /// Command to open machine set status form
        /// </summary>
        public ICommand OpenSetStatusFormCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MachineItemCollectionViewModel(IDataAccessServiceFactory dataAccessService)
        {
            // Initialize
            _dataAccessService = dataAccessService;
            _machineItemViewModel = new MachineItemViewModel();
            _ringItems = new ObservableCollection<MachineItemViewModel>();
            _pinItems = new ObservableCollection<MachineItemViewModel>();

            // Set up data changed delegate
            _dataAccessService.OnDataChanged = UpdateMachineCollection;

            // Create commands
            OpenMachineConfigurationFormCommand = new RelayCommand(OpenMachineConfigurationForm);
            OpenSetStatusFormCommand = new RelayCommand(OpenSetStatusForm);
            

            // Load data
            _dataLoader = LoadMachineData();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Opens configuration used in configuring machine item
        /// </summary>
        /// <param name="parameter">The machine to configure</param>
        private void OpenMachineConfigurationForm()
        {
            // Turn off pop up control
            MachineItemViewModel.IsPopupOpen = false;

            // Make sure admin user is authorized
            if (AuthenticationService.IsAdminUserAuthorized is false) return;

            // Open configuration form
            IsConfigurationFormOpen = true;

            // Create machine configuration view model
            _machineConfigurationViewModel = new MachineConfigurationViewModel(MachineItemViewModel, _dataAccessService, this);

            // Update machine configuration view model property
            OnPropertyChanged(nameof(MachineConfigurationViewModel));
        }
        private void OpenSetStatusForm()
        {
            // Turn off pop up control
            MachineItemViewModel.IsPopupOpen = false;

            // Open form
            IsSetStatusFormOpen = true;

            // Create machine configuration view model
            _machineSetStatusViewModel = new MachineSetStatusViewModel(MachineItemViewModel, _dataAccessService, this);

            // Update machine configuration view model property
            OnPropertyChanged(nameof(MachineSetStatusViewModel));
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

            // Ensure pin and ring collections are empty
            _pinItems.Clear();
            _ringItems.Clear();

            // if we don't have any data in the database
            if ((await machineTable.GetAllEntitiesAsync()).Count < 1)
            {
                // Generate default ring and pins machine
                var defaultPinionMachineData = MachinesDataGenerator.GenerateDefaultMachineItems(Department.Pinion, 14);
                var defaultRingMachineData = MachinesDataGenerator.GenerateDefaultMachineItems(Department.Ring, 14);

                // Get admin user
                UserDataModel? admin = (await userTable.GetAllEntitiesAsync()).ToList().FirstOrDefault(user => user.LastName is "admin");

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
                    _pinItems.Add(DataResolver.ResolveToMachineItemViewModel(data, OnItemSelectionChanged));
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
                    _ringItems.Add(DataResolver.ResolveToMachineItemViewModel(data, OnItemSelectionChanged));
                }
            }

            // Go through data from database
            foreach (MachineDataModel data in await machineTable.GetAllEntitiesAsync())
            {
                // Resolve data
                MachineItemViewModel machineItem = DataResolver.ResolveToMachineItemViewModel(data, OnItemSelectionChanged);

                // If machine data is owned by pinion
                if (machineItem.Owner is Department.Pinion)
                {
                    // Populate pin item list with data
                    _pinItems.Add(machineItem);
                }
                // Otherwise
                else
                {
                    // Populate ring item list with data
                    _ringItems.Add(machineItem);
                }
            }
        }

        /// <summary>
        /// Updates machine items list with the latest information from db
        /// </summary>
        /// <param name="item">The item that changed</param>
        /// <returns><see cref="bool"/></returns>
        public bool UpdateMachineCollection(object item) 
        {
            // Resolve the new item that changed
            MachineItemViewModel newItem = DataResolver.ResolveToMachineItemViewModel((MachineDataModel)item, OnItemSelectionChanged);

            // If new item is pinion
            if(newItem.Owner is Department.Pinion)
            {
                // Insert item into pinion list
                InsertNewItem(newItem, _pinItems);
            }
            // Otherwise
            else
            {
                // Insert item into ring list
                InsertNewItem(newItem, _ringItems);
            }

            return default;
        }

        /// <summary>
        /// Inserts a <see cref="MachineItemViewModel"/> into an <see cref="ObservableCollection{T}"/>
        /// in a specific index that is set internally in this method
        /// </summary>
        /// <param name="item">The item to insert</param>
        /// <param name="itemList">The list to insert item into</param>
        private void InsertNewItem(MachineItemViewModel item, ObservableCollection<MachineItemViewModel> itemList)
        {
            MachineItemViewModel? currentItemToChange = itemList.FirstOrDefault(x => x.Id == item.Id);

            if (currentItemToChange is not null)
            {
                int index = itemList.IndexOf(currentItemToChange);

                DispatcherService.Invoke(() =>
                {
                    itemList.RemoveAt(index);
                    itemList.Insert(index, item);
                });
            }
        }

        #endregion

        #region Event Methods

        /// <summary>
        ///  Item selection event for when any machine item's gets a mouse click
        /// </summary>
        /// <param name="sender">The event source</param>
        /// <param name="commandKind">The event args</param>
        [SuppressPropertyChangedWarnings]
        private void OnItemSelectionChanged(object? sender, CommandKind commandKind)
        {
            // Get source of event
            MachineItemViewModel? selectedItem = (sender as MachineItemViewModel);

            // Reset pop up
            _machineItemViewModel.IsPopupOpen = false;

            // Make sure event source isn't null
            if (selectedItem is not null)
            {
                // Set pop up control data context
                _machineItemViewModel = selectedItem;

                // Sort and apply command accordingly
                switch (commandKind)
                {
                    case CommandKind.DataFormCommand:
                        break;

                    case CommandKind.PopCommand:
                        _machineItemViewModel.IsPopupOpen = true;
                        break;

                    default:
                        Debugger.Break();
                        throw new InvalidOperationException("Command not configured");
                }
                // Update pop up control as needed
                OnPropertyChanged(nameof(MachineItemViewModel));
            }
        }

        #endregion
    }
}