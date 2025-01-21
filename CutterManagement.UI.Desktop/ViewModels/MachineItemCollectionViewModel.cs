using CutterManagement.Core;
using PropertyChanged;
using System.Collections.ObjectModel;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="MachineItemCollectionControl"/>
    /// </summary>
    public class MachineItemCollectionViewModel : ViewModelBase, ISubscribeToMessages
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

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MachineItemCollectionViewModel(IDataAccessServiceFactory dataAccessService)
        {
            // Initialize
            _dataAccessService = dataAccessService;
            _ringItems = new ObservableCollection<MachineItemViewModel>();
            _pinItems = new ObservableCollection<MachineItemViewModel>();

            // Load data
            _dataLoader = LoadMachineData();

            // Register this object to receive messages from messenger
            Messenger.MessageSender.RegisterMessenger(this);
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
                UserDataModel admin = (await userTable.GetAllEntitiesAsync()).ToList().First(user => user.LastName is "admin");

                foreach (MachineDataModel data in defaultPinionMachineData)
                {
                    // NOTE: Default machine is generated using admin as default user 

                    // Set admin as the default user for the generated machines
                    data.Users.Add(admin);

                    // Create machine db entity
                    await machineTable.CreateNewEntityAsync(data);
                  
                    // Populate item list
                    _pinItems.Add(DataResolver.ResolveToMachineItemViewModel(data, _dataAccessService, OnItemSelectionChanged));
                }

                foreach (MachineDataModel data in defaultRingMachineData)
                {
                    // NOTE: Default machine is generated using admin as default user 

                    // Set admin as the default user for the generated machines
                    data.Users.Add(admin);

                    // Create machine db entity
                    await machineTable.CreateNewEntityAsync(data);

                    // Populate item list
                    _ringItems.Add(DataResolver.ResolveToMachineItemViewModel(data, _dataAccessService, OnItemSelectionChanged));
                }

                // Do nothing else
                return;
            }

            // Go through each data from database
            foreach (MachineDataModel data in await machineTable.GetAllEntitiesAsync())
            {
                // Resolve data
                MachineItemViewModel machineItem = DataResolver.ResolveToMachineItemViewModel(data, _dataAccessService, OnItemSelectionChanged);

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
        /// <param name="sender">The item that changed</param>
        /// <returns><see cref="bool"/></returns>
        public void UpdateMachineCollection(MachineDataModel machineItem) 
        {
            // Resolve the new item that changed
            MachineItemViewModel newItem = DataResolver.ResolveToMachineItemViewModel(machineItem, _dataAccessService, OnItemSelectionChanged);

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
        }

        /// <summary>
        /// Inserts a <see cref="MachineItemViewModel"/> into an <see cref="ObservableCollection{T}"/>
        /// in a specific index that is set internally in this method
        /// </summary>
        /// <param name="item">The item to insert</param>
        /// <param name="itemList">The list to insert item into</param>
        private void InsertNewItem(MachineItemViewModel item, ObservableCollection<MachineItemViewModel> itemList)
        {
            // Find the incoming item
            MachineItemViewModel? currentItemToChange = itemList.FirstOrDefault(x => x.Id == item.Id);

            // If the item exists
            if (currentItemToChange is not null)
            {
                // Get item index on the collection 
                int index = itemList.IndexOf(currentItemToChange);

                // Replace the item with the latest data
                DispatcherService.Invoke(() =>
                {
                    itemList.RemoveAt(index);
                    itemList.Insert(index, item);
                });
            }
            // Otherwise we assume the new item is an item being created
        }

        #endregion

        #region Event Methods

        /// <summary>
        ///  Item selection event for when any machine item's gets a mouse click
        /// </summary>
        /// <param name="sender">The event source</param>
        /// <param name="commandKind">The event args</param>
        [SuppressPropertyChangedWarnings]
        private void OnItemSelectionChanged(object? sender, EventArgs args)
        {
            // In both collections, find item that is previously selected
            MachineItemViewModel? pinItem = _pinItems.FirstOrDefault(item => item.IsPopupOpen is true);
            MachineItemViewModel? ringItem = _ringItems.FirstOrDefault(item => item.IsPopupOpen is true);

            // Make sure item is not null
            if (pinItem is not null)
            {
                // Reset it's selection 
                pinItem.IsPopupOpen = false;
            }
            
            // Make sure item is not null
            if(ringItem is not null)
            {
                // Reset it's selection 
                ringItem.IsPopupOpen = false;
            }
        }

        #endregion

        #region Messages

        /// <summary>
        /// Receive message from <see cref="Messenger"/>
        /// </summary>
        /// <param name="message">The message received</param>
        public void ReceiveMessage(IMessage message)
        {
            if(message.GetType() == typeof(MachineDataModel))
            {
                UpdateMachineCollection((MachineDataModel)message);
            }
        }

        #endregion
    }
}