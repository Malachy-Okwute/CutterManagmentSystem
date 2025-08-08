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

        private IDataAccessService<MachineDataModel> _machineTable;

        private IDataAccessService<UserDataModel> _userTable;

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
            _machineTable = _machineService.DataBaseAccess.GetDbTable<MachineDataModel>();

            // Get users
            _userTable = _machineService.DataBaseAccess.GetDbTable<UserDataModel>();

            // Get first machine
            _firstMachine = await _machineTable.GetEntityByIdAsync(_machineItem.Id);

            // Find second machine
             _secondMachine = (await _machineTable.GetAllEntitiesAsync())
                .FirstOrDefault(x => x.MachineSetId == _firstMachine?.MachineSetId && x.MachineNumber != _firstMachine.MachineNumber && x.Owner == _firstMachine.Owner);

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
                foreach (UserDataModel userData in await _userTable.GetAllEntitiesAsync())
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


            // Get user table
            var userTable = _machineService.DataBaseAccess.GetDbTable<UserDataModel>();

            // Get production part log table
            IDataAccessService<ProductionPartsLogDataModel> productionLogTable = _machineService.DataBaseAccess.GetDbTable<ProductionPartsLogDataModel>();


            UserDataModel? user = await userTable.GetEntityByIdAsync(_user.Id);

            // New data from database
            MachineDataModel? data = null;

            // Dummy
            MachineDataModel dummyMachine = new MachineDataModel();

            // Listen for changes 
            _machineTable.DataChanged += (s, e) =>
            {
                // Set new data
                data = e as MachineDataModel;
                // Send out message
                Messenger.MessageSender.SendMessage(data ?? throw new ArgumentNullException("Selected Machine data cannot be null"));

                // Log cmm data
                ProductionPartsLogHelper.LogProductionProgress(user, data, productionLogTable);
            };

            if (_firstMachine is not null && _secondMachine is not null)
            {
                dummyMachine.Status = _firstMachine.Status;
                dummyMachine.PartNumber = _firstMachine.PartNumber;
                dummyMachine.CutterDataModelId = _firstMachine.CutterDataModelId;
                dummyMachine.Cutter = _firstMachine.Cutter ?? throw new ArgumentNullException("Cutter cannot be null");
                dummyMachine.Cutter.MachineDataModelId = _firstMachine.Cutter.MachineDataModelId;
                dummyMachine.FrequencyCheckResult = _firstMachine.FrequencyCheckResult;
                //dummyMachine.StatusMessage = Comment;
                dummyMachine.DateTimeLastModified = DateTime.Now;

                _firstMachine.Status = MachineStatus.Warning;
                _firstMachine.PartNumber = null!;
                _firstMachine.Cutter.MachineDataModelId = null;
                _firstMachine.CutterDataModelId = null;
                _firstMachine.Cutter = null!;
                _firstMachine.FrequencyCheckResult = FrequencyCheckResult.Setup;

                await _machineTable.UpdateEntityAsync(_firstMachine);

                _firstMachine.Status = _secondMachine.Status;
                _firstMachine.StatusMessage = $"Swapped cutter with {_secondMachine.MachineNumber}";
                _firstMachine.PartNumber = _secondMachine.PartNumber;
                _firstMachine.Cutter = _secondMachine.Cutter;
                _firstMachine.CutterDataModelId = _secondMachine.CutterDataModelId;
                _firstMachine.Cutter.MachineDataModelId = _secondMachine.Cutter.MachineDataModelId;
                _firstMachine.FrequencyCheckResult = _secondMachine.FrequencyCheckResult;
                _firstMachine.DateTimeLastModified = DateTime.Now;

                _secondMachine.Status = dummyMachine.Status;
                _secondMachine.StatusMessage = $"Swapped cutter with {_firstMachine.MachineNumber}";
                _secondMachine.PartNumber = dummyMachine.PartNumber;
                _secondMachine.Cutter = dummyMachine.Cutter;
                _secondMachine.CutterDataModelId = dummyMachine.CutterDataModelId;
                _secondMachine.Cutter.MachineDataModelId = dummyMachine.Cutter.MachineDataModelId;
                _secondMachine.FrequencyCheckResult = dummyMachine.FrequencyCheckResult;
                _secondMachine.DateTimeLastModified = DateTime.Now;

                // Set the user performing this operation
                _firstMachine.MachineUserInteractions.Add(new MachineUserInteractions
                {
                    UserDataModel = user ?? throw new NullReferenceException($"User with the name {user?.FirstName.PadRight(6)} {user?.LastName} not found"),
                    MachineDataModel = _firstMachine
                });

                _secondMachine.MachineUserInteractions.Add(new MachineUserInteractions
                {
                    UserDataModel = user ?? throw new NullReferenceException($"User with the name {user?.FirstName.PadRight(6)} {user?.LastName} not found"),
                    MachineDataModel = _secondMachine
                });

                // Save new info to database
                await _machineTable.UpdateEntityAsync(_firstMachine);
                await _machineTable.UpdateEntityAsync(_secondMachine);
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