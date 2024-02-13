using Microsoft.Extensions.Configuration;

namespace AstTrader.DbSeeder.Utils;

public class AppConnectionStrings
{
    public AppConnectionStrings(ConfigurationManager config)
    {
        config.GetSection("ConnectionStrings").Bind(this);
    }

    public string MongoDbConnString { get; set; } = string.Empty;
    public string KiteAuthToken { get; set; } = string.Empty;
}
