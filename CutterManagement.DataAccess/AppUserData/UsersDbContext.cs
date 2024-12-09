using CutterManagement.Core;
using Microsoft.EntityFrameworkCore;

namespace CutterManagement.DataAccess
{
    /// <summary>
    /// Users database context
    /// <remark>
    /// Database provider = SQLite
    /// </remark>
    /// </summary>
    public class UsersDbContext : DbContext
    {
        /// <summary>
        /// Users table
        /// </summary>
        public DbSet<UserDataModel> Users => Set<UserDataModel>();

        /// <summary>
        /// Default constructor
        /// </summary>
        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options) { }

        /// <summary>
        /// Configure table columns
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent API
            modelBuilder.Entity<UserDataModel>().HasKey(user => user.Id);
            modelBuilder.Entity<UserDataModel>().Property(user => user.FirstName).IsRequired();
            modelBuilder.Entity<UserDataModel>().Property(user => user.LastName).IsRequired();
            modelBuilder.Entity<UserDataModel>().Property(user => user.Shift).IsRequired();
        }
    }
}
