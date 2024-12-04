using CutterManagement.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CutterManagement.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<MachineDataModel>? MachineDataStore { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MachineDataModel>().HasKey(x => x.Id);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.SetID).HasMaxLength(6);
            modelBuilder.Entity<MachineDataModel>().Property(x => x.Owner).IsRequired();
        }
    }
}
