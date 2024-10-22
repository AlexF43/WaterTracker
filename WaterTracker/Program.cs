using System.Net;
using System.Security;
using WaterTracker.Components;
using Microsoft.EntityFrameworkCore;
using WaterTracker;
using WaterTracker.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add this block for Entity Framework and SQLite
builder.Services.AddDbContext<WaterTrackerDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();

app.MapControllers(); 

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<WaterTrackerDbContext>();
    context.Database.EnsureCreated();

    void addUser(string id, string pwd)
    {
        var user = new User{userId = id, userPwd = pwd};
    }
}

app.Run();
