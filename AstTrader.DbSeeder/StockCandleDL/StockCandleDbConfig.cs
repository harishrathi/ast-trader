using AstTrader.DbSeeder.Utils;

using MongoDB.Driver;

namespace AstTrader.DbSeeder.StockCandleDL
{
    internal class StockCandleDbConfig : CollectionConfigurator<StockCandle>
    {
        public static string CollectionName => throw new NotImplementedException();
        private readonly string _collectionName;

        public StockCandleDbConfig(IMongoDatabase db, string name)
            : base(db)
        {
            _collectionName = name;
        }

        public override void ConfigureCollection()
        {
            var collection = Database.GetCollection<StockCandle>(_collectionName);

            //index-name should be unique
            var keys = Builders<StockCandle>.IndexKeys.Ascending(x => x.DateTime);
            var model = new CreateIndexModel<StockCandle>(keys, new CreateIndexOptions { Unique = true });
            collection.Indexes.CreateOne(model);
        }
    }
}
