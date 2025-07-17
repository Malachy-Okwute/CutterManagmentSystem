using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutterManagement.Core
{
    /// <summary>
    /// Data model for information updates
    /// </summary>
    public class InfoUpdateDataModel : DataModelBase, IMessage
    {
        /// <summary>
        /// True if this it is currently archived
        /// </summary>
        public bool IsArchived { get; set; }

        /// <summary>
        /// Title of this information update
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Date this information was published
        /// </summary>
        public string PublishDate { get; set; }

        /// <summary>
        /// Date this information was lasts modified
        /// </summary>
        public string LastUpdatedDate { get; set; } 

        /// <summary>
        /// The actual information
        /// </summary>
        public string Information { get; set; }

        /// <summary>
        /// Information updates and user navigation property
        /// </summary>
        public ICollection<InfoUpdateUserRelations> InfoUpdateUserRelations { get; set; } = new List<InfoUpdateUserRelations>();

    }
}
