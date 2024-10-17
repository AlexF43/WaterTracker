using System.Net;
using System.Security;
using Microsoft.EntityFrameworkCore;

namespace WaterTracker;

public class WaterTrackerDbContext : DbContext
{
    public DbSet<WaterTrackerDbContext> ModelClass { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=watertrackerdatabase.db");
}

class User
{
    private string userId;

    private SecureString userPwd;
    User(string userId, string pwd)
    {
        this.userId = userId;
        userPwd = new NetworkCredential("", pwd).SecurePassword;
    }

}

class WaterUsage
{
    public WaterUsage(string usageId, string userId, string usageName, DateTime date, string usageType, int usedSec, double totalUsage)
    {
        this.usageId = usageId;
        this.userId = userId;
        this.usageName = usageName;
        this.date = date;
        this.usageType = usageType;
        this.usedSec = usedSec;
        this.totalUsage = totalUsage;
    }

    private string usageId;
    private string userId;
    private string usageName;
    private DateTime date;
    private string usageType;
    private int usedSec;
    private double totalUsage;
}

class WaterAmount
{
    private string usageType;
    private double usageLiterPerSec;
    public WaterAmount(string usageType, double usageLiterPerSec)
    {
        this.usageType = usageType;
        this.usageLiterPerSec = usageLiterPerSec;
    }
}

// todo create model class, replace "modelclass" with class name and run following dotnet commands
// dotnet ef migrations add InitialCreate
// dotnet ef database update