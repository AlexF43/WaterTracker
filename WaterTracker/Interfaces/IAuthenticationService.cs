namespace WaterTracker.Interfaces;

public interface IAuthenticationService
{
    Task<bool> SignUpAsync(string username, string password, string confirmPassword);
    Task<bool> LoginAsync(string username, string password);
    Task LogoutAsync();
    Task<string> GetTokenAsync();
    Task<bool> IsAuthenticatedAsync();
    Task<string> GetHelloMessageAsync();
}