using CutterManagement.Core;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Provides services for machine items
    /// </summary>
    public class MachineService : IMachineService
    {
        /// <summary>
        /// Data access factory
        /// </summary>
        private IDataAccessServiceFactory _dataAccessService;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dataAccessService">Data access factory</param>
        public MachineService(IDataAccessServiceFactory dataAccessService)
        {
            _dataAccessService = dataAccessService;
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
            IDataAccessService<MachineDataModel> machineTable = _dataAccessService.GetDbTable<MachineDataModel>();

            // Listen for when data actually changed
            machineTable.DataChanged += (s, e) => { data = e as MachineDataModel; };

            // Get the specific item from db
            MachineDataModel? machineData = await machineTable.GetEntityByIdAsync(newData.Id);

            // Register machine validation
            DataValidationService.RegisterValidator(new MachineValidation());

            // Validate incoming data
            ValidationResult result = DataValidationService.Validate(newData);

            // Make sure we have the item and incoming data is valid
            if (machineData is not null && result.IsValid)
            {
                // Wire new data 
                machineData.MachineNumber = newData.MachineNumber;
                machineData.MachineSetId = newData.MachineSetId;
                machineData.Status = newData.Status;
                machineData.StatusMessage = newData.StatusMessage ?? string.Empty;
                machineData.DateTimeLastModified = DateTime.Now;

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


        public async Task<ValidationResult> SetStatus(MachineDataModel newData, int userId, Action<MachineDataModel> callback)
        {
            // Data that will be changing
            MachineDataModel? data = null;

            // Get tables
            IDataAccessService<MachineDataModel> machineTable = _dataAccessService.GetDbTable<MachineDataModel>();
            IDataAccessService<UserDataModel> userTable = _dataAccessService.GetDbTable<UserDataModel>();
            IDataAccessService<MachineDataModelUserDataModel> machineUserJoinTable = _dataAccessService.GetDbTable<MachineDataModelUserDataModel>();

            // Listen for when data actually changed
            machineTable.DataChanged += (s, e) => 
            {
                data = e as MachineDataModel;
                callback?.Invoke(data ?? throw new ArgumentNullException("Cannot find machine"));
            };

            // Get items from db
            MachineDataModel? machineData = await machineTable.GetEntityByIdAsync(newData.Id);
            UserDataModel? user = await userTable.GetEntityByIdAsync(userId);

            // Register machine validation
            DataValidationService.RegisterValidator(new MachineValidation());

            // Validate incoming data
            ValidationResult result = DataValidationService.Validate(newData);

            // Make sure we have the item and incoming data is valid
            if (machineData is not null && result.IsValid)
            {
                // Wire new data 
                machineData.Status = newData.Status;
                machineData.StatusMessage = newData.StatusMessage;
                machineData.DateTimeLastModified = DateTime.Now;

                MachineDataModelUserDataModel machineUserJoinData = new MachineDataModelUserDataModel
                {
                    MachineDataModel = machineData,
                    UserDataModel = user ?? throw new ArgumentNullException("User not found")
                };

                // Save new data
                await machineTable.UpdateEntityAsync(machineData ?? throw new ArgumentException($"Could not configure entity: {machineData}"));
                await machineUserJoinTable.CreateNewEntityAsync(machineUserJoinData);
            }

            return result;
        }
    }
}
