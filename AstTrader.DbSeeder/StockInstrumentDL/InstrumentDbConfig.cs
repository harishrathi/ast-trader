using AstTrader.DbSeeder.Utils;

using MongoDB.Driver;

namespace AstTrader.DbSeeder.StockInstrumentDL
{
    public class InstrumentDbConfig : CollectionConfigurator<Instrument>
    {
        public static string CollectionName => "Instruments";

        public InstrumentDbConfig(IMongoDatabase db)
            : base(db)
        {
        }

        public override void ConfigureCollection()
        {
            var collection = Database.GetCollection<Instrument>(CollectionName);

            //exchange-symbol combination shoudl be unique (also as per zerodha)
            var keys = Builders<Instrument>.IndexKeys.Ascending(x => x.Exchange).Ascending(x => x.Symbol);
            var model = new CreateIndexModel<Instrument>(keys, new CreateIndexOptions { Unique = true });
            collection.Indexes.CreateOne(model);
        }
    }
}
