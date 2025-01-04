using CutterManagement.Core;
using System.Globalization;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="UserItemControl"/>
    /// </summary>
    public class UserItemViewModel : ViewModelBase
    {
        /// <summary>
        /// User id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// User last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The full name of this user
        /// </summary>
        public string UserFullName { get; set; }

        /// <summary>
        /// The initials of this user, extracted from first and last name
        /// </summary>
        public string UserInitials { get; set; }

        /// <summary>
        /// The current shift of user
        /// </summary>
        public UserShift UserShift { get; set; }
    }
}
