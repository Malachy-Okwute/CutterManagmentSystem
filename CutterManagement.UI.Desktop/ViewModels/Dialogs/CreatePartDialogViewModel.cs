using CutterManagement.Core;
using CutterManagement.Core.Services;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="CreatePartDialog"/>
    /// </summary>
    public class CreatePartDialogViewModel : DialogViewModelBase, IDialogWindowCloseRequest
    {
        #region Private Fields

        /// <summary>
        /// The kind of part (Gear / Pinion)
        /// </summary>
        private PartKind _kind;

        /// <summary>
        /// Access to database
        /// </summary>
        private IDataAccessServiceFactory _dataServiceFactory;

        #endregion

        #region Public Properties

        /// <summary>
        /// The unique part number
        /// </summary>
        public string PartNumber { get; set; }

        /// <summary>
        /// Part summary number
        /// </summary>
        public string SummaryNumber { get; set; }

        /// <summary>
        /// Part model number
        /// </summary>
        public string ModelNumber { get; set; }

        /// <summary>
        /// Number of teeth on part
        /// </summary>
        public string ToothCount { get; set; }

        /// <summary>
        /// The kind of part (Gear / Pinion)
        /// </summary>
        public PartKind Kind 
        { 
            get => _kind;
            set => _kind = value; 
        }

        /// <summary>
        /// Kind of parts
        /// </summary>
        public Dictionary<PartKind, string> PartKindCollection { get; set; }

        #endregion

        #region Public Event

        /// <summary>
        /// Event to close dialog window
        /// </summary>
        public event EventHandler<DialogWindowCloseRequestedEventArgs> DialogWindowCloseRequest;

        #endregion

        #region Public Command 

        /// <summary>
        /// Command to create a part
        /// </summary>
        public ICommand CreatePartCommand { get; set; }

        /// <summary>
        /// Command to cancel part creation process
        /// </summary>
        public ICommand CancelPartCreationCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public CreatePartDialogViewModel(IDataAccessServiceFactory dataServiceFactory)
        {
            _dataServiceFactory = dataServiceFactory;
            PartKindCollection = new Dictionary<PartKind, string>();
            Kind = PartKind.None;

            foreach (PartKind part in Enum.GetValues<PartKind>())
            {
                PartKindCollection.Add(part, EnumHelpers.GetDescription(part));
            }

            CreatePartCommand = new RelayCommand(async () => await CreatePart());
            CancelPartCreationCommand = new RelayCommand(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsMessageSuccess)));
        }

        private async Task CreatePart()
        {
            // Create new part
            PartDataModel newPart = new PartDataModel
            {
                PartNumber = PartNumber,
                PartToothCount = ToothCount,
                SummaryNumber = SummaryNumber,
                Model = ModelNumber,
                Kind = Kind,
            };

            // Validate incoming data
            ValidationResult result = DataValidationService.Validate(newPart);

            // Set success flag
            IsMessageSuccess = result.IsValid;

            // If validation passes
            if (result.IsValid)
            {
                // Get parts table
                IDataAccessService<PartDataModel> partsTable = _dataServiceFactory.GetDbTable<PartDataModel>();
                // Listen for when parts is created
                partsTable.DataChanged += PartsTable_DataChanged;
                // commit the newly created part to the parts table
                await partsTable.CreateNewEntityAsync(newPart);
                // Unhook event
                partsTable.DataChanged -= PartsTable_DataChanged;
            }

            // Set message
            Message = result.IsValid ? "Part created successfully" : result.ErrorMessage;

            // Briefly show message
            await DialogService.InvokeDialogFeedbackMessage(this);

            // If successful...
            if (IsMessageSuccess)
            {
                // Send dialog window close request
                DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsMessageSuccess));
            }
        }

        /// <summary>
        /// Update parts list with latest data from database
        /// </summary>
        /// <param name="sender">Origin of this event</param>
        /// <param name="e">The actual data that changed</param>
        private void PartsTable_DataChanged(object? sender, object e) => Messenger.MessageSender.SendMessage((PartDataModel)e);

        #endregion

    }
}