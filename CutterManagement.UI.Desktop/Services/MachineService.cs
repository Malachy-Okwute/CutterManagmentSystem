using CutterManagement.Core;
using CutterManagement.DataAccess;
using System.Diagnostics;

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
        public async Task<(ValidationResult, MachineDataModel?)> Configure(MachineDataModel newData)
        {
            // Data that will be changing
            MachineDataModel? data = null;

            // Get machine table
            IDataAccessService<MachineDataModel> machineTable = _dataAccessServiceFactory.GetDbTable<MachineDataModel>();

            // Listen for when data actually changed
            machineTable.DataChanged += (s, e) => { data = e as MachineDataModel; };

            // Get the specific item from db
            MachineDataModel? machineData = await machineTable.GetEntityByIdAsync(newData.Id);

            // Validate incoming data
            ValidationResult result = DataValidationService.Validate(newData);

            // Make sure new machine number is not already being used
            foreach (var item in await machineTable.GetAllEntitiesAsync())
            {
                if (item.MachineNumber.Equals(newData.MachineNumber))
                {
                    result.ErrorMessage = "SelectedMachine number already exist";
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
                await machineTable.UpdateEntityAsync(machineData ?? throw new ArgumentException($"Could not configure entity: {machineData}"));

                // Unhook event
                machineTable.DataChanged -= delegate { };

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
        public async Task<ValidationResult> SetStatus(MachineDataModel newData, int userId, Action<MachineDataModel> callback)
        {
            // Data that will be changing
            MachineDataModel? data = null;

            // Get machines table
            IDataAccessService<MachineDataModel> machineTable = _dataAccessServiceFactory.GetDbTable<MachineDataModel>();

            /// Get users table
            IDataAccessService<UserDataModel> userTable = _dataAccessServiceFactory.GetDbTable<UserDataModel>();

            // Listen for when data actually changed
            machineTable.DataChanged += (s, e) =>
            {
                data = e as MachineDataModel;
                callback?.Invoke(data ?? throw new ArgumentNullException("Cannot find machine item that changed"));
            };

            // Get machine
            MachineDataModel? machineData = await machineTable.GetEntityByIdAsync(newData.Id);

            // Get user
            UserDataModel? user = await userTable.GetEntityByIdAsync(userId);

            // Validate incoming data
            ValidationResult result = DataValidationService.Validate(newData);

            // Make sure we have the item and incoming data is valid
            if (machineData is not null && result.IsValid)
            {
                // Wire new machine data 
                machineData.Status = newData.Status;
                machineData.StatusMessage = newData.StatusMessage;
                machineData.DateTimeLastModified = DateTime.Now;

                // Set the user performing this operation including the machine involved
                machineData.MachineUserInteractions.Add(new MachineUserInteractions
                {
                    UserDataModel = user ?? throw new NullReferenceException($"User with the name {user?.FirstName.PadRight(6)} {user?.LastName} not found"),
                    MachineDataModel = machineData
                });

                // Update db with the new data
                await machineTable.UpdateEntityAsync(machineData ?? throw new ArgumentException($"Could not configure entity: {machineData}"));

                // Unhook event
                machineTable.DataChanged -= delegate { };
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
        public async Task AdjustPieceCount(int Id, int count, Func<Task<bool?>> verifyUserIntention)
        {
            // Data that will be changing
            MachineDataModel? data = null;

            // Get machines table
            IDataAccessService<MachineDataModel> machineTable = _dataAccessServiceFactory.GetDbTable<MachineDataModel>();

            // Listen for when data actually changed
            machineTable.DataChanged += (s, e) =>
            {
                data = e as MachineDataModel;

                // Send out message
                Messenger.MessageSender.SendMessage(data ?? throw new ArgumentNullException("SelectedMachine data cannot be null"));
            };

            // Get machine
            MachineDataModel? machine = await machineTable.GetEntityByIdAsync(Id);

            // Make sure machine is not null
            if (machine is not null)
            {
                // Result of user intention prompt
                bool? result = null;

                // If the difference between current and new value is more than 100...
                if (GetValueDifference(count, machine.Cutter.Count) > 100)
                {
                    // Prompt user
                    result = await verifyUserIntention.Invoke();

                    // If user did not mean to enter value, Cancel this process.
                    if (result is not true) return;
                }

                // Set new value
                machine.Cutter.Count = count;

                // Set message
                machine.StatusMessage = $"Piece count adjusted. {DateTime.Now.ToString("g")}";

                // Set date
                machine.DateTimeLastModified = DateTime.Now;

                // Update db with the new data
                await machineTable.UpdateEntityAsync(machine ?? throw new ArgumentException($"Unable to update count"));
            }

            // Unhook event
            machineTable.DataChanged -= delegate { };
        }

        /// <summary>
        /// Relocates cutter from one machine to another machine
        /// </summary>
        /// <param name="machineSendingCutterId">The machine currently with cutter</param>
        /// <param name="machineReceivingCutterId">The id of machine receiving cutter</param>
        /// <param name="userId">Id to user carrying out this process</param>
        public async Task RelocateCutter(int machineSendingCutterId, int machineReceivingCutterId, int userId, string comment)
        {
            // Data that will be changing
            MachineDataModel? data = null;

            // Get machine db table
            IDataAccessService<MachineDataModel> machineTable = _dataAccessServiceFactory.GetDbTable<MachineDataModel>();

            // Get cutter db table
            IDataAccessService<CutterDataModel> cutterTable = _dataAccessServiceFactory.GetDbTable<CutterDataModel>();

            // Get user db table
            IDataAccessService<UserDataModel> userTable = _dataAccessServiceFactory.GetDbTable<UserDataModel>();

            // Listen for when data actually changed
            machineTable.DataChanged += (s, e) =>
            {
                data = e as MachineDataModel;

                // Send out message
                Messenger.MessageSender.SendMessage(data ?? throw new ArgumentNullException("SelectedMachine data cannot be null"));
            };

            // Get machine that will be receiving cutter
            MachineDataModel? receivingMachine = await machineTable.GetEntityByIdAsync(machineReceivingCutterId);
            MachineDataModel? sendingMachine = await machineTable.GetEntityByIdAsync(machineSendingCutterId);


            // Get user carrying out this operation
            UserDataModel? user = await userTable.GetEntityByIdAsync(userId);

            // Make sure both machines are not null
            if (sendingMachine is not null && receivingMachine is not null)
            {
                // Get cutter
                CutterDataModel? cutter = await cutterTable.GetEntityByIdAsync(sendingMachine.Cutter.Id);

                // Transfer data
                receivingMachine.Status = sendingMachine.Status;
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
                sendingMachine.Status = MachineStatus.Warning;
                sendingMachine.DateTimeLastModified = DateTime.Now;

                // Set the user performing this operation including the machine involved
                sendingMachine.MachineUserInteractions.Add(new MachineUserInteractions
                {
                    UserDataModel = user ?? throw new NullReferenceException($"User with the name {user?.FirstName.PadRight(6)} {user?.LastName} not found"),
                    MachineDataModel = sendingMachine
                });

                // Update db with the new data
                await userTable.UpdateEntityAsync(user);
                await cutterTable.UpdateEntityAsync(cutter);
                await machineTable.UpdateEntityAsync(receivingMachine);
                await machineTable.UpdateEntityAsync(sendingMachine);

                // Unhook event
                machineTable.DataChanged -= delegate { };
            }
        }

        /// <summary>
        /// Capture and records CMM data of a specific cutter
        /// </summary>
        public async Task CaptureAndRecordCMMData(int userId, int machineId, string comment, CMMDataModel incomingCMMData)
        {
            // The data that changed
            MachineDataModel? data = null;

            // Get machine table
            IDataAccessService<MachineDataModel> machineTable = _dataAccessServiceFactory.GetDbTable<MachineDataModel>();

            // Get user table
            IDataAccessService<UserDataModel> userTable = _dataAccessServiceFactory.GetDbTable<UserDataModel>();

            // Get cutter table
            IDataAccessService<CutterDataModel> cutterTable = _dataAccessServiceFactory.GetDbTable<CutterDataModel>();

            // Listen for when machine table data actually changed
            machineTable.DataChanged += (s, e) =>
            {
                data = e as MachineDataModel;
                // Send out message
                Messenger.MessageSender.SendMessage(data ?? throw new ArgumentNullException("SelectedMachine data cannot be null"));
            };

            // Attempt to get machine
            MachineDataModel? machine = await machineTable.GetEntityByIdAsync(machineId);

            // Attempt to get user
            UserDataModel? user = await userTable.GetEntityByIdAsync(userId);

            // Make sure we have machine
            if (machine is not null)
            {
                // Attempt to get current cutter
                CutterDataModel cutter = await cutterTable.GetEntityByIdAsync(machine.Cutter.Id) ?? throw new ArgumentNullException("Cutter not found");

                // Set cmm data
                machine.Cutter.CMMData.Add(new CMMDataModel
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
                });

                // Set other machine information
                machine.Cutter.Count = int.Parse(incomingCMMData.Count);
                machine.StatusMessage = comment ?? "Passed CMM check";
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
                await machineTable.UpdateEntityAsync(machine);
                await cutterTable.UpdateEntityAsync(cutter);

                // Unhook event
                machineTable.DataChanged -= delegate { };
            }
        }

        /// <summary>
        /// Remove cutter from a machine
        /// </summary>
        /// <param name="machineId">The machine to remove cutter from</param>
        /// <param name="userId">The user removing the cutter</param>
        /// <param name="keepCutter">True if cutter is to stay in department</param>
        /// <param name="newData">Data changing on the machine that is having it's cutter removed</param>
        public async Task RemoveCutter(int machineId, int userId, bool keepCutter, MachineDataModel newData)
        {
            // New data from database
            MachineDataModel? data = null;

            // Get machine table
            IDataAccessService<MachineDataModel> machineTable = _dataAccessServiceFactory.GetDbTable<MachineDataModel>();
            // Get user table
            IDataAccessService<UserDataModel> userTable = _dataAccessServiceFactory.GetDbTable<UserDataModel>();
            // Get cutter table
            IDataAccessService<CutterDataModel> cutterTable = _dataAccessServiceFactory.GetDbTable<CutterDataModel>();
            // Get cmm data table
            IDataAccessService<CMMDataModel> cmmTable = _dataAccessServiceFactory.GetDbTable<CMMDataModel>();

            // Listen for changes 
            machineTable.DataChanged += (s, e) =>
            {
                // Set new data
                data = e as MachineDataModel;
                // Send out message
                Messenger.MessageSender.SendMessage(data ?? throw new ArgumentNullException("SelectedMachine data cannot be null"));
            };

            // Get machine
            MachineDataModel? machine = await machineTable.GetEntityByIdAsync(machineId);

            // Get user
            UserDataModel? user = await userTable.GetEntityByIdAsync(userId);

            // If machine is not null...
            if (machine is not null)
            {
                // Current cutter
                CutterDataModel? cutter = await cutterTable.GetEntityByIdIncludingRelatedPropertiesAsync(machine.Cutter.Id, c => c.CMMData);

                // CMM data associated with cutter
                CMMDataModel? cmmData = await cmmTable.GetEntityByIdAsync(cutter.Id);

                // Set new information
                machine.FrequencyCheckResult = FrequencyCheckResult.Setup;
                machine.Status = MachineStatus.Warning;
                machine.StatusMessage = newData.StatusMessage ?? $"Cutter was removed. {DateTime.Now.ToString("g")}";
                machine.DateTimeLastModified = DateTime.Now;
                machine.Cutter.CutterChangeInfo = newData.Cutter.CutterChangeInfo;
                machine.Cutter.LastUsedDate = DateTime.Now;
                machine.Cutter.Condition = CutterCondition.Used;
                machine.Cutter.Count = newData.Cutter.Count;
                machine.Cutter.MachineDataModelId = null;
                machine.CutterDataModelId = null;
                machine.PartNumber = null!;
                machine.PartToothSize = "0";

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

                    // Remove every associated cmm data from table
                    //foreach(CMMDataModel cmm in cutter.CMMData.ToList()) { await cmmTable.DeleteEntityAsync(cmm); } // Uncomment once history table is implemented and record of cutter being sent back to be rebuilt is taken

                    // Remove cutter from table
                    //await cutterTable.DeleteEntityAsync(cutter); // Uncomment once history table is implemented and record of cutter being sent back to be rebuilt is taken

                    // ToDo: Remove this code and uncomment the above code
                    await cutterTable.UpdateEntityAsync(cutter);

                }
                else
                {
                    // Update cutter information 
                    await cutterTable.UpdateEntityAsync(cutter);
                }

                // Update database
                await machineTable.UpdateEntityAsync(machine);

                // Stop listening for changes
                machineTable.DataChanged -= delegate { };
            }
        }
    }
}
