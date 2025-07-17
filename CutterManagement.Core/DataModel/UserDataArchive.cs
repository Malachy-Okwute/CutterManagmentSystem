namespace CutterManagement.Core
{
    /// <summary>
    /// User data model archive
    /// </summary>
    public class UserDataArchive : DataModelBase
    {
        /// <summary>
        /// User first name 
        /// </summary>
        public string FirstName { get; set; } 

        /// <summary>
        /// User last name 
        /// </summary>
        public string LastName { get; set; } 

        /// <summary>
        /// User shift
        /// </summary>
        public UserShift Shift { get; set; }

        /// <summary>
        /// True if user is an active user
        /// </summary>
        public bool IsActive { get; set; } 

        /// <summary>
        /// True if this user is no longer accessible / currently archived
        /// </summary>
        public bool IsArchived { get; set; }

        /// <summary>
        /// Machine and users navigation property collection
        /// </summary>
        public ICollection<MachineUserInteractions> MachineUserInteractions { get; set; } = new List<MachineUserInteractions>();

        /// <summary>
        /// Information updates and user navigation property
        /// </summary>
        public ICollection<InfoUpdateUserRelations> InfoUpdateUserRelations { get; set; } = new List<InfoUpdateUserRelations>();
    }
}
