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
        /// Machine and users skip navigation properties
        /// </summary>
        public ICollection<MachineDataModelUserDataModel> MachinesAndUsers { get; set; }
    }
}
