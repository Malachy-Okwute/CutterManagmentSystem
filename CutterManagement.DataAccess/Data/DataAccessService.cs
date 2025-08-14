using CutterManagement.Core;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace CutterManagement.DataAccess
{
    /// <summary>
    /// Provides access to the application database
    /// </summary>
    public class DataAccessService<T> : IDataAccessService<T>, IDisposable where T : class
    {
        /// <summary>
        /// Locker
        /// </summary>
        private readonly object _locker = new object();

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
                catch (Exception msg) 
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
            IReadOnlyList<T> entities;
            
            lock(_locker)
            {
                entities = _dbTable.ToList();
            }

            return await Task.FromResult(entities);
        }

        /// <summary>
        /// Get an entity by id
        /// </summary>
        /// <param name="entityId">The id of the entity to get</param>
        /// <param name="navProperty">Expression used to get the navigation properties</param>
        /// <returns><see cref="Task{T}"/> of <see cref="T"/></returns>
        public async Task<T?> GetEntityByIdAsync(int? entityId, Expression<Func<T, DataModelBase>>? navProperty = null) 
        {
            // Check if the entityId is null
            if (entityId is null)
            {
                return null;
            }
            // Check if the navigation property is requested    
            if (navProperty is not null)
            {
                return await _dbTable.Include(navProperty).FirstOrDefaultAsync(entity => EF.Property<int>(entity, "Id") == entityId);
            }

            // Return the entity by id without navigation properties
            return await _dbTable.FindAsync(entityId);
        }

        /// <summary>
        /// Gets an entity including it's navigation properties
        /// </summary>
        /// <typeparam name="TProperty">The navigation property</typeparam>
        /// <param name="entityId">The main entity id to get</param>
        /// <param name="includeExpression">Expression used to get the navigation properties</param>
        /// <returns><see cref="Task{T}"/> of <see cref="T"/></returns>
        public async Task<T> GetEntityWithCollectionsByIdAsync<TProperty>(int entityId, Expression<Func<T, ICollection<TProperty>>> includeExpression) where TProperty : class
        {
            return await _dbTable.Include(includeExpression).FirstAsync(entity => EF.Property<int>(entity, "Id") == entityId);
        }

        /// <summary>
        /// Update an entity
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <returns><see cref="Task"/></returns>
        public async Task SaveEntityAsync(T entity)
        {
            int result = 0;
            using var transaction = await _applicationDbContext.Database.BeginTransactionAsync();

            try
            {
                _dbTable.Update(entity);

                result = await _applicationDbContext.SaveChangesAsync();

                _applicationDbContext.Database.CommitTransaction();

                if (result > 0)
                {
                    DataChanged?.Invoke(this, entity);
                }
            }
            catch (Exception msg) // TODO: Undo changes if db transaction failed
            {
                if (result == 0)
                {
                    await transaction.RollbackAsync();
                }

                Debug.WriteLine(msg.Message);
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
            int result = 0;
            using var transaction = await _applicationDbContext.Database.BeginTransactionAsync();

            try
            {
                _dbTable.Remove(entity);
                result = await _applicationDbContext.SaveChangesAsync();
                _applicationDbContext.Database.CommitTransaction();


                if (result > 0)
                {
                    DataChanged?.Invoke(this, entity);
                }
            }
            catch (Exception msg) 
            {
                if (result == 0)
                {
                    await transaction.RollbackAsync();
                }

                Debug.WriteLine(msg.Message);
                Debugger.Break();
            }
        }

        public void Dispose()
        {
            _applicationDbContext.Dispose();
        }
    }
}
