namespace CMS
{
    /// <summary>
    /// Manages data associated with machines
    /// </summary>
    public interface IMachineDataService
    {
        /// <summary>
        /// Retrieves a collection of machines
        /// </summary>
        Dictionary<string, MachineDataModel> GetMachines();

        /// <summary>
        /// Retrieves a collection of parts 
        /// </summary>
        Dictionary<string, PartDataModel> GetParts();

        void LoadMachines();
        void LoadParts();

        /// <summary>
        /// Adds <see cref="MachineDataModel"/> to the collection of machines
        /// </summary>
        /// <param name="machine">The machine to add</param>
        /// <returns>An error message due to validation or success message if successful</returns>
        string AddMachine(MachineDataModel machine);

        /// <summary>
        /// Adds <see cref="PartDataModel"/> to the collection of parts
        /// </summary>
        /// <param name="part">The part to add</param>
        /// <returns>An error message due to validation or success message if successful</returns>
        string AddPart(PartDataModel part);

        /// <summary>
        /// Removes <see cref="MachineDataModel"/> from collection of machines
        /// </summary>
        /// <param name="machine">The machine to remove</param>
        /// <returns>An error message due to validation or success message if successful</returns>
        string RemoveMachine(MachineDataModel machine);

        /// <summary>
        /// Removes <see cref="PartDataModel"/> from collection of parts
        /// </summary>
        /// <param name="part">The part to remove</param>
        /// <returns>An error message due to validation or success message if successful</returns>
        string RemovePart(PartDataModel part);

        /// <summary>
        /// Modifies values of a <see cref="MachineDataModel"/> item
        /// </summary>
        /// <param name="machine">The machine to modify</param>
        /// <returns>An error message due to validation or success message if successful</returns
        string UpdateMachineInfo(MachineDataModel machine);

        /// <summary>
        /// Modifies values of a <see cref="PartDataModel"/> item
        /// </summary>
        /// <param name="machine">The part to modify</param>
        /// <returns>An error message due to validation or success message if successful</returns>
        string UpdatePartInfo(PartDataModel part);

        void RemoveCutter(MachineDataModel machine);
        void AssignCutter(CutterDataModel cutter, MachineDataModel machine);
        void UpdateMachineInformation(MachineDataModel machine);
        void SwapMachineCutter(MachineDataModel firstMachine, MachineDataModel secondMachine);

    }
}