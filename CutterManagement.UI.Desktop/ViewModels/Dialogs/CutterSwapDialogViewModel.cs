using CutterManagement.Core;
using CutterManagement.Core.Services;
using CutterManagement.DataAccess;
using System.IO;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="CutterSwapDialog"/>
    /// </summary>
    public class CutterSwapDialogViewModel : DialogViewModelBase, IDialogWindowCloseRequest
    {
        #region Private Fields

        /// <summary>
        /// Provides service to machine
        /// </summary>
        private readonly IMachineService _machineService;

        /// <summary>
        /// First machine
        /// </summary>
        private MachineDataModel? _firstMachine;

        /// <summary>
        /// Second machine
        /// </summary>
        private MachineDataModel? _secondMachine;

        /// <summary>
        /// Machine initiating cutter swap operation
        /// </summary>
        private MachineItemViewModel _machineItem;

        /// <summary>
        /// User that is setting this machine status
        /// </summary>
        private UserDataModel _user;

        #endregion

        #region Public Properties

        /// <summary>
        /// Properties of machine initiating cutter swap
        /// </summary>
        public CutterSwapHelper FirstMachine { get; set; }

        /// <summary>
        /// Properties of machine to swap cutter with
        /// </summary>
        public CutterSwapHelper SecondMachine { get; set; }

        /// <summary>
        /// Comment associated with this process. if any
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// User that is setting this machine status
        /// </summary>
        public UserDataModel User
        {
            get => _user;
            set => _user = value;
        }

        /// <summary>
        /// Collection of users
        /// </summary>
        public Dictionary<UserDataModel, string> UserCollection { get; set; }

        #endregion

        #region Public Event

        /// <summary>
        /// Event that handles closing of <see cref="CutterSwapDialog"/>
        /// </summary>
        public event EventHandler<DialogWindowCloseRequestedEventArgs> DialogWindowCloseRequest;

        #endregion

        #region Public Commands

        /// <summary>
        /// Command to swap cutters
        /// </summary>
        public ICommand SwapCuttersCommand { get; set; }

        /// <summary>
        /// Command to cancel cutter swap process
        /// </summary>
        public ICommand CancelCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="machineService">Provides service to machines</param>
        public CutterSwapDialogViewModel(IMachineService machineService)
        {
            _machineService = machineService;
            UserCollection = new Dictionary<UserDataModel, string>();

            // Create commands
            SwapCuttersCommand = new RelayCommand(async () => await SwapCutters());
            CancelCommand = new RelayCommand(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess)));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get machine reference
        /// </summary>
        public void GetMachine(MachineItemViewModel machineItem) => _machineItem = machineItem;

        /// <summary>
        /// Initialize cutter swapping procedure
        /// </summary>
        public async Task<bool> InitializeCutterSwapping()
        {
            // Get machine table
            using var machineTable = _machineService.DataBaseAccess.GetDbTable<MachineDataModel>();

            // Get users
            using var userTable = _machineService.DataBaseAccess.GetDbTable<UserDataModel>();

            // Get first machine
            _firstMachine = await machineTable.GetEntityByIdAsync(_machineItem.Id, x => x.Cutter);

            // Filter and find second machine id
             var getSecondMachineId = (await machineTable.GetAllEntitiesAsync())
                                      .FirstOrDefault(x => x.MachineSetId == _firstMachine?.MachineSetId && 
                                      x.MachineNumber != _firstMachine.MachineNumber &&
                                      x.Owner == _firstMachine.Owner)?.Id;

            // Get second machine
            _secondMachine = await machineTable.GetEntityByIdAsync(getSecondMachineId, x => x.Cutter);

            // Make sure we have cutter to swap
            if (_firstMachine?.Cutter is null) return false;

            // Make sure second machine in the set is available
            if (_secondMachine is null)
            {
                // Error message
                string errorMessage = $"[ {_firstMachine?.MachineNumber} ] does not currently have a machine to swap cutter with.";

                // Show dialog
                await DialogService.InvokeFeedbackDialog(this, errorMessage);

                // Do nothing else
                return false;
            }

            // Make sure second machine has cutter
            if (_secondMachine.Cutter is null)
            {
                // Error message
                string errorMessage = $"A cutter is required for [ {_secondMachine.MachineNumber} ] to exchange its cutter with [ {_firstMachine.MachineNumber} ].";

                // Show dialog
                await DialogService.InvokeFeedbackDialog(this, errorMessage);

                // Do nothing else
                return false;
            }

            // If both machines are available
            if(_firstMachine is not null && _secondMachine is not null)
            {
                // Set Properties
                FirstMachine = new CutterSwapHelper(_firstMachine.MachineNumber, 
                    $"{_firstMachine.Cutter.CutterNumber}-{_firstMachine.Cutter.Model}", _firstMachine.PartNumber, _firstMachine.Cutter.Count.ToString());
                SecondMachine = new CutterSwapHelper(_secondMachine.MachineNumber,
                    $"{_secondMachine.Cutter.CutterNumber}-{_secondMachine.Cutter.Model}", _secondMachine.PartNumber, _secondMachine.Cutter.Count.ToString());

                // Get users
                foreach (UserDataModel userData in await userTable.GetAllEntitiesAsync())
                {
                    // Do not load admin user
                    if (userData.LastName is "admin")
                        continue;

                    UserCollection.Add(userData, userData.FirstName.PadRight(10) + userData.LastName);
                }

                // Set current user 
                _user = UserCollection.FirstOrDefault().Key;

                // Update UI
                OnPropertyChanged(nameof(User));
            }

            // Return result
            return (_firstMachine is not null && _secondMachine is not null);
        }

        /// <summary>
        /// Swaps cutters between 2 machines in a set
        /// </summary>
        private async Task SwapCutters()
        {
            // ToDo: Refactor code below

            // Get machine table
            using var machineTable1 = _machineService.DataBaseAccess.GetDbTable<MachineDataModel>();
            using var machineTable2 = _machineService.DataBaseAccess.GetDbTable<MachineDataModel>();

            // Get user table
            using var userTable = _machineService.DataBaseAccess.GetDbTable<UserDataModel>();

            // Get production part log table
            IDataAccessService<ProductionPartsLogDataModel> productionLogTable = _machineService.DataBaseAccess.GetDbTable<ProductionPartsLogDataModel>();

            UserDataModel? user = await userTable.GetEntityByIdAsync(_user.Id);

            // New data from database
            MachineDataModel? data = null;

            // Dummy
            MachineDataModel dummyMachine = new MachineDataModel();

            // Listen for changes 
            machineTable1.DataChanged += (s, e) =>
            {
                // Set new data
                data = e as MachineDataModel;
                // Send out message
                Messenger.MessageSender.SendMessage(data ?? throw new ArgumentNullException("Selected Machine data cannot be null"));

                // Log cmm data
                //ProductionPartsLogHelper.LogProductionProgress(user, data, productionLogTable);
            };

            machineTable2.DataChanged += (s, e) =>
            {
                // Set new data
                data = e as MachineDataModel;
                // Send out message
                Messenger.MessageSender.SendMessage(data ?? throw new ArgumentNullException("Selected Machine data cannot be null"));

                // Log cmm data
                //ProductionPartsLogHelper.LogProductionProgress(user, data, productionLogTable);
            };

            if (_firstMachine is not null && _secondMachine is not null)
            {
                dummyMachine= _firstMachine;
                dummyMachine.DateTimeLastModified = DateTime.Now;

                _firstMachine.Status = MachineStatus.Warning;
                _firstMachine.PartNumber = null!;
                _firstMachine.CutterDataModelId = null;
                _firstMachine.Cutter.MachineDataModelId = null;
                _firstMachine.Cutter = null!;
                _firstMachine.FrequencyCheckResult = FrequencyCheckResult.Setup;

                await machineTable1.SaveEntityAsync(_firstMachine);

                // Get up to date data
                MachineDataModel? firstMachine = await machineTable1.GetEntityByIdAsync(_firstMachine.Id);

                if(firstMachine is not null)
                {
                    firstMachine.Status = _secondMachine.Status == MachineStatus.IsDownForMaintenance ? MachineStatus.Warning : _secondMachine.Status;
                    firstMachine.StatusMessage = $"Swapped cutter with {_secondMachine.MachineNumber}";
                    firstMachine.PartNumber = _secondMachine.PartNumber;
                    firstMachine.Cutter = _secondMachine.Cutter;
                    firstMachine.CutterDataModelId = _secondMachine.CutterDataModelId;
                    firstMachine.Cutter.MachineDataModelId = _firstMachine.Id;
                    firstMachine.FrequencyCheckResult = _secondMachine.FrequencyCheckResult;
                    firstMachine.DateTimeLastModified = DateTime.Now;
                    
                    // Set the user conducting this operation
                    firstMachine.MachineUserInteractions.Add(new MachineUserInteractions
                {
                    UserDataModel = user ?? throw new NullReferenceException($"User with the name {user?.FirstName.PadRight(6)} {user?.LastName} not found"),
                    MachineDataModel = firstMachine
                    });

                    _secondMachine.Status = MachineStatus.Warning;
                    _secondMachine.PartNumber = null!;
                    _secondMachine.Cutter.MachineDataModelId = null;
                    _secondMachine.Cutter = null!;
                    _secondMachine.CutterDataModelId = null;
                    _secondMachine.FrequencyCheckResult = FrequencyCheckResult.Setup;

                    await machineTable1.SaveEntityAsync(_secondMachine);
                    await machineTable1.SaveEntityAsync(firstMachine);
                }

                // Get up to date data
                MachineDataModel? secondMachine = await machineTable1.GetEntityByIdAsync(_secondMachine.Id);

                if(secondMachine is not null)
                {
                    secondMachine.Status = dummyMachine.Status == MachineStatus.IsDownForMaintenance ? MachineStatus.Warning : dummyMachine.Status;
                    secondMachine.StatusMessage = $"Swapped cutter with {_firstMachine.MachineNumber}";
                    secondMachine.PartNumber = dummyMachine.PartNumber;
                    secondMachine.Cutter = dummyMachine.Cutter;
                    secondMachine.CutterDataModelId = dummyMachine.CutterDataModelId;
                    secondMachine.Cutter.MachineDataModelId = _secondMachine.Id;
                    secondMachine.FrequencyCheckResult = dummyMachine.FrequencyCheckResult;
                    secondMachine.DateTimeLastModified = DateTime.Now;

                    // Set the user conducting this operation
                    secondMachine.MachineUserInteractions.Add(new MachineUserInteractions
                    {
                        UserDataModel = user ?? throw new NullReferenceException($"User with the name {user?.FirstName.PadRight(6)} {user?.LastName} not found"),
                        MachineDataModel = secondMachine
                    });

                    // Save new info to database
                    await machineTable2.SaveEntityAsync(secondMachine);
                }
            }

            // Set flag
            IsSuccess = true;

            // Notify user of the outcome
            await DialogService.InvokeAlertDialog(this, "Cutters swapped successfully").ContinueWith(x =>
            {
                DispatcherService.Invoke(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess)));                
            });
        }

        #endregion
    }
}