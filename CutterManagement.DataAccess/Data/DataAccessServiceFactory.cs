using CutterManagement.Core;

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
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Event that gets fired whenever data changed in the database
        /// </summary>
        public event EventHandler<object> DataChanged;

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
        /// <param name="dbContext">Database context</param>
        public DataAccessServiceFactory(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets the desired table <see cref="IDataAccessService{T}"/> passed in as T
        /// </summary>
        /// <typeparam name="T">The type of table to get</typeparam>
        /// <returns><see cref="IDataAccessService{T}"/></returns>
        public IDataAccessService<T> GetDbTable<T>() where T : class
        {
            IDataAccessService<T> dataService = new DataAccessService<T>(_dbContext);

            dataService.DataChanged += DataChanged;

            return dataService;
        }
    }
}
