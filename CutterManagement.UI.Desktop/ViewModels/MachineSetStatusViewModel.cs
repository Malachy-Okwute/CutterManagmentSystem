using CutterManagement.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="SetStatusControl"/>
    /// </summary>
    public class MachineSetStatusViewModel : ViewModelBase, IDisposable
    {
        /// <summary>
        /// Machine item
        /// </summary>
        private MachineItemViewModel _machineItemViewModel;

        /// <summary>
        /// Collection of machine items
        /// </summary>
        private MachineItemCollectionViewModel _machineItemCollectionVM;

        /// <summary>
        /// Machine service
        /// </summary>
        private IMachineService _machineService;

        /// <summary>
        /// Data access service
        /// </summary>
        private IDataAccessServiceFactory _dataAccessService;

        /// <summary>
        /// The current status of the item to configure
        /// </summary>
        private object _currentStatus;

        /// <summary>
        /// Message to display about the configuration process result
        /// </summary>
        private string _message;

        /// <summary>
        /// Label indicating current machine number
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// New machine status message
        /// </summary>
        public string MachineStatusMessage { get; set; }

        /// <summary>
        /// Collection of status options available
        /// </summary>
        public Dictionary<MachineStatus, string> StatusCollection { get; set; }

        /// <summary>
        /// The current status of the item to configure
        /// </summary>
        public object CurrentStatus
        {
            get => _currentStatus;
            set => _currentStatus = value;
        }

        /// <summary>
        /// Message to display about the configuration process result
        /// </summary>
        public string Message
        {
            get => _message;
            set => _message = value;
        }

        public MachineSetStatusViewModel(MachineItemViewModel machineItemViewModel, IDataAccessServiceFactory dataAccessService, MachineItemCollectionViewModel machineItemCollectionVM)
        {
            // Initialize
            _machineItemViewModel = machineItemViewModel;
            _machineItemCollectionVM = machineItemCollectionVM;
            _dataAccessService = dataAccessService;
            CurrentStatus = _machineItemViewModel.Status;
            Label = _machineItemViewModel.MachineNumber;
            StatusCollection = new Dictionary<MachineStatus, string>();
            _machineService = new MachineService(_dataAccessService);

            foreach (MachineStatus status in Enum.GetValues<MachineStatus>())
            {
                // Add every status
                StatusCollection.Add(status, EnumHelpers.GetDescription(status));
            }
        }

        public void Dispose()
        {
            MachineStatusMessage = string.Empty;
        }
    }
}
