namespace CutterManagement.Core
{
    /// <summary>
    /// Configuration service for machine items
    /// </summary>
    public interface IMachineConfigurationService 
    {
        /// <summary>
        /// Configures a machine item
        /// </summary>
        /// <param name="item">The item to configure</param>
        /// <returns><see cref="Task{ValidationResult}"/></returns>
        Task<ValidationResult> Configure(MachineDataModel newData);
    }
}
