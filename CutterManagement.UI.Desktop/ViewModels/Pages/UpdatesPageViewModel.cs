using CutterManagement.Core;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="UpdatesPage"/>
    /// </summary>
    public class UpdatesPageViewModel : ViewModelBase
    {
        private readonly IDataAccessServiceFactory _dataFactory;

        private ObservableCollection<InfoUpdatesItemViewModel> _infoUpdates;

        public ICommand OpenNewInfoUpdateCommand { get; set; }
        public ObservableCollection<InfoUpdatesItemViewModel> InfoUpdates
        {
            get => _infoUpdates;
            set => _infoUpdates = value;
        }

        public UpdatesPageViewModel(IDataAccessServiceFactory dataFactory)
        {
            _dataFactory = dataFactory;

            _ = LoadUpdates();

            OpenNewInfoUpdateCommand = new RelayCommand(() => DialogService.InvokeDialog(new NewInfoUpdateDialogViewModel()));
        }

        private async Task LoadUpdates()
        {
            var infoUpdateTable = _dataFactory.GetDbTable<InfoUpdateDataModel>();

            foreach (var info in (await infoUpdateTable.GetAllEntitiesAsync()))
            {
                _infoUpdates.Add(new InfoUpdatesItemViewModel
                {
                    Author = info.Author,
                    Title = info.Title,
                    PublishDate = info.PublishDate,
                    LastUpdatedDate = info.LastUpdatedDate,
                    Information = info.Information,
                });
            }
        }
    }
}
