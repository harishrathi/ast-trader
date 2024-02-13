using AstTrader.Database.DbSetup;
using AstTrader.DbSeeder.Utils;
using MongoDB.Driver;

namespace AstTrader.Server.AppServices;

public static class MongoDbConfigurator
{
    public static MongoClient AddDbService(this IServiceCollection services, AppConnectionStrings connStrings)
    {
        var mongoUrlBuilder = new MongoUrlBuilder(connStrings.MongoDbConnString);
        var mongoUrl = mongoUrlBuilder.ToMongoUrl();
        var mongoClientSettings = MongoClientSettings.FromUrl(mongoUrl);
        mongoClientSettings.ApplicationName = mongoUrl.DatabaseName;
        var mongoClient = new MongoClient(mongoClientSettings);
        var mongoDatabase = mongoClient.GetDatabase(mongoUrl.DatabaseName);

        // services.AddSingleton(mongoDatabase);
        // services.AddSingleton<IMongoClient>(mongoClient);
        services.AddSingleton<IAppDbService>(new AppDbService(mongoDatabase));

        return mongoClient;
    }
}
