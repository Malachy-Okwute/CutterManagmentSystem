using CutterManagement.Core;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private bool _isBusy;

        /// <summary>
        /// Command to open new info dialog command
        /// </summary>
        public ICommand OpenNewInfoUpdateCommand { get; set; }

        /// <summary>
        /// Collection of news and updates
        /// </summary>
        public ObservableCollection<InfoUpdatesItemViewModel> InfoUpdates
        {
            get => _infoUpdates;
            set => _infoUpdates = value;
        }

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

            OpenNewInfoUpdateCommand = new RelayCommand(OpenNewInfoUpdateDialog);
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
            if (_isBusy) return;
            {
                try
                {
                    _isBusy = true;

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
                    _isBusy = false;
                }
            }
        }

        /// <summary>
        /// Add info update to collection
        /// </summary>
        /// <param name="infoUpdate">The item to add to the list</param>
        private async Task AddInfoUpdate(InfoUpdateDataModel infoUpdate)
        {
            // Make sure item doesn't already exist
            if (_infoUpdates.Any(i => i.Id != infoUpdate.Id) || _infoUpdates.IsNullOrEmpty())
            {
                var infoUpdateTable = _dataFactory.GetDbTable<InfoUpdateDataModel>();

                UserDataModel? user = (await infoUpdateTable.GetEntityByIdIncludingRelatedPropertiesAsync(infoUpdate.Id, i => i.InfoUpdateUserRelations))?
                      .InfoUpdateUserRelations
                      .FirstOrDefault()?
                      .UserDataModel;


                _infoUpdates.Add(new InfoUpdatesItemViewModel
                {
                    Id = infoUpdate.Id,
                    Author = $"{user?.FirstName} {user?.LastName}",
                    Title = infoUpdate.Title,
                    PublishDate = infoUpdate.PublishDate,
                    LastUpdatedDate = infoUpdate.LastUpdatedDate,
                    Information = infoUpdate.Information,
                });
            }

            // Update property
            OnPropertyChanged(nameof(InfoUpdates));
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
