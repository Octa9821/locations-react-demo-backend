using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LocationsAPI.Models
{
    public class LocationContext : DbContext
    {
        public LocationContext(DbContextOptions<LocationContext> options) : base(options) { }
        public DbSet<Location> Locations { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<Location>().ToTable("Location");
            modelBuilder.Entity<User>().ToTable("User");
        }
    }
}
