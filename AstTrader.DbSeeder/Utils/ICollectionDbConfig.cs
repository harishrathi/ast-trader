using MongoDB.Driver;

namespace AstTrader.DbSeeder.Utils
{
    public interface ICollectionDbConfig<T>
        where T : ICollectionBase
    {
        public IMongoDatabase Database { get; }
        public abstract void CreateCollection(string name);
        public abstract void ConfigureCollection();
    }

    public abstract class CollectionConfigurator<T> : ICollectionDbConfig<T>
        where T : ICollectionBase
    {
        public IMongoDatabase Database { get; private set; }

        public CollectionConfigurator(IMongoDatabase db)
        {
            Database = db;
        }

        public void RecreateCollection(string name)
        {
            DropCollection(name);
            CreateCollection(name);
        }

        public void CreateCollection(string name)
        {
            var collectionNames = Database.ListCollectionNames().ToList();
            if (!collectionNames.Any(x => x == name))
            {
                Database.CreateCollection(name);
            }
        }

        public void DropCollection(string name)
        {
            var collectionNames = Database.ListCollectionNames().ToList();
            if (collectionNames.Any(x => x == name))
            {
                Database.DropCollection(name);
            }
        }

        public abstract void ConfigureCollection();
    }

}
