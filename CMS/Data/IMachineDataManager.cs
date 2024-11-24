
namespace CMS
{
    /// <summary>
    /// Manages data associated with machines
    /// </summary>
    public interface IMachineDataManager
    {
        /// <summary>
        /// Adds <see cref="Machine"/> to the collection of machines
        /// </summary>
        /// <param name="machine">The machine to add</param>
        /// <returns>An error message due to validation or success message if successful</returns>
        string AddMachine(Machine machine);

        /// <summary>
        /// Adds <see cref="Part"/> to the collection of parts
        /// </summary>
        /// <param name="part">The part to add</param>
        /// <returns>An error message due to validation or success message if successful</returns>
        string AddPart(Part part);

        /// <summary>
        /// Removes <see cref="Machine"/> from collection of machines
        /// </summary>
        /// <param name="machine">The machine to remove</param>
        /// <returns>An error message due to validation or success message if successful</returns>
        string RemoveMachine(Machine machine);

        /// <summary>
        /// Removes <see cref="Part"/> from collection of parts
        /// </summary>
        /// <param name="part">The part to remove</param>
        /// <returns>An error message due to validation or success message if successful</returns>
        string RemovePart(Part part);

        /// <summary>
        /// Modifies values of a <see cref="Machine"/> item
        /// </summary>
        /// <param name="machine">The machine to modify</param>
        /// <returns>An error message due to validation or success message if successful</returns
        string UpdateMachineInfo(Machine machine);

        /// <summary>
        /// Modifies values of a <see cref="Part"/> item
        /// </summary>
        /// <param name="machine">The part to modify</param>
        /// <returns>An error message due to validation or success message if successful</returns>
        string UpdatePartInfo(Part part);
    }
}