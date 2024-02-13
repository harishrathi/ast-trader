using MongoDB.Driver;

namespace AstTrader.Database.DbSetup;

public interface IAppDbService : IDisposable
{
    public IMongoDatabase MongoDatabase { get; }
}
