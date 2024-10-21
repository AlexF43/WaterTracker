using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Security;
using Microsoft.EntityFrameworkCore;

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

[PrimaryKey(nameof(userId))]
public class User
{
    public string userId{ get; set; }
    public string userPwd{ get; set; }
}

[PrimaryKey(nameof(usageId))]
public class WaterUsage
{
    [ForeignKey(nameof(userId))]
    private string usageId{ get; set; }
    private string userId{ get; set; }
    private string usageName{ get; set; }
    private DateTime date{ get; set; }
    
    [ForeignKey(nameof(usageType))]
    private string usageType{ get; set; }
    private int usedSec{ get; set; }
    private double totalUsage{ get; set; }
}

[PrimaryKey(nameof(usageType))]
public class WaterAmount
{
    private string usageType{ get; set; }
    private double usageLiterPerSec { get; set; }
}

// todo create model class, replace "modelclass" with class name and run following dotnet commands
// dotnet ef migrations add InitialCreate
// dotnet ef database update