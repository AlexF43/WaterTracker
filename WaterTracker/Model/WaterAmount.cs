using Microsoft.EntityFrameworkCore;

namespace WaterTracker.Model;

[PrimaryKey(nameof(usageType))]
public class WaterAmount
{
    public string usageType{ get; set; }
    public double usageLiterPerSec { get; set; }
}