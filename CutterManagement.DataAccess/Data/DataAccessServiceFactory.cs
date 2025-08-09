using CutterManagement.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace CutterManagement.DataAccess
{
    /// <summary>
    /// Factory for <see cref="IDataAccessService{T}"/>
    /// </summary>
    public class DataAccessServiceFactory : IDataAccessServiceFactory
    {
        /// <summary>
        /// Database context
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
        /// <param name="serviceProvider">Database context</param>
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
            var scopedContext = _serviceProvider.CreateScope();
            var context = scopedContext.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return new DataAccessService<T>(context ?? throw new ArgumentNullException("Context is null"));
        }        
    }
}
