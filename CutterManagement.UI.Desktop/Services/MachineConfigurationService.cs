using CutterManagement.Core;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Facilitates configuring of a machine
    /// </summary>
    [Obsolete("This class is no longer in use")]
    public class MachineConfigurationService //: IMachineService
    {
        /// <summary>
        /// Data access factory
        /// </summary>
        private IDataAccessServiceFactory _dataAccessService;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dataAccessService">Data access factory</param>
        public MachineConfigurationService(IDataAccessServiceFactory dataAccessService)
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
            using var machineTable = _dataAccessService.GetDbTable<MachineDataModel>();

            EventHandler<object>? handler = null;

            // Listen for when data actually changed
            handler += (s, e) =>
            {
                // Unsubscribe from the event to avoid memory leaks
                machineTable.DataChanged -= handler;
                // Cast the event data to MachineDataModel
                data = e as MachineDataModel; 
            };

            // Subscribe to the data changed event
            machineTable.DataChanged += handler;

            // Get the specific item from db
            MachineDataModel? machineData = await machineTable.GetEntityByIdAsync(newData.Id);

            // Validate incoming data
            ValidationResult result = DataValidationService.Validate(newData);

            // Make sure new machine number is not already being used
            foreach (var item in await machineTable.GetAllEntitiesAsync())
            {
                if(item.MachineNumber.Equals(newData.MachineNumber))
                {
                    result.ErrorMessage = $"{newData.MachineNumber} number already exist";
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
                await machineTable.SaveEntityAsync(machineData ?? throw new ArgumentException($"Could not configure entity: {machineData}"));

                // Return result
                return (result, data);
            }

            // Return result
            return (result, data);
        }

    }
}
