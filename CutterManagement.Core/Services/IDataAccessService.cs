using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutterManagement.Core
{
    public interface IDataAccessService
    {
        Task EnsureDatabaseAsync();
    }
}
