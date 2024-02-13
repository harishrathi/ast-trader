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
            appSetting.ConnectionStrings.MongoDb.Should().NotBeNullOrEmpty();
            appSetting.ConnectionStrings.MongoDbName.Should().NotBeNullOrEmpty();
            appSetting.ConnectionStrings.KiteAuthToken.Should().NotBeNullOrEmpty();
        }

        public static ApplicationSettings GetAppSettings()
        {
            var configRoot = AppConfigUtils.InitConfig();
            configRoot.Should().NotBeNull();

            var appSetting = new ApplicationSettings();
            appSetting.InitConnStrings(configRoot);

            return appSetting;
        }
    }
}
