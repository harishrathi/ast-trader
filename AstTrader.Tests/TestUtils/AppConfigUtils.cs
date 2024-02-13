using Microsoft.Extensions.Configuration;

namespace AstTrader.Tests.TestUtils
{
    internal static class AppConfigUtils
    {
        public static ConfigurationManager InitConfig()
        {
            var root = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", false)
               .AddUserSecrets(Assembly.GetExecutingAssembly(), false)
               .Build();

            return (ConfigurationManager)root;
        }
    }
}
