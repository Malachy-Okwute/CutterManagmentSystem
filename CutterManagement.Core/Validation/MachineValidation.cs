namespace CutterManagement.Core
{
    public class MachineValidation : DataValidationBase<MachineDataModel> 
    {
        public override ValidationResult Validate(MachineDataModel machine)
        {
            ValidationResult result = CreateValidationInstance();

            if (machine is null)
                return ErrorReport(result, "A valid machine item is required");

            else if (string.IsNullOrEmpty(machine.MachineNumber))
                return ErrorReport(result, "Machine ID is required");

            else if (int.TryParse(machine.MachineNumber, out var number) is false)
                return ErrorReport(result, "Machine ID must be a number");

            else if (machine.MachineNumber.Count() > 3 || machine.MachineNumber.Count() < 3)
                return ErrorReport(result, "Machine ID must be a 3 digits number");

            else if (string.IsNullOrEmpty(machine.MachineSetId))
                return ErrorReport(result, "Machine set ID is required");

            else if (int.TryParse(machine.MachineSetId, out var setNumber) is false)
                return ErrorReport(result, "Machine set ID must be a number");

            else if (machine.MachineSetId.Count() > 3 || machine.MachineSetId.Count() < 3)
                return ErrorReport(result, "Machine set ID must be a 3 digits number");
            
            else if (string.IsNullOrEmpty(machine.StatusMessage))
                return ErrorReport(result, "Comment is required");

            else if (machine.Owner is Department.None)
                return ErrorReport(result, "Machine owner is required");

            return result;
        }
    }
}
