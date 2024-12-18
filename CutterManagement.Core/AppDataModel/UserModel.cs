namespace CutterManagement.Core
{
    public class UserModel
    {
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
