using NUnit.Framework;
using WaterTracker.Controller;
using WaterTracker.Model;
using WaterTracker.Model.DTO.WaterTrackingDTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using WaterTracker.Model.DTO;
using WaterTracker;

namespace TrackerTesting
{
    [TestFixture]
    public class WaterTrackingTests
    {
        private WaterTrackerDbContext _context;
        private WaterTrackingController _controller;
        private const string TEST_USER_ID = "test-user-id";

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<WaterTrackerDbContext>()
                .UseInMemoryDatabase(databaseName: "WaterTrackerTest")
                .Options;

            _context = new WaterTrackerDbContext(options);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _controller = new WaterTrackingController(_context);
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, TEST_USER_ID),
        new Claim(ClaimTypes.Name, "testuser")
    };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            SeedTestData();
        }

        [TearDown]
        public void TearDown()
        {
            _context?.Dispose();
        }

        private void SeedTestData()
        {
            _context.WaterAmounts.AddRange(
                new WaterAmount { usageType = "shower", usageLiterPerSec = 0.2 },
                new WaterAmount { usageType = "sink", usageLiterPerSec = 0.1 }
            );
            
            _context.Users.Add(new User
            {
                userId = TEST_USER_ID,
                userName = "testuser",
                dailyGoal = 2.5,
                weeklyGoal = 17.5
            });

            _context.SaveChanges();
        }

        [Test]
        public async Task LogWater_CalculatesTotalUsageCorrectly()
        {
            var request = new CreateWaterUseageRequest
            {
                UsageName = "Test Shower",
                UsageType = "Shower",
                UsedSeconds = 300
            };

            var result = await _controller.LogWater(request);
            var waterUsage = await _context.WaterUsages
                .FirstOrDefaultAsync(w => w.userId == TEST_USER_ID);

            Assert.That(result, Is.InstanceOf<OkResult>());
            Assert.That(waterUsage, Is.Not.Null);
            Assert.That(waterUsage.totalUsage, Is.EqualTo(60.0));
        }


        [Test]
        public async Task MultipleWaterUsages_CalculatesTotalCorrectly()
        {
            var shower = new CreateWaterUseageRequest
            {
                UsageName = "Morning Shower",
                UsageType = "Shower",
                UsedSeconds = 300
            };

            var sink = new CreateWaterUseageRequest
            {
                UsageName = "Washing Dishes",
                UsageType = "tap",
                UsedSeconds = 600
            };

            await _controller.LogWater(shower);
            await _controller.LogWater(sink);

            var result = await _controller.GetWaterUsage(DateTime.UtcNow.Date, DateTime.UtcNow.Date);
            var okResult = result as OkObjectResult;
            var usages = okResult?.Value as List<UseageListResponse>;

            Assert.That(usages?.Count, Is.EqualTo(2));
            Assert.That(usages?.Sum(u => u.TotalUsage), Is.EqualTo(120.0));
        }

        [Test]
        public async Task SetGoals_UpdatesGoalsCorrectly()
        {
            var newGoals = new GoalDTO
            {
                DailyGoal = 3.0,
                WeeklyGoal = 21.0
            };

            var result = await _controller.SetGoals(newGoals);
            var updatedUser = await _context.Users.FindAsync(TEST_USER_ID);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(updatedUser?.dailyGoal, Is.EqualTo(3.0));
            Assert.That(updatedUser?.weeklyGoal, Is.EqualTo(21.0));
        }
    }
}
