namespace WaterTracker.Model.DTO;

public class SignupRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}