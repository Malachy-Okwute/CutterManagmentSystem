using CutterManagement.Core;
using CutterManagement.Core.Services;
using System.Windows.Data;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="CutterRemovalDialogViewModel"/>
    /// </summary>
    public class CutterRelocationDialogViewModel : DialogViewModelBase, IDialogWindowCloseRequest
    {
        #region Private Fields

        /// <summary>
        /// Provides service to machine
        /// </summary>
        private readonly IMachineService _machineService;

        /// <summary>
        /// User that is setting this machine status
        /// </summary>
        private UserDataModel _user;
        
        /// <summary>
        /// Selected machine
        /// </summary>
        private MachineDataModel _selectedMachine;

        #endregion

        #region Public Properties

        /// <summary>
        /// Machine unique id 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Machine unique number of machine that have cutter
        /// </summary>
        public string MachineNumber { get; set; }

        /// <summary>
        /// Part number that is currently running on the machine
        /// </summary>
        public string PartNumber { get; set; }

        /// <summary>
        /// Cutter number on the machine
        /// </summary>
        public string CutterNumber { get; set; }

        /// <summary>
        /// The current piece count 
        /// </summary>
        public string Count { get; set; }

        /// <summary>
        /// Comment associated with this process. if any
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// The department the machine to relocate cutter from belong to
        /// </summary>
        public Department Owner { get; set; }

        /// <summary>
        /// Collection of available machines for this process
        /// </summary>
        public Dictionary<MachineDataModel, string> MachineCollection { get; set; }

        /// <summary>
        /// Collection of users
        /// </summary>
        public Dictionary<UserDataModel, string> UserCollection { get; set; }

        /// <summary>
        /// User that is setting this machine status
        /// </summary>
        public UserDataModel User
        {
            get => _user;
            set => _user = value;
        }

        /// <summary>
        /// Selected machine
        /// </summary>
        public MachineDataModel SelectedMachine
        {
            get => _selectedMachine;
            set => _selectedMachine = value;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Event that handles closing of <see cref="CutterRelocationDialog"/>
        /// </summary>
        public event EventHandler<DialogWindowCloseRequestedEventArgs> DialogWindowCloseRequest;

        #endregion

        #region Public Commands

        /// <summary>
        /// Command to cancel this process and close the <see cref="CutterRelocationDialog"/>
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// Command to relocate cutter to a different machine
        /// </summary>
        public ICommand RelocateCutterCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="machineService">Machine service</param>
        public CutterRelocationDialogViewModel(IMachineService machineService)
        {
            _machineService = machineService;
            UserCollection = new Dictionary<UserDataModel, string>();
            MachineCollection = new Dictionary<MachineDataModel, string>();

            // Create commands
            CancelCommand = new RelayCommand(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess)));
            RelocateCutterCommand = new RelayCommand(async () => await RelocateCutter());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reloads users
        /// </summary>
        public async Task ReloadUsers() => await GetUsers();

        /// <summary>
        /// Reloads valid machines 
        /// </summary>
        public async Task ReloadMachines() => await GetCorrespondingMachines();

        /// <summary>
        /// Relocates cutter and the associated part to a different machine
        /// </summary>
        private async Task RelocateCutter()
        {
            // Make sure we have machine
            if (SelectedMachine.Owner is Department.None)
            {
                // Define message
                string errorMessage = "Select a machine to continue this process";

                // Show dialog
                await DialogService.InvokeFeedbackDialog(this, errorMessage);

                // Do nothing else
                return;
            }

            // Relocate cutter
            await _machineService.RelocateCutter(Id, SelectedMachine.Id, _user.Id, Comment);

            // Set flag
            IsSuccess = true;
            // Define message
            string message = "Cutter relocated successfully";

            // Close dialog
            await DialogService.InvokeAlertDialog(this, message).ContinueWith(_ =>
            {
                DispatcherService.Invoke(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess)));
            });
        }

        /// <summary>
        /// Gets valid machines to accept cutter being relocated
        /// </summary>
        private async Task GetCorrespondingMachines()
        {
            // Make sure we have no machine from previous caller
            MachineCollection.Clear();

            // Add default machine
            MachineCollection.Add(new MachineDataModel(), "Select machine");

            // Get machine db table
            IDataAccessService<MachineDataModel> machineTable = _machineService.DataBaseAccess.GetDbTable<MachineDataModel>();

            foreach (MachineDataModel machine in await machineTable.GetAllEntitiesAsync())
            {
                // Add machines that are in the same department and also doesn't currently have cutter
                if (machine.Owner == Owner && machine.Cutter is null)
                    MachineCollection.Add(machine, machine.MachineNumber);
            }

            // Set current user
            _selectedMachine = MachineCollection.FirstOrDefault().Key;

            // Update UI
            OnPropertyChanged(nameof(SelectedMachine));

            // Refresh UI
            CollectionViewSource.GetDefaultView(MachineCollection).Refresh();
        }

        /// <summary>
        /// Load users
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        private async Task GetUsers()
        {
            // Make sure we have no user from previous caller
            UserCollection.Clear();

            // Get user db table
            IDataAccessService<UserDataModel> users = _machineService.DataBaseAccess.GetDbTable<UserDataModel>();

            foreach (UserDataModel userData in await users.GetAllEntitiesAsync())
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

            // Refresh UI
            CollectionViewSource.GetDefaultView(UserCollection).Refresh();
        }

        #endregion
    }
}
