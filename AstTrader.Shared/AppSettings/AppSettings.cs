using Microsoft.Extensions.Configuration;

namespace AstTrader.DbSeeder.Utils;

public class AppSettings
{
    public AppConnectionStrings ConnectionStrings { get; set; } = null!;

    public void InitConnStrings(ConfigurationManager config)
    {
        ConnectionStrings = new AppConnectionStrings(config);
    }
}
