using Serilog.Filters;
using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;

namespace CMS
{
    /// <summary>
    /// Manages data associated with machines
    /// </summary>
    public class MachineDataService : IMachineDataService
    {
        /// <summary>
        /// Collection of machines
        /// </summary>
        private readonly Dictionary<string, MachineDataModel> _machinesRecord;

        /// <summary>
        /// Collection of parts 
        /// </summary>
        private readonly Dictionary<string, PartDataModel> _partsRecord;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MachineDataService()
        {
            // Initialize resource objects
            _machinesRecord = new Dictionary<string, MachineDataModel>();
            _partsRecord = new Dictionary<string, PartDataModel>();
        }

        /// <summary>
        /// Retrieves a collection of machines
        /// </summary>
        public Dictionary<string, MachineDataModel> GetMachines() => _machinesRecord; 

        /// <summary>
        /// Retrieves a collection of parts 
        /// </summary>
        public Dictionary<string, PartDataModel> GetParts() => _partsRecord; 

        /// <summary>
        /// Adds <see cref="MachineDataModel"/> to the collection of machines
        /// </summary>
        /// <param name="machine">The machine to add</param>
        /// <returns>An error message due to validation or success message if successful</returns>
        public string AddMachine(MachineDataModel machine)
        {
            // Register machine validation
            DataValidationService.RegisterValidator(new MachineValidation());

            // Validate machine
            ValidationResult result = DataValidationService.Validate(machine);

            // If there is any error...
            if (result.IsValid is false)
                // Exit and return error the error message to the caller
                return result.ErrorMessage!;

            // If machine isn't already on the collection list...
            if (!_machinesRecord.ContainsKey(machine.Id))
                // Add machine to collection
                _machinesRecord.Add(machine.Id, machine);

            // Return success message
            return $"Successfully added machine #{machine.Id}";
        }

        /// <summary>
        /// Adds <see cref="PartDataModel"/> to the collection of parts
        /// </summary>
        /// <param name="part">The part to add</param>
        /// <returns>An error message due to validation or success message if successful</returns>
        public string AddPart(PartDataModel part)
        {
            // Register machine validation
            DataValidationService.RegisterValidator(new PartValidation());

            // Validate machine
            ValidationResult result = DataValidationService.Validate(part);

            // If there is any error...
            if (result.IsValid is false)
                // Exit and return error the error message to the caller
                return result.ErrorMessage!;

            // If item isn't already on the collection list...
            if (!_partsRecord.ContainsKey(part.Id))
                // Add item to collection
                _partsRecord.Add(part.Id, part);

            // Return success
            return $"Successfully added part #{part.Id}";
        }

        /// <summary>
        /// Removes <see cref="MachineDataModel"/> from collection of machines
        /// </summary>
        /// <param name="machine">The machine to remove</param>
        /// <returns>An error message due to validation or success message if successful</returns>
        public string RemoveMachine(MachineDataModel machine)
        {
            // If we can't find the item in the collection...
            if(!_machinesRecord.ContainsKey(machine.Id))
                // Exit it this function with error message
                return $"Cannot find machine #{machine.Id}";
            // Otherwise...
            else
                // Remove item
                _machinesRecord.Remove(machine.Id);

            // Return success
            return $"Successfully removed machine #{machine.Id}";
        }

        /// <summary>
        /// Removes <see cref="PartDataModel"/> from collection of parts
        /// </summary>
        /// <param name="part">The part to remove</param>
        /// <returns>An error message due to validation or success message if successful</returns>
        public string RemovePart(PartDataModel part)
        {
            // If we can't find the item in the collection...
            if (!_partsRecord.ContainsKey(part.Id))
                // Exit it this function with error message
                return $"Cannot find part #{part.Id}";
            // Otherwise...
            else
                // Remove item
                _machinesRecord.Remove(part.Id);

            // Return success
            return $"Successfully removed part #{part.Id}";
        }

        /// <summary>
        /// Modifies values of a <see cref="MachineDataModel"/> item
        /// </summary>
        /// <param name="machine">The machine to modify</param>
        /// <returns>An error message due to validation or success message if successful</returns
        public string UpdateMachineInfo(MachineDataModel machine)
        {
            
            if (!_machinesRecord.ContainsKey(machine.Id))
                return $"Cannot find machine #{machine.Id}";
            else
                _machinesRecord[machine.Id] = machine;

            return $"Successfully modified machine #{machine.Id}";
        }

        /// <summary>
        /// Modifies values of a <see cref="PartDataModel"/> item
        /// </summary>
        /// <param name="machine">The part to modify</param>
        /// <returns>An error message due to validation or success message if successful</returns>
        public string UpdatePartInfo(PartDataModel part)
        {
            if (!_partsRecord.ContainsKey(part.Id))
                return $"Cannot find part #{part.Id}";
            else
                _partsRecord[part.Id] = part;

            return $"Successfully modified part #{part.Id}";
        }

        public void AssignCutter(CutterDataModel cutter, MachineDataModel machine)
        {
        }

        public void RemoveCutter(MachineDataModel machine)
        {
        }

        public void SwapMachineCutter(MachineDataModel firstMachine, MachineDataModel secondMachine)
        {
        }

        public void UpdateMachineInformation(MachineDataModel machine)
        {
        }

        public void LoadMachines()
        {
            
        }

        public void LoadParts()
        {
            throw new NotImplementedException();
        }
    }
}
