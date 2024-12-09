namespace CutterManagement.Core
{
    /// <summary>
    /// Access to user database
    /// </summary>
    public interface IUserDataAccessService
    {

        /// <summary>
        /// Get all user data from database
        /// </summary>
        /// <returns>A collection of <see cref="UserDataModel"></returns>
        Task<ICollection<UserDataModel>> GetAllUserAsync();


        /// <summary>
        /// Try to get the specified user
        /// </summary>
        /// <param name="user">The user to get</param>
        /// <returns><see cref="UserDataModel"/></returns>
        Task<UserDataModel?> GetUserAsync(UserDataModel user);

        /// <summary>
        /// Update the specified user data on database
        /// </summary>
        /// <param name="user">The user to update</param>
        /// <returns><see cref="Task"/></returns>
        Task UpdateUserAsync(UserDataModel user);

        /// <summary>
        /// Adds a user entry to database
        /// </summary>
        /// <param name="user">The user to add to database</param>
        /// <returns><see cref="Task"/></returns>
        Task AddUserAsync(UserDataModel user);

        /// <summary>
        /// Remove the specified user from database
        /// </summary>
        /// <param name="user">The user to remove</param>
        /// <returns><see cref="Task"/></returns>
        Task RemoveUserAsync(UserDataModel user);

        /// <summary>
        /// Makes sure we have database
        /// </summary>
        /// <returns><see cref="bool"/> True if we have database, otherwise false</returns>
        Task<bool> EnsureDbCreatedAsync();
    }
}
