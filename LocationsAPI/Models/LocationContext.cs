using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LocationsAPI.Models
{
    public class LocationContext : DbContext
    {
        public LocationContext(DbContextOptions<LocationContext> options) : base(options) { }
        public DbSet<Location> Locations { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
    }
}
