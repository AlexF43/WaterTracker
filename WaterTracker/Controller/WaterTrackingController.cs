using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaterTracker.Model;
using WaterTracker.Model.DTO;
using WaterTracker.Model.DTO.WaterTrackingDTO;

namespace WaterTracker.Controller;


[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WaterTrackingController : ControllerBase
{
    private readonly WaterTrackerDbContext _context;

    public WaterTrackingController(WaterTrackerDbContext context)
    {
        _context = context;
    }

    private string GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    
    [HttpPost]
    public async Task<IActionResult> LogWater([FromBody] CreateWaterUseageRequest request)
    {
        try
        {
            var userId = GetUserId();
            var waterAmount = await _context.WaterAmounts.FindAsync(request.UsageType);
            
            if (waterAmount == null)
                return BadRequest("Invalid usage type");

            var waterUsage = new WaterUsage
            {
                usageId = Guid.NewGuid().ToString(),
                userId = userId,
                usageName = request.UsageName,
                date = DateTime.UtcNow,
                usageType = request.UsageType,
                usedSec = request.UsedSeconds,
                totalUsage = request.UsedSeconds * waterAmount.usageLiterPerSec
            };

            _context.WaterUsages.Add(waterUsage);
            await _context.SaveChangesAsync();

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while creating water usage");
        }
        
        
    }
    
    [HttpGet("amounts")]
    public async Task<IActionResult> GetAmounts()
    {
        try
        {
            var query = _context.WaterAmounts;
            List<UsageAmountResponse> responseList = new List<UsageAmountResponse>();
            foreach (var item in query)
            {
                responseList.Add(new UsageAmountResponse{UsageType = item.usageType, UsageLiterPerSec = item.usageLiterPerSec});
            }
            return Ok(responseList);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An internal error occurred while fetching usage amounts");
        }
    }
    
    [HttpGet("goals")]
    public async Task<IActionResult> GetGoals()
    {
        try
        {
            var userId = GetUserId();
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return NotFound("User not found");

            var response = new GoalDTO
            {
                DailyGoal = user.dailyGoal,
                WeeklyGoal = user.weeklyGoal
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while fetching goals");
        }
    }

    [HttpPost("goals")]
    public async Task<IActionResult> SetGoals([FromBody] GoalDTO request)
    {
        try
        {
            var userId = GetUserId();
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return NotFound("User not found");

            user.dailyGoal = request.DailyGoal;
            user.weeklyGoal = request.WeeklyGoal;

            await _context.SaveChangesAsync();
            return Ok(new GoalDTO
            {
                DailyGoal = user.dailyGoal,
                WeeklyGoal = user.weeklyGoal
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while setting goals");
        }
    }

    [HttpGet("usage")]
    public async Task<IActionResult> GetWaterUsage([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        try
        {
            var userId = GetUserId();
            var query = _context.WaterUsages
                .Where(w => w.userId == userId);
            
            if (startDate.HasValue)
                query = query.Where(w => w.date.Date >= startDate.Value.Date);
            
            if (endDate.HasValue)
                query = query.Where(w => w.date.Date <= endDate.Value.Date);
            
            var usages = await query
                .OrderByDescending(w => w.date)
                .Select(w => new UseageListResponse
                {
                    UsageId = w.usageId,
                    UsageName = w.usageName,
                    UsageType = w.usageType,
                    Date = w.date,
                    UsedSeconds = w.usedSec,
                    TotalUsage = w.totalUsage
                })
                .ToListAsync();

            return Ok(usages);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while fetching water usage");
        }
    }
}