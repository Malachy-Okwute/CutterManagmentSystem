namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Defines rules for validating a <see cref="PartDataModel"/>
    /// </summary>
    public class PartValidation : DataValidationBase<PartDataModel>
    {
        /// <summary>
        /// Validates a part
        /// </summary>
        /// <param name="part">The part to validate</param>
        /// <returns><see cref="ValidationResult"/> reporting whether this validation failed or not</returns>
        public override ValidationResult Validate(PartDataModel part)
        {
            // Create an instance of Validation-Result to be reported
            ValidationResult result = CreateValidationInstance();

            // Rules for validating a part are defined here
            if (part is null)
                return ErrorReport(result, "A valid part is required");

            else if (string.IsNullOrEmpty(part.Id))
                return ErrorReport(result, "A valid part ID is required");

            else if (string.IsNullOrEmpty(part.PartToothCount))
                return ErrorReport(result, "PartDataModel tooth count is required");

            else if (int.TryParse(part.PartToothCount, out var number) is false)
                return ErrorReport(result, "PartDataModel tooth count must be a number");

            else if (part.Model is null)
                return ErrorReport(result, "PartDataModel model is required");

            // Return validation result to the caller
            return result;
        }
    }
}
