namespace CutterManagement.Core
{
    public class AuthenticationService : IDisposable
    {
        private readonly object _locker = new object();

        private static Timer? _timer;

        public static bool IsAdminUserAuthorized { get; private set; }

        public AuthenticationResult Authenticate(string username, string password) 
        {
            AuthenticationResult result = new AuthenticationResult { ErrorMessages = new List<string>() };

            if (string.IsNullOrEmpty(username))
            {
                result.ErrorMessages.Add("Username is required");
            }

            if (string.IsNullOrEmpty(password))
            {
                result.ErrorMessages.Add("Password is required");
            }

            if(string.Equals(username, "admin", StringComparison.InvariantCultureIgnoreCase) && 
                !string.Equals(password, "admin@048", StringComparison.InvariantCultureIgnoreCase))
            {
                result.ErrorMessages.Add("Invalid log in credential");
            }

            if(!string.Equals(username, "admin", StringComparison.InvariantCultureIgnoreCase) && 
                !string.Equals(password, "admin@048", StringComparison.InvariantCultureIgnoreCase))
            {
                result.ErrorMessages.Add("Invalid log in credential");
            }

            if(!string.Equals(username, "admin", StringComparison.InvariantCultureIgnoreCase) && 
                string.Equals(password, "admin@048", StringComparison.InvariantCultureIgnoreCase))
            {
                result.ErrorMessages.Add("Invalid log in credential");
            }

            if(result.Success)
            {
                IsAdminUserAuthorized = true;
                StartAdminPrivilegeSessionCountDown();
            }
            else
            {
                IsAdminUserAuthorized = false;
            }

            return result;
        }

        private void StartAdminPrivilegeSessionCountDown()
        {
            lock(_locker)
            {
                IsAdminUserAuthorized = true;
                _timer = new Timer(TimerElapsed, null, 0, Timeout.Infinite);
                _timer.Change((int)TimeSpan.FromMinutes(120).TotalMilliseconds, Timeout.Infinite);
            }
        }

        private void TimerElapsed(object? state)
        {
            if (Monitor.TryEnter(_locker))
            {
                try
                {
                    if(_timer is null)
                    {
                        return;
                    }

                    StopAdminPrivilegeSessionCountDown();
                    Dispose();
                }
                finally
                {
                    Monitor.Exit(_locker);
                }
            }
        }

        private void StopAdminPrivilegeSessionCountDown()
        {
            lock (_locker)
            {
                _timer?.Change(Timeout.Infinite, Timeout.Infinite);
                IsAdminUserAuthorized = false;
            }
        }

        public void Dispose()
        {
            lock (_locker)
            {
                if (_timer != null)
                {
                    WaitHandle waitHandle = new AutoResetEvent(false);
                    _timer.Dispose(waitHandle);
                    WaitHandle.WaitAll(new[] { waitHandle }, TimeSpan.FromMinutes(1));
                    _timer = null;
                }
            }
        }
    }
}
