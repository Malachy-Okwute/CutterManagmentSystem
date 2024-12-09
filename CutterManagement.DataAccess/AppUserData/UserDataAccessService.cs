using CutterManagement.Core;

namespace CutterManagement.DataAccess
{
    /// <summary>
    /// Access to user database
    /// </summary>
    public class UserDataAccessService : IUserDataAccessService
    {
        /// <summary>
        /// User database
        /// </summary>
        private readonly UsersDbContext _userDb;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="userDb">User database</param>
        public UserDataAccessService(UsersDbContext userDb)
        {
            _userDb = userDb;
        }

        /// <summary>
        /// Get all user data from database
        /// </summary>
        /// <returns>A collection of <see cref="UserDataModel"></returns>
        public async Task<ICollection<UserDataModel>> GetAllUserAsync()
        {
            return await Task.FromResult(_userDb.Users.ToList());
        }

        /// <summary>
        /// Try to get the specified user
        /// </summary>
        /// <param name="user">The user to get</param>
        /// <returns><see cref="UserDataModel"/></returns>
        public async Task<UserDataModel?> GetUserAsync(UserDataModel user)
        {
            return await Task.FromResult(_userDb.Users.FirstOrDefault(entry => entry.Id == user.Id));
        }

        /// <summary>
        /// Adds a user entry to database
        /// </summary>
        /// <param name="user">The user to add to database</param>
        /// <returns><see cref="Task"/></returns>
        public async Task AddUserAsync(UserDataModel user)
        {
            await _userDb.Users.AddAsync(user);
            
            _userDb.SaveChanges();
        }

        /// <summary>
        /// Update the specified user data on database
        /// </summary>
        /// <param name="user">The user to update</param>
        /// <returns><see cref="Task"/></returns>
        public async Task UpdateUserAsync(UserDataModel user)
        {
            // Find the user entry
            UserDataModel? userEntry = _userDb.Users.FirstOrDefault(entry => entry.Id == user.Id);

            // If user entry is not null
            if (userEntry != null)
            {
                // Update user entry information 
                await Task.FromResult(_userDb.Users.Update(userEntry));
            }

            _userDb.SaveChanges();
        }

        /// <summary>
        /// Remove the specified user from database
        /// </summary>
        /// <param name="user">The user to remove</param>
        /// <returns><see cref="Task"/></returns>
        public async Task RemoveUserAsync(UserDataModel user)
        {
            await Task.FromResult(_userDb.Users.Remove(user));

            _userDb.SaveChanges();
        }

        /// <summary>
        /// Makes sure we have database
        /// </summary>
        /// <returns><see cref="bool"/> True if we have database, otherwise false</returns>
        public async Task<bool> EnsureDbCreatedAsync()
        {
            // If we don't have database...
            if (!_userDb.Database.CanConnect())
            {
                // Create it
                await _userDb.Database.EnsureCreatedAsync();
            }

            // Tell if we can connect to db or not
            return _userDb.Database.CanConnect();
        }

    }
}
