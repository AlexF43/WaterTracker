using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WaterTracker.Model;

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