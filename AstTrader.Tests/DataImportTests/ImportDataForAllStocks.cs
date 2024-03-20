using AstTrader.DbSeeder.StockCandleDL;
using AstTrader.DbSeeder.StockInstrumentDL;
using AstTrader.DbSeeder.Utils;
using AstTrader.DbSeeder.Zerodha;
using AstTrader.Tests.TestUtils;

namespace AstTrader.Tests.DataImportTests
{
    [TestFixture]
    public class ImportDataForAllStocks
    {
        [Test, Explicit]
        public void GetAllStockInstruments()
        {
            var appSetting = GlobalTestUtils.GetAppConnectionString();

            var mongoDb = new MongoDbConnect(appSetting);
            var database = mongoDb.GetDatabase();

            var instrumentRepo = new InstrumentRepo(database);
            var instruments = instrumentRepo.GetAllStockInstruments();

            instruments.Count.Should().BeGreaterThan(0);
        }

        [Test, Explicit]
        public void GetDataForAllStocks()
        {
            var appSetting = GlobalTestUtils.GetAppConnectionString();

            var mongoDb = new MongoDbConnect(appSetting);
            var database = mongoDb.GetDatabase();

            var instrumentRepo = new InstrumentRepo(database);
            var instruments = instrumentRepo.GetAllStockInstruments();

            var remaining = instruments.Where(x => x.DataFetchedOn == DateTime.MinValue).ToList();
            foreach (var instr in remaining)
            {
                TestContext.Progress.WriteLine($"Getting data for => {instr.Exchange}:{instr.Symbol}, Count: {remaining.IndexOf(instr)} of {remaining.Count}");
                database.CreateCollection(instr.CollectionNameForDailyData());

                var wrapper = new KiteWrapper(appSetting);
                var stockCandleCollection = database.GetCollection<StockCandle>(instr.CollectionNameForDailyData());

                var candlesList = new List<StockCandle>();
                var dates = StockCandleExtensions.NseDateRanges;
                for (int i = 0; i + 1 < StockCandleExtensions.NseDateRanges.Count; i++)
                {
                    var data = wrapper.GetData4Daily(instr, dates[i], dates[i + 1].AddDays(-1));
                    candlesList.AddRange(data.OrderBy(x => x.TimeStamp).Select(x => x.ToEodCandle()));
                }

                if (candlesList.Any())
                {
                    var records = candlesList.Select(x => new InsertOneModel<StockCandle>(x)).ToList();
                    stockCandleCollection.BulkWrite(records);
                }

                instr.UpdateForData(candlesList);
                var instrumentCollection = database.GetCollection<Instrument>(InstrumentDbConfig.CollectionName);
                var filter = Builders<Instrument>.Filter.Eq(x => x.Id, instr.Id);
                instrumentCollection.ReplaceOne(filter, instr);

                Thread.Sleep(100);
            }
        }
    }
}
