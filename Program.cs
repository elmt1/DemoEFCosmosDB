using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DemoEFCosmos
{
    class Program
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
        public class Parent
        {
            public Guid Id { get; set; }
            public Singleton? Singleton { get; set; }
            public ICollection<Child>? Children { get; set; }
        }
        public class Singleton
        {
            public string SingletonName { get; set; } = string.Empty;
        }
        public class Child
        {
            public string ChildName { get; set; } = string.Empty;
        }
        static void Main(string[] args)
        {
            using (var context = new DemoContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                var children = new HashSet<Child>()
                {
                    new Child() { ChildName = "Name1 " + DateTime.Now },
                    new Child() { ChildName = "Name2 " + DateTime.Now }
                };

                var addParent = new Parent()
                {
                    Id = Guid.NewGuid(),
                    Singleton = new Singleton() { SingletonName = "Name " + DateTime.Now },
                    Children = children
                };

                context.SaveChanges();

                Debug.WriteLine("Count Before: " + addParent.Children?.Count);
                Debug.WriteLine("Count Before: " + children.Count);

                // Reading the parent seems to double up the children 
                var dummy = context.Parent.ToList();
                Debug.WriteLine("Count Middle: " + addParent.Children?.Count);
                Debug.WriteLine("Count Middle: " + children.Count);

                // 2nd read doesn't change anything
                var dummy2 = context.Parent.ToList();
                Debug.WriteLine("Count After: " + addParent.Children?.Count);
                Debug.WriteLine("Count After: " + children.Count);
            }
        }
    }
}
