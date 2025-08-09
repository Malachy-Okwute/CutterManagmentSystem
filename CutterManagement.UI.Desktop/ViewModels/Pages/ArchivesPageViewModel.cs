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

        /// <summary>
        /// True if still loading parts
        /// </summary>
        public bool IsLoading { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// Command to open a dialog that will be used to introduce a non existing part into parts collection
        /// </summary>
        public ICommand OpenCreatePartDialogCommand { get; set; }

        /// <summary>
        /// Command to delete a part
        /// </summary>
        public ICommand DeletePartCommand { get; set; }

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

            _ = LoadParts();

            // Create commands
            OpenCreatePartDialogCommand = new RelayCommand(OpenCreatePartDialog);
            DeletePartCommand = new RelayCommand(async () => await DeletePart());

            // Register this object to receive messages from messenger
            Messenger.MessageSender.RegisterMessenger(this);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Open dialog to create a new part
        /// </summary>
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
            IsLoading = true;

            // Clear part collection
            _partCollection.Clear();

            // Get parts table
            using var partsTable = _dataServiceFactory.GetDbTable<PartDataModel>();

            // Check if there is any part in the database
            if ((await partsTable.GetAllEntitiesAsync()).Any() is false)
            {
                // Update UI
                OnPropertyChanged(nameof(IsPartCollectionEmpty));
                return;
            }

            foreach (PartDataModel part in await partsTable.GetAllEntitiesAsync())
            {
                // Add users
                AddPartToPartCollection(part);
            }

            IsLoading = false;

            // Update UI
            OnPropertyChanged(nameof(IsPartCollectionEmpty));

            //CollectionViewSource.GetDefaultView(Users).Refresh();
        }

        /// <summary>
        /// Reload part collection
        /// </summary>
        /// <returns></returns>
        private async Task ReloadParts() => await LoadParts();

        /// <summary>
        /// Add a part to part-collection
        /// </summary>
        /// <param name="part">The part to add to the collection</param>
        private void AddPartToPartCollection(PartDataModel part)
        {
            var partItem = new PartItemViewModel
            {
                Id = part.Id,
                Kind = part.Kind,
                ModelNumber = part.Model,
                PartNumber = part.PartNumber,
                ToothCount = part.PartToothCount,
                SummaryNumber = part.SummaryNumber
            };

            partItem.PartItemSelected += (s, e) => _partCollection.ToList().ForEach(x => x.IsEditMode = false);

            _partCollection.Add(partItem);
        }

        /// <summary>
        /// Update part collection
        /// </summary>
        /// <param name="part">The part to update our collection with</param>
        private void UpdatePartCollection(PartDataModel part)
        {
            // Check if part is already in collection
            if (_partCollection.Any(p => p.Id == part.Id))
                // If it is, don't add it 
                return;

            // If it isn't, add it
            AddPartToPartCollection(part);

            // Update UI
            OnPropertyChanged(nameof(IsPartCollectionEmpty));
        }

        /// <summary>
        /// Deletes a part
        /// </summary>
        private async Task DeletePart()
        {
            PartItemViewModel? part = _partCollection.ToList().FirstOrDefault(x => x.IsEditMode == true);

            using var partTable = _dataServiceFactory.GetDbTable<PartDataModel>();

            var partToDelete = await partTable.GetEntityByIdAsync(part?.Id);

            if(partToDelete is not null)
            {
                await partTable.DeleteEntityAsync(partToDelete);

                await ReloadParts();
            }
        }

        #endregion

        #region Messeges

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

        #endregion    
    }
}
