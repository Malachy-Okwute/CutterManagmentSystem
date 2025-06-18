namespace CutterManagement.Core
{
    /// <summary>
    /// A basic user data model
    /// </summary>
    public class UserDataModel : DataModelBase, IMessage
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
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Machine and users navigation property collection
        /// </summary>
        public ICollection<MachineUserInteractions> MachineUserInteractions { get; set; } = new List<MachineUserInteractions>();
    }
}
