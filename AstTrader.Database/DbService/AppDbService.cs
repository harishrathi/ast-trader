using MongoDB.Driver;

namespace AstTrader.Database.DbSetup;

public class AppDbService : IAppDbService
{
    public IMongoDatabase MongoDatabase { get; }

    public IMongoClient MongoClient
    {
        get
        {
            return MongoDatabase.Client;
        }
    }

    public AppDbService(IMongoDatabase mongoDatabase)
    {
        MongoDatabase = mongoDatabase;
    }

    public void Dispose()
    {
        MongoDatabase.Client.Cluster.Dispose();
    }
}
