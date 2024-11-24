using System.Diagnostics.Contracts;
using System.Reflection.PortableExecutable;

namespace CMS
{
    public class MachineValidation : DataValidationBase<Machine> 
    {
        public override ValidationResult Validate(Machine machine)
        {
            ValidationResult result = CreateValidationInstance();

            if (machine is null)
                return ErrorReport(result, "A valid machine is required");

            else if (string.IsNullOrEmpty(machine.UniqueID))
                return ErrorReport(result, "A valid machine ID is required");

            else if (machine.UniqueID.Count() > 3 || machine.UniqueID.Count() < 3)
                return ErrorReport(result, "Machine ID must be 3 digits");

            else if (string.IsNullOrEmpty(machine.UniqueSetID))
                return ErrorReport(result, "A valid machine set ID is required");

            else if (int.TryParse(machine.UniqueID, out var number) is false)
                return ErrorReport(result, "Machine ID must be a number");

            else if (machine.MachineOwner is Department.None)
                return ErrorReport(result, "Machine owner is required");

            return result;
        }
    }
}
