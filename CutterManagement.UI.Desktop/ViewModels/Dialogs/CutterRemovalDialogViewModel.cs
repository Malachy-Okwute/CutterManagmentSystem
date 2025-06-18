using CutterManagement.Core;
using CutterManagement.Core.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="CutterRemovalDialog"/>
    /// </summary>
    public class CutterRemovalDialogViewModel : DialogViewModelBase, IDialogWindowCloseRequest
    {
        #region Private Fields

        /// <summary>
        /// Provides service to machine
        /// </summary>
        private readonly IMachineService _machineService;

        /// <summary>
        /// Currently selected 
        /// </summary>
        private CutterRemovalReason _cutterRemovalReason;

        /// <summary>
        /// User that is setting this machine status
        /// </summary>
        private UserDataModel _user;

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
        /// Current number
        /// </summary>
        public string CutterNumber { get; set; }

        /// <summary>
        /// Part count
        /// </summary>
        public string PartCount { get; set; }

        /// <summary>
        /// Current part count
        /// </summary>
        public string CurrentPartCount { get; set; }

        /// <summary>
        /// Previous part count
        /// </summary>
        public string PreviousPartCount { get; set; }

        /// <summary>
        /// Comment
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// True if cutter is to stay in the department
        /// </summary>
        public bool KeepCutter { get; set; }

        /// <summary>
        /// True if cutter needs to be rebuilt
        /// </summary>
        public bool RebuildCutter { get; set; }

        /// <summary>
        /// Collection of users
        /// </summary>
        public Dictionary<UserDataModel, string> UsersCollection { get; set; }

        /// <summary>
        /// Collection of cutter removal reasons
        /// </summary>
        public Dictionary<CutterRemovalReason, string> CutterRemovalReasonCollection { get; set; }

        /// <summary>
        /// User that is setting this machine status
        /// </summary>
        public UserDataModel User
        {
            get => _user;
            set => _user = value;
        }

        /// <summary>
        /// User that is setting this machine status
        /// </summary>
        public CutterRemovalReason CutterRemovalReason
        {
            get => _cutterRemovalReason;
            set => _cutterRemovalReason = value;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Event to close dialog window
        /// </summary>
        public event EventHandler<DialogWindowCloseRequestedEventArgs> DialogWindowCloseRequest;

        #endregion

        #region Public Commands

        /// <summary>
        /// Command to cancel cutter removal process
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// Command to remove cutter
        /// </summary>
        public ICommand RemoveCutterCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dataFactory">Data factory</param>
        public CutterRemovalDialogViewModel(IMachineService machineService)
        {
            _machineService = machineService;
            CutterRemovalReasonCollection = new Dictionary<CutterRemovalReason, string>();
            UsersCollection = new Dictionary<UserDataModel, string>();

            foreach (CutterRemovalReason reason in Enum.GetValues<CutterRemovalReason>())
            {
                // Add every reason
                CutterRemovalReasonCollection.Add(reason, EnumHelpers.GetDescription(reason));
            }

            CancelCommand = new RelayCommand(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess)));
            RemoveCutterCommand = new RelayCommand(async () => await RemoveCutter());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load users
        /// </summary>
        public async Task GetUsers()
        {
            // Get user db table
            IDataAccessService<UserDataModel> users = _machineService.DataBaseAccess.GetDbTable<UserDataModel>();

            foreach (UserDataModel userData in await users.GetAllEntitiesAsync())
            {
                // Do not load admin user
                if (userData.LastName is "admin")
                    continue;

                UsersCollection.Add(userData, userData.FirstName.PadRight(10) + userData.LastName);
            }

            // Set current user
            _user = UsersCollection.FirstOrDefault().Key;

            // Update UI
            OnPropertyChanged(nameof(User));

            // Refresh UI
            CollectionViewSource.GetDefaultView(UsersCollection).Refresh();
        }

        /// <summary>
        /// Removes cutter from a machine item
        /// </summary>
        private async Task RemoveCutter()
        {
            //ToDo: Refactor code below

            // Make sure piece count is entered
            if (PartCount.IsNullOrEmpty())
            {
                await DialogService.InvokeFeedbackDialog(this, "Enter part piece-count");

                return;
            }

            // Make sure new piece count is greater than current count
            if (int.Parse(PartCount) < int.Parse(CurrentPartCount))
            {
                await DialogService.InvokeFeedbackDialog(this, $"Piece-count must be greater or equal to previous-count");

                return;
            }

            // Make sure either "Keep" or "Rebuild" is selected
            if (KeepCutter is false && RebuildCutter is false)
            {
                await DialogService.InvokeFeedbackDialog(this, $"Choose \" Keep \" or \" Rebuild \" for this check");

                return;
            }

            // If count is greater than previous count by more than 100
            if ((int.Parse(PartCount) - int.Parse(CurrentPartCount)) > 100)
            {
                string warningMessage = $"Previous count is {int.Parse(CurrentPartCount)}. Do you mean to enter {PartCount} ?";

                // Verify piece count is reasonable
                bool? response = await DialogService.InvokeFeedbackDialog(this, warningMessage, FeedbackDialogKind.Prompt);

                // If user did not mean to enter the current part number
                if (response is false)
                {
                    // Cancel this process
                    return;
                }
            }

            // Make sure we have a reason for removing cutter
            if (CutterRemovalReason == CutterRemovalReason.None)
            {
                await DialogService.InvokeFeedbackDialog(this, $"Select reason for removing cutter");

                return;
            }

            // Remove cutter
            await _machineService.RemoveCutter(Id, _user.Id, KeepCutter, new MachineDataModel
            {
                FrequencyCheckResult = FrequencyCheckResult.Setup,
                Status = MachineStatus.Warning,
                StatusMessage = Comment ?? $"Cutter was removed. {DateTime.Now.ToString("g")}",
                DateTimeLastModified = DateTime.Now,
                Cutter = new CutterDataModel
                {
                    CutterChangeInfo = CutterRemovalReason,
                    LastUsedDate = DateTime.Now,
                    Condition = CutterCondition.Used,
                    Count = int.Parse(PartCount),
                    MachineDataModelId = null,
                },
                CutterDataModelId = null,
                PartNumber = null!,
                PartToothSize = "0",
            });

            // Set flag
            IsSuccess = true;

            // Close dialog
            await DialogService.InvokeAlertDialog(this, "Cutter removed successfully").ContinueWith(_ =>
            {
                DispatcherService.Invoke(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess)));
            });
        }

        #endregion
    }
}
