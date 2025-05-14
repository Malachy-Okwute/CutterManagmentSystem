using CutterManagement.Core;
using CutterManagement.Core.Services;
using System.Threading.Tasks;
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
        /// Data factory
        /// </summary>
        private IDataAccessServiceFactory _dataFactory;

        #endregion

        #region Public Properties

        /// <summary>
        /// Unique machine id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Before corrections value
        /// </summary>
        public string BeforeCorrections { get; set; }

        /// <summary>
        /// After corrections value
        /// </summary>
        public string AfterCorrections { get; set; }

        /// <summary>
        /// Pressure angle coast value
        /// </summary>
        public string PressureAngleCoast { get; set; }

        /// <summary>
        /// Pressure angle drive value
        /// </summary>
        public string PressureAngleDrive { get; set; }

        /// <summary>
        /// Spiral angle coast value
        /// </summary>
        public string SpiralAngleCoast { get; set; }

        /// <summary>
        /// Spiral angle drive value
        /// </summary>
        public string SpiralAngleDrive { get; set; }

        /// <summary>
        /// Fr value (Runout)
        /// </summary>
        public string Fr { get; set; }

        /// <summary>
        /// Part size according to measurement
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// Piece count
        /// </summary>
        public string Count { get; set; }

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
        public CMMCheckDialogViewModel(IDataAccessServiceFactory dataFactory)
        {
            _dataFactory = dataFactory;

            // Commands
            CancelCommand = new RelayCommand(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsMessageSuccess)));
            RecordCMMDataCommand = new RelayCommand(async () => await RecordCMMData());
        }

        #endregion

        private async Task RecordCMMData()
        {
            MachineDataModel? data = null;

            IDataAccessService<MachineDataModel> machineTable = _dataFactory.GetDbTable<MachineDataModel>();

            machineTable.DataChanged += (s, e) =>
            {
                data = s as MachineDataModel;

                Messenger.MessageSender.SendMessage(data ?? throw new ArgumentNullException("Machine data cannot be null"));
            };

            MachineDataModel? machine = await machineTable.GetEntityByIdAsync(Id);

            if(machine is not null)
            {

            }

        }
    }
}
