namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for deactivated user item
    /// </summary>
    public class DeactivatedUserViewModel
    {
        /// <summary>
        /// Unique Id of this user
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// True if this user is selected
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// User's full name
        /// </summary>
        public string UserFullName { get; set; }
    }
}
