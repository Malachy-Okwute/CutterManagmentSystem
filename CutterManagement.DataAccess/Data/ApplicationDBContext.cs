using CutterManagement.Core;
using Microsoft.EntityFrameworkCore;

namespace CutterManagement.DataAccess
{
    /// <summary>
    /// Application database client
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Users table
        /// </summary>
        public DbSet<UserDataModel> Users => Set<UserDataModel>();

        /// <summary>
        /// Machine data table
        /// </summary>
        public DbSet<MachineDataModel> Machines => Set<MachineDataModel>();

        /// <summary>
        /// Parts table
        /// </summary>
        public DbSet<PartDataModel> Parts => Set<PartDataModel>();

        /// <summary>
        /// Cutters table
        /// </summary>
        public DbSet<CutterDataModel> Cutters => Set<CutterDataModel>();
        
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="options">Database context options use in configuring the database</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        /// <summary>
        /// Configures database tables 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MachineDataModel>().HasKey(x => x.Id);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.MachineNumber).HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.MachineSetId).HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.Count).HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.PartToothSize).HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.EntryCreatedDateTime).HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.Owner).HasConversion<string>().IsRequired();
            modelBuilder.Entity<MachineDataModel>().Property(x => x.Status).HasConversion<string>().HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.FrequencyCheckResult).HasConversion<string>().HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.CutterChangeInfo).HasConversion<string>().HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.CutterChangeComment).HasMaxLength(400);

            modelBuilder.Entity<MachineDataModel>().OwnsMany(x => x.Part, part =>
            {
                part.Property<int>("Id");
                part.HasKey("Id");
            });

            modelBuilder.Entity<MachineDataModel>().OwnsMany(x => x.Cutter, cutter =>
            {
                cutter.Property<int>("Id");
                cutter.HasKey("Id");
            });

            modelBuilder.Entity<MachineDataModel>().OwnsMany(x => x.Users, user =>
            {
                user.Property<int>("Id");
                user.HasKey("Id");
            });

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Update the database with the most up to date migration or 
        /// generates database if it hasn't been created yet
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        public async Task UpdateDatabaseMigrateAsync()
        {
            try
            {
                // Run migration
                await Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                // Log ex.Message as error or warning

                throw new Exception(ex.Message);
            }
        }
    }
}
