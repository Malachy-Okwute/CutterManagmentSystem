using CutterManagement.Core;
using System.Net.Http;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Provides services for machine items
    /// </summary>
    public interface IMachineService 
    {
        /// <summary>
        /// Http client
        /// </summary>
        IHttpClientFactory HttpClientFactory { get; }

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
        Task<(ValidationResult, MachineDataModel?)> ConfigureAsync(MachineDataModel newData);

        /// <summary>
        /// Sets machine status
        /// </summary>
        Task<ValidationResult> SetStatusAsync(MachineDataModel newData, int userId, Action<MachineDataModel> callback);

        /// <summary>
        /// Adjusts piece count on a machine
        /// </summary>
        /// <param name="Id">Id of the machine whose piece count is being adjusted</param>
        /// <param name="count">The current piece count on the machine</param>
        /// <param name="verifyUserIntention">Confirms user intention when new piece count is over a certain limit</param>
        Task AdjustPieceCountAsync(int Id, int count, Func<Task<bool?>> verifyUserIntention);

        /// <summary>
        /// Relocates cutter from one machine to another machine
        /// </summary>
        /// <param name="machineSendingCutter">The machine currently with cutter</param>
        /// <param name="machineReceivingCutterId">The id of machine receiving cutter</param>
        /// <param name="userId">Id to user carrying out this process</param>
        //Task RelocateCutter(MachineDataModel machineSendingCutter, int machineReceivingCutterId, int userId);
        Task RelocateCutterAsync(int machineSendingCutterId, int machineReceivingCutterId, int userId, string comment);

        /// <summary>
        /// Capture and records CMM data of a specific cutter
        /// </summary>
        Task CaptureAndRecordCMMDataAsync(int userId, int machineId, string comment, CMMDataModel incomingCMMData);

        /// <summary>
        /// Remove cutter from a machine
        /// </summary>
        /// <param name="machineId">The machine to remove cutter from</param>
        /// <param name="userId">The user removing the cutter</param>
        /// <param name="keepCutter">True if cutter is to stay in department</param>
        /// <param name="newData">Data changing on the machine that is having it's cutter removed</param>
        Task RemoveCutterAsync(int machineId, int userId, bool keepCutter, MachineDataModel newData);

        /// <summary>
        /// Logs the production progress of a machine, including cutter and part details, to the production log table.
        /// </summary>
        /// <remarks>
        /// This method records key production details, such as machine number, cutter
        /// information, part number, and user details,  into the production log table. If the cutter information is
        /// missing (<see cref="MachineDataModel.Cutter"/> is <see langword="null"/>),  the method does not perform any
        /// logging.
        /// </remarks>
        /// <param name="data">The machine id containing details about the machine, cutter, and production status.  The <see
        /// <param name="user">The user associated with the operation. Can be <see langword="null"/> if no user is specified.</param>
        /// cref="MachineDataModel.Cutter"/> property must not be <see langword="null"/>; otherwise, the method will
        /// return without logging.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task LogProductionProgressAsync(int machineId, UserDataModel? user);
    }
}
