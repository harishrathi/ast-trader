using Microsoft.Extensions.Configuration;

namespace AstTrader.Tests.TestUtils
{
    internal static class AppConfigUtils
    {
        public static IConfigurationRoot ConfigRoot { get; private set; } = null!;

        public static IConfigurationRoot InitConfig()
        {
            ConfigRoot = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", false)
               .AddUserSecrets(Assembly.GetExecutingAssembly(), false)
               .Build();

            return ConfigRoot;
        }

    }
}
