using Microsoft.EntityFrameworkCore;

namespace WaterTracker.Model;

[PrimaryKey(nameof(userId))]
public class User
{
    public string userId{ get; set; }
    public string userName { get; set; }
    public string userPwd{ get; set; }
}