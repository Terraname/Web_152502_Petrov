using Microsoft.EntityFrameworkCore;
using Web_152502_Petrov.Domain.Entities;

namespace Web_152502_Petrov.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Drug> Drugs { get; set; }
        public DbSet<Cathegory> Cathegories { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Cathegory>().HasMany(g => g.Drugs).WithOne(p => p.cathegory).HasForeignKey(p => p.CathegoryID);
            modelBuilder.Entity<Drug>().HasOne(p => p.Cathegory);
        }
    }
}
