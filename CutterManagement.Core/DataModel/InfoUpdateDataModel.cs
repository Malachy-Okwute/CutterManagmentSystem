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
        /// True if move is attached to this data point
        /// </summary>
        public bool HasAttachedMoves => int.TryParse(PartNumberWithMove, out var result) is true;

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
        /// The kind of part (Gear / Pinion)
        /// </summary>
        public PartKind Kind { get; set; }

        /// <summary>
        /// The selected part number 
        /// </summary>
        public string PartNumberWithMove { get; set; } 

        /// <summary>
        /// Pressure angle value on coast
        /// </summary>
        public string PressureAngleCoast { get; set; }

        /// <summary>
        /// Pressure angle value on drive
        /// </summary>
        public string PressureAngleDrive { get; set; }

        /// <summary>
        /// Spiral angle value on coast
        /// </summary>
        public string SpiralAngleCoast { get; set; }

        /// <summary>
        /// Spiral angle value on drive
        /// </summary>
        public string SpiralAngleDrive { get; set; }

        /// <summary>
        /// Unique user foreign key
        /// </summary>
        public int UserDataModelId { get; set; }

        /// <summary>
        /// The actual user / author of this update
        /// </summary>
        /// <remarks>Navigation property</remarks>
        public UserDataModel UserDataModel { get; set; }

    }
}
