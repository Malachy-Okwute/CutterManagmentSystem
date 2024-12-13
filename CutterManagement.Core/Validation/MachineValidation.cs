namespace CutterManagement.Core
{
    public class MachineValidation : DataValidationBase<MachineDataModel> 
    {
        public override ValidationResult Validate(MachineDataModel machine)
        {
            ValidationResult result = CreateValidationInstance();

            if (machine is null)
                return ErrorReport(result, "A valid machine is required");

            else if (string.IsNullOrEmpty(machine.MachineId))
                return ErrorReport(result, "A valid machine ID is required");

            else if (machine.MachineId.Count() > 3 || machine.MachineId.Count() < 3)
                return ErrorReport(result, "MachineDataModel ID must be 3 digits");

            else if (string.IsNullOrEmpty(machine.SetId))
                return ErrorReport(result, "A valid machine set ID is required");

            else if (int.TryParse(machine.MachineId, out var number) is false)
                return ErrorReport(result, "MachineDataModel ID must be a number");

            else if (machine.Owner is Department.None)
                return ErrorReport(result, "MachineDataModel owner is required");

            return result;
        }
    }
}
