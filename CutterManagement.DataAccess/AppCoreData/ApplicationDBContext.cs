using CutterManagement.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CutterManagement.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<MachineDataModel> MachineDataStore => Set<MachineDataModel>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MachineDataModel>().HasKey(x => x.Id);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.MachineId).HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.SetId).HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.Count).HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.PartToothSize).HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.DateTime).HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.Owner).IsRequired();
            modelBuilder.Entity<MachineDataModel>().Property(x => x.Status).HasMaxLength(100);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.FrequencyCheckResult).HasMaxLength(100);

            modelBuilder.Entity<MachineDataModel>().OwnsMany(x => x.Part, part =>
            {
                part.WithOwner().HasForeignKey("PartForeignId");
                part.Property<int>("Id");
                part.HasKey("Id");
            });
            modelBuilder.Entity<MachineDataModel>().OwnsMany(x => x.Cutter, cutter =>
            {
                cutter.WithOwner().HasForeignKey("CutterForeignId");
                cutter.Property<int>("Id");
                cutter.HasKey("Id");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
