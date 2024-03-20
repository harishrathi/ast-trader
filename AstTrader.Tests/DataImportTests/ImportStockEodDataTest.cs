using AstTrader.DbSeeder.StockCandleDL;
using AstTrader.DbSeeder.StockInstrumentDL;
using AstTrader.DbSeeder.Utils;
using AstTrader.DbSeeder.Zerodha;
using AstTrader.Tests.TestUtils;

namespace AstTrader.Tests.DataImportTests
{
    [TestFixture]
    public class ImportStockEodDataTest
    {
        public static string Symbol => "NSE:JFC";

        [Test, Explicit]
        public void CreateCollection()
        {
            var appSetting = GlobalTestUtils.GetAppConnectionString();

            var mongoDb = new MongoDbConnect(appSetting);
            var database = mongoDb.GetDatabase();

            var instrumentRepo = new InstrumentRepo(database);
            var instrument = instrumentRepo.GetInstrument(Symbol);

            database.CreateCollection(instrument.CollectionNameForDailyData());
        }

        [Test, Explicit]
        public void FetchData()
        {
            var appSetting = GlobalTestUtils.GetAppConnectionString();

            var mongoDb = new MongoDbConnect(appSetting);
            var database = mongoDb.GetDatabase();

            var instrumentRepo = new InstrumentRepo(database);
            var instrument = instrumentRepo.GetInstrument(Symbol);

            var wrapper = new KiteWrapper(appSetting);
            var stockCandleCollection = database.GetCollection<StockCandle>(instrument.CollectionNameForDailyData());

            var candlesList = new List<StockCandle>();
            var dates = StockCandleExtensions.NseDateRanges;
            for (int i = 0; i + 1 < StockCandleExtensions.NseDateRanges.Count; i++)
            {
                var data = wrapper.GetData4Daily(instrument, dates[i], dates[i + 1].AddDays(-1));
                candlesList.AddRange(data.OrderBy(x => x.TimeStamp).Select(x => x.ToEodCandle()));
            }

            var records = candlesList.Select(x => new InsertOneModel<StockCandle>(x)).ToList();
            stockCandleCollection.BulkWrite(records);

            instrument.UpdateForData(candlesList);
            var instrumentCollection = database.GetCollection<Instrument>(InstrumentDbConfig.CollectionName);
            var filter = Builders<Instrument>.Filter.Eq(x => x.Id, instrument.Id);
            instrumentCollection.ReplaceOne(filter, instrument);
        }
    }
}
