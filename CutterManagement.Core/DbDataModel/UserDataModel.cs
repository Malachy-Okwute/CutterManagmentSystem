namespace CutterManagement.Core
{ 
    /// <summary>
    /// A basic user data model
    /// </summary>
    public class UserDataModel : DbDataModelBase
    {
        /// <summary>
        /// Foreign id to <see cref="MachineDataModel"/>
        /// </summary>
        public int MachineDataModelId { get; set; }

        /// <summary>
        /// Navigation property <see cref="MachineDataModel"/>
        /// </summary>
        public MachineDataModel MachineData { get; set; }

        /// <summary>
        /// User first name 
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// User last name 
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// User shift
        /// </summary>
         public UserShift Shift { get; set; }

        /// <summary>
        /// Data this entry was created
        /// </summary>
        public DateTime EntryCreatedDateTime { get; set; } 
    }
}
