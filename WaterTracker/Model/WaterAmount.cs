using Microsoft.EntityFrameworkCore;

namespace WaterTracker.Model;

[PrimaryKey(nameof(usageType))]
public class WaterAmount
{
    private string usageType{ get; set; }
    private double usageLiterPerSec { get; set; }
}