using CutterManagement.Core;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Provides services for machine items
    /// </summary>
    public interface IMachineService 
    {
        /// <summary>
        /// Access to database
        /// </summary>
        IDataAccessServiceFactory DataBaseAccess { get; }

        /// <summary>
        /// Gets a view model for a dialog window
        /// </summary>
        /// <typeparam name="T">The type of dialog window view model to get</typeparam>
        /// <returns>Dialog window view model</returns>
        T GetDialogViewModel<T>() where T : DialogViewModelBase;

        /// <summary>
        /// Configures a machine to be used
        /// </summary>
        /// <param name="newData">New information about the machine configured</param>
        Task<(ValidationResult, MachineDataModel?)> Configure(MachineDataModel newData);

        /// <summary>
        /// Sets machine status
        /// </summary>
        Task<ValidationResult> SetStatus(MachineDataModel newData, int userId, Action<MachineDataModel> callback);

        /// <summary>
        /// Adjusts piece count on a machine
        /// </summary>
        /// <param name="Id">Id of the machine whose piece count is being adjusted</param>
        /// <param name="count">The current piece count on the machine</param>
        /// <param name="verifyUserIntention">Confirms user intention when new piece count is over a certain limit</param>
        Task AdjustPieceCount(int Id, int count, Func<Task<bool?>> verifyUserIntention);

        /// <summary>
        /// Relocates cutter from one machine to another machine
        /// </summary>
        /// <param name="machineSendingCutter">The machine currently with cutter</param>
        /// <param name="machineReceivingCutterId">The id of machine receiving cutter</param>
        /// <param name="userId">Id to user carrying out this process</param>
        Task RelocateCutter(MachineDataModel machineSendingCutter, int machineReceivingCutterId, int userId);

    }
}
