using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutterManagement.UI.Desktop
{
    public static class ShiftHelper
    {
        /// <summary>
        /// Gets the current shift
        /// </summary>
        public static string GetCurrentShift()
        {
            // Get current time of the day
            TimeSpan now = DateTime.Now.TimeOfDay;

            // Define start and end of shifts
            TimeSpan shift1Start = new TimeSpan(7, 0, 0);   // 7:00 AM
            TimeSpan shift1End = new TimeSpan(15, 0, 0);    // 3:00 PM
            TimeSpan shift2Start = new TimeSpan(15, 0, 0);  // 3:00 PM
            TimeSpan shift2End = new TimeSpan(23, 0, 0);    // 11:00 PM
            //TimeSpan shift3Start = new TimeSpan(23, 0, 0);  // 11:00 PM
            //TimeSpan shift3End = new TimeSpan(7, 0, 0);     // 7:00 AM (next day)

            // 1st shift
            if (now >= shift1Start && now < shift1End)
            {
                return "1st Shift";
            }
            // 2nd shift
            else if (now >= shift2Start && now < shift2End)
            {
                return "2nd Shift";
            }
            else // Overnight shift (11 PM - 7 AM)
            {
                // Handles time between 11 PM and midnight, and midnight to 7 AM
                return "3rd Shift";
            }
        }

    }
}
