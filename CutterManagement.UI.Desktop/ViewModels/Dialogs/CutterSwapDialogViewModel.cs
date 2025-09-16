using CutterManagement.Core;
using CutterManagement.Core.Services;
using System.Net.Http;
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
            HttpClient client = _machineService.HttpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:7057");

            var machineCollection = await ServerRequest.GetDataCollection<MachineDataModel>(client, "MachineDataModel");

            var userCollection = await ServerRequest.GetDataCollection<UserDataModel>(client, "UserDataModel");

            // Get first machine
            var firstMachine = machineCollection?.Single(m => m.Id == _machineItem.Id);

            // Filter and find second machine id
            _secondMachine = machineCollection?.FirstOrDefault(x => x.MachineSetId == firstMachine?.MachineSetId &&
                                     x.MachineNumber != firstMachine.MachineNumber &&
                                     x.Owner == firstMachine.Owner)?.Id;

            // Get second machine
            var secondMachine = machineCollection?.Single(m => m.Id == _secondMachine);

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
            if (firstMachine is not null && secondMachine is not null)
            {
                // Set Properties
                FirstMachine = new CutterSwapHelper(firstMachine.MachineNumber,
                    $"{firstMachine.Cutter.CutterNumber}-{firstMachine.Cutter.SummaryNumber}", firstMachine.PartNumber, firstMachine.Cutter.Count.ToString());
                SecondMachine = new CutterSwapHelper(secondMachine.MachineNumber,
                    $"{secondMachine.Cutter.CutterNumber}-{secondMachine.Cutter.SummaryNumber}", secondMachine.PartNumber, secondMachine.Cutter.Count.ToString());

                userCollection?.ForEach(user =>
                {
                    // Do not load admin user
                    if (user.LastName is "admin")
                        return;

                    UserCollection.Add(user, user.FirstName.PadRight(10) + user.LastName);
                });

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

            HttpClient client = _machineService.HttpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7057");

            var firstMachine = await ServerRequest.GetData<MachineDataModel>(client, $"MachineDataModel/{initialMachineId}");
            var secondMachine = await ServerRequest.GetData<MachineDataModel>(client, $"MachineDataModel/{secondMachineId}");
            var user = await ServerRequest.GetData<UserDataModel>(client, $"UserDataModel/{_user.Id}");

            // Dummy
            MachineDataModel? dummyMachine = new MachineDataModel();

            // If both machines are available
            if (firstMachine is not null && secondMachine is not null && user is not null)
            {
                // Create a dummy machine to hold first machine's data
                dummyMachine.Status = firstMachine.Status;
                dummyMachine.StatusMessage = firstMachine.StatusMessage;
                dummyMachine.PartNumber = firstMachine.PartNumber;
                dummyMachine.CutterDataModelId = firstMachine.CutterDataModelId;
                dummyMachine.Cutter = firstMachine.Cutter;
                dummyMachine.FrequencyCheckResult = firstMachine.FrequencyCheckResult;
                dummyMachine.DateTimeLastModified = DateTime.Now;

                // Pass second machine's data to first machine
                firstMachine.Status = secondMachine.Status == MachineStatus.IsDownForMaintenance ? MachineStatus.Warning : secondMachine.Status;
                firstMachine.StatusMessage = $"Swapped cutter with {secondMachine.MachineNumber}";
                firstMachine.PartNumber = secondMachine.PartNumber;
                firstMachine.CutterDataModelId = secondMachine.CutterDataModelId;
                firstMachine.Cutter = secondMachine.Cutter;
                firstMachine.FrequencyCheckResult = secondMachine.FrequencyCheckResult;
                firstMachine.DateTimeLastModified = DateTime.Now;

                // Set the user conducting this operation
                firstMachine.MachineUserInteractions.Add(new MachineUserInteractions
                {
                    UserDataModelId = user.Id,
                    MachineDataModelId = firstMachine.Id
                });

                await _machineService.RemoveCutterAsync(secondMachine.Id, user.Id, true, new MachineDataModel
                {
                    FrequencyCheckResult = FrequencyCheckResult.Setup,
                    Status = MachineStatus.Warning,
                    StatusMessage = Comment ?? string.Empty,
                    DateTimeLastModified = DateTime.Now,
                    Cutter = new CutterDataModel
                    {
                        CutterChangeInfo = CutterRemovalReason.ChangOver,
                        LastUsedDate = DateTime.Now,
                        Condition = secondMachine.Cutter.Count > 0 ? CutterCondition.Used : CutterCondition.New,
                        Count = secondMachine.Cutter.Count,
                        MachineDataModelId = null,
                    },
                    CutterDataModelId = null,
                    PartNumber = null!,
                    PartToothSize = "0",
                });

                var firstMachinePostResponse = await ServerRequest.PutData(client,"MachineDataModel", firstMachine);

                if(firstMachinePostResponse.IsSuccessStatusCode)
                {
                    Messenger.MessageSender.SendMessage(firstMachine);
                }

                // Pass dummy machine's data to second machine
                secondMachine.Cutter = dummyMachine.Cutter;
                secondMachine.CutterDataModelId = dummyMachine.CutterDataModelId;
                secondMachine.PartNumber = dummyMachine.PartNumber;
                secondMachine.FrequencyCheckResult = dummyMachine.FrequencyCheckResult;
                secondMachine.Status = dummyMachine.Status == MachineStatus.IsDownForMaintenance ? MachineStatus.Warning : dummyMachine.Status;
                secondMachine.StatusMessage = $"Swapped cutter with {firstMachine.MachineNumber}";
                secondMachine.DateTimeLastModified = DateTime.Now;

                // Set the user conducting this operation
                secondMachine.MachineUserInteractions.Add(new MachineUserInteractions
                {
                    UserDataModelId = user.Id,
                    MachineDataModelId = secondMachine.Id
                });

                var secondMachinePostResponse = await ServerRequest.PutData(client, "MachineDataModel", secondMachine);

                if (secondMachinePostResponse.IsSuccessStatusCode)
                {
                    // Send out message
                    Messenger.MessageSender.SendMessage(secondMachine);
                }

                // Reset dummy machine
                dummyMachine = null;
            }

            //Set flag
            IsSuccess = true;

            //Notify user of the outcome
            await DialogService.InvokeAlertDialog(this, "Cutters swapped successfully").ContinueWith(x =>
            {
                DispatcherService.Invoke(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess)));
            });
        }
    }

    #endregion
}