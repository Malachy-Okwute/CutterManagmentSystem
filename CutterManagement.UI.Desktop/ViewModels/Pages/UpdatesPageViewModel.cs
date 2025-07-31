using CutterManagement.Core;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="UpdatesPage"/>
    /// </summary>
    public class UpdatesPageViewModel : ViewModelBase, ISubscribeToMessages
    {
        /// <summary>
        /// Access to database
        /// </summary>
        private readonly IDataAccessServiceFactory _dataFactory;

        /// <summary>
        /// View model for <see cref="NewInfoUpdateDialog"/>
        /// </summary>
        private readonly NewInfoUpdateDialogViewModel _newInfoUpdateDialog;

        /// <summary>
        /// Collection of news and updates
        /// </summary>
        private ObservableCollection<InfoUpdatesItemViewModel> _infoUpdates;
        
        /// <summary>
        /// Is busy flag
        /// </summary>
        public bool IsBusy { get; set; }

        /// <summary>
        /// Collection of news and updates
        /// </summary>
        public ObservableCollection<InfoUpdatesItemViewModel> InfoUpdates
        {
            get => _infoUpdates;
            set => _infoUpdates = value;
        }

        /// <summary>
        /// True if information update is empty, Otherwise false
        /// </summary>
        public bool IsInfoUpdateEmpty => _infoUpdates.Any();

        /// <summary>
        /// Command to open new info dialog command
        /// </summary>
        public ICommand OpenNewInfoUpdateCommand { get; set; }

        /// <summary>
        /// Command to edit information update
        /// </summary>
        public ICommand EditInfoUpdateCommand { get; set; }

        /// <summary>
        /// Command to delete an information update
        /// </summary>
        public ICommand DeleteInfoUpdateCommand { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dataFactory">Access to database</param>
        /// <param name="NewInfoUpdateDialog">New info dialog view model</param>
        public UpdatesPageViewModel(IDataAccessServiceFactory dataFactory, NewInfoUpdateDialogViewModel NewInfoUpdateDialog)
        {
            _dataFactory = dataFactory;
            _newInfoUpdateDialog = NewInfoUpdateDialog;
            _infoUpdates = new ObservableCollection<InfoUpdatesItemViewModel>();

            _ = LoadUpdates();

            // Register this object to receive messages from messenger
            Messenger.MessageSender.RegisterMessenger(this);

            // Create commands
            OpenNewInfoUpdateCommand = new RelayCommand(OpenNewInfoUpdateDialog);
            EditInfoUpdateCommand = new RelayCommand(async (itemId) => await EditInfoUpdate(Convert.ToInt32(itemId)));
            DeleteInfoUpdateCommand = new RelayCommand(async (itemId) => await DeleteInfoUpdate(Convert.ToInt32(itemId)));
        }

        /// <summary>
        /// Edits information update
        /// </summary>
        /// <param name="itemId">The id of information to edit</param>
        private async Task EditInfoUpdate(int itemId)
        {
            var infoTable = _dataFactory.GetDbTable<InfoUpdateDataModel>();
            var infoRelationsTable = _dataFactory.GetDbTable<InfoUpdateDataModel>();

            InfoUpdateDataModel? info = await infoTable.GetEntityByIdAsync(itemId);
            UserDataModel? user = (await infoRelationsTable.GetEntityByIdIncludingRelatedPropertiesAsync(itemId, i => i.InfoUpdateUserRelations))?
                                 .InfoUpdateUserRelations
                                 .FirstOrDefault()?
                                 .UserDataModel;

            if(info is not null && user is not null)
            {
                _newInfoUpdateDialog.IsEditMode = true;
                _newInfoUpdateDialog.Id = info.Id;
                _newInfoUpdateDialog.Title = info.Title;
                _newInfoUpdateDialog.Information = info.Information;
                _newInfoUpdateDialog.User = user;

                DialogService.InvokeDialog(_newInfoUpdateDialog);
            }
        }

        /// <summary>
        /// Deletes information update
        /// </summary>
        /// <param name="itemId">The id of information to edit</param>
        private async Task DeleteInfoUpdate(int itemId)
        {
            var infoTable = _dataFactory.GetDbTable<InfoUpdateDataModel>();

            InfoUpdateDataModel? info = await infoTable.GetEntityByIdAsync(itemId);

            infoTable.DataChanged += (s, e) =>
            {
                InfoUpdatesItemViewModel? itemToRemove = _infoUpdates.FirstOrDefault(i => i.Id == itemId);

                if (itemToRemove is not null)
                {
                    _infoUpdates.Remove(itemToRemove);
                }

                OnPropertyChanged(nameof(InfoUpdates));
            };

            if (info is not null)
            {
                await infoTable.DeleteEntityAsync(info);

                OnPropertyChanged(nameof(IsInfoUpdateEmpty));
            }
        }

        /// <summary>
        /// Open dialog
        /// </summary>
        private void OpenNewInfoUpdateDialog()
        {
            DialogService.InvokeDialog(_newInfoUpdateDialog);
        }

        /// <summary>
        /// Load existing news and updates into view
        /// </summary>
        private async Task LoadUpdates()
        {
            if (IsBusy) return;
            {
                try
                {
                    IsBusy = true;

                    // Make sure we have empty collection to start with
                    _infoUpdates.Clear();

                    // Get info update table
                    var infoUpdateTable = _dataFactory.GetDbTable<InfoUpdateDataModel>();

                    foreach (var info in (await infoUpdateTable.GetAllEntitiesAsync())) await AddInfoUpdate(info);


                    // Sort in a descending order
                    CollectionViewSource.GetDefaultView(InfoUpdates).SortDescriptions.Add(new SortDescription(nameof(InfoUpdatesItemViewModel.PublishDate), ListSortDirection.Descending));

                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        /// <summary>
        /// Add info update to collection
        /// </summary>
        /// <param name="infoUpdate">The item to add to the list</param>
        private async Task AddInfoUpdate(InfoUpdateDataModel infoUpdate)
        {
            // If item already exist...
            if(_infoUpdates.ToList().Exists(i => i.Id == infoUpdate.Id))
            {
                // Remove it to add an updated data
                _infoUpdates.RemoveAt(_infoUpdates.IndexOf(_infoUpdates.Single(i => i.Id == infoUpdate.Id)));
            }

            var infoUpdateTable = _dataFactory.GetDbTable<InfoUpdateDataModel>();

            UserDataModel? user = (await infoUpdateTable.GetEntityByIdIncludingRelatedPropertiesAsync(infoUpdate.Id, i => i.InfoUpdateUserRelations))?
                                    .InfoUpdateUserRelations
                                    .FirstOrDefault()?
                                    .UserDataModel;

            // Add item to collection
            _infoUpdates.Add(new InfoUpdatesItemViewModel
            {
                Id = infoUpdate.Id,
                Author = $"{user?.FirstName} {user?.LastName}",
                Title = infoUpdate.Title,
                PublishDate = infoUpdate.PublishDate,
                LastUpdatedDate = infoUpdate.LastUpdatedDate,
                Information = infoUpdate.Information,

                // Attached move
                Kind = infoUpdate.Kind.ToString(),
                PartNumber = infoUpdate.PartNumberWithMove,
                PressureAngleCoast = infoUpdate.PressureAngleCoast,
                PressureAngleDrive = infoUpdate.PressureAngleDrive,
                SpiralAngleCoast = infoUpdate.SpiralAngleCoast,
                SpiralAngleDrive = infoUpdate.SpiralAngleDrive,
            });

            // Update property
            OnPropertyChanged(nameof(InfoUpdates));
            OnPropertyChanged(nameof(IsInfoUpdateEmpty));
        }

        /// <summary>
        /// Received messages
        /// </summary>
        /// <param name="message">The message received</param>
        public void ReceiveMessage(IMessage message)
        {
            if (message.GetType() == typeof(InfoUpdateDataModel))
            {
                _ = AddInfoUpdate((InfoUpdateDataModel)message);
            }
        }
    }
}
