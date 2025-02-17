using CutterManagement.Core;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="ArchivesPage"/>
    /// </summary>
    public class ArchivesPageViewModel : ViewModelBase, ISubscribeToMessages
    {
        #region Private Fields

        /// <summary>
        /// Collection of parts
        /// </summary>
        private ObservableCollection<PartItemViewModel> _partCollection;

        /// <summary>
        /// Access to database
        /// </summary>
        private IDataAccessServiceFactory _dataServiceFactory;

        /// <summary>
        /// Task loader
        /// </summary>
        private Task _loader;

        #endregion

        #region Properties

        /// <summary>
        /// Collection of parts
        /// </summary>
        public ObservableCollection<PartItemViewModel> PartCollection 
        {
            get => _partCollection;
            set => _partCollection = value; 
        }

        /// <summary>
        /// True if part collection has no part, Otherwise false
        /// </summary>
        public bool IsPartCollectionEmpty => _partCollection.Any();

        #endregion

        #region Commands

        /// <summary>
        /// Command to open a dialog that will be used to introduce a non existing part into parts collection
        /// </summary>
        public ICommand OpenCreatePartDialogCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataServiceFactory">Access to database</param>
        public ArchivesPageViewModel(IDataAccessServiceFactory dataServiceFactory)
        {
            _dataServiceFactory = dataServiceFactory;
            _partCollection = new ObservableCollection<PartItemViewModel>();

            _loader = LoadParts();

            // Create commands
            OpenCreatePartDialogCommand = new RelayCommand(OpenCreatePartDialog);

            // Register this object to receive messages from messenger
            Messenger.MessageSender.RegisterMessenger(this);
        }

        #endregion

        private void OpenCreatePartDialog()
        {
            CreatePartDialogViewModel createPartVM = new CreatePartDialogViewModel(_dataServiceFactory);
            DialogService.InvokeDialog(createPartVM);
        }

        /// <summary>
        /// Loads parts from db
        /// </summary>
        private async Task LoadParts()
        {
            // Get parts table
            IDataAccessService<PartDataModel> partsTable = _dataServiceFactory.GetDbTable<PartDataModel>();

            if((await partsTable.GetAllEntitiesAsync()).Any() is false)
            {
                OnPropertyChanged(nameof(IsPartCollectionEmpty));
                return;
            }

            foreach (PartDataModel part in await partsTable.GetAllEntitiesAsync())
            {
                // Add users
                AddPartToPartCollection(part);
            }

            OnPropertyChanged(nameof(IsPartCollectionEmpty));

            //CollectionViewSource.GetDefaultView(Users).Refresh();
        }

        private void AddPartToPartCollection(PartDataModel part)
        {
            _partCollection.Add(new PartItemViewModel
            {
                Id = part.Id,
                Kind = part.Kind,
                ModelNumber = part.Model,
                PartNumber = part.PartNumber,
                ToothCount = part.PartToothCount,
                SummaryNumber = part.SummaryNumber
            });
        }

        private void UpdatePartCollection(PartDataModel message)
        {
            if (_partCollection.Any(p => p.Id == message.Id))
                return;

            AddPartToPartCollection(message);

            OnPropertyChanged(nameof(IsPartCollectionEmpty));
        }

        /// <summary>
        /// Receive message from <see cref="Messenger"/>
        /// </summary>
        /// <param name="message">The message received</param>
        public void ReceiveMessage(IMessage message)
        {
            if (message.GetType() == typeof(PartDataModel))
            {
                UpdatePartCollection((PartDataModel)message);
            }
        }
    }
}
