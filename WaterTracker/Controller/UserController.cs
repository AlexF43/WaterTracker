using Microsoft.AspNetCore.Mvc;
using WaterTracker.Model;
using WaterTracker.Model.DTO;

namespace WaterTracker.Controller;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly WaterTrackerDbContext _context;

    public UserController(WaterTrackerDbContext context)
    {
        _context = context;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] SignupRequest request)
    {
        try
        {
            Console.WriteLine("in signup api");
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

            return Ok("User sucessfully registered");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An unknown errror occurred");
        }
    }
    
    [HttpGet("hello")]
    public IActionResult Hello()
    {   
        Console.WriteLine("Hello endpoint");
        return Ok("Hello World!");
    }
}