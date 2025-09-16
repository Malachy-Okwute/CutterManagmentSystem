﻿using CutterManagement.Core;
using CutterManagement.Core.Services;
using System.Net.Http;
using System.Windows.Data;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="MachineSetupDialog"/>
    /// </summary>
    public class MachineSetupDialogViewModel : DialogViewModelBase, IDialogWindowCloseRequest
    {
        #region Private Fields

        /// <summary>
        /// Machine item that is being setup
        /// </summary>
        private MachineItemViewModel _machineItemViewModel;

        /// <summary>
        /// Provides services to machine
        /// </summary>
        private readonly IMachineService _machineService;

        /// <summary>
        /// Cutter number
        /// </summary>
        private string _cutterNumber;

        /// <summary>
        /// Collection of part
        /// </summary>
        private List<PartDataModel> _parts;

        /// <summary>
        /// Collection of cutter
        /// </summary>
        private List<CutterDataModel> _cutters;

        /// <summary>
        /// Collection of part number
        /// </summary>
        private Dictionary<int, string> _partNumberCollection;

        /// <summary>
        /// True if cutter number is valid
        /// Otherwise false
        /// </summary>
        private bool _isCutterNumberValid => _cutterNumber.Count() >= 6;

        #endregion

        #region Public Properties

        /// <summary>
        /// Machine number
        /// </summary>
        public string MachineNumber { get; set; }

        /// <summary>
        /// Collection of cutter
        /// </summary>
        public List<CutterDataModel> Cutters
        {
            get => _cutters;
            set => _cutters = value;
        }

        /// <summary>
        /// Collection of part
        /// </summary>
        public List<PartDataModel> Parts
        {
            get => _parts;
            set => _parts = value;
        }

        /// <summary>
        /// Cutter number
        /// </summary>
        public string CutterNumber
        {
            get => _cutterNumber;
            set
            {
                // ToDo: refactor this code to use be more readable
                if (!(value.StartsWith("P", true, null) || value.StartsWith("R", true, null)))
                {
                    _cutterNumber = string.Empty;
                    IsDoneButtonActive = false;
                    ClearPartCollection();
                    return;
                }
                else
                {
                    IsDoneButtonActive = true;
                }

                if (value.Count() > 1 && int.TryParse(value.Substring(1), out var number) is false)
                {
                    value = value.Remove(value.Count() - 1);
                }

                _cutterNumber = value.ToUpper();

                if (_isCutterNumberValid)
                {
                    GetCorrespondingPartNumbers();
                }
                else if (_partNumberCollection.Count > 1)
                {
                    ClearPartCollection();
                }

                if (_isCutterNumberValid is false)
                {
                    CutterCondition = string.Empty;
                    CutterType = string.Empty;
                }

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Condition of cutter indicating if cutter is a used or new cutter
        /// </summary>
        public string CutterCondition { get; set; }

        /// <summary>
        /// Type of cutter
        /// <para>Pinion | Ring</para>
        /// </summary>
        public string CutterType { get; set; }

        /// <summary>
        /// Part selected
        /// </summary>
        public int SelectedPart { get; set; }

        /// <summary>
        /// Collection of part number
        /// </summary>
        public Dictionary<int, string> PartNumberCollection
        {
            get => _partNumberCollection;
            set => _partNumberCollection = value;
        }

        /// <summary>
        /// True if done button should be active 
        /// Otherwise false
        /// </summary>
        public bool IsDoneButtonActive { get; set; }

        /// <summary>
        /// True if we are just changing part number
        /// </summary>
        public bool IsChangePartNumber { get; set; }

        #endregion

        #region Public Events

        /// <summary>
        /// Event to close dialog window
        /// </summary>
        public event EventHandler<DialogWindowCloseRequestedEventArgs> DialogWindowCloseRequest;

        #endregion

        #region Commands

        /// <summary>
        /// Command to cancel machine setup process
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// Command to Setup machine 
        /// </summary>
        public ICommand SetupMachineCommand { get; set; }

        #endregion 

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dataFactory">data factory</param>
        public MachineSetupDialogViewModel(IMachineService machineService)
        {
            Title = "Setup";
            _machineService = machineService;
            IsChangePartNumber = true;
            _cutters = new();
            _parts = new();
            _partNumberCollection = new Dictionary<int, string>
            {
                // Default KVP
                { 0, "Select a part" }
            };
            SelectedPart = PartNumberCollection.First().Key;

            // Commands
            CancelCommand = new RelayCommand(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess)));
            SetupMachineCommand = new RelayCommand(async () => await SetupMachine());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the machine we're currently setting up
        /// </summary>
        /// <param name="machineItemViewModel">The actual machine</param>
        public void GetMachineItem(MachineItemViewModel machineItemViewModel) 
        {
            _machineItemViewModel = machineItemViewModel;
            MachineNumber = _machineItemViewModel.MachineNumber;
        }

        /// <summary>
        /// Set up machine with the desired available information 
        /// </summary>
        private async Task SetupMachine()
        {
            HttpClient client = _machineService.HttpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7057");

            var cutterCollection = await ServerRequest.GetDataCollection<CutterDataModel>(client, $"CutterDataModel");
            var machineItem = await ServerRequest.GetData<MachineDataModel>(client, $"MachineDataModel/{_machineItemViewModel.Id}");

            // Get the desired cutter 
            CutterDataModel desiredCutter = _cutters.Single(cutterNumber => cutterNumber.CutterNumber == _cutterNumber);

            // If no part was selected
            if (desiredCutter.MachineDataModelId is not null && desiredCutter.MachineDataModelId.Equals(_machineItemViewModel.Id) is false)
            {
                // ToDo: Notify user that cutter is already in use as soon as cutter number is entered

                // Show feed back message
                await DialogService.InvokeFeedbackDialog(this, "Cutter already in use on another machine");

                // Do nothing else
                return;
            }

            // If no part was selected
            if (SelectedPart == 0)
            {
                // Show feed back message
                await DialogService.InvokeFeedbackDialog(this, "Select a part number");

                // Do nothing else
                return;
            }

            // -------- If we get to this point --------- //

            // Mark message as a success
            IsSuccess = true;

            if (machineItem is not null)
            {
                // Update machine information
                machineItem.Cutter = desiredCutter;
                machineItem.CutterDataModelId = desiredCutter.Id;
                machineItem.PartNumber = PartNumberCollection[SelectedPart];
                machineItem.FrequencyCheckResult = FrequencyCheckResult.Setup;
                machineItem.Status = MachineStatus.Warning;
                machineItem.StatusMessage = "Waiting for CMM check result";
                machineItem.DateTimeLastModified = DateTime.Now;

                // Update machine information
                var putResponse = await ServerRequest.PutData(client, "MachineDataModel", machineItem);

                if (putResponse.IsSuccessStatusCode)
                {
                    // Send out message
                    Messenger.MessageSender.SendMessage(machineItem);
                }

                //if(Title.Equals("Setup", StringComparison.OrdinalIgnoreCase) is false)
                if (machineItem.FrequencyCheckResult == FrequencyCheckResult.Setup)
                {
                    // Close dialog
                    await DialogService.InvokeAlertDialog(this, "Machine setup is successful").ContinueWith(_ =>
                    {
                        DispatcherService.Invoke(() => DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess)));
                    });
                }
                else
                {
                    // Close dialog
                    DialogWindowCloseRequest?.Invoke(this, new DialogWindowCloseRequestedEventArgs(IsSuccess));
                }
            }
        }

        /// <summary>
        /// Fetch cutters 
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        private async Task GetCutters()
        {
            // Clear the current collection
            _cutters.Clear();

            HttpClient client = _machineService.HttpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7057");

            var cutterCollection = await ServerRequest.GetDataCollection<CutterDataModel>(client, $"CutterDataModel");

            //Add all cutters to the collection
           cutterCollection?.ForEach(_cutters.Add);
        }

        /// <summary>
        /// Fetch parts
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        private async Task GetParts() 
        {
            // Clear the current collection
            _parts.Clear();

            HttpClient client = _machineService.HttpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7057");

            var partCollection = await ServerRequest.GetDataCollection<PartDataModel>(client, $"PartDataModel");

            partCollection?.ToList().ForEach((part) =>
            {
                if (part.PartNumber.Equals(_machineItemViewModel.PartNumber) is false) _parts.Add(part);
            });
        }

        /// <summary>
        /// Get part number for a specific cutter
        /// </summary>
        private void GetCorrespondingPartNumbers()
        {
            //Get parts
            Task.Run(GetParts).ContinueWith(async _ =>
            {
                // Get cutter
                CutterDataModel? cutter = _cutters.FirstOrDefault(c => c.CutterNumber == _cutterNumber);

                // If owners do not match or we can't find cutter...
                if (cutter is null || !(_machineItemViewModel.Owner.Equals(cutter?.Owner)))
                {
                    ClearPartCollection();
                    CutterType = "Unknown";
                    CutterCondition = "Cutter not found";
                    SelectedPart = PartNumberCollection.First().Key;
                    return;
                }

                HttpClient client = _machineService.HttpClientFactory.CreateClient();
                client.BaseAddress = new Uri("https://localhost:7057");

                //var cutterCollection = await ServerRequest.GetDataCollection<CutterDataModel>(client, $"CutterDataModel");
                var machineCollection = await ServerRequest.GetDataCollection<MachineDataModel>(client, $"MachineDataModel");

                machineCollection?.ForEach(item =>
                {
                    // Filter a specific kind of cutter
                    if (!(item.Owner.Equals(cutter.Owner))) return;

                    // If cutter number is found
                    if (cutter.CutterNumber.Equals(item.Cutter?.CutterNumber) && MachineNumber.Equals(item?.MachineNumber) is false)
                    {
                        // Run on UI thread
                        DispatcherService.Invoke(async () =>
                        {
                            // Show feed back message
                            await DialogService.InvokeFeedbackDialog(this, "Cutter already in use");

                        });

                        // Do nothing else
                        return;
                    }
                });

                // Get all part numbers that uses the current cutter number
                _parts.Where(part => (part.SummaryNumber == cutter.SummaryNumber) && (part.Kind == cutter.Kind))
                      .ToList().ForEach(part =>
                      {
                          if (_partNumberCollection.ContainsKey(part.Id) is false)
                          {
                              _partNumberCollection.Add(part.Id, part.PartNumber);
                          }
                      });

                CutterCondition = $"{EnumHelpers.GetDescription(cutter.Condition)}: {cutter.Count}";
                CutterType = EnumHelpers.GetDescription(cutter.Kind);

                // Refresh part number collection in view
                DispatcherService.Invoke(() => CollectionViewSource.GetDefaultView(PartNumberCollection).Refresh());
            });
        }

        /// <summary>
        /// Clear part collection
        /// </summary>
        private void ClearPartCollection()
        {
            foreach (var item in _partNumberCollection)
            {
                if (item.Key == 0) continue;

                _partNumberCollection.Remove(item.Key);
            }

            SelectedPart = PartNumberCollection.First().Key;

            // Refresh part number collection in view
            DispatcherService.Invoke(() => CollectionViewSource.GetDefaultView(PartNumberCollection).Refresh());
        }

        /// <summary>
        /// Reload cutters
        /// </summary>
        /// <returns></returns>
        public async Task ReloadCutters() => await GetCutters();
        
        #endregion    
    }
}