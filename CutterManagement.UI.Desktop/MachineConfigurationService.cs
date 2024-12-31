using CutterManagement.Core;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Configuration service for machine items
    /// </summary>
    public class MachineConfigurationService : IMachineConfigurationService
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
        /// <returns><see cref="Task"/></returns>
        /// <exception cref="ArgumentException">
        /// Throws an exception if item could not be configured
        /// </exception>
        public async Task Configure(object item)
        {
            // Get the item to configure
            if(item is not MachineItemViewModel viewModel) return;

            // Get machine table
            IDataAccessService<MachineDataModel> machineTable = _dataAccessService.GetDbTable<MachineDataModel>();
           
            // Get the specific item from db
            MachineDataModel? machineData = await machineTable.GetEntityByIdAsync(viewModel.Id);

            // Make sure we have the item 
            if (machineData is not null)
            {
                // Wire new data
                machineData.MachineNumber = viewModel.MachineNumber;
                machineData.MachineSetId = viewModel.MachineSetNumber;
                machineData.Status = viewModel.Status;
                machineData.StatusMessage = viewModel.StatusMessage ?? string.Empty;
            }

            // Save new data
            await machineTable.UpdateEntityAsync(machineData ?? throw new ArgumentException($"Could not configure entity: {machineData}"));
        }
    }
}
