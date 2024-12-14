using CutterManagement.Core;
using Microsoft.EntityFrameworkCore;

namespace CutterManagement.DataAccess
{
    public class ApplicationDbContext : DbContext
    {


        public DbSet<UserDataModel> Users => Set<UserDataModel>();
        public DbSet<MachineDataModel> MachineData => Set<MachineDataModel>();
        public DbSet<PartDataModel> Parts => Set<PartDataModel>();
        public DbSet<CutterDataModel> Cutters => Set<CutterDataModel>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MachineDataModel>().HasKey(x => x.Id);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.MachineNumber).HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.MachineSetId).HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.Count).HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.PartToothSize).HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.DateTime).HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.Owner).IsRequired();
            modelBuilder.Entity<MachineDataModel>().Property(x => x.Status).HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.FrequencyCheckResult).HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.CutterChangeInfo).HasMaxLength(100);
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

            base.OnModelCreating(modelBuilder);
        }
    }
}
