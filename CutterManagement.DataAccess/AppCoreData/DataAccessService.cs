using CutterManagement.Core;
using Microsoft.EntityFrameworkCore;

namespace CutterManagement.DataAccess
{
    public class DataAccessService : IDataAccessService
    {
        protected ApplicationDbContext _applicationDbContext;

        public DataAccessService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        /// <summary>
        /// Update the database with the most up to date migration or 
        /// generates database if it hasn't been created yet
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        public async Task UpdateDatabaseMigrateAsync()
        {
            try
            {
                await _applicationDbContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                // Log ex.Message as error or warning

                throw new Exception(ex.Message);
            }
        }
    }
}
