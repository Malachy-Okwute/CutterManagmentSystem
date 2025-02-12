using CutterManagement.Core;
using CutterManagement.Core.Services;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="CreatePartDialog"/>
    /// </summary>
    public class CreatePartDialogViewModel : DialogViewModelBase, IDialogWindowCloseRequest
    {
        /// <summary>
        /// The kind of part (Gear / Pinion)
        /// </summary>
        private PartKind _kind;

        /// <summary>
        /// Access to database
        /// </summary>
        private IDataAccessServiceFactory _dataServiceFactory;

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

        /// <summary>
        /// Event to close dialog window
        /// </summary>
        public event EventHandler<DialogWindowCloseRequestedEventArgs> DialogWindowCloseRequest;

        public event EventHandler FeedbackRequested;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CreatePartDialogViewModel(IDataAccessServiceFactory dataServiceFactory)
        {
            Kind = PartKind.None;
            _dataServiceFactory = dataServiceFactory;
            PartKindCollection = new Dictionary<PartKind, string>();

            foreach (PartKind part in Enum.GetValues<PartKind>())
            {
                PartKindCollection.Add(part, EnumHelpers.GetDescription(part));
            }
        }
    }
}
