using System.Net;
using System.Security;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WaterTracker.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WaterTracker;
using WaterTracker.Model;
using WaterTracker.Services;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<WaterTrackerDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<JWTService>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<WaterTrackingService>();
builder.Services.AddHttpClient();

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN";
    options.Cookie.Name = "XSRF-TOKEN";
    options.Cookie.SameSite = SameSiteMode.Strict;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var antiforgery = context.RequestServices.GetRequiredService<IAntiforgery>();
    var tokens = antiforgery.GetAndStoreTokens(context);
    context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, 
        new CookieOptions { HttpOnly = false });
    await next(context);
});

app.UseAntiforgery();

app.MapControllers().WithMetadata(new IgnoreAntiforgeryTokenAttribute());
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<WaterTrackerDbContext>();

    context.WaterAmounts.ExecuteDelete();
    //Example Data Add
    List<WaterAmount> waterAmounts = new List<WaterAmount>()
    {
        new WaterAmount { usageType = "Shower", usageLiterPerSec = 0.2},
        new WaterAmount { usageType = "Toilet Flush - Half", usageLiterPerSec = 4.5},
        new WaterAmount { usageType = "Toilet Flush - Full", usageLiterPerSec = 9},
        new WaterAmount {usageType = "Dishwashing - Hand", usageLiterPerSec = 0.25},
        new WaterAmount {usageType = "Dishwashing - Machine", usageLiterPerSec = 18},
        new WaterAmount {usageType = "Washing Machine", usageLiterPerSec = 72},
        new WaterAmount {usageType ="tap", usageLiterPerSec = 0.25}
    };
    foreach (var amount in waterAmounts)
    {
        context.WaterAmounts.Add(amount);
    }
    context.Database.EnsureCreated();
    context.SaveChanges();
}

app.Run();