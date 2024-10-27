using WaterTracker.Model;
using WaterTracker.Model.DTO;

namespace WaterTracker.Interfaces;

public interface IWaterTrackingService
{
    Task<string> GetTokenAsync();
    Task<GoalDTO> GetGoalAsync();
    Task<GoalDTO> SetGoalAsync(double dailyValue, double weeklyValue);
    Task<List<WaterAmount>> GetAmountListAsync();
    Task<List<WaterUsage>> GetWaterUsage(DateTime startDate, DateTime endDate);
    Task<List<WaterUsage>> GetWaterUsageAll(DateTime startDate);
    Task<Dictionary<string, double>> GetWeeklyWaterUsage(DateTime startDate, DateTime endDate);
    Task<bool> AddUsageAsync(DateTime date, string name, string type, int usedTime);
    Task<double> GetLifetimeWaterUsageAsync();
    void ClearToken();
}