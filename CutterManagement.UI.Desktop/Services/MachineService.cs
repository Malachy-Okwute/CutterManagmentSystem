using CutterManagement.Core;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Documents;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Facilitates configuring of a machine
    /// </summary>
    public class MachineService : IMachineService
    {
        /// <summary>
        /// Http client
        /// </summary>
        public IHttpClientFactory HttpClientFactory { get; }

        /// <summary>
        /// View model factory
        /// </summary>
        private IDialogViewModelFactory _dialogViewModelFactory;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MachineService(IHttpClientFactory httpClientFactory, IDialogViewModelFactory? dialogViewModelFactory)
        {
            HttpClientFactory = httpClientFactory;

            if (dialogViewModelFactory is not null)
            {
                _dialogViewModelFactory = dialogViewModelFactory;
            }
        }

        /// <summary>
        /// Gets a view model for a dialog window
        /// </summary>
        /// <typeparam name="T">The type of dialog window view model to get</typeparam>
        /// <returns>Dialog window view model</returns>
        public T GetDialogViewModel<T>() where T : DialogViewModelBase
        {
            return _dialogViewModelFactory.GetService<T>();
        }


        /// <summary>
        /// Configures a machine item
        /// </summary>
        /// <param name="item">The item to configure</param>
        /// <returns><see cref="Task{ValidationResult}"/></returns>
        /// <exception cref="ArgumentException">
        /// Throws an exception if item could not be configured
        /// </exception>
        public async Task<(ValidationResult, MachineDataModel?)> ConfigureAsync(MachineDataModel newData)
        {
            HttpClient client = HttpClientFactory.CreateClient("CutterManagementApi");
            client.BaseAddress = new Uri($"https://localhost:7057");

            // Validate incoming data
            ValidationResult validationResult = DataValidationService.Validate(newData);

            if (validationResult.IsValid is false)
            {
                return (validationResult, newData);
            }

            var serverResponse = await ServerRequest.ModifyAndSaveChanges<MachineDataModel>(client, $"MachineDataModel/{newData.Id}", async machineItem =>
            {
                // Make sure we have the item and incoming data is valid
                if (machineItem is not null)
                {
                    // Wire new data 
                    machineItem.MachineNumber = newData.MachineNumber;
                    machineItem.MachineSetId = newData.MachineSetId;
                    machineItem.Status = newData.Status;
                    machineItem.StatusMessage = newData.StatusMessage ?? string.Empty;
                    machineItem.DateTimeLastModified = DateTime.Now;
                    machineItem.IsConfigured = newData.IsConfigured;

                    // Update server with new data
                    var putResponse = await ServerRequest.PutData(client, $"MachineDataModel", machineItem);

                    // TODO: Wait and process response from server 

                    newData = machineItem;
                }
            });

            // Return result
            return (validationResult, newData);
        }

        /// <summary>
        /// Set machine status <see cref="MachineStatus"/>
        /// <para>
        /// T is <see cref="ValidationResult"/>
        /// </para>
        /// </summary>
        /// <param name="newData">Machine containing the status to set</param>
        /// <param name="userId">The user executing this status set procedure</param>
        /// <param name="callback">Status set callback</param>
        /// <returns><see cref="Task{T}"/></returns>
        public async Task<ValidationResult> SetStatusAsync(MachineDataModel newData, int userId, Action<MachineDataModel> callback)
        {
            HttpClient client = HttpClientFactory.CreateClient("CutterManagementApi");
            client.BaseAddress = new Uri($"https://localhost:7057");

            var machineItem = await ServerRequest.GetData<MachineDataModel>(client, $"MachineDataModel/{newData.Id}");
            var userItem = await ServerRequest.GetData<UserDataModel>(client, $"UserDataModel/{userId}"); 

            // Validate incoming data
            ValidationResult result = DataValidationService.Validate(newData);

            // Make sure we have the item and incoming data is valid
            if (machineItem is not null && userItem is not null && result.IsValid)
            {
                // Wire new machine data 
                machineItem.Status = newData.Status;
                machineItem.DateTimeLastModified = DateTime.Now;
                machineItem.StatusMessage = newData.StatusMessage ?? $"Machine status updated. {DateTime.Now.ToString("g")}";

                // Set the user performing this operation including the machine involved
                machineItem.MachineUserInteractions.Add(new MachineUserInteractions
                {
                    UserDataModelId = userItem.Id,
                    MachineDataModelId = machineItem.Id
                });

                // Update server with new data
                var putResponse = await ServerRequest.PutData(client, $"MachineDataModel", machineItem);

                if (putResponse.IsSuccessStatusCode is false)
                {
                    return new ValidationResult { ErrorMessage = "Unexpected server error" };
                }

                // Send out message
                callback?.Invoke(machineItem);
            }

            // Return result
            return result;
        }

        /// <summary>
        /// Get a difference between two values
        /// </summary>
        /// <param name="firstValue">The first value</param>
        /// <param name="secondValue">The second value</param>
        /// <returns>Difference between two values</returns>
        private int GetValueDifference(int firstValue, int secondValue)
        {
            if (secondValue > firstValue)
            {
                return secondValue - firstValue;
            }

            return firstValue - secondValue;
        }

        /// <summary>
        /// Adjusts piece count on a machine
        /// </summary>
        /// <param name="Id">Id of the machine whose piece count is being adjusted</param>
        /// <param name="count">The current piece count on the machine</param>
        /// <param name="verifyUserIntention">Confirms user intention when new piece count is over a certain limit</param>
        public async Task AdjustPieceCountAsync(int Id, int count, Func<Task<bool?>> verifyUserIntention)
        {
            HttpClient client = HttpClientFactory.CreateClient("CutterManagementApi");
            

            var machineItem = await ServerRequest.GetData<MachineDataModel>(client, $"MachineDataModel/{Id}");
            var cutterItem = await ServerRequest.GetData<CutterDataModel>(client, $"CutterDataModel/{machineItem?.CutterDataModelId}"); // Find a different way to do this

            // Make sure machine is not null
            if (machineItem is not null && cutterItem is not null)
            {

                // Result of user intention prompt
                bool? result = null;

                // If the difference between current and new value is more than 100...
                if (GetValueDifference(count, cutterItem.Count) > 100)
                {
                    // Prompt user
                    result = await verifyUserIntention.Invoke();

                    // If user did not mean to enter value, Cancel this process.
                    if (result is not true) return;
                }

                // Set new value
                cutterItem.Count = count;

                // Set message
                machineItem.StatusMessage = $"Piece count adjusted. {DateTime.Now.ToString("g")}";

                // Set date
                machineItem.DateTimeLastModified = DateTime.Now;

                var putResponse = await ServerRequest.PutData(client, "CutterDataModel", machineItem);;

                if(putResponse.IsSuccessStatusCode)
                {
                    // Send out message
                    Messenger.MessageSender.SendMessage(machineItem);
                }
            }
        }

        /// <summary>
        /// Relocates cutter from one machine to another machine
        /// </summary>
        /// <param name="machineSendingCutterId">The machine currently with cutter</param>
        /// <param name="machineReceivingCutterId">The id of machine receiving cutter</param>
        /// <param name="userId">Id to user carrying out this process</param>
        public async Task RelocateCutterAsync(int machineSendingCutterId, int machineReceivingCutterId, int userId, string comment)
        {
            HttpClient client = HttpClientFactory.CreateClient("CutterManagementApi");
            

            var sendingMachineItem = await ServerRequest.GetData<MachineDataModel>(client, $"MachineDataModel/{machineSendingCutterId}");
            var sendingMachineCutterItem = await ServerRequest.GetData<CutterDataModel>(client, $"CutterDataModel/{sendingMachineItem?.CutterDataModelId}");
            var receivingMachineItem = await ServerRequest.GetData<MachineDataModel>(client, $"MachineDataModel/{machineReceivingCutterId}");
            var receivingMachineCutterItem = await ServerRequest.GetData<CutterDataModel>(client, $"CutterDataModel/{receivingMachineItem?.CutterDataModelId}");
            var userItem = await ServerRequest.GetData<UserDataModel>(client, $"UserDataModel/{userId}");

            // Make sure both machines are not null
            if (sendingMachineItem is not null && receivingMachineItem is not null && 
                receivingMachineCutterItem is not null && sendingMachineCutterItem is not null && userItem is not null)
            {
                // Transfer data
                receivingMachineItem.Status = sendingMachineItem.Status == MachineStatus.IsDownForMaintenance ? MachineStatus.Warning : sendingMachineItem.Status;
                receivingMachineItem.PartNumber = sendingMachineItem.PartNumber;
                receivingMachineItem.CutterDataModelId = sendingMachineItem.CutterDataModelId;
                receivingMachineCutterItem = sendingMachineCutterItem;
                receivingMachineItem.FrequencyCheckResult = sendingMachineItem.FrequencyCheckResult;
                receivingMachineItem.StatusMessage = sendingMachineItem.StatusMessage ?? $"Received cutterItem relocated from {sendingMachineItem.MachineNumber} machineItem. {DateTime.Now.ToString("g")}";
                receivingMachineItem.DateTimeLastModified = DateTime.Now;

                // Set the user performing this operation including the machine involved
                receivingMachineItem.MachineUserInteractions.Add(new MachineUserInteractions
                {
                    UserDataModelId = userItem.Id,
                    MachineDataModelId = receivingMachineItem.Id
                });

                // Clear data
                sendingMachineCutterItem.MachineDataModelId = null;
                sendingMachineItem.PartNumber = null!;
                sendingMachineItem.CutterDataModelId = null;
                sendingMachineItem.StatusMessage = $"Sent cutterItem to {receivingMachineItem.MachineNumber} machineItem. {DateTime.Now.ToString("g")}";
                sendingMachineItem.FrequencyCheckResult = FrequencyCheckResult.Setup;
                sendingMachineItem.Status = sendingMachineItem.Status == MachineStatus.IsDownForMaintenance ? MachineStatus.IsDownForMaintenance : MachineStatus.Warning;
                sendingMachineItem.DateTimeLastModified = DateTime.Now;

                // Set the user performing this operation including the machine involved
                sendingMachineItem.MachineUserInteractions.Add(new MachineUserInteractions
                {
                    UserDataModelId = userItem.Id,
                    MachineDataModelId = sendingMachineItem.Id
                });

                var putResponseUser = await ServerRequest.PutData(client, "UserDataModel", userItem);
                var putResponseFromSendingMachine = await ServerRequest.PutData(client, "MachineDataModel", sendingMachineItem);
                var putResponseFromReceivingMachine = await ServerRequest.PutData(client, "MachineDataModel", receivingMachineItem);
                var putResponseFromSendingMachineCutter = await ServerRequest.PutData(client, "CutterDataModel", sendingMachineCutterItem);
                var putResponseFromReceivingMachineCutter = await ServerRequest.PutData(client, "CutterDataModel", receivingMachineCutterItem);

                // Send out message
                Messenger.MessageSender.SendMessage(sendingMachineItem);
                Messenger.MessageSender.SendMessage(receivingMachineItem);
            }
        }

        /// <summary>
        /// Capture and records CMM data of a specific cutter
        /// </summary>
        public async Task CaptureAndRecordCMMDataAsync(int userId, int machineId, string comment, CMMDataModel incomingCMMData)
        {
            HttpClient client = HttpClientFactory.CreateClient("CutterManagementApi");
            

            var machineItem = await ServerRequest.GetData<MachineDataModel>(client, $"MachineDataModel/{machineId}");
            var cutterItem = await ServerRequest.GetData<CutterDataModel>(client, $"CutterDataModel/{machineItem?.CutterDataModelId}");
            var userItem = await ServerRequest.GetData<UserDataModel>(client, $"UserDataModel/{userId}");

            // Make sure we have machine
            if (machineItem is not null && cutterItem is not null)
            {
                CMMDataModel cmmData = new CMMDataModel
                {
                    // Set cmm data
                    BeforeCorrections = incomingCMMData.BeforeCorrections,
                    AfterCorrections = incomingCMMData.AfterCorrections,
                    PressureAngleCoast = incomingCMMData.PressureAngleCoast,
                    PressureAngleDrive = incomingCMMData.PressureAngleDrive,
                    SpiralAngleCoast = incomingCMMData.SpiralAngleCoast,
                    SpiralAngleDrive = incomingCMMData.SpiralAngleDrive,
                    Fr = incomingCMMData.Fr,
                    Size = incomingCMMData.Size,
                    Count = incomingCMMData.Count,
                    CutterDataModelId = cutterItem.Id,
                };

                // Set cmm data
                cutterItem.CMMData.Add(cmmData);

                // Set other machine information
                cutterItem.Count = int.Parse(incomingCMMData.Count);
                machineItem.StatusMessage = string.IsNullOrEmpty(comment) ? "Passed CMM check" : $"Passed CMM check. {comment}";
                machineItem.Status = MachineStatus.IsRunning;
                machineItem.FrequencyCheckResult = FrequencyCheckResult.Passed;
                machineItem.DateTimeLastModified = DateTime.Now;

                // Set the user performing this operation
                machineItem.MachineUserInteractions.Add(new MachineUserInteractions
                {
                    UserDataModelId = userItem?.Id,
                    MachineDataModelId = machineItem.Id
                });

                var putMachineResponse = await ServerRequest.PutData(client, "MachineDataModel", machineItem);
                var putCutterResponse = await ServerRequest.PutData(client, "CutterDataModel", cutterItem);

                if(putMachineResponse.IsSuccessStatusCode )
                {
                    // Send out message
                    Messenger.MessageSender.SendMessage(machineItem);
                }
            }
        }

        /// <summary>
        /// Remove cutter from a machine
        /// </summary>
        /// <param name="machineId">The machine to remove cutter from</param>
        /// <param name="userId">The user removing the cutter</param>
        /// <param name="keepCutter">True if cutter is to stay in department</param>
        /// <param name="newData">Data changing on the machine that is having it's cutter removed</param>
        public async Task RemoveCutterAsync(int machineId, int userId, bool keepCutter, MachineDataModel newData)
        {
            HttpClient client = HttpClientFactory.CreateClient("CutterManagementApi");
            

            var machineItem = await ServerRequest.GetData<MachineDataModel>(client, $"MachineDataModel/{machineId}");
            var cutterItem = await ServerRequest.GetData<CutterDataModel>(client, $"CutterDataModel/{machineItem?.CutterDataModelId}");
            var userItem = await ServerRequest.GetData<UserDataModel>(client, $"UserDataModel/{userId}");

            // If machine is not null...
            if (machineItem is not null && cutterItem is not null)
            {
                // Set cutter information
                cutterItem.CutterChangeInfo = newData.Cutter.CutterChangeInfo;
                cutterItem.LastUsedDate = DateTime.Now;
                cutterItem.Count = newData.Cutter.Count;
                cutterItem.CMMData.ToList().AddRange(cutterItem.CMMData);
                cutterItem.Condition = newData.Cutter.Condition;
                cutterItem.MachineDataModel = null!;
                cutterItem.MachineDataModelId = null;

                // Set new information
                machineItem.FrequencyCheckResult = FrequencyCheckResult.Setup;
                machineItem.Status = MachineStatus.Warning;
                machineItem.StatusMessage = string.IsNullOrEmpty(newData.StatusMessage) ? $"Cutter was removed. {DateTime.Now.ToString("g")}" : $"Cutter was removed. {newData.StatusMessage}";
                machineItem.DateTimeLastModified = DateTime.Now;
                machineItem.PartNumber = newData.PartNumber;
                machineItem.PartToothSize = "0";
                machineItem.Cutter = null!;
                machineItem.CutterDataModelId = null;

                // Set the user performing this operation
                machineItem.MachineUserInteractions.Add(new MachineUserInteractions
                {
                    UserDataModelId = userItem?.Id,
                    MachineDataModelId = machineItem.Id
                });

                // If cutter needs rebuilding...
                if (keepCutter is false)
                {
                    // NOTE: Never delete entry from database, move data that is not needed to history table or archive
                    // ToDo: Record this event in a history table

                    var putCutterResponse = await ServerRequest.PutData(client, "CutterDataModel", cutterItem);
                }
                else
                {
                    // Update cutter information 
                    var putCutterResponse = await ServerRequest.PutData(client, "CutterDataModel", cutterItem);
                }

                // Update database
                var putMachineResponse = await ServerRequest.PutData(client, "MachineDataModel", machineItem);

                if (putMachineResponse.IsSuccessStatusCode)
                {
                    // Send out message
                    Messenger.MessageSender.SendMessage(machineItem);
                }
            }
        }   

        /// <summary>
        /// Logs the production progress of a machine, including cutter and part details, to the production log table.
        /// </summary>
        /// <remarks>
        /// This method records key production details, such as machine number, cutter
        /// information, part number, and user details,  into the production log table. If the cutter information is
        /// missing (<see cref="MachineDataModel.Cutter"/> is <see langword="null"/>),  the method does not perform any
        /// logging.
        /// </remarks>
        /// <param name="user">The user associated with the operation. Can be <see langword="null"/> if no user is specified.</param>
        /// <param name="data">The machine data containing details about the machine, cutter, and production status.  
        /// <see cref="MachineDataModel.Cutter"/> property must not be <see langword="null"/>; otherwise, the method will
        /// return without logging.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task LogProductionProgressAsync(int machineId, UserDataModel? user)
        {
            HttpClient client = HttpClientFactory.CreateClient("CutterManagementApi");
            

            var machine = await ServerRequest.GetData<MachineDataModel>(client, $"MachineDataModel/{machineId}");
            var cutter = await ServerRequest.GetData<CutterDataModel>(client, $"CutterDataModel/{machine?.CutterDataModelId}");

            // Make sure we have machine
            if (machine is null || machine.Cutter is null) return;

            // Log production data progress    
            var log = new ProductionPartsLogDataModel
            {
                MachineNumber = machine.MachineNumber,
                CutterNumber = machine.Cutter.CutterNumber,
                PartNumber = machine.PartNumber,
                Comment = machine.StatusMessage,
                SummaryNumber = machine.Cutter.SummaryNumber,
                FrequencyCheckResult = machine.FrequencyCheckResult.ToString(),
                PieceCount = machine.Cutter.Count.ToString(),
                UserFullName = $"{user?.FirstName} {user?.LastName}" ?? "n/a",
                ToothCount = "n/a (coming soon)",
                CurrentShift = "n/a (coming soon)",
                ToothSize = string.IsNullOrEmpty(machine.PartToothSize) ? "n/a" : machine.PartToothSize,
                CMMData = machine.Cutter.CMMData.LastOrDefault() ?? null,
                CutterChangeInfo = machine.Cutter.CutterChangeInfo.ToString(),
            };

            var postResponse = ServerRequest.PostData(client, "ProductionPartsLogDataModel", log);
        }
    }
}
