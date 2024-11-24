namespace CMS
{
    /// <summary>
    /// Validation service interface
    /// </summary>
    /// <typeparam name="T">The data to validate</typeparam>
    public interface IValidator<T>
    {
        /// <summary>
        /// Validates data
        /// </summary>
        /// <param name="data">The information to validate</param>
        /// <returns>Result of the validation whether successful or failed</returns>
        ValidationResult Validate(T data);
    }
}
