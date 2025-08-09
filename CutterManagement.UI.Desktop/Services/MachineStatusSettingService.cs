using CutterManagement.Core;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Facilitates setting status of a machine
    /// </summary>
    [Obsolete("This class is no longer in use")]
    public class MachineStatusSettingService //: IMachineService
    {
        /// <summary>
        /// Data access factory
        /// </summary>
        private IDataAccessServiceFactory _dataAccessService;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dataAccessService">Data access factory</param>
        public MachineStatusSettingService(IDataAccessServiceFactory dataAccessService)
        {
            _dataAccessService = dataAccessService;
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
            using var machineTable = _dataAccessService.GetDbTable<MachineDataModel>();

            /// Get users table
            using var userTable = _dataAccessService.GetDbTable<UserDataModel>();

            // Event handler to listen for when data changes
            EventHandler<object>? handler = null;

            // Listen for when data actually changed
            handler += (s, e) =>
            {
                // Unsubscribe from the event to avoid memory leaks
                machineTable.DataChanged -= handler;
                // Cast the event data to MachineDataModel
                data = e as MachineDataModel;
                // Invoke callback 
                callback?.Invoke(data ?? throw new ArgumentNullException("Cannot find machine item that changed"));
            };

            // Subscribe to data changed event
            machineTable.DataChanged += handler;

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
            }

            // Return result
            return result;
        }
    }
}
