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
    public abstract class DataModelBase
    {
        /// <summary>
        /// Unique id of table
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Date this entry was created
        /// </summary>
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
