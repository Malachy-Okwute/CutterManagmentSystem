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

        ///// <summary>
        ///// Machine navigation property collection 
        ///// </summary>
        //public ICollection<MachineDataModel> MachineDataModel { get; set; } = new List<MachineDataModel>();

        /// <summary>
        /// Machine and users navigation property collection
        /// </summary>
        public ICollection<MachineUserInteractions> MachineUserInteractions { get; set; } = new List<MachineUserInteractions>();
    }
}
