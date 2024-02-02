using Microsoft.Extensions.Configuration;

namespace AstTrader.DbSeeder.Utils
{
    public class ApplicationSettings
    {
        public class ConnStrings
        {
            public string MongoDb { get; set; } = string.Empty;
            public string MongoDbName { get; set; } = string.Empty;
            public string KiteAuthToken { get; set; } = string.Empty;
        }

        public ConnStrings ConnectionStrings { get; set; } = null!;

        public void InitConnStrings(IConfigurationRoot root)
        {
            ConnectionStrings = root.GetSection("ConnectionStrings").Get<ConnStrings>() ?? throw new NullReferenceException();
        }
    }
}
