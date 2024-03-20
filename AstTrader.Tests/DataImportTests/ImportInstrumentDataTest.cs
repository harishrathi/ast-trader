using AstTrader.DbSeeder.StockInstrumentDL;
using AstTrader.DbSeeder.Utils;
using AstTrader.DbSeeder.Zerodha;
using AstTrader.Tests.TestUtils;

namespace AstTrader.Tests.DataImportTests
{
    [TestFixture]
    public class ImportInstrumentDataTest
    {
        [Test, Explicit]
        public void ImportInstruments()
        {
            var appSetting = GlobalTestUtils.GetAppConnectionString();

            var mongoDb = new MongoDbConnect(appSetting);
            var database = mongoDb.GetDatabase();
            var seeder = new InstrumentDbConfig(database);
            seeder.CreateCollection(InstrumentDbConfig.CollectionName);

            var wrapper = new KiteWrapper(appSetting);
            var kiteInstruments = wrapper.GetSymbolsForNSE();

            var records = kiteInstruments
                .Where(x => !string.IsNullOrEmpty(x.Name))
                .Select(x => x.ToTbInstrument())
                .Select(x => new InsertOneModel<Instrument>(x))
                .ToList();

            var collection = database.GetCollection<Instrument>(InstrumentDbConfig.CollectionName);
            collection.BulkWrite(records);
        }
    }
}
