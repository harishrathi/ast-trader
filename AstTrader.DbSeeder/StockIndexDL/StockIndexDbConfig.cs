using AstTrader.DbSeeder.Utils;

using MongoDB.Driver;

namespace AstTrader.DbSeeder.StockIndexDL
{
    public class StockIndexDbConfig : CollectionConfigurator<StockIndex>
    {
        public static string CollectionName => "StockIndex";

        public StockIndexDbConfig(IMongoDatabase db)
            : base(db)
        {
        }

        public override void ConfigureCollection()
        {
            var collection = Database.GetCollection<StockIndex>(CollectionName);

            //index-name should be unique
            var keys = Builders<StockIndex>.IndexKeys.Ascending(x => x.IndexName);
            var model = new CreateIndexModel<StockIndex>(keys, new CreateIndexOptions { Unique = true });
            collection.Indexes.CreateOne(model);
        }
    }
}
