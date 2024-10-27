using WaterTracker.Model;

namespace WaterTracker.Interfaces;

public interface IJWTService
{
    string GenerateToken(User user);
}