namespace CutterManagement.Core
{
    /// <summary>
    /// Provides services for machine items
    /// </summary>
    public interface IMachineService 
    {
        /// <summary>
        /// Configures a machine item
        /// </summary>
        /// <param name="item">The item to configure</param>
        /// <returns><see cref="Task{ValidationResult}"/></returns>
        Task<(ValidationResult, MachineDataModel?)> Configure(MachineDataModel newData);
    }
}
