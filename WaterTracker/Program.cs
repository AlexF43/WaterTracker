using System.Net;
using System.Security;
using WaterTracker.Components;
using Microsoft.EntityFrameworkCore;
using WaterTracker;
using WaterTracker.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<WaterTrackerDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add CORS to allow the frontend to communicate with the API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddControllers(); // Make sure this is here
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

var app = builder.Build();

// In the middleware section, make sure these are in this order:
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAll"); // Add this line
app.UseAntiforgery();

// Make sure these two lines are present and in this order:
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();



using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<WaterTrackerDbContext>();
    context.Database.EnsureCreated();
}

app.Run();
