using CutterManagement.Core;
using CutterManagement.Core.Services;
using System.Windows.Input;

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
            Initialize();

            CreatePartCommand = new RelayCommand(async () => await CreatePart());
            CancelPartCreationCommand = new RelayCommand(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess)));
        }

        #endregion

        #region Methods

        /// <summary>
        /// View model initialization
        /// </summary>
        protected override void Initialize()
        {
            PartKindCollection = new Dictionary<PartKind, string>();
            Kind = PartKind.None;

            foreach (PartKind part in Enum.GetValues<PartKind>())
            {
                PartKindCollection.Add(part, EnumHelpers.GetDescription(part));
            }
        }

        /// <summary>
        /// Create a new part
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        private async Task CreatePart()
        {
            // Get parts table
            using var partsTable = _dataServiceFactory.GetDbTable<PartDataModel>();

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
            IsSuccess = result.IsValid;

            // If validation passes
            if (result.IsValid)
            {
                // Make sure part doesn't already exist
                if ((await partsTable.GetAllEntitiesAsync()).Any(part => part.PartNumber.Equals(newPart.PartNumber)))
                {
                    // Set message
                    string errorMessage = "Part number already exists";
                    // Set success flag
                    IsSuccess = false;
                    // Briefly show message
                    await DialogService.InvokeFeedbackDialog(this, errorMessage);
                    // Do nothing else
                    return;
                }

                // Create event handler
                EventHandler<object>? handler = null;

                // Define event
                handler += (s, e) =>
                {
                    // Unhook event
                    partsTable.DataChanged -= handler;

                    // Update parts list with latest data from database
                    Messenger.MessageSender.SendMessage((PartDataModel)e);
                };

                // Listen for when parts is created
                partsTable.DataChanged += handler;

                // commit the newly created part to the parts table
                await partsTable.CreateNewEntityAsync(newPart);
            }

            // Set message
            string message = result.IsValid ? "Part created successfully" : result.ErrorMessage;

            if(result.IsValid)
            {
                // Briefly show message
                await DialogService.InvokeAlertDialog(this, message);
            }
            else
            {
                await DialogService.InvokeFeedbackDialog(this, message);
            }

            // If successful...
            if (IsSuccess)
            {
                // Send dialog window close request
                DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess));
            }
        }

        #endregion
    }
}