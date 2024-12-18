namespace CutterManagement.Core
{
    public class MachineValidation : DataValidationBase<MachineDataModel> 
    {
        public override ValidationResult Validate(MachineDataModel machine)
        {
            ValidationResult result = CreateValidationInstance();

            if (machine is null)
                return ErrorReport(result, "A valid machine is required");

            else if (machine.MachineNumber is 0)
                return ErrorReport(result, "A valid machine ID is required");

            else if (machine.MachineNumber.ToString().Count() > 3 || machine.MachineNumber.ToString().Count() < 3)
                return ErrorReport(result, "MachineDataModel ID must be 3 digits");

            else if (machine.MachineSetId is 0)
                return ErrorReport(result, "A valid machine set ID is required");

            //else if (int.TryParse(machine.MachineNumber, out var number) is false)
            //    return ErrorReport(result, "MachineDataModel ID must be a number");

            else if (machine.Owner is Department.None)
                return ErrorReport(result, "MachineDataModel owner is required");

            return result;
        }
    }
}
