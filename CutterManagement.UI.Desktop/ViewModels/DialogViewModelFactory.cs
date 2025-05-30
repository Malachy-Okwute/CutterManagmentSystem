using Microsoft.Extensions.DependencyInjection;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// Dialog view model factory
    /// </summary>
    public interface IDialogViewModelFactory
    {
        /// <summary>
        /// Gets an instance of a dialog view model
        /// </summary>
        /// <typeparam name="T">The type of dialog view model to get</typeparam>
        T GetService<T>() where T : DialogViewModelBase;
    }

    /// <summary>
    /// Dialog view model factory
    /// </summary>
    public class DialogViewModelFactory : IDialogViewModelFactory
    {
        /// <summary>
        /// Service provider
        /// </summary>
        private IServiceProvider _serviceProvider;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="serviceProvider">Service provider</param>
        public DialogViewModelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets an instance of a dialog view model
        /// </summary>
        /// <typeparam name="T">The type of dialog view model to get</typeparam>
        public T GetService<T>() where T : DialogViewModelBase
        {
            return _serviceProvider.GetRequiredService<T>();
        }
    }
}
