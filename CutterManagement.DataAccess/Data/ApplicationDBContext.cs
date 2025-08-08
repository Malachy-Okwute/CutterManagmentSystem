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
        /// User archive table
        /// </summary>
        public DbSet<UserDataArchive> UsersArchive => Set<UserDataArchive>();

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

        /// <summary>
        /// Production part log / history
        /// </summary>
        public DbSet<ProductionPartsLogDataModel> ProductionPartsLog => Set<ProductionPartsLogDataModel>();

        /// <summary>
        /// Production part-log data archive
        /// </summary>
        public DbSet<ProductionPartsLogDataArchive> ProductionPartsLogDataArchive => Set<ProductionPartsLogDataArchive>();

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

            #region User Data Archive Configuration

            modelBuilder.Entity<UserDataArchive>().HasKey(x => x.Id);
            modelBuilder.Entity<UserDataArchive>().Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<UserDataArchive>().Property(x => x.LastName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<UserDataArchive>().Property(x => x.Shift).HasConversion<string>().IsRequired().HasMaxLength(100);

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
            modelBuilder.Entity<CutterDataModel>().Property(x => x.CutterChangeInfo).HasConversion<string>().IsRequired().HasMaxLength(100);
            modelBuilder.Entity<CutterDataModel>().Property(x => x.Condition).IsRequired().HasMaxLength(100);
            //modelBuilder.Entity<CutterDataModel>().Property(x => x.Condition).HasConversion<string>().IsRequired().HasMaxLength(100);

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
            modelBuilder.Entity<InfoUpdateDataModel>().Property(x => x.Information).IsRequired();
            modelBuilder.Entity<InfoUpdateDataModel>().Property(x => x.Title).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<InfoUpdateDataModel>().Property(x => x.DateCreated).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<InfoUpdateDataModel>().Property(x => x.PublishDate).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<InfoUpdateDataModel>().Property(x => x.LastUpdatedDate).IsRequired().HasMaxLength(100);
            // Attached moves
            modelBuilder.Entity<InfoUpdateDataModel>().Property(x => x.PressureAngleCoast).HasMaxLength(100).IsRequired(false);
            modelBuilder.Entity<InfoUpdateDataModel>().Property(x => x.PressureAngleDrive).HasMaxLength(100).IsRequired(false);
            modelBuilder.Entity<InfoUpdateDataModel>().Property(x => x.SpiralAngleCoast).HasMaxLength(100).IsRequired(false);
            modelBuilder.Entity<InfoUpdateDataModel>().Property(x => x.SpiralAngleDrive).HasMaxLength(100).IsRequired(false);
            modelBuilder.Entity<InfoUpdateDataModel>().Property(x => x.PartNumberWithMove).HasMaxLength(100).IsRequired(false);
            modelBuilder.Entity<InfoUpdateDataModel>().Property(x => x.Kind).HasConversion<string>().HasMaxLength(100);

            #endregion

            #region Production Parts Log Data Model Configuration

            modelBuilder.Entity<ProductionPartsLogDataModel>().HasKey(x => x.Id);
            modelBuilder.Entity<ProductionPartsLogDataModel>().Property(x => x.IsArchived);
            modelBuilder.Entity<ProductionPartsLogDataModel>().Property(x => x.MachineNumber).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<ProductionPartsLogDataModel>().Property(x => x.CutterNumber).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<ProductionPartsLogDataModel>().Property(x => x.PartNumber).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<ProductionPartsLogDataModel>().Property(x => x.Model).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<ProductionPartsLogDataModel>().Property(x => x.ToothCount).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<ProductionPartsLogDataModel>().Property(x => x.PieceCount).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<ProductionPartsLogDataModel>().Property(x => x.ToothSize).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<ProductionPartsLogDataModel>().Property(x => x.UserFullName).IsRequired(false).HasMaxLength(100);
            modelBuilder.Entity<ProductionPartsLogDataModel>().Property(x => x.Comment).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<ProductionPartsLogDataModel>().Property(x => x.CutterChangeInfo).IsRequired(false).HasMaxLength(100);
            modelBuilder.Entity<ProductionPartsLogDataModel>().Property(x => x.FrequencyCheckResult).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<ProductionPartsLogDataModel>().Property(x => x.CurrentShift).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<ProductionPartsLogDataModel>().Property(x => x.DateTimeOfCheck).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<ProductionPartsLogDataModel>().Property(x => x.DateCreated).IsRequired().HasMaxLength(100);

            #endregion

            #region Production Parts Log Data Archive Model Configuration

            modelBuilder.Entity<ProductionPartsLogDataArchive>().HasKey(x => x.Id);
            modelBuilder.Entity<ProductionPartsLogDataArchive>().Property(x => x.IsArchived);
            modelBuilder.Entity<ProductionPartsLogDataArchive>().Property(x => x.MachineNumber).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<ProductionPartsLogDataArchive>().Property(x => x.CutterNumber).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<ProductionPartsLogDataArchive>().Property(x => x.PartNumber).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<ProductionPartsLogDataArchive>().Property(x => x.Model).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<ProductionPartsLogDataArchive>().Property(x => x.ToothCount).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<ProductionPartsLogDataArchive>().Property(x => x.PieceCount).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<ProductionPartsLogDataArchive>().Property(x => x.ToothSize).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<ProductionPartsLogDataArchive>().Property(x => x.UserFullName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<ProductionPartsLogDataArchive>().Property(x => x.Comment).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<ProductionPartsLogDataArchive>().Property(x => x.FrequencyCheckResult).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<ProductionPartsLogDataArchive>().Property(x => x.CurrentShift).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<ProductionPartsLogDataArchive>().Property(x => x.CutterChangeInfo).IsRequired(false).HasMaxLength(100);
            //modelBuilder.Entity<ProductionPartsLogDataArchive>().Property(x => x.CMMData).IsRequired(false).HasMaxLength(100);
            modelBuilder.Entity<ProductionPartsLogDataArchive>().Property(x => x.DateTimeOfCheck).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<ProductionPartsLogDataArchive>().Property(x => x.DateCreated).IsRequired().HasMaxLength(100);

            #endregion

            #region Relationship Configurations

            // Users and machines
            modelBuilder.Entity<MachineUserInteractions>().HasOne(u => u.UserDataModel).WithMany(mui => mui.MachineUserInteractions).HasForeignKey(fk => fk.UserDataModelId);
            modelBuilder.Entity<MachineUserInteractions>().HasOne(m => m.MachineDataModel).WithMany(mui => mui.MachineUserInteractions).HasForeignKey(fk => fk.MachineDataModelId);

            // Users and information updates
            modelBuilder.Entity<InfoUpdateUserRelations>().HasOne(u => u.UserDataModel).WithMany(mui => mui.InfoUpdateUserRelations).HasForeignKey(fk => fk.UserDataModelId);
            modelBuilder.Entity<InfoUpdateUserRelations>().HasOne(i => i.InfoUpdateDataModel).WithMany(mui => mui.InfoUpdateUserRelations).HasForeignKey(fk => fk.InfoUpdatesDataModelId)
                                                          .OnDelete(DeleteBehavior.ClientCascade);

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
                //await Database.MigrateAsync();

                await Database.EnsureCreatedAsync(); // For development
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
