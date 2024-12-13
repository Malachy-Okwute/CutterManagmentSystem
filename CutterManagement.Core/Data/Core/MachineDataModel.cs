using System.ComponentModel.DataAnnotations;

namespace CutterManagement.Core
{
    /// <summary>
    /// Machine data model
    /// </summary>
    public class MachineDataModel
    {
        /// <summary>
        /// The unique id used to identify this data in db
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The unique set id assigned to this machine object
        /// </summary>
        public string MachineId { get; set; } = string.Empty;

        /// <summary>
        /// The unique set id assigned to this machine object
        /// </summary>
        public string SetId { get; set; } = string.Empty;

        /// <summary>
        /// The count representing the number of parts produced by this machine
        /// </summary>
        public string Count { get; set; } = string.Empty;

        /// <summary>
        /// Measured tooth size of part
        /// </summary>
        public string PartToothSize { get; set; } = string.Empty;

        /// <summary>
        /// The last date and time data record was updated
        /// </summary>
        public DateTime DateTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The dept. owner of this machine
        /// </summary>
        public Department Owner { get; set; }

        /// <summary>
        /// The status of this machine indicating whether it's running, sitting idle or down for maintenance
        /// </summary>
        public MachineStatus Status { get; set; }

        /// <summary>
        /// The result of a frequency check
        /// Options = Passed or Failed
        /// </summary>
        public FrequencyCheckResult FrequencyCheckResult { get; set; }

        /// <summary>
        /// Collection of part
        /// </summary>
        public ICollection<PartDataModel>? Part { get; set; }

        /// <summary>
        /// Collection of cutters
        /// </summary>
        public ICollection<CutterDataModel>? Cutter { get; set; }
    }
}
