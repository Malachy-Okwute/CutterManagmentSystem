using Serilog.Filters;
using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;

namespace CMS
{
    /// <summary>
    /// Manages data associated with machines
    /// </summary>
    public class MachineDataManager : IMachineDataManager
    {
        /// <summary>
        /// Collection of machines
        /// </summary>
        public readonly Dictionary<string, Machine> Machines;

        /// <summary>
        /// Collection of parts 
        /// </summary>
        public readonly Dictionary<string, Part> Parts;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MachineDataManager()
        {
            // Initialize resource objects
            Machines = new Dictionary<string, Machine>();
            Parts = new Dictionary<string, Part>();
        }

        /// <summary>
        /// Adds <see cref="Machine"/> to the collection of machines
        /// </summary>
        /// <param name="machine">The machine to add</param>
        /// <returns>An error message due to validation or success message if successful</returns>
        public string AddMachine(Machine machine)
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
            if (!Machines.ContainsKey(machine.UniqueID))
                // Add machine to collection
                Machines.Add(machine.UniqueID, machine);

            // Return success message
            return $"Successfully added machine #{machine.UniqueID}";
        }

        /// <summary>
        /// Adds <see cref="Part"/> to the collection of parts
        /// </summary>
        /// <param name="part">The part to add</param>
        /// <returns>An error message due to validation or success message if successful</returns>
        public string AddPart(Part part)
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
            if (!Parts.ContainsKey(part.UniqueID))
                // Add item to collection
                Parts.Add(part.UniqueID, part);

            // Return success
            return $"Successfully added part #{part.UniqueID}";
        }

        /// <summary>
        /// Removes <see cref="Machine"/> from collection of machines
        /// </summary>
        /// <param name="machine">The machine to remove</param>
        /// <returns>An error message due to validation or success message if successful</returns>
        public string RemoveMachine(Machine machine)
        {
            // If we can't find the item in the collection...
            if(!Machines.ContainsKey(machine.UniqueID))
                // Exit it this function with error message
                return $"Cannot find machine #{machine.UniqueID}";
            // Otherwise...
            else
                // Remove item
                Machines.Remove(machine.UniqueID);

            // Return success
            return $"Successfully removed machine #{machine.UniqueID}";
        }

        /// <summary>
        /// Removes <see cref="Part"/> from collection of parts
        /// </summary>
        /// <param name="part">The part to remove</param>
        /// <returns>An error message due to validation or success message if successful</returns>
        public string RemovePart(Part part)
        {
            // If we can't find the item in the collection...
            if (!Parts.ContainsKey(part.UniqueID))
                // Exit it this function with error message
                return $"Cannot find part #{part.UniqueID}";
            // Otherwise...
            else
                // Remove item
                Machines.Remove(part.UniqueID);

            // Return success
            return $"Successfully removed part #{part.UniqueID}";
        }

        /// <summary>
        /// Modifies values of a <see cref="Machine"/> item
        /// </summary>
        /// <param name="machine">The machine to modify</param>
        /// <returns>An error message due to validation or success message if successful</returns
        public string UpdateMachineInfo(Machine machine)
        {
            
            if (!Machines.ContainsKey(machine.UniqueID))
                return $"Cannot find machine #{machine.UniqueID}";
            else
                Machines[machine.UniqueID] = machine;

            return $"Successfully modified machine #{machine.UniqueID}";
        }

        /// <summary>
        /// Modifies values of a <see cref="Part"/> item
        /// </summary>
        /// <param name="machine">The part to modify</param>
        /// <returns>An error message due to validation or success message if successful</returns>
        public string UpdatePartInfo(Part part)
        {
            if (!Parts.ContainsKey(part.UniqueID))
                return $"Cannot find part #{part.UniqueID}";
            else
                Parts[part.UniqueID] = part;

            return $"Successfully modified part #{part.UniqueID}";
        }
    }
}
