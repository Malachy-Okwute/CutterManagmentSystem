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
        /// CMM data
        /// </summary>
        public DbSet<CMMDataModel> CMMData => Set<CMMDataModel>();

        /// <summary>
        /// Information updates
        /// </summary>
        public DbSet<InfoUpdateDataModel> InfoUpdates => Set<InfoUpdateDataModel>();

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
            modelBuilder.Entity<MachineDataModel>().Property(x => x.IsConfigured).HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.PartNumber).HasMaxLength(100).IsRequired(false);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.PartToothSize).HasMaxLength(100).IsRequired(false);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.StatusMessage).HasMaxLength(100).IsRequired(false);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.DateCreated).HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.DateTimeLastSetup).HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.DateTimeLastModified).HasMaxLength(100);
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
            modelBuilder.Entity<PartDataModel>().Property(x => x.SummaryNumber).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<PartDataModel>().Property(x => x.Model).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<PartDataModel>().Property(x => x.Kind).HasConversion<string>().IsRequired().HasMaxLength(100);

            #endregion

            #region Cutter Data Model Configuration

            modelBuilder.Entity<CutterDataModel>().HasKey(x => x.Id);
            modelBuilder.Entity<CutterDataModel>().Property(x => x.Model).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<CutterDataModel>().Property(x => x.CutterNumber).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<CutterDataModel>().Property(x => x.Kind).HasConversion<string>().IsRequired().HasMaxLength(100);
            modelBuilder.Entity<CutterDataModel>().Property(x => x.Owner).HasConversion<string>().IsRequired().HasMaxLength(100);
            modelBuilder.Entity<CutterDataModel>().Property(x => x.Condition).IsRequired().HasMaxLength(100);
            //modelBuilder.Entity<CutterDataModel>().Property(x => x.Condition).HasConversion<string>().IsRequired().HasMaxLength(100);
            modelBuilder.Entity<CutterDataModel>().Property(x => x.CutterChangeInfo).HasConversion<string>().IsRequired().HasMaxLength(100);

            #endregion

            #region CMM Data Model Configuration

            modelBuilder.Entity<CMMDataModel>().HasKey(x => x.Id);
            modelBuilder.Entity<CMMDataModel>().Property(x => x.BeforeCorrections).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<CMMDataModel>().Property(x => x.AfterCorrections).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<CMMDataModel>().Property(x => x.PressureAngleCoast).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<CMMDataModel>().Property(x => x.PressureAngleDrive).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<CMMDataModel>().Property(x => x.SpiralAngleCoast).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<CMMDataModel>().Property(x => x.SpiralAngleDrive).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<CMMDataModel>().Property(x => x.Fr).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<CMMDataModel>().Property(x => x.Size).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<CMMDataModel>().Property(x => x.Count).IsRequired().HasMaxLength(100);

            #endregion

            #region Info Updates Model Configuration

            modelBuilder.Entity<InfoUpdateDataModel>().HasKey(x => x.Id);
            modelBuilder.Entity<InfoUpdateDataModel>().Property(x => x.Author).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<InfoUpdateDataModel>().Property(x => x.Title).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<InfoUpdateDataModel>().Property(x => x.Information).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<InfoUpdateDataModel>().Property(x => x.LastUpdatedDate).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<InfoUpdateDataModel>().Property(x => x.DateCreated).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<InfoUpdateDataModel>().Property(x => x.PublishDate).IsRequired().HasMaxLength(100);

            #endregion

            #region Relationship Configurations

            // Parts and machines
            //modelBuilder.Entity<PartDataModel>().HasMany(p => p.MachineDataModel).WithMany(m => m.Parts);
            //modelBuilder.Entity<MachineDataModel>().HasMany(p => p.Parts).WithMany(m => m.MachineDataModel);

            // Users and machines
            modelBuilder.Entity<MachineUserInteractions>().HasOne(u => u.UserDataModel).WithMany(mui => mui.MachineUserInteractions).HasForeignKey(fk => fk.UserDataModelId);
            modelBuilder.Entity<MachineUserInteractions>().HasOne(m => m.MachineDataModel).WithMany(mui => mui.MachineUserInteractions).HasForeignKey(fk => fk.MachineDataModelId);

            // Users and information updates
            modelBuilder.Entity<InfoUpdateUserRelations>().HasOne(u => u.UserDataModel).WithMany(mui => mui.InfoUpdateUserRelations).HasForeignKey(fk => fk.UserDataModelId);
            modelBuilder.Entity<InfoUpdateUserRelations>().HasOne(i => i.InfoUpdateDataModel).WithMany(mui => mui.InfoUpdateUserRelations).HasForeignKey(fk => fk.InfoUpdatesDataModelId);

            // Cutter and machine
            modelBuilder.Entity<MachineDataModel>().HasOne(m => m.Cutter).WithOne(c => c.MachineDataModel).HasForeignKey<CutterDataModel>(c => c.MachineDataModelId);

            // CMMData and cutter
            modelBuilder.Entity<CutterDataModel>().HasMany(m => m.CMMData).WithOne(c => c.CutterDataModel).HasForeignKey(c => c.CutterDataModelId);

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
