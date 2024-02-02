using AstTrader.DbSeeder.Utils;

namespace AstTrader.Tests.TestUtils
{
    [TestFixture]
    public class SystemExtensionTests
    {
        [Test]
        public void CalculateCagrTest1()
        {
            var result = SystemExtensions.CaculateCAGR(10, 100);
            result.Should().BeInRange(30, 40);
        }

        [Test]
        public void CalculateCagrTest2()
        {
            var result = SystemExtensions.CaculateCAGR(10, 700);
            result.Should().BeInRange(2, 10);
        }
    }
}
