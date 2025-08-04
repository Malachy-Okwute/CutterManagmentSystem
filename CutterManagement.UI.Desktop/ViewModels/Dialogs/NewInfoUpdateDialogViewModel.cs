using CutterManagement.Core;
using CutterManagement.Core.Services;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO.Packaging;
using System.Windows.Data;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="NewInfoUpdateDialog"/>
    /// </summary>
    public class NewInfoUpdateDialogViewModel : DialogViewModelBase, IDialogWindowCloseRequest
    {
        #region Private Fields

        /// <summary>
        /// Data access
        /// </summary>
        private IDataAccessServiceFactory _dataFactory;

        /// <summary>
        /// The author of this information
        /// </summary>
        private UserDataModel _user;

        /// <summary>
        /// Flag indicating that users are currently being fetched
        /// </summary>
        private bool _isFetchingUsers;

        /// <summary>
        /// The kind of part (Gear / Pinion)
        /// </summary>
        private PartKind _kind;

        #endregion

        #region Public Properties

        /// <summary>
        /// Unique associate with this information
        /// <para>
        /// NOTE: Used when editing this information
        /// </para>
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        /// True if information is being edited
        /// </summary>
        public bool IsEditMode { get; set; }

        /// <summary>
        /// Button content
        /// </summary>
        public string ButtonContent => IsEditMode ? "Update" : "Broadcast";

        /// <summary>
        /// Title of this information update
        /// </summary>
        public new string Title { get; set; }

        /// <summary>
        /// Author of this information update
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// The actual information
        /// </summary>
        public string Information { get; set; }

        /// <summary>
        /// Collection of users
        /// </summary>
        public Dictionary<UserDataModel, string> UsersCollection { get; set; }

        /// <summary>
        /// The author of this information update
        /// </summary>
        public UserDataModel User
        {
            get => _user;
            set => _user = value;
        }

        /// <summary>
        /// True if new adjustment control open
        /// </summary>
        public bool IsNewAdjustmentControlOpen { get; set; }

        /// <summary>
        /// The kind of part (Gear / Pinion)
        /// </summary>
        public PartKind Kind
        {
            get => _kind;
            set 
            {
                _kind = value;

                _ = GetPartNumbers(_kind);
            }

        }

        /// <summary>
        /// Available type of parts
        /// <para>
        /// Used when attaching move adjustment
        /// </para>
        /// </summary>
        public Dictionary<PartKind, string> PartKindCollection { get; set; }

        /// <summary>
        /// Collection of part number
        /// </summary>
        public Dictionary<PartDataModel, string> PartNumberCollection { get; set; }

        /// <summary>
        /// The selected part number 
        /// </summary>
        public PartDataModel? SelectedPartNumber { get; set; }

        /// <summary>
        /// Pressure angle value on coast
        /// </summary>
        public string PressureAngleCoast { get; set; }

        /// <summary>
        /// Pressure angle value on drive
        /// </summary>
        public string PressureAngleDrive { get; set; }

        /// <summary>
        /// Spiral angle value on coast
        /// </summary>
        public string SpiralAngleCoast { get; set; }

        /// <summary>
        /// Spiral angle value on drive
        /// </summary>
        public string SpiralAngleDrive { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Event that is called to close dialog window
        /// </summary>
        public event EventHandler<DialogWindowCloseRequestedEventArgs> DialogWindowCloseRequest;

        #endregion

        #region Commands

        /// <summary>
        /// Command to cancel this operation
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// Command to broadcast new information and update
        /// </summary>
        public ICommand BroadcastCommand { get; set; }

        /// <summary>
        /// Command to show adjustment control
        /// </summary>
        public ICommand OpenNewAdjustmentControlCommand { get; set; }

        /// <summary>
        /// Command to hide adjustment control
        /// </summary>
        public ICommand CloseNewAdjustmentControlCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public NewInfoUpdateDialogViewModel(IDataAccessServiceFactory dataFactory)
        {
            _dataFactory = dataFactory;
            Initialize();

            CancelCommand = new RelayCommand(() =>
            {
                DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess));
                ClearDataResidue();
            });
            BroadcastCommand = new RelayCommand(async () => await BroadcastInformation());
            OpenNewAdjustmentControlCommand = new RelayCommand(() => IsNewAdjustmentControlOpen = true);
            CloseNewAdjustmentControlCommand = new RelayCommand(() => IsNewAdjustmentControlOpen = false);
        }

        #endregion

        #region Methods

        /// <summary>
        /// View model initialization
        /// </summary>
        protected override void Initialize()
        {
            // Initialize properties
            UsersCollection = new Dictionary<UserDataModel, string>();
            PartKindCollection = new Dictionary<PartKind, string>();
            PartNumberCollection = new Dictionary<PartDataModel, string>();
            _kind = PartKind.None;

            // Get users
            _ = GetUsers();

            // Get part kinds
            foreach (PartKind part in Enum.GetValues<PartKind>())
            {
                PartKindCollection.Add(part, EnumHelpers.GetDescription(part));
            }
        }

        /// <summary>
        /// Broadcast new information   
        /// </summary>
        private async Task BroadcastInformation()
        {
            InfoUpdateDataModel? data = null;

            // Get info table
            var infoUpdateTable = _dataFactory.GetDbTable<InfoUpdateDataModel>();
            // Get users table
            var userTable = _dataFactory.GetDbTable<UserDataModel>();

            // Get user
            UserDataModel? user = await userTable.GetEntityByIdAsync(_user.Id);
            // Listen for when data changed in the database
            infoUpdateTable.DataChanged += (ss, e) =>
            {
                data = e as InfoUpdateDataModel;

                if(data is not null)
                {
                    Messenger.MessageSender.SendMessage(data);
                }
            };
            // Get info from db (Used when editing information).
            InfoUpdateDataModel? existingInfo = await infoUpdateTable.GetEntityByIdAsync(Id);
            // Create new information
            InfoUpdateDataModel newInfo = new InfoUpdateDataModel
            {
                Title = Title,
                Information = Information,
                DateCreated = DateTime.Now,
                PublishDate = DateTime.Now.ToString("MM-dd-yyyy ~ hh:mm tt"),
                LastUpdatedDate = DateTime.Now.ToString("MM-dd-yyyy ~ hh:mm tt"),

                // Attached moves
                Kind = Kind,
                PartNumberWithMove = SelectedPartNumber?.PartNumber ?? string.Empty,
                PressureAngleCoast = string.IsNullOrEmpty(PressureAngleCoast) ? "0" : PressureAngleCoast,
                PressureAngleDrive = string.IsNullOrEmpty(PressureAngleDrive) ? "0" : PressureAngleDrive,
                SpiralAngleCoast = string.IsNullOrEmpty(SpiralAngleCoast) ? "0" : SpiralAngleCoast,
                SpiralAngleDrive = string.IsNullOrEmpty(SpiralAngleDrive) ? "0" : SpiralAngleDrive, 
            };


            // If user is not null
            if(user is not null)
            {
                // Set the user performing this operation
                newInfo.InfoUpdateUserRelations.Add(new InfoUpdateUserRelations
                {
                    UserDataModel = user,
                    InfoUpdateDataModel = newInfo
                });

                // If we are not in editing mode...
                if (IsEditMode is false)
                {
                    // Validate data
                    ValidationResult result = new ValidationResult();
                    result = DataValidationService.Validate(newInfo);

                    // If validation failed...
                    if (result.IsValid is false)
                    {
                        // Notify user with error message
                        await DialogService.InvokeFeedbackDialog(this, result.ErrorMessage);
                        // Do nothing else
                        return;
                    }

                    // Create new information entry in database
                    await infoUpdateTable.CreateNewEntityAsync(newInfo);
                }
                // Otherwise...
                else if (IsEditMode && existingInfo is not null)
                {
                    // Validate data
                    ValidationResult result = new ValidationResult();
                    result = DataValidationService.Validate(existingInfo);

                    if(result.IsValid is false)
                    {
                        // Notify user with error message
                        await DialogService.InvokeFeedbackDialog(this, result.ErrorMessage);
                        // Do nothing else
                        return;
                    }

                    // Reset edit mode
                    IsEditMode = false;

                    // Set latest info to the existing info
                    existingInfo.Title = Title;
                    existingInfo.Information = Information;
                    existingInfo.LastUpdatedDate = DateTime.Now.ToString("MM-dd-yyyy ~ hh:mm tt");
                    existingInfo.InfoUpdateUserRelations.Add(new InfoUpdateUserRelations
                    {
                        UserDataModel = user,
                        InfoUpdateDataModel = existingInfo
                    });

                    // Attached moves if available
                    existingInfo.Kind = Kind;
                    existingInfo.PartNumberWithMove = SelectedPartNumber?.PartNumber ?? string.Empty;
                    existingInfo.PressureAngleCoast = string.IsNullOrEmpty(PressureAngleCoast) ? "0" : PressureAngleCoast;
                    existingInfo.PressureAngleDrive = string.IsNullOrEmpty(PressureAngleDrive) ? "0" : PressureAngleDrive;
                    existingInfo.SpiralAngleCoast = string.IsNullOrEmpty(SpiralAngleCoast) ? "0" : SpiralAngleCoast;
                    existingInfo.SpiralAngleDrive = string.IsNullOrEmpty(SpiralAngleDrive) ? "0" : SpiralAngleDrive; 

                    // Update existing info
                    await infoUpdateTable.UpdateEntityAsync(existingInfo);
                }

                // Stop listening for data changing
                infoUpdateTable.DataChanged -= delegate { };
            }

            // Mark process as successful
            IsSuccess = true;
            // Close dialog
            DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess));
            // Clear data 
            ClearDataResidue();
        }

        /// <summary>
        /// Load users
        /// </summary>
        private async Task GetUsers()
        {
            // Get user db table
            IDataAccessService<UserDataModel> users = _dataFactory.GetDbTable<UserDataModel>();

            if (_isFetchingUsers) return;
            {
                _isFetchingUsers = true;

                try
                {
                    // Collection of users
                    IReadOnlyList<UserDataModel> collectionOfUsers = await users.GetAllEntitiesAsync();

                    foreach (UserDataModel userData in collectionOfUsers)
                    {
                        // Do not load admin user
                        if (userData.LastName is "admin")
                            continue;

                        UsersCollection.Add(userData, userData.FirstName.PadRight(10) + userData.LastName);
                    }
                }
                finally
                {
                    _isFetchingUsers = false;
                }
            }

            // Set current user
            _user = UsersCollection.FirstOrDefault().Key;

            // Update UI
            OnPropertyChanged(nameof(User));

            // Refresh UI
            CollectionViewSource.GetDefaultView(UsersCollection).Refresh();
        }

        /// <summary>
        /// Gets part numbers of a specified kind from database
        /// </summary>
        /// <param name="partKind">The kind of parts to get</param>
        private async Task GetPartNumbers(PartKind partKind)
        {
            PartNumberCollection.Clear();

            var partTable = _dataFactory.GetDbTable<PartDataModel>();

            IEnumerable<PartDataModel> parts = (await partTable.GetAllEntitiesAsync()).Where(p => p.Kind == partKind);

            parts.ToList().ForEach(part => PartNumberCollection.Add(part, part.PartNumber));

            CollectionViewSource.GetDefaultView(PartNumberCollection).Refresh();
        }

        /// <summary>
        /// Clears any data residue
        /// </summary>
        private void ClearDataResidue()
        {
            Title = Author = Information = string.Empty;
            PressureAngleCoast = string.Empty;
            PressureAngleDrive = string.Empty;
            SpiralAngleCoast = string.Empty;
            SpiralAngleDrive = string.Empty;
            SelectedPartNumber = null;
            _kind = PartKind.None;

            // Set current user
            _user = UsersCollection.FirstOrDefault().Key;
        }

        #endregion
    }
}
