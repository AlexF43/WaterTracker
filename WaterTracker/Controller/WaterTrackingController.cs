using Microsoft.AspNetCore.Mvc;
using WaterTracker.Services;

namespace WaterTracker.Controller;

[ApiController]
[Route("api/[controller]")]

public class WaterTrackingController : ControllerBase
{
    private readonly WaterTrackerDbContext _context;
    private readonly JWTService _jwtService;
    
    public WaterTrackingController(WaterTrackerDbContext context, JWTService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }
    
    
}