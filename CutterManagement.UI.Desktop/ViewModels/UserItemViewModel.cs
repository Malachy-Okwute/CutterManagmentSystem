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
        /// The full name of this user
        /// </summary>
        private string _userFullName;

        /// <summary>
        /// The initials of this user, extracted from first and last name
        /// </summary>
        private string _userInitials;

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
        public string UserFullName
        {
            get => _userFullName;
            set
            {
                if (string.IsNullOrEmpty(FirstName) is false && string.IsNullOrEmpty(LastName) is false)
                {
                    _userFullName = string.Join(" ", FirstName, LastName);
                    _userFullName = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// The initials of this user, extracted from first and last name
        /// </summary>
        public string UserInitials 
        {
            get => _userInitials;
            set
            {
                if(string.IsNullOrEmpty(FirstName) is false && string.IsNullOrEmpty(LastName) is false)
                {
                    _userInitials = string.Join("", FirstName[0], LastName[0]);
                    _userInitials = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// The current shift of user
        /// </summary>
        public object UserShift { get; set; }

        /// <summary>
        /// Collection of shifts available
        /// </summary>
        public Dictionary<UserShift, string> ShiftCollection { get; set; }
    }
}
