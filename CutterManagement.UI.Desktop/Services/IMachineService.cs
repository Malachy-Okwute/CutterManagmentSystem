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
    }
}
