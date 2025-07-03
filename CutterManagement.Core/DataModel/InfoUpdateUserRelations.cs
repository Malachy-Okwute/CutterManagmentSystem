using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutterManagement.Core
{
    /// <summary>
    /// Information update and user's navigation table
    /// </summary>
    public class InfoUpdateUserRelations : DataModelBase
    {
        /// <summary>
        /// Information update foreign key
        /// </summary>
        public int? InfoUpdatesDataModelId { get; set; }

        /// <summary>
        /// Associated Information update
        /// </summary>
        public InfoUpdateDataModel InfoUpdateDataModel { get; set; }

        /// <summary>
        /// User foreign key
        /// </summary>
        public int? UserDataModelId { get; set; }

        /// <summary>
        /// Associated user object
        /// </summary>
        public UserDataModel UserDataModel { get; set; }

        /// <summary>
        /// Date and time of last entry
        /// </summary>
        public DateTime LastEntryDateTime { get; set; } = DateTime.Now;

    }
}
