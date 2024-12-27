namespace CutterManagement.Core
{
    /// <summary>
    /// Result of an attempt to authenticate admin credentials 
    /// when trying to log in
    /// </summary>
    public class AuthenticationResult
    {
        /// <summary>
        /// Error messages
        /// </summary>
        public List<string> ErrorMessages { get; set; }

        /// <summary>
        /// True if attempt to log-in succeeded without error
        /// </summary>
        public bool Success => ErrorMessages.Count == 0;
    }
}