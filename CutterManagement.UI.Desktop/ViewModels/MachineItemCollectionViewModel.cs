using CutterManagement.Core;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;

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
        /// Provides services to machine
        /// </summary>
        private readonly IMachineService _machineService;

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
        public MachineItemCollectionViewModel(IMachineService machineService)
        {
            // Initialize
            _machineService = machineService;
            _ringItems = new ObservableCollection<MachineItemViewModel>();
            _pinItems = new ObservableCollection<MachineItemViewModel>();

            // Load data
            _ = LoadMachineData();

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
            try
            {
                HttpClient client = _machineService.HttpClientFactory.CreateClient("CutterManagementApi");

                var machines = await ServerRequest.GetDataCollection<MachineDataModel>(client, "MachineDataModel");
                var users = await ServerRequest.GetDataCollection<UserDataModel>(client, "UserDataModel");

                // Ensure pin and ring collections are empty
                _pinItems.Clear();
                _ringItems.Clear();

                // if we don't have any data in the database
                if (machines is not null && machines.Any() is false)
                {
                    // Get admin user
                    UserDataModel? admin = users?.Single(user => user.LastName is "admin");

                    // Generate default ring and pins machine
                    GenerateMachineAndSaveDefaultMachinesToDatabase(client, admin ?? throw new Exception("user cannot be null"));

                    // Do nothing else
                    return;
                }

                machines?.ForEach(async machine =>
                {
                    // Resolve data
                    var machineItem = await DataResolver.ResolveToMachineItemViewModel(machine, _machineService, OnItemSelectionChanged);

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
                });
            }
            catch (Exception)
            {
                //DialogService.InvokeAlertDialog(this, ex.Message);
                Debugger.Break();
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
            MachineItemViewModel newItem = Task.Run(async () => await DataResolver.ResolveToMachineItemViewModel(machineItem, _machineService, OnItemSelectionChanged)).Result;

            // Make sure new item is not null
            if(newItem is not null)
            {
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

        /// <summary>
        /// Generates and save default machine items to database
        /// </summary>
        private void GenerateMachineAndSaveDefaultMachinesToDatabase(HttpClient client, UserDataModel user)
        {
            // Generate default ring and pins machine
            var defaultPinionMachineData = MachinesDataGenerator.GenerateDefaultMachineItems(Department.Pinion, 15);
            var defaultRingMachineData = MachinesDataGenerator.GenerateDefaultMachineItems(Department.Ring, 15);

            // Save and add each item to UI
            defaultPinionMachineData.ForEach(async machineItem =>
            {
                // NOTE: Default machine is generated using admin as default user 

                machineItem.MachineUserInteractions.Add(new MachineUserInteractions
                {
                    UserDataModelId = user?.Id,
                    MachineDataModel = machineItem
                });

                // Create machine db entity
                await ServerRequest.PutData(client, "MachineDataModel", machineItem);

                // Populate item list
                _pinItems.Add(await DataResolver.ResolveToMachineItemViewModel(machineItem, _machineService, OnItemSelectionChanged));
            });

            // Save and add each item to UI
            defaultRingMachineData.ForEach(async machineItem =>
            {
                // NOTE: Default machine is generated using admin as default user 

                machineItem.MachineUserInteractions.Add(new MachineUserInteractions
                {
                    UserDataModelId = user?.Id,
                    MachineDataModel = machineItem
                });

                // Create machine db entity
                await ServerRequest.PutData(client, "MachineDataModel", machineItem);

                // Populate item list
                _pinItems.Add(await DataResolver.ResolveToMachineItemViewModel(machineItem, _machineService, OnItemSelectionChanged));
            });
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
            MachineItemViewModel? pinItem = _pinItems.FirstOrDefault(item => (item.IsPopupOpen is true) || (item.CanEditPieceCount is false));
            MachineItemViewModel? ringItem = _ringItems.FirstOrDefault(item => item.IsPopupOpen is true || (item.CanEditPieceCount is false));

            if (pinItem is not null)
            {
                // Reset it's selection 
                pinItem.IsPopupOpen = false;

                // Cancel piece count edit
                pinItem.CanEditPieceCount = true;
            }

            // Make sure item is not null
            if (ringItem is not null)
            {
                // Reset it's selection 
                ringItem.IsPopupOpen = false;

                // Cancel piece count edit
                ringItem.CanEditPieceCount = true;
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