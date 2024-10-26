using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WaterTracker.Model;

[PrimaryKey(nameof(usageId))]
public class WaterUsage
{
    [ForeignKey(nameof(userId))] public string usageId{ get; set; }
    public string userId{ get; set; }
    public string usageName{ get; set; }
    public DateTime date{ get; set; }
    
    [ForeignKey(nameof(usageType))]
    public string usageType{ get; set; }
    public int usedSec{ get; set; }
    public double totalUsage{ get; set; }
}