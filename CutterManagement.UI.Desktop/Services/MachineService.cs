using CutterManagement.Core;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Facilitates configuring of a machine
    /// </summary>
    public class MachineService : IMachineService
    {
        /// <summary>
        /// Data access factory
        /// </summary>
        private IDataAccessServiceFactory _dataAccessServiceFactory;

        /// <summary>
        /// View model factory
        /// </summary>
        private IDialogViewModelFactory _dialogViewModelFactory;

        /// <summary>
        /// Access to database
        /// </summary>
        public IDataAccessServiceFactory DataBaseAccess => _dataAccessServiceFactory;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dataAccessService">Data access factory</param>
        public MachineService(IDataAccessServiceFactory dataAccessService, IDialogViewModelFactory? dialogViewModelFactory)
        {
            _dataAccessServiceFactory = dataAccessService;

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
            // Data that will be changing
            MachineDataModel? data = null;

            // Get machine table
            using var machineTable = _dataAccessServiceFactory.GetDbTable<MachineDataModel>();

            // Use an event handler to listen for data changes
            EventHandler<object>? handler = null;

            // Set up the event handler
            handler = (s, e) => 
            {
                // Unhook the event handler to prevent memory leaks
                machineTable.DataChanged -= handler;
                // Cast the event data to MachineDataModel
                data = e as MachineDataModel; 
            };

            // Subscribe to the DataChanged event
            machineTable.DataChanged += handler;

            // Get the specific item from db
            MachineDataModel? machineData = await machineTable.GetEntityByIdAsync(newData.Id);

            // Validate incoming data
            ValidationResult result = DataValidationService.Validate(newData);

            // Make sure new machine number is not already being used
            foreach (var item in await machineTable.GetAllEntitiesAsync())
            {
                if (item.MachineNumber.Equals(newData.MachineNumber))
                {
                    result.ErrorMessage = "Machine number already exist";
                    result.IsValid = false;
                }
            }

            // Make sure we have the item and incoming data is valid
            if (machineData is not null && result.IsValid)
            {
                // Wire new data 
                machineData.MachineNumber = newData.MachineNumber;
                machineData.MachineSetId = newData.MachineSetId;
                machineData.Status = newData.Status;
                machineData.StatusMessage = newData.StatusMessage ?? string.Empty;
                machineData.DateTimeLastModified = DateTime.Now;
                machineData.IsConfigured = newData.IsConfigured;

                // Save new data
                await machineTable.SaveEntityAsync(machineData);

                // Return result
                return (result, data);
            }

            // Return result
            return (result, data);
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
            // Data that will be changing
            MachineDataModel? data = null;

            // Get machines table
            using var machineTable = _dataAccessServiceFactory.GetDbTable<MachineDataModel>();

            /// Get users table
            using var userTable = _dataAccessServiceFactory.GetDbTable<UserDataModel>();

            // Get machine
            MachineDataModel? machineData = await machineTable.GetEntityByIdAsync(newData.Id);

            // Get user
            UserDataModel? user = await userTable.GetEntityByIdAsync(userId);

            // Use an event handler to listen for data changes
            EventHandler<object>? handler = null;

            // Set up the event handler
            handler += (s, e) =>
            {
                // Unhook the event handler to prevent memory leaks
                machineTable.DataChanged -= handler;
                // Cast the event data to MachineDataModel
                data = e as MachineDataModel;
                // Send out message
                callback?.Invoke(data ?? throw new ArgumentNullException("Cannot find machine item that changed"));
            };

            // Subscribe to the DataChanged event
            machineTable.DataChanged += handler;

            // Validate incoming data
            ValidationResult result = DataValidationService.Validate(newData);

            // Make sure we have the item and incoming data is valid
            if (machineData is not null && result.IsValid)
            {
                // Wire new machine data 
                machineData.Status = newData.Status;
                machineData.DateTimeLastModified = DateTime.Now;
                machineData.StatusMessage = newData.StatusMessage ?? $"Machine status updated. {DateTime.Now.ToString("g")}";

                // Set the user performing this operation including the machine involved
                machineData.MachineUserInteractions.Add(new MachineUserInteractions
                {
                    UserDataModel = user ?? throw new NullReferenceException($"User with the name {user?.FirstName.PadRight(6)} {user?.LastName} not found"),
                    MachineDataModel = machineData
                });

                // Update db with the new data
                await machineTable.SaveEntityAsync(machineData);
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
            // Data that will be changing
            MachineDataModel? data = null;

            // Get machines table
            using var machineTable = _dataAccessServiceFactory.GetDbTable<MachineDataModel>();
            // Get cutter table
            using var cutterTable = _dataAccessServiceFactory.GetDbTable<CutterDataModel>();

            // Use an event handler to listen for data changes
            EventHandler<object>? handler = null;

            // Set up the event handler
            handler += (s, e) =>
            {
                // Unhook the event handler to prevent memory leaks
                machineTable.DataChanged -= handler;
                // Cast the event data to MachineDataModel
                data = e as MachineDataModel;

                // Send out message
                Messenger.MessageSender.SendMessage(data ?? throw new ArgumentNullException("SelectedMachine data cannot be null"));
            };

            // Subscribe to the DataChanged event
            machineTable.DataChanged += handler;

            // Get machine
            MachineDataModel? machine = await machineTable.GetEntityByIdAsync(Id, cutter => cutter.Cutter);

            // Make sure machine is not null
            if (machine is not null)
            {
                // Result of user intention prompt
                bool? result = null;

                CutterDataModel cutter = await cutterTable.GetEntityByIdAsync(machine.Cutter.Id) ?? throw new NullReferenceException("Cutter not found");

                // If the difference between current and new value is more than 100...
                if (GetValueDifference(count, cutter.Count) > 100)
                {
                    // Prompt user
                    result = await verifyUserIntention.Invoke();

                    // If user did not mean to enter value, Cancel this process.
                    if (result is not true) return;
                }

                // Set new value
                cutter.Count = count;

                // Set message
                machine.StatusMessage = $"Piece count adjusted. {DateTime.Now.ToString("g")}";

                // Set date
                machine.DateTimeLastModified = DateTime.Now;

                // Update db with the new data
                await cutterTable.SaveEntityAsync(cutter);
                await machineTable.SaveEntityAsync(machine);
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
            // Data that will be changing
            MachineDataModel? data = null;

            // Get machine db tables
            using var sendingMachineTable = _dataAccessServiceFactory.GetDbTable<MachineDataModel>();
            using var receivingMachineTable = _dataAccessServiceFactory.GetDbTable<MachineDataModel>();

            // Get cutter db table
            using var cutterTable = _dataAccessServiceFactory.GetDbTable<CutterDataModel>();

            // Get user db table
            using var userTable = _dataAccessServiceFactory.GetDbTable<UserDataModel>();

            // Get machine that will be receiving cutter
            MachineDataModel? receivingMachine = await receivingMachineTable.GetEntityByIdAsync(machineReceivingCutterId);
            MachineDataModel? sendingMachine = await sendingMachineTable.GetEntityByIdAsync(machineSendingCutterId, cutter => cutter.Cutter);

            // Get user carrying out this operation
            UserDataModel? user = await userTable.GetEntityByIdAsync(userId);

            // Use an event handler to listen for data changes
            EventHandler<object>? handler = null;

            // Set up the event handler
            handler += (sender, e) =>
            {
                // Unhook the event handler to prevent memory leaks
                if(sender == sendingMachineTable)
                {
                    sendingMachineTable.DataChanged -= handler;
                }
                else
                {
                    receivingMachineTable.DataChanged -= handler;
                }

                // Cast the event data to MachineDataModel
                data = e as MachineDataModel;

                // Send out message
                Messenger.MessageSender.SendMessage(data ?? throw new ArgumentNullException("SelectedMachine data cannot be null"));
            };

            // Subscribe to the DataChanged event
            sendingMachineTable.DataChanged += handler;
            receivingMachineTable.DataChanged += handler;

            // Make sure both machines are not null
            if (sendingMachine is not null && receivingMachine is not null)
            {
                // Get cutter
                CutterDataModel? cutter = await cutterTable.GetEntityByIdAsync(sendingMachine.Cutter.Id);

                // Transfer data
                receivingMachine.Status = sendingMachine.Status == MachineStatus.IsDownForMaintenance ? MachineStatus.Warning : sendingMachine.Status;
                receivingMachine.PartNumber = sendingMachine.PartNumber;
                receivingMachine.CutterDataModelId = sendingMachine.CutterDataModelId;
                receivingMachine.Cutter = cutter ?? throw new ArgumentNullException("Cutter cannot be null");
                receivingMachine.FrequencyCheckResult = sendingMachine.FrequencyCheckResult;
                receivingMachine.StatusMessage = sendingMachine.StatusMessage ?? $"Received cutter relocated from {sendingMachine.MachineNumber} machine. {DateTime.Now.ToString("g")}";
                receivingMachine.DateTimeLastModified = DateTime.Now;

                // Set the user performing this operation including the machine involved
                receivingMachine.MachineUserInteractions.Add(new MachineUserInteractions
                {
                    UserDataModel = user ?? throw new NullReferenceException($"User with the name {user?.FirstName.PadRight(6)} {user?.LastName} not found"),
                    MachineDataModel = receivingMachine
                });

                // Clear data
                sendingMachine.PartNumber = null!;
                sendingMachine.CutterDataModelId = null;
                sendingMachine.Cutter.MachineDataModelId = null;
                sendingMachine.StatusMessage = $"Sent cutter to {receivingMachine.MachineNumber} machine. {DateTime.Now.ToString("g")}";
                sendingMachine.FrequencyCheckResult = FrequencyCheckResult.Setup;
                sendingMachine.Status = sendingMachine.Status == MachineStatus.IsDownForMaintenance ? MachineStatus.IsDownForMaintenance : MachineStatus.Warning;
                sendingMachine.DateTimeLastModified = DateTime.Now;

                // Set the user performing this operation including the machine involved
                sendingMachine.MachineUserInteractions.Add(new MachineUserInteractions
                {
                    UserDataModel = user ?? throw new NullReferenceException($"User with the name {user?.FirstName.PadRight(6)} {user?.LastName} not found"),
                    MachineDataModel = sendingMachine
                });

                // Update db with the new data
                await cutterTable.SaveEntityAsync(cutter);
                await userTable.SaveEntityAsync(user);
                await sendingMachineTable.SaveEntityAsync(sendingMachine);
                await receivingMachineTable.SaveEntityAsync(receivingMachine);
            }
        }

        /// <summary>
        /// Capture and records CMM data of a specific cutter
        /// </summary>
        public async Task CaptureAndRecordCMMDataAsync(int userId, int machineId, string comment, CMMDataModel incomingCMMData)
        {
            // The data that changed
            MachineDataModel? data = null;

            // Get machine table
            var machineTable = _dataAccessServiceFactory.GetDbTable<MachineDataModel>();

            // Get user table
            using var userTable = _dataAccessServiceFactory.GetDbTable<UserDataModel>();

            // Get cutter table
            using var cutterTable = _dataAccessServiceFactory.GetDbTable<CutterDataModel>();

            // Attempt to get machine
            MachineDataModel? machine = await machineTable.GetEntityByIdAsync(machineId, cutter => cutter.Cutter);

            // Attempt to get user
            UserDataModel? user = await userTable.GetEntityByIdAsync(userId);

            // Use an event handler to listen for data changes
            EventHandler<object>? handler = null;

            // Set up the event handler
            handler += async (s, e) =>
            {
                // Unhook the event handler to prevent memory leaks
                machineTable.DataChanged -= handler;
                // Cast the event data to MachineDataModel
                data = e as MachineDataModel;
                // Send out message
                Messenger.MessageSender.SendMessage(data ?? throw new ArgumentNullException("SelectedMachine data cannot be null"));

                // log that cutter was relocated
                if (user is not null && data is not null)
                {
                    await LogProductionProgressAsync(data.Id, user);
                }

                // Dispose tables to free resources
                machineTable.Dispose();
            };

            // Subscribe to the DataChanged event
            machineTable.DataChanged += handler;

            // Make sure we have machine
            if (machine is not null)
            {
                // Attempt to get current cutter
                CutterDataModel cutter = await cutterTable.GetEntityByIdAsync(machine.Cutter.Id) ?? throw new NullReferenceException("Cutter not found");

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
                    CutterDataModel = cutter,
                };

                // Set cmm data
                machine.Cutter.CMMData.Add(cmmData);

                // Set other machine information
                machine.Cutter.Count = int.Parse(incomingCMMData.Count);
                machine.StatusMessage = string.IsNullOrEmpty(comment) ? "Passed CMM check" : $"Passed CMM check. {comment}";
                machine.Status = MachineStatus.IsRunning;
                machine.FrequencyCheckResult = FrequencyCheckResult.Passed;
                machine.DateTimeLastModified = DateTime.Now;

                // Set the user performing this operation
                machine.MachineUserInteractions.Add(new MachineUserInteractions
                {
                    UserDataModel = user ?? throw new NullReferenceException($"User with the name {user?.FirstName.PadRight(6)} {user?.LastName} not found"),
                    MachineDataModel = machine
                });

                // Update information in database
                await cutterTable.SaveEntityAsync(cutter);
                await machineTable.SaveEntityAsync(machine);
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
            // New data from database
            MachineDataModel? data = null;

            // Get machine table
            var machineTable = _dataAccessServiceFactory.GetDbTable<MachineDataModel>();
            // Get user table
            using var userTable = _dataAccessServiceFactory.GetDbTable<UserDataModel>();
            // Get cutter table
            using var cutterTable = _dataAccessServiceFactory.GetDbTable<CutterDataModel>();
            // Get cmm data table
            using var cmmTable = _dataAccessServiceFactory.GetDbTable<CMMDataModel>();

            // Get machine
            MachineDataModel? machine = await machineTable.GetEntityByIdAsync(machineId, x => x.Cutter);

            // Get user
            UserDataModel? user = await userTable.GetEntityByIdAsync(userId);

            // Use an event handler to listen for data changes
            EventHandler<object>? handler = null;

            // Set up the event handler
            handler += async (s, e) =>
            {
                // Unhook the event handler to prevent memory leaks
                machineTable.DataChanged -= handler;
                // Set new data
                data = e as MachineDataModel;
                // Send out message
                Messenger.MessageSender.SendMessage(data ?? throw new ArgumentNullException("Selected machine data cannot be null"));

                if (user is not null && data is not null)
                {
                    await LogProductionProgressAsync(data.Id, user);
                }

                // Dispose tables to free resources
                machineTable.Dispose();
            };

            // Subscribe to the DataChanged event
            machineTable.DataChanged += handler;

            // If machine is not null...
            if (machine is not null)
            {
                // Get current cutter with the associated CMM data
                CutterDataModel? cutter = await cutterTable.GetEntityWithCollectionsByIdAsync(machine.Cutter.Id, cmm => cmm.CMMData);

                // Set cutter information
                cutter.CutterChangeInfo = newData.Cutter.CutterChangeInfo;
                cutter.LastUsedDate = DateTime.Now;
                cutter.Count = newData.Cutter.Count;
                cutter.CMMData.ToList().AddRange(cutter.CMMData);
                cutter.Condition = newData.Cutter.Condition;
                cutter.MachineDataModel = null!;
                cutter.MachineDataModelId = null;
                
                // Set new information
                machine.FrequencyCheckResult = FrequencyCheckResult.Setup;
                machine.Status = MachineStatus.Warning;
                machine.StatusMessage = string.IsNullOrEmpty(newData.StatusMessage) ? $"Cutter was removed. {DateTime.Now.ToString("g")}" : $"Cutter was removed. {newData.StatusMessage}";
                machine.DateTimeLastModified = DateTime.Now;
                machine.PartNumber = newData.PartNumber;
                machine.PartToothSize = "0";
                machine.Cutter = null!;
                machine.CutterDataModelId = null;

                // Set the user performing this operation
                machine.MachineUserInteractions.Add(new MachineUserInteractions
                {
                    UserDataModel = user ?? throw new NullReferenceException($"User with the name {user?.FirstName.PadRight(6)} {user?.LastName} not found"),
                    MachineDataModel = machine
                });

                // If cutter needs rebuilding...
                if (keepCutter is false)
                {
                    // NOTE: Never delete entry from database, move data that is not needed to history table or archive
                    // ToDo: Record this event in a history table

                    await cutterTable.SaveEntityAsync(cutter);
                }
                else
                {
                    // Update cutter information 
                    await cutterTable.SaveEntityAsync(cutter);
                }

                // Update database
                await machineTable.SaveEntityAsync(machine);
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
            // Get machine table
            using var machineTable = _dataAccessServiceFactory.GetDbTable<MachineDataModel>();

            // Get production log table
            using var productionLogTable = _dataAccessServiceFactory.GetDbTable<ProductionPartsLogDataModel>();

            // Get machine by id
            MachineDataModel? machine = await machineTable.GetEntityByIdAsync(machineId, c => c.Cutter);

            // Make sure we have machine
            if (machine is null || machine.Cutter is null) return;

            // Log production data progress    
            await productionLogTable.CreateNewEntityAsync(new ProductionPartsLogDataModel
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
            });
        }

    }
}
