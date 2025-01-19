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
        Task<ValidationResult> SetStatus(MachineDataModel newData, int userId, Action<MachineDataModel> callback);
    }
}
