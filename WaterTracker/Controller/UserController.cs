using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaterTracker.Model;
using WaterTracker.Model.DTO;
using WaterTracker.Services;

namespace WaterTracker.Controller;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly WaterTrackerDbContext _context;
    private readonly JWTService _jwtService;

    public UserController(WaterTrackerDbContext context, JWTService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("signup")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Signup([FromBody] SignupRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                return BadRequest("Username and password are required");

            if (request.Password != request.ConfirmPassword)
                return BadRequest("Passwords do not match");
            
            if (_context.Users.Any(u => u.userName == request.Username))
                return BadRequest("Username already exists");
            
            var user = new User
            {
                userId = Guid.NewGuid().ToString(),
                userName = request.Username,
                userPwd = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = _jwtService.GenerateToken(user);
            return Ok(new { token });
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An unknown error occurred");
        }
    }

    [HttpPost("login")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var user = _context.Users.FirstOrDefault(u => u.userName == request.Username);
            
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.userPwd))
                return Unauthorized("Invalid username or password");

            var token = _jwtService.GenerateToken(user);
            return Ok(new { token });
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An unknown error occurred");
        }
    }

    [Authorize]
    [HttpGet("hello")]
    public IActionResult Hello()
    {   
        var username = User.FindFirst(ClaimTypes.Name)?.Value;
        return Ok($"Hello {username}!");
    }
}