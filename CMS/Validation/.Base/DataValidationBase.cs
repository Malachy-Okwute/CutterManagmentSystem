namespace CMS
{
    /// <summary>
    /// Data validation base
    /// </summary>
    /// <typeparam name="T">The object to validate</typeparam>
    public abstract class DataValidationBase<T> : IValidator<T>
    {
        /// <summary>
        /// Validates  data associated with <see cref="T"/>
        /// </summary>
        /// <param name="data">The object that needs it's data validated</param>
        /// <returns></returns>
        public abstract ValidationResult Validate(T data);

        /// <summary>
        /// Create an instance of <see cref="ValidationResult"/>
        /// </summary>
        /// <returns><see cref="ValidationResult"/></returns>
        protected ValidationResult CreateValidationInstance()
        {
            return new ValidationResult { IsValid = true };
        }

        /// <summary>
        /// Reports an error message that occurred during validation process
        /// </summary>
        /// <param name="validationResult">The validation result</param>
        /// <param name="errorMessage">The error message associated with the validation result</param>
        /// <returns><see cref="ValidationResult"/></returns>
        protected ValidationResult ErrorReport(ValidationResult validationResult, string errorMessage)
        {
            validationResult.IsValid = false;
            validationResult.ErrorMessage = errorMessage;

            return validationResult;
        }
    }
}
