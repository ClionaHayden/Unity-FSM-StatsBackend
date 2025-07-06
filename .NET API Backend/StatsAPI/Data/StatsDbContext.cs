using Microsoft.EntityFrameworkCore;
using StatsAPI.Models;

// DbContext class to manage database connection and entity sets
public class StatsDbContext : DbContext 
{
    // Constructor accepting options (like connection string) passed from Startup.cs
    public StatsDbContext(DbContextOptions<StatsDbContext> options) : base(options) { }

    // DbSet representing the PlayerStats table in the database
    public DbSet<PlayerStats> PlayerStats  { get; set; }
}
