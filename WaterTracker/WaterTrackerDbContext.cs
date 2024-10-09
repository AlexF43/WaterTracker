using Microsoft.EntityFrameworkCore;

namespace WaterTracker;

public class WaterTrackerDbContext : DbContext
{
    public DbSet<WaterTrackerDbContext> ModelClass { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=watertrackerdatabase.db");
}

// todo create model class, replace "modelclass" with class name and run following dotnet commands
// dotnet ef migrations add InitialCreate
// dotnet ef database update