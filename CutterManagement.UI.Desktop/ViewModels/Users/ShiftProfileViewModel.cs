using CutterManagement.Core;
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
            GetCurrentShift();

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
        /// Gets the current shift
        /// </summary>
        private void GetCurrentShift()
        {
            // Get current time of the day
            TimeSpan now = DateTime.Now.TimeOfDay;

            // Define start and end of shifts
            TimeSpan shift1Start = new TimeSpan(7, 0, 0);   // 7:00 AM
            TimeSpan shift1End = new TimeSpan(15, 0, 0);    // 3:00 PM
            TimeSpan shift2Start = new TimeSpan(15, 0, 0);  // 3:00 PM
            TimeSpan shift2End = new TimeSpan(23, 0, 0);    // 11:00 PM
            TimeSpan shift3Start = new TimeSpan(23, 0, 0);  // 11:00 PM
            TimeSpan shift3End = new TimeSpan(7, 0, 0);     // 7:00 AM (next day)

            // 1st shift
            if (now >= shift1Start && now < shift1End)
            {
                CurrentShift = "1st Shift";
            }
            // 2nd shift
            else if (now >= shift2Start && now < shift2End)
            {
                CurrentShift = "2nd Shift";
            }
            else // Overnight shift (11 PM - 7 AM)
            {
                // Handles time between 11 PM and midnight, and midnight to 7 AM
                CurrentShift = "3rd Shift";
            }

            // Update property
            OnPropertyChanged(nameof(CurrentShift));
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

                GetCurrentShift();

                // If shift changed...
                if(previousShift.Equals(CurrentShift, StringComparison.OrdinalIgnoreCase) is false)
                {
                    // Get number of user in the current shift
                    _ = GetNumberOfUsersInCurrentShift(); 
                }
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
                    var usersTable = _dataFactory.GetDbTable<UserDataModel>();
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
