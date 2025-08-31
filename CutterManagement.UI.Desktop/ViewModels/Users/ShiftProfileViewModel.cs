using CutterManagement.Core;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace CutterManagement.UI.Desktop
{
    /// <summary>
    /// View model for <see cref="UserProfileControl"/>
    /// </summary>
    public class ShiftProfileViewModel : ViewModelBase
    {
        /// <summary>
        /// Access to db data
        /// </summary>
        private IDataAccessServiceFactory _dataFactory;

        /// <summary>
        /// Timer 
        /// </summary>
        private DispatcherTimer _timer;

        /// <summary>
        /// Flag indicating that users are currently being fetched
        /// </summary>
        private bool _isFetchingUsers;

        /// <summary>
        /// Current date of the day
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Current time of the day
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// The current shift running
        /// </summary>
        public string CurrentShift { get; set; }

        /// <summary>
        /// Number of users in current shift
        /// </summary>
        public string NumberOfUsers { get; set; }

        /// <summary>
        /// True if shift info show be shown 
        /// </summary>
        public bool ShowShiftInfo { get; set; }

        /// <summary>
        /// Command to show or hide shift information
        /// </summary>
        public ICommand ShowHideShiftInfoCommand { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dataFactory">db data access</param>
        public ShiftProfileViewModel(IDataAccessServiceFactory dataFactory)
        {
            _dataFactory = dataFactory;

            // Get current shift
            CurrentShift = ShiftHelper.GetCurrentShift();

            // Monitor shift change
            InitializeShiftProfileDetails();
            
            // Create commands
            ShowHideShiftInfoCommand = new RelayCommand(ShowHideShiftInfo);
        }

        /// <summary>
        /// Display or hides shift information
        /// </summary>
        private void ShowHideShiftInfo()
        {
            ShowShiftInfo ^= true;
        }

        /// <summary>
        /// Gathers details associated with current shift
        /// </summary>
        private void InitializeShiftProfileDetails()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            _timer.Tick +=  (s, e) =>
            {
                // Update date 
                Date = DateTime.Now.ToString("D");
                // And time
                Time = DateTime.Now.ToString("t");

                string previousShift = CurrentShift;
                string previousNumberOfUsers = NumberOfUsers;

                CurrentShift = ShiftHelper.GetCurrentShift();

                // If shift changed...
                if (previousShift.Equals(CurrentShift, StringComparison.OrdinalIgnoreCase) is false || string.IsNullOrEmpty(NumberOfUsers))
                {
                    // Get number of user in the current shift
                    _ = GetNumberOfUsersInCurrentShift(); 
                }

                // Update UI
                OnPropertyChanged(nameof(CurrentShift));
            };

            _timer.Start();
        }

        /// <summary>
        /// Fetches the number of users in current shift
        /// </summary>
        /// <returns></returns>
        private async Task GetNumberOfUsersInCurrentShift()
        {
            if (_isFetchingUsers) return;
            {
                _isFetchingUsers = true;

                try
                {
                    using var usersTable = _dataFactory.GetDbTable<UserDataModel>();
                    NumberOfUsers = (await usersTable.GetAllEntitiesAsync())
                        .Count(user => EnumHelpers.GetDescription(user.Shift) == CurrentShift && user.IsArchived is false && user.FirstName != "resource")
                        .ToString();
                }
                finally
                {
                        _isFetchingUsers = false;
                }
            }
        }

        /// <summary>
        /// Stop monitoring for shift change
        /// </summary>
        public void StopShiftChangeMonitoring()
        {
            _timer?.Stop();
        }
    }
}
