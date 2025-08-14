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
        /// Second machine unique id
        /// </summary>
        private int? _secondMachine;

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
            SwapCuttersCommand = new RelayCommand(async () => await SwapCutters(_machineItem?.Id, _secondMachine));
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
            var firstMachine = await machineTable.GetEntityByIdAsync(_machineItem.Id, x => x.Cutter);

            // Filter and find second machine id
             _secondMachine = (await machineTable.GetAllEntitiesAsync()).FirstOrDefault(x => x.MachineSetId == firstMachine?.MachineSetId && 
                                      x.MachineNumber != firstMachine.MachineNumber &&
                                      x.Owner == firstMachine.Owner)?.Id;

            // Get second machine
            var secondMachine = await machineTable.GetEntityByIdAsync(_secondMachine, x => x.Cutter);

            // Make sure we have cutter to swap
            if (firstMachine?.Cutter is null) return false;

            // Make sure second machine in the set is available
            if (secondMachine is null)
            {
                // Error message
                string errorMessage = $"[ {firstMachine?.MachineNumber} ] does not currently have a machine to swap cutter with.";

                // Show dialog
                await DialogService.InvokeFeedbackDialog(this, errorMessage);

                // Do nothing else
                return false;
            }

            // Make sure second machine has cutter
            if (secondMachine.Cutter is null)
            {
                // Error message
                string errorMessage = $"[ {secondMachine.MachineNumber} ] must be setup with a cutter to complete this operation.";

                // Show dialog
                await DialogService.InvokeFeedbackDialog(this, errorMessage);

                // Do nothing else
                return false;
            }

            // If both machines are available
            if(firstMachine is not null && secondMachine is not null)
            {
                // Set Properties
                FirstMachine = new CutterSwapHelper(firstMachine.MachineNumber, 
                    $"{firstMachine.Cutter.CutterNumber}-{firstMachine.Cutter.Model}", firstMachine.PartNumber, firstMachine.Cutter.Count.ToString());
                SecondMachine = new CutterSwapHelper(secondMachine.MachineNumber,
                    $"{secondMachine.Cutter.CutterNumber}-{secondMachine.Cutter.Model}", secondMachine.PartNumber, secondMachine.Cutter.Count.ToString());

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
            return (firstMachine is not null && secondMachine is not null);
        }

        /// <summary>
        /// Swaps cutters between two machines in a set
        /// </summary>
        /// <param name="initialMachineId">Unique id of the first machine</param>
        /// <param name="secondMachineId">Unique id of the second machine</param>
        private async Task SwapCutters(int? initialMachineId, int? secondMachineId)
        {
            // ToDo: Refactor code below

            // Get machine table
            using var machineTable = _machineService.DataBaseAccess.GetDbTable<MachineDataModel>();

            // Get user table
            using var userTable = _machineService.DataBaseAccess.GetDbTable<UserDataModel>();

            // Get production part log table
            IDataAccessService<ProductionPartsLogDataModel> productionLogTable = _machineService.DataBaseAccess.GetDbTable<ProductionPartsLogDataModel>();

            // Get user
            UserDataModel? user = await userTable.GetEntityByIdAsync(_user.Id);

            // Get first machine
            MachineDataModel? firstMachine = await machineTable.GetEntityByIdAsync(initialMachineId, cutter => cutter.Cutter);
            // Get second machine
            MachineDataModel? secondMachine = await machineTable.GetEntityByIdAsync(secondMachineId, cutter => cutter.Cutter);

            // New data from database
            MachineDataModel? data = null;

            // Dummy
            MachineDataModel dummyMachine = new MachineDataModel();

            // Listen for changes 
            machineTable.DataChanged += (s, e) =>
            {
                // Set new data
                data = e as MachineDataModel;
                // Send out message
                Messenger.MessageSender.SendMessage(data ?? throw new ArgumentNullException("Selected Machine data cannot be null"));

                // Log cmm data
                //ProductionPartsLogHelper.LogProductionProgress(user, data, productionLogTable);
            };

            // If both machines are available
            if (firstMachine is not null && secondMachine is not null)
            {
                // Create a dummy machine to hold first machine's data
                dummyMachine.Status = firstMachine.Status;
                dummyMachine.StatusMessage = firstMachine.StatusMessage;
                dummyMachine.PartNumber = firstMachine.PartNumber;
                dummyMachine.CutterDataModelId = firstMachine.CutterDataModelId;
                dummyMachine.FrequencyCheckResult = firstMachine.FrequencyCheckResult;
                dummyMachine.DateTimeLastModified = DateTime.Now;

                // Pass second machine's data to first machine
                firstMachine.Status = secondMachine.Status == MachineStatus.IsDownForMaintenance ? MachineStatus.Warning : secondMachine.Status;
                firstMachine.StatusMessage = $"Swapped cutter with {secondMachine.MachineNumber}";
                firstMachine.PartNumber = secondMachine.PartNumber;
                firstMachine.CutterDataModelId = secondMachine.CutterDataModelId;
                firstMachine.Cutter.MachineDataModelId = firstMachine.Id;
                firstMachine.FrequencyCheckResult = secondMachine.FrequencyCheckResult;
                firstMachine.DateTimeLastModified = DateTime.Now;
                    
                // Set the user conducting this operation
                firstMachine.MachineUserInteractions.Add(new MachineUserInteractions
                {
                    UserDataModel = user ?? throw new NullReferenceException($"User with the name {user?.FirstName.PadRight(6)} {user?.LastName} not found"),
                    MachineDataModel = firstMachine
                });

                // Pass dummy machine's data to second machine
                secondMachine.Status = dummyMachine.Status == MachineStatus.IsDownForMaintenance ? MachineStatus.Warning : dummyMachine.Status;
                secondMachine.StatusMessage = $"Swapped cutter with {firstMachine.MachineNumber}";
                secondMachine.PartNumber = dummyMachine.PartNumber;
                secondMachine.CutterDataModelId = dummyMachine.CutterDataModelId;
                secondMachine.Cutter.MachineDataModelId = secondMachine.Id;
                secondMachine.FrequencyCheckResult = dummyMachine.FrequencyCheckResult;
                secondMachine.DateTimeLastModified = DateTime.Now;

                // Set the user conducting this operation
                secondMachine.MachineUserInteractions.Add(new MachineUserInteractions
                {
                    UserDataModel = user ?? throw new NullReferenceException($"User with the name {user?.FirstName.PadRight(6)} {user?.LastName} not found"),
                    MachineDataModel = secondMachine
                });

                // Save new info to database
                await machineTable.SaveEntityAsync(secondMachine);
                await machineTable.SaveEntityAsync(firstMachine);
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