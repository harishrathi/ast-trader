using AstTrader.DbSeeder.Utils;

namespace AstTrader.Tests.TestUtils
{
    [TestFixture]
    internal class AppSettingTest
    {
        [Test]
        public void ReadUserSecrets()
        {
            var appSetting = GetAppSettings();
            appSetting.ConnectionStrings.Should().NotBeNull();
            appSetting.ConnectionStrings.MongoDbConnString.Should().NotBeNullOrEmpty();
            appSetting.ConnectionStrings.KiteAuthToken.Should().NotBeNullOrEmpty();
        }

        public static AppSettings GetAppSettings()
        {
            var configRoot = AppConfigUtils.InitConfig();
            configRoot.Should().NotBeNull();

            var appSetting = new AppSettings();
            appSetting.InitConnStrings(configRoot);

            return appSetting;
        }
    }
}
