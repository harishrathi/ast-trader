using Microsoft.Extensions.Configuration;

namespace TradeAstra.Core.Config;

public class AppConnectionString
{
    private readonly IConfigurationRoot root;
    private AppConnectionString(IConfigurationRoot root)
    {
        this.root = root;
    }

    public string MongoDbConnString => root.GetConnectionString("MongoDbConnString") ?? throw new NullReferenceException("MongoDbConnString");
    public string KiteAuthToken => root.GetConnectionString("KiteAuthToken") ?? throw new NullReferenceException("KiteAuthToken");
    public string SqlDbConnString => root.GetConnectionString("SqlDbConnString") ?? throw new NullReferenceException("SqlDbConnString");

    public static AppConnectionString Init(IConfigurationRoot root) => new AppConnectionString(root);
}
