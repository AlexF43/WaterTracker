using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Security;
using Microsoft.EntityFrameworkCore;
using WaterTracker.Model;

namespace WaterTracker;

public class WaterTrackerDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<WaterUsage> WaterUsages { get; set; }
    public DbSet<WaterAmount> WaterAmounts { get; set; }

    public WaterTrackerDbContext(DbContextOptions<WaterTrackerDbContext> options) : base(options)
    {
        
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=watertrackerdatabase.db");
    
}
