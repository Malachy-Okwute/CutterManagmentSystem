using CutterManagement.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutterManagement.DataAccess
{
    internal class DataAccessService : IDataAccessService
    {
        protected ApplicationDbContext _applicationDbContext;

        public DataAccessService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task EnsureDatabaseAsync()
        {
            await _applicationDbContext.Database.EnsureCreatedAsync();
        }
    }
}
