namespace CutterManagement.Core
{ 
    /// <summary>
    /// A basic user data model
    /// </summary>
    public class UserDataModel
    {
        /// <summary>
        /// Unique id of this user to be identified with in database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User first name 
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// User last name 
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Role of user
        /// </summary>
        public UserRole Role { get; set; }  

        /// <summary>
        /// User shift
        /// </summary>
         public UserShift Shift { get; set; }

        /// <summary>
        /// Data this entry was created
        /// </summary>
        public DateTime EntryDate { get; set; } 
    }
}
