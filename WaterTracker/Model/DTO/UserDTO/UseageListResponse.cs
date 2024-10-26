namespace WaterTracker.Model.DTO;

public class UseageListResponse
{
    public string UsageId { get; set; }
    public string UsageName { get; set; }
    public string UsageType { get; set; }
    public DateTime Date { get; set; }
    public int UsedSeconds { get; set; }
    public double TotalUsage { get; set; }
}