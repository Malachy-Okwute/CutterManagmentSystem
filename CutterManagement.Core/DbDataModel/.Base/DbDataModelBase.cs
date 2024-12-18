using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutterManagement.Core
{
    /// <summary>
    /// Base data model for database
    /// </summary>
    public class DbDataModelBase
    {
        /// <summary>
        /// Unique id of table
        /// </summary>
        public int Id { get; set; }
    }
}
