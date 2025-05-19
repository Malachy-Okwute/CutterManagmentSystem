using CutterManagement.Core;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

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
        /// Event to run whenever data changes in the database
        /// </summary>
        public event EventHandler<object>? DataChanged;

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

                    int result = await _applicationDbContext.SaveChangesAsync();

                    if (result > 0)
                    {
                        DataChanged?.Invoke(this, entity);
                    }
                }
                catch (Exception msg) // Use custom exception here
                {
                    Debug.WriteLine(msg.Message);
                    Debugger.Break();
                }
                finally
                {
                    
                }
            }
        }

        /// <summary>
        /// Get all entities in the database table
        /// </summary>
        /// <returns><see cref="Task"/> of <see cref="IReadOnlyList{T}"/></returns>
        public async Task<IReadOnlyList<T>> GetAllEntitiesAsync()
        {
            return await _dbTable.ToListAsync();
        }

        /// <summary>
        /// Get an entity by id
        /// </summary>
        /// <param name="entityId">The id of the entity to get</param>
        /// <returns><see cref="Task{T}"/> of <see cref="T"/></returns>
        public async Task<T?> GetEntityByIdAsync(int? entityId)
        {
           return await _dbTable.FindAsync(entityId);
        }

        /// <summary>
        /// Gets an entity including it's list many navigation properties
        /// </summary>
        /// <typeparam name="TProperty">The navigation property</typeparam>
        /// <param name="entityId">The main entity id to get</param>
        /// <param name="includeExpression">Expression used to get the navigation properties</param>
        /// <returns><see cref="Task{T}"/> of <see cref="T"/></returns>
        public async Task<T> GetEntityByIdIncludingRelatedPropertiesAsync<TProperty>(int entityId, Expression<Func<T, ICollection<TProperty>>> includeExpression) where TProperty : class
        {
            return await _dbTable.Include(includeExpression)
                           .FirstAsync(entity => EF.Property<int>(entity, "Id") == entityId);

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
                int result = await _applicationDbContext.SaveChangesAsync();

                if (result > 0)
                {
                    DataChanged?.Invoke(this, entity);
                }
            }
            catch (Exception msg) // TODO: Use custom exception here
            {
                Debug.WriteLine(msg.Message);
                Debugger.Break();
            }
            finally
            {
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
                int result = await _applicationDbContext.SaveChangesAsync();

                if (result > 0)
                {
                    DataChanged?.Invoke(this, entity);
                }
            }
            catch (Exception msg) // Use custom exception here
            {
                Debug.WriteLine(msg.Message);
                Debugger.Break();
            }
        }
    }
}
