using CutterManagement.Core;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CutterManagement.DataAccess
{
    /// <summary>
    /// Provides access to the application database
    /// </summary>
    public class DataAccessService<T> : IDataAccessService<T> where T : class
    {
        /// <summary>
        /// The application database client
        /// </summary>
        protected ApplicationDbContext _applicationDbContext;

        /// <summary>
        /// A table of type <c>T</c> in the application database
        /// </summary>
        protected DbSet<T> _dbTable;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="applicationDbContext">The application database client</param>
        public DataAccessService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            _dbTable = _applicationDbContext.Set<T>();
        }

        /// <summary>
        /// Create a new entry record and save it in the database
        /// </summary>
        /// <param name="entity">The new to save to database</param>
        /// <returns><see cref="Task"/></returns>
        public async Task CreateNewEntityAsync(T entity)
        {
            if (entity is not null) 
            {
                try
                {
                    await _dbTable.AddAsync(entity);
                    await _applicationDbContext.SaveChangesAsync();
                }
                catch (Exception msg) // Use custom exception here
                {
                    Debugger.Break();
                    Console.WriteLine(msg);
                }
            }
        }

        /// <summary>
        /// Get all entities in the database table
        /// </summary>
        /// <returns><see cref="Task"/> of <see cref="List{T}"/></returns>
        public async Task<IReadOnlyList<T>> GetAllEntitiesAsync()
        {
            return await _dbTable.ToListAsync();
        }

        /// <summary>
        /// Get an entity by id
        /// </summary>
        /// <param name="entityId">The id of the entity to get</param>
        /// <returns><see cref="Task"/> of <see cref="T"/></returns>
        public async Task<T?> GetEntityByIdAsync(int? entityId)
        {
           return await _dbTable.FindAsync(entityId);
        }

        /// <summary>
        /// Update an entity
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <returns><see cref="Task"/></returns>
        public async Task UpdateEntityAsync(T entity)
        {
            try
            {
                _dbTable.Update(entity);
                await _applicationDbContext.SaveChangesAsync();
            }
            catch (Exception msg) // Use custom exception here
            {
                Console.WriteLine(msg);
                Debugger.Break();
            }
        }

        /// <summary>
        /// Remove entity from the database
        /// </summary>
        /// <param name="entity">The entity to remove</param>
        /// <returns><see cref="Task"/></returns>
        public async Task DeleteEntityAsync(T entity)
        {
            try
            {
                _dbTable.Remove(entity);
                await _applicationDbContext.SaveChangesAsync();
            }
            catch (Exception msg) // Use custom exception here
            {
                Console.WriteLine(msg);
                Debugger.Break();
            }
        }
    }
}
