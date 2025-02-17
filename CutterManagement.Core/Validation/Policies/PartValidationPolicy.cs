namespace CutterManagement.Core
{
    /// <summary>
    /// Defines rules for validating a <see cref="PartDataModel"/>
    /// </summary>
    public class PartValidationPolicy : DataValidationBase<PartDataModel>
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

            if (part is null)
                return ErrorReport(result, "A valid part is required");

            else if (string.IsNullOrEmpty(part.PartNumber))
                return ErrorReport(result, "A valid part number is required");

            else if (string.IsNullOrEmpty(part.Model))
                return ErrorReport(result, "Model number is required");

            else if (string.IsNullOrEmpty(part.SummaryNumber))
                return ErrorReport(result, "Summary number is required");

            else if (string.IsNullOrEmpty(part.PartToothCount))
                return ErrorReport(result, "Tooth count is required");

            else if (int.TryParse(part.PartToothCount, out var number) is false)
                return ErrorReport(result, "Tooth count must be a number");

            else if (part.Kind == PartKind.None)
                return ErrorReport(result, "Please select type of part");

            // Return validation result to the caller
            return result;
        }
    }
}
