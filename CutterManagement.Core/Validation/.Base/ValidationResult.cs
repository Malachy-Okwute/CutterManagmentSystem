namespace CutterManagement.Core
{
    /// <summary>
    /// Result of a data validation
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// True if validation is successful, otherwise false
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// The error message associated with validation result when validation failed
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;
    }
}