using Microsoft.EntityFrameworkCore;

namespace DemoEFCosmos
{
    public class DemoContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseCosmos(
                "https://localhost:8081",
                "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                databaseName: "Locations");

        public DbSet<Parent>? Parent { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("Demo");
            modelBuilder.Entity<Parent>().OwnsOne(b => b.Singleton);
            modelBuilder.Entity<Parent>().OwnsMany(b => b.Children);
        }
    }
}
