using CutterManagement.Core;
using CutterManagement.Core.Services;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="FrequencyCheckDialog"/>
    /// </summary>
    public class FrequencyCheckDialogViewModel : DialogViewModelBase, IDialogWindowCloseRequest
    {
        #region Private Fields

        /// <summary>
        /// User that is setting this machine status
        /// </summary>
        private UserDataModel _user;

        /// <summary>
        /// Passed check
        /// </summary>
        private FrequencyCheckResult _passedCheck = FrequencyCheckResult.Passed;

        /// <summary>
        /// Failed check
        /// </summary>
        private FrequencyCheckResult _failedCheck = FrequencyCheckResult.Failed;

        /// <summary>
        /// Provides service to machine
        /// </summary>
        private readonly IMachineService _machineService;

        #endregion

        #region Public Properties

        /// <summary>
        /// Unique machine id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Machine number
        /// </summary>
        public string MachineNumber { get; set; }

        /// <summary>
        /// Part number
        /// </summary>
        public string PartNumber { get; set; }

        /// <summary>
        /// Part count
        /// </summary>
        public string PartCount { get; set; }

        /// <summary>
        /// Part tooth size
        /// </summary>
        public string? PartToothSize { get; set; }

        /// <summary>
        /// Previous part count
        /// </summary>
        public string PreviousPartCount { get; set; } 

        /// <summary>
        /// Previous part tooth size
        /// </summary>
        public string? PreviousPartToothSize { get; set; }

        /// <summary>
        /// Comment
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// True if frequency check passed
        /// </summary>
        public bool PassedCheck { get; set; }

        /// <summary>
        /// True if frequency check failed
        /// </summary>
        public bool FailedCheck { get; set; }

        /// <summary>
        /// Collection of users
        /// </summary>
        public Dictionary<UserDataModel, string> UsersCollection { get; set; }

        /// <summary>
        /// User that is setting this machine status
        /// </summary>
        public UserDataModel User
        {
            get => _user;
            set => _user = value;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Close dialog request event
        /// </summary>
        public event EventHandler<DialogWindowCloseRequestedEventArgs> DialogWindowCloseRequest;

        #endregion

        #region Public Commands

        /// <summary>
        /// Command to cancel frequency check process
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// Command to update machine data
        /// </summary>
        public ICommand UpdateCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FrequencyCheckDialogViewModel(IMachineService machineService)
        {
            UsersCollection = new Dictionary<UserDataModel, string>();
            _machineService = machineService;

            _ = GetUsers();

            // Create commands
            CancelCommand = new RelayCommand(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess)));
            UpdateCommand = new RelayCommand(async () => await UpdateMachine());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates machine info
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        private async Task UpdateMachine()
        {
            HttpClient client = _machineService.HttpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7057");

            var machineItem = await ServerRequest.GetData<MachineDataModel>(client, $"MachineDataModel/{Id}");
            var userItem = await ServerRequest.GetData<UserDataModel>(client, $"UserDataModel/{_user.Id}");
            
            // If machine is not null...
            if (machineItem is not null)
            {
                // Make sure piece count is entered
                if (PartCount.IsNullOrEmpty())
                {
                    await DialogService.InvokeFeedbackDialog(this, "Enter part piece-count");

                    return;
                }

                // Make sure new piece count is greater than current count
                if (int.Parse(PartCount) <= machineItem.Cutter.Count)
                {
                    await DialogService.InvokeFeedbackDialog(this, "Piece-count must be greater than previous-count");

                    return;
                }

                // Make sure either "Pass" or "Fail" is selected
                if (PassedCheck is false && FailedCheck is false)
                {
                    await DialogService.InvokeFeedbackDialog(this, "Choose \" Passed \" or \" Failed \" for this check");

                    return;
                }

                // ToDo: Prompt user if the want remove or keep cutter 
                // Prompt users enter why part failed
                if (FailedCheck is true && string.IsNullOrEmpty(Comment))
                {
                    await DialogService.InvokeFeedbackDialog(this, "Please specify the issue with part in the comment section");

                    return;
                }

                // If count is greater than previous count by more than 100
                if ((int.Parse(PartCount) - machineItem.Cutter.Count) > 100)
                {
                    string errorMessage = $"Current count is {machineItem.Cutter.Count}. Do you mean to enter {PartCount} ?";

                    // Verify piece count is reasonable
                    bool? response = await DialogService.InvokeFeedbackDialog(this, errorMessage, FeedbackDialogKind.Prompt);

                    // If user did not mean to enter the current part number
                    if (response is false)
                    {
                        // Cancel this process
                        return;
                    }
                }

                // Set new information
                machineItem.Cutter.Count = int.Parse(PartCount);
                machineItem.PartToothSize = PartToothSize ?? machineItem.PartToothSize;
                machineItem.FrequencyCheckResult = PassedCheck ? _passedCheck : _failedCheck;
                machineItem.Status = (machineItem.FrequencyCheckResult == _failedCheck) ? MachineStatus.Warning : MachineStatus.IsRunning;
                machineItem.StatusMessage = (machineItem.FrequencyCheckResult == _failedCheck) ? (Comment!) : (Comment ?? "In good condition");
                machineItem.DateTimeLastModified = DateTime.Now;

                // Set the user performing this operation
                machineItem.MachineUserInteractions.Add(new MachineUserInteractions
                {
                    UserDataModelId = userItem?.Id,
                    MachineDataModelId = machineItem.Id
                });

                // Update machine on database
                var putResponse = await ServerRequest.PutData<MachineDataModel>(client, "MachineDataModel", machineItem);

                if(putResponse.IsSuccessStatusCode)
                {
                    // Send out message
                    Messenger.MessageSender.SendMessage(machineItem);
                }

                // Close dialog
                DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess));
            }
        }

        /// <summary>
        /// Load users
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        private async Task GetUsers()
        {
            HttpClient client = _machineService.HttpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7057");

            var userCollection = await ServerRequest.GetDataCollection<UserDataModel>(client, $"UserDataModel");

            userCollection?.ForEach(user =>
            {
                // ToDo: Consider users that might be staying late or coming in early

                // If user is not admin, is active, not archived and is in current shift
                if (user.LastName is "admin" || user.IsActive is false || user.IsArchived ||
                    EnumHelpers.GetDescription(user.Shift) != ShiftHelper.GetCurrentShift())
                    return;

                UsersCollection.Add(user, user.FirstName.PadRight(10) + user.LastName);
            });

            // Set current user
            _user = UsersCollection.FirstOrDefault().Key;

            // Update UI
            OnPropertyChanged(nameof(User));

            // Refresh UI
            CollectionViewSource.GetDefaultView(UsersCollection).Refresh();
        }

        #endregion
    }
}