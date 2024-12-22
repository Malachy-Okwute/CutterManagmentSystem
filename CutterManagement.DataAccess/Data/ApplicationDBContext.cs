using CutterManagement.Core;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CutterManagement.DataAccess
{
    /// <summary>
    /// Application database client
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        #region Public Properties

        /// <summary>
        /// Machine data table
        /// </summary>
        public DbSet<MachineDataModel> Machines => Set<MachineDataModel>();

        /// <summary>
        /// Users table
        /// </summary>
        public DbSet<UserDataModel> Users => Set<UserDataModel>();

        /// <summary>
        /// Parts table
        /// </summary>
        public DbSet<PartDataModel> Parts => Set<PartDataModel>();

        /// <summary>
        /// Cutters table
        /// </summary>
        public DbSet<CutterDataModel> Cutters => Set<CutterDataModel>();

        /// <summary>
        /// Machine and users many to many relationship table
        /// </summary>
        public DbSet<MachineDataModelUserDataModel> MachineDataModelUserDataModels => Set<MachineDataModelUserDataModel>();

        /// <summary>
        /// Machine and parts many to many relationship table
        /// </summary>
        public DbSet<MachineDataModelPartDataModel> MachineDataModelPartDataModels => Set<MachineDataModelPartDataModel>();

        /// <summary>
        /// Machine and cutters many to many relationship table
        /// </summary>
        public DbSet<MachineDataModelCutterDataModel> MachineDataModelCutterDataModels => Set<MachineDataModelCutterDataModel>();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="options">Database context options use in configuring the database</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        #endregion

        #region Methods

        /// <summary>
        /// Configures database tables 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Machine Data Model Configuration

            modelBuilder.Entity<MachineDataModel>().HasKey(x => x.Id);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.MachineNumber).HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.MachineSetId).HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.Count).HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.PartToothSize).HasMaxLength(100).IsRequired(false);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.DateCreated).HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.Owner).HasConversion<string>().IsRequired().HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.Status).HasConversion<string>().HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.FrequencyCheckResult).HasConversion<string>().HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.CutterChangeInfo).HasConversion<string>().HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.CutterChangeComment).HasMaxLength(400).IsRequired(false);

            #endregion

            #region User Data Model Configuration

            modelBuilder.Entity<UserDataModel>().HasKey(x => x.Id);
            modelBuilder.Entity<UserDataModel>().Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<UserDataModel>().Property(x => x.LastName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<UserDataModel>().Property(x => x.Shift).HasConversion<string>().IsRequired().HasMaxLength(100);

            #endregion

            #region Part Data Model Configuration

            modelBuilder.Entity<PartDataModel>().HasKey(x => x.Id);
            modelBuilder.Entity<PartDataModel>().Property(x => x.PartNumber).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<PartDataModel>().Property(x => x.PartToothCount).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<PartDataModel>().Property(x => x.Model).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<PartDataModel>().Property(x => x.Kind).HasConversion<string>().IsRequired().HasMaxLength(100);

            #endregion

            #region Part Data Model Configuration

            modelBuilder.Entity<CutterDataModel>().HasKey(x => x.Id);
            modelBuilder.Entity<CutterDataModel>().Property(x => x.CutterNumber).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<CutterDataModel>().Property(x => x.Model).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<CutterDataModel>().Property(x => x.Kind).HasConversion<string>().IsRequired().HasMaxLength(100);
            modelBuilder.Entity<CutterDataModel>().Property(x => x.Owner).HasConversion<string>().IsRequired().HasMaxLength(100);
            modelBuilder.Entity<CutterDataModel>().Property(x => x.Condition).HasConversion<string>().IsRequired().HasMaxLength(100);

            #endregion

            #region Many To Many Relationship Configurations

            // Machine and user
            modelBuilder.Entity<MachineDataModelUserDataModel>().HasKey(mu => new {mu.MachineDataModelId, mu.UserDataModelId });
            modelBuilder.Entity<MachineDataModelUserDataModel>().HasOne(mu => mu.UserDataModel).WithMany(x => x.MachinesAndUsers).HasForeignKey(mu => mu.UserDataModelId);
            modelBuilder.Entity<MachineDataModelUserDataModel>().HasOne(mu => mu.MachineDataModel).WithMany(x => x.MachinesAndUsers).HasForeignKey(mu => mu.MachineDataModelId);

            // Machine and part
            modelBuilder.Entity<MachineDataModelPartDataModel>().HasKey(mp => new { mp.MachineDataModelId, mp.PartDataModelId });
            modelBuilder.Entity<MachineDataModelPartDataModel>().HasOne(mp => mp.PartDataModel).WithMany(x => x.MachinesAndParts).HasForeignKey(mp => mp.PartDataModelId);
            modelBuilder.Entity<MachineDataModelPartDataModel>().HasOne(mp => mp.MachineDataModel).WithMany(x => x.MachinesAndParts).HasForeignKey(mp => mp.MachineDataModelId);

            // Machine and cutter
            modelBuilder.Entity<MachineDataModelCutterDataModel>().HasKey(mc => new { mc.MachineDataModelId, mc.CutterDataModelId});
            modelBuilder.Entity<MachineDataModelCutterDataModel>().HasOne(mc => mc.CutterDataModel).WithMany(x => x.MachinesAndCutters).HasForeignKey(mp => mp.CutterDataModelId);
            modelBuilder.Entity<MachineDataModelCutterDataModel>().HasOne(mc => mc.MachineDataModel).WithMany(x => x.MachinesAndCutters).HasForeignKey(mp => mp.MachineDataModelId);

            #endregion
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
                Debugger.Break();
                Console.WriteLine(ex.Message);
            }
        }

        #endregion
    }
}
