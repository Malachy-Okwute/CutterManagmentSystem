using CutterManagement.Core;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
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
        /// Http client factory
        /// </summary>
        private readonly IHttpClientFactory _httpFactory;

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
        public UpdatesPageViewModel(IHttpClientFactory httpFactory, NewInfoUpdateDialogViewModel NewInfoUpdateDialog)
        {
            _httpFactory = httpFactory;
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
            HttpClient client = _httpFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7057/");

            var info = await ServerRequest.GetData<InfoUpdateDataModel>(client, $"InfoUpdateDataModel/{itemId}");
            var partCollection = await ServerRequest.GetDataCollection<PartDataModel>(client, $"PartDataModel");

            if(info is not null && info.UserDataModel is not null)
            {
                _newInfoUpdateDialog.IsEditMode = true;
                _newInfoUpdateDialog.Id = info.Id;
                _newInfoUpdateDialog.Title = info.Title;
                _newInfoUpdateDialog.Information = info.Information;
                _newInfoUpdateDialog.User = _newInfoUpdateDialog.UsersCollection.Single(u => u.Key.Id == info.UserDataModel.Id).Key;

                if (info.HasAttachedMoves)
                {
                    _newInfoUpdateDialog.Kind = info.Kind;
                    _newInfoUpdateDialog.SelectedPartNumber = partCollection?.FirstOrDefault(part => part.PartNumber == info.PartNumberWithMove);
                    _newInfoUpdateDialog.PressureAngleCoast = info.PressureAngleCoast;
                    _newInfoUpdateDialog.PressureAngleDrive = info.PressureAngleDrive;
                    _newInfoUpdateDialog.SpiralAngleCoast = info.SpiralAngleCoast;
                    _newInfoUpdateDialog.SpiralAngleDrive = info.SpiralAngleDrive;
                }

                DialogService.InvokeDialog(_newInfoUpdateDialog);
            }
        }

        /// <summary>
        /// Deletes information update
        /// </summary>
        /// <param name="itemId">The id of information to edit</param>
        private async Task DeleteInfoUpdate(int itemId)
        {
            HttpClient client = _httpFactory.CreateClient();
            

            var info = await ServerRequest.GetData<InfoUpdateDataModel>(client, $"InfoUpdateDataModel/{itemId}");

            if (info is not null)
            {
                var deleteResponse = await ServerRequest.DeleteData<InfoUpdateDataModel>(client, $"InfoUpdateDataModel/{info.Id}");
                InfoUpdatesItemViewModel? itemToRemove = _infoUpdates.FirstOrDefault(i => i.Id == itemId);
                
                if (deleteResponse.IsSuccessStatusCode && itemToRemove is not null)
                {
                    _infoUpdates.Remove(itemToRemove);
                    OnPropertyChanged(nameof(InfoUpdates));
                    OnPropertyChanged(nameof(IsInfoUpdateEmpty));
                }
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

                    HttpClient client = _httpFactory.CreateClient();
                    

                    // Make sure we have empty collection to start with
                    _infoUpdates.Clear();

                    var infoCollection = await ServerRequest.GetDataCollection<InfoUpdateDataModel>(client, $"InfoUpdateDataModel");

                    if (infoCollection is not null)
                    {
                        infoCollection.ForEach(async info => await AddInfoUpdate(info));
                    }

                    // Sort in a descending order
                    CollectionViewSource.GetDefaultView(InfoUpdates).SortDescriptions.Add(new SortDescription(nameof(InfoUpdatesItemViewModel.PublishDate), ListSortDirection.Descending));
                }
                catch (HttpRequestException ex)
                {
                    var test = ex.HttpRequestError;
                    var msg = ex.Message;
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
            HttpClient client = _httpFactory.CreateClient();
            

            var user = await ServerRequest.GetData<UserDataModel>(client, $"UserDataModel/{infoUpdate.UserDataModelId}");

            // If item already exist...
            if (_infoUpdates.ToList().Exists(i => i.Id == infoUpdate.Id))
            {
                // Remove it to add an updated data
                _infoUpdates.RemoveAt(_infoUpdates.IndexOf(_infoUpdates.Single(i => i.Id == infoUpdate.Id)));
            }

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
