using Microsoft.Extensions.Configuration;
using TradeAstra.Core.Config;

namespace AstTrader.Tests;

internal static class GlobalTestUtils
{
    public static ConfigurationManager GetDefaultAppSettings()
    {
        var root = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", false)
           .AddJsonFile("appsettings.UserSecrets.json", true)
           .Build();

        return (ConfigurationManager)root;
    }

    public static AppConnectionString GetAppConnectionString()
    {
        var configManager = GetDefaultAppSettings();
        return AppConnectionString.Init(configManager);
    }
}
