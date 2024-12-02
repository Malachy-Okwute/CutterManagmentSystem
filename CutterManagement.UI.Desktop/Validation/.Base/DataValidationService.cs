namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Data validation service
    /// </summary>
    public class DataValidationService
    {
        /// <summary>
        /// Contains the validation rules information used in validating data
        /// </summary>
        private static readonly IDictionary<Type, object> _dataValidationRegistry = new Dictionary<Type, object>();

        /// <summary>
        /// Registers a specific validation type containing validation rules
        /// </summary>
        /// <typeparam name="T">The validation type to register</typeparam>
        /// <param name="validator">The validation rules</param>
        public static void RegisterValidator<T>(IValidator<T> validator)
        {
            _dataValidationRegistry[typeof(T)] = validator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Throws exception if validation rules is not found in the registry</exception>
        public static ValidationResult Validate<T>(T data)
        {
            if (_dataValidationRegistry.TryGetValue(typeof(T), out var validator))
                return ((IValidator<T>)validator).Validate(data);

            throw new InvalidOperationException($"No validator requested for type {typeof(T).Name}");
        }
    }
}
