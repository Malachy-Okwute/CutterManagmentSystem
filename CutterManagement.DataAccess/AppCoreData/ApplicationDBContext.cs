using CutterManagement.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CutterManagement.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public required DbSet<MachineDataModel> MachineDataStore { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MachineDataModel>().HasKey(x => x.Id);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.SetID).HasMaxLength(6);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.Owner).IsRequired();

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=(localdb)\\ProjectModels;Database=CutterManagementSystemDatabase;Trusted_Connection=True;");

            base.OnConfiguring(optionsBuilder);
        }
    }
}
