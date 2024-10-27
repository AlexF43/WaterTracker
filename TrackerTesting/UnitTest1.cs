using NUnit.Framework;
using WaterTracker.Model;

namespace TrackerTesting
{
    [TestFixture]
    public class WaterAmountTests
    {
        [Test]
        public void WaterAmount_HoldsCorrectValues()
        {            
            var waterAmount = new WaterAmount
            {
                usageType = "Shower",
                usageLiterPerSec = 2.5
            };
          
            Assert.That(waterAmount.usageType, Is.EqualTo("Shower"));
            Assert.That(waterAmount.usageLiterPerSec, Is.EqualTo(2.5));
        }
    }
}
