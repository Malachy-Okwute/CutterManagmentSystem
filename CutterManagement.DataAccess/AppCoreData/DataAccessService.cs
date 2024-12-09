using CutterManagement.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Generates database instance if it hasn't been created yet
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        public async Task GenerateDatabaseAsync()
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
