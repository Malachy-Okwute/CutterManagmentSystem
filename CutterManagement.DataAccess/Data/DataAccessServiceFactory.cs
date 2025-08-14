using CutterManagement.Core;
using Microsoft.Extensions.DependencyInjection;

namespace CutterManagement.DataAccess
{
    /// <summary>
    /// Factory for <see cref="IDataAccessService{T}"/>
    /// </summary>
    public class DataAccessServiceFactory : IDataAccessServiceFactory
    {
        /// <summary>
        /// Service provider
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Default constructor
        /// <para>
        /// Design-time constructor
        /// </para>
        /// </summary>
        public DataAccessServiceFactory() { }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="serviceProvider">Service provider</param>
        public DataAccessServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets the desired table <see cref="IDataAccessService{T}"/> passed in as T
        /// </summary>
        /// <typeparam name="T">The type of table to get</typeparam>
        /// <returns><see cref="IDataAccessService{T}"/></returns>
        public IDataAccessService<T> GetDbTable<T>() where T : class
        {
            // Create a scope for the current request
            var scopedContext = _serviceProvider.CreateScope();

            // Get db context using the scope created
            var context = scopedContext.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Serve context
            return new DataAccessService<T>(context ?? throw new ArgumentNullException("Context is null"));
        }        
    }
}
