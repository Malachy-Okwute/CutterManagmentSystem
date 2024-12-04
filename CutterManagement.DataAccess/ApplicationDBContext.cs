using CutterManagement.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CutterManagement.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<MachineDataModel>? MachineDataStore { get; set; }

        //public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=CutterManagementSystemDatabase;Trusted_Connection=True;");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MachineDataModel>().HasKey(x => x.Id);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.SetID).HasMaxLength(6);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.Owner).IsRequired();
        }
    }
}
