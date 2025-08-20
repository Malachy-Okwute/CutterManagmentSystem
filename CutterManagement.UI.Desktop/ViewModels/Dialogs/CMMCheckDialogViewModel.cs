using CutterManagement.Core;
using CutterManagement.Core.Services;
using System.Windows.Data;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="CMMCheckDialog"/>
    /// </summary>
    public class CMMCheckDialogViewModel : DialogViewModelBase, IDialogWindowCloseRequest
    {
        #region Private Fields

        /// <summary>
        /// Provides services to machine
        /// </summary>
        private readonly IMachineService _machineService;

        /// <summary>
        /// User that verified and confirmed cmm data
        /// </summary>
        private UserDataModel _user;

        /// <summary>
        /// Loads user
        /// </summary>
        private Task _taskLoader;

        /// <summary>
        /// Before corrections value
        /// </summary>
        private string _beforeCorrections;

        /// <summary>
        /// After corrections value
        /// </summary>
        private string _afterCorrections;

        /// <summary>
        /// Pressure angle coast value
        /// </summary>
        private string _pressureAngleCoast;

        /// <summary>
        /// Pressure angle drive value
        /// </summary>
        private string _pressureAngleDrive;

        /// <summary>
        /// Spiral angle coast value
        /// </summary>
        private string _spiralAngleCoast;

        /// <summary>
        /// Spiral angle drive value
        /// </summary>
        private string _spiralAngleDrive;

        /// <summary>
        /// Fr value (Runout)
        /// </summary>
        private string _fr;

        /// <summary>
        /// Part size according to measurement
        /// </summary>
        private string _size;

        /// <summary>
        /// True if update button is active, 
        /// Otherwise false
        /// </summary>
        private string _count;

        #endregion

        #region Public Properties

        /// <summary>
        /// Unique machine id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Before corrections value
        /// </summary>
        public string BeforeCorrections 
        { 
            get => _beforeCorrections;
            set
            {
                _beforeCorrections = value;
                IsUpdateButtonActive = CanUpdate();
                OnPropertyChanged();
            } 
        }

        /// <summary>
        /// After corrections value
        /// </summary>
        public string AfterCorrections
        {
            get => _afterCorrections;
            set
            {
                _afterCorrections = value;
                IsUpdateButtonActive = CanUpdate();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Pressure angle coast value
        /// </summary>
        public string PressureAngleCoast
        {
            get => _pressureAngleCoast;
            set
            {
                _pressureAngleCoast = value;
                IsUpdateButtonActive = CanUpdate();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Pressure angle drive value
        /// </summary>
        public string PressureAngleDrive
        {
            get => _pressureAngleDrive;
            set
            {
                _pressureAngleDrive = value;
                IsUpdateButtonActive = CanUpdate();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Spiral angle coast value
        /// </summary>
        public string SpiralAngleCoast
        {
            get => _spiralAngleCoast;
            set
            {
                _spiralAngleCoast = value;
                IsUpdateButtonActive = CanUpdate();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Spiral angle drive value
        /// </summary>
        public string SpiralAngleDrive
        {
            get => _spiralAngleDrive;
            set
            {
                _spiralAngleDrive = value;
                IsUpdateButtonActive = CanUpdate();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Fr value (Runout)
        /// </summary>
        public string Fr
        {
            get => _fr;
            set
            {
                _fr = value;
                IsUpdateButtonActive = CanUpdate();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Part size according to measurement
        /// </summary>
        public string Size
        {
            get => _size;
            set
            {
                _size = value;
                IsUpdateButtonActive = CanUpdate();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Piece count
        /// </summary>
        public string Count
        {
            get => _count;
            set
            {
                _count = value;
                IsUpdateButtonActive = CanUpdate();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Current part size of a used cutter
        /// </summary>
        public string CurrentCount { get; set; }

        /// <summary>
        /// Comment associated with this cmm check
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// True if update button is active, 
        /// Otherwise false
        /// </summary>
        public bool IsUpdateButtonActive { get; set; }

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
        /// Event that is invoked when user cancels or proceeds with recording CMM data
        /// </summary>
        public event EventHandler<DialogWindowCloseRequestedEventArgs> DialogWindowCloseRequest;

        #endregion

        #region Commands 

        /// <summary>
        /// Command to cancel CMM data recording process
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// Command to record CMM data
        /// </summary>
        public ICommand RecordCMMDataCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dataFactory"></param>
        public CMMCheckDialogViewModel(IMachineService machineService)
        {
            UsersCollection = new Dictionary<UserDataModel, string>();

            _machineService = machineService;

            _taskLoader = GetUsers();

            // Commands
            CancelCommand = new RelayCommand(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess)));
            RecordCMMDataCommand = new RelayCommand(async () => await RecordCMMData());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Takes a record of CMM data associated with a part measured on a CMM
        /// </summary>
        private async Task RecordCMMData()
        {
            // Make sure piece count is greater than current count
            if(int.TryParse(CurrentCount, out int result) is true && int.Parse(Count) <= int.Parse(CurrentCount))
            {
                // Define message
                string errorMessage = $"Piece-count must be greater than previous-count";

                // Show message
                await DialogService.InvokeFeedbackDialog(this, errorMessage);

                // Do nothing else
                return;
            }

            // Data to capture
            CMMDataModel cmmData = new CMMDataModel
            {
                BeforeCorrections = BeforeCorrections,
                AfterCorrections = AfterCorrections,
                PressureAngleCoast = PressureAngleCoast,
                PressureAngleDrive = PressureAngleDrive,
                SpiralAngleCoast = SpiralAngleCoast,
                SpiralAngleDrive = SpiralAngleDrive,
                Fr = Fr,
                Size = Size,
                Count = Count,
            };

            // Capture and record CMM Data
            await  _machineService.CaptureAndRecordCMMDataAsync(_user.Id, Id, Comment, cmmData);

            // Mark message as a success
            IsSuccess = true;

            // Define message
            string successMessage = "CMM data recorded successfully";

            // Close dialog
            await DialogService.InvokeAlertDialog(this, successMessage).ContinueWith(_ =>
            {
                DispatcherService.Invoke(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess)));
            });
        }

        /// <summary>
        /// Checks if data entered can be updated
        /// </summary>
        /// <returns>True if data can be updated</returns>
        public bool CanUpdate()
        {
            return double.TryParse(BeforeCorrections, out double beforeCorrectionResult) is true &&
                   double.TryParse(AfterCorrections, out double afterCorrectionResult) is true &&
                   double.TryParse(PressureAngleCoast, out double pressureAngleCoast) is true &&
                   double.TryParse(PressureAngleDrive, out double pressureAngleDrive) is true &&
                   double.TryParse(SpiralAngleCoast, out double spiralAngleCoast) is true &&
                   double.TryParse(SpiralAngleDrive, out double spiralAngleDrive) is true &&
                   double.TryParse(Fr, out double fr) is true &&
                   double.TryParse(Count, out double count) is true &&
                   double.TryParse(Size, out double size) is true;
        }

        /// <summary>
        /// Load users
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        private async Task GetUsers()
        {
            // Get user db table
            using var users = _machineService.DataBaseAccess.GetDbTable<UserDataModel>();

            foreach (UserDataModel userData in await users.GetAllEntitiesAsync())
            {
                // Do not load admin user
                if (userData.LastName is "admin")
                    continue;

                UsersCollection.Add(userData, userData.FirstName.PadRight(10) + userData.LastName);
            }

            // Set default user
            _user = UsersCollection.FirstOrDefault().Key;

            // Update UI
            OnPropertyChanged(nameof(User));

            // Refresh UI
            CollectionViewSource.GetDefaultView(UsersCollection).Refresh();
        }

        #endregion
    }
}
