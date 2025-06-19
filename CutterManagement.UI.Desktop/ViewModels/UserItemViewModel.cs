using CutterManagement.Core;
using System.Globalization;
using System.Windows.Input;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="UserItemControl"/>
    /// </summary>
    public class UserItemViewModel : ViewModelBase
    {
        #region Properties

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
        /// True if part item is in edit mode
        /// </summary>
        public bool IsEditMode { get; set; }

        /// <summary>
        /// The current shift of user
        /// </summary>
        public string UserShift { get; set; }

        /// <summary>
        /// User current shift
        /// <para>
        /// NOTE: Facilitates sorting users by shift.
        /// </para>
        /// </summary>
        public UserShift Shift { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Event that is raised when this item is selected
        /// </summary>
        public event EventHandler UserItemSelected;

        #endregion

        #region Commands

        /// <summary>
        /// Command to enter into part edit mode
        /// </summary>
        public ICommand EnterEditModeCommand { get; set; }

        /// <summary>
        /// Command to reset edit mode
        /// </summary>
        public ICommand ResetEditModeCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserItemViewModel()
        {
            // Create commands
            EnterEditModeCommand = new RelayCommand(EnterEditMode);
            ResetEditModeCommand = new RelayCommand(OnPartItemSelected);
        }

        #endregion

        /// <summary>
        /// Activates edit mode
        /// </summary>
        private void EnterEditMode()
        {
            // Raise on-selected event
            OnPartItemSelected();
            // Set flag
            IsEditMode = true;
        }

        /// <summary>
        /// Raises item selected event whenever this item is selected
        /// </summary>
        private void OnPartItemSelected() => UserItemSelected?.Invoke(this, EventArgs.Empty);
    }
}
