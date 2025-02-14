namespace WaterTracker.Model.DTO.WaterTrackingDTO;

public class CreateWaterUseageRequest
{
    public DateTime Time { get; set; }
    public string UsageName { get; set; }
    public string UsageType { get; set; }
    public int UsedSeconds { get; set; }
}