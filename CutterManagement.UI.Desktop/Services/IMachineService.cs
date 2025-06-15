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
        //Task RelocateCutter(MachineDataModel machineSendingCutter, int machineReceivingCutterId, int userId);
        Task RelocateCutter(int machineSendingCutterId, int machineReceivingCutterId, int userId, string comment);

        /// <summary>
        /// Capture and records CMM data of a specific cutter
        /// </summary>
        Task CaptureAndRecordCMMData(int userId, int machineId, string comment, CMMDataModel incomingCMMData);

        /// <summary>
        /// Remove cutter from a machine
        /// </summary>
        /// <param name="machineId">The machine to remove cutter from</param>
        /// <param name="userId">The user removing the cutter</param>
        /// <param name="keepCutter">True if cutter is to stay in department</param>
        /// <param name="newData">Data changing on the machine that is having it's cutter removed</param>
        Task RemoveCutter(int machineId, int userId, bool keepCutter, MachineDataModel newData);

    }
}
