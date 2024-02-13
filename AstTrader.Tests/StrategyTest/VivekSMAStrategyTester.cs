using AstTrader.DbSeeder.StockCandleDL;
using AstTrader.DbSeeder.StockInstrumentDL;
using AstTrader.DbSeeder.Utils;
using AstTrader.Tests.TestUtils;
using AstTrader.Strategy;
using AstTrader.DbSeeder.StockIndexDL;
using AstTrader.Strategy.VivekSinghal;

namespace AstTrader.Tests.StrategyTest;

[TestFixture]
public class VivekSMAStrategyTester
{
    [Test, Explicit]
    public void Sma_Test_OnV40()
    {
        var appSetting = AppSettingTest.GetAppSettings();

        var mongoDb = new MongoDbConnect(appSetting);
        var database = mongoDb.GetDatabase();

        var indexCollection = database.GetCollection<StockIndex>(StockIndexDbConfig.CollectionName);
        var index = indexCollection.Find(x => x.IndexName == "Vivek_Singhal_V40").FirstOrDefault();
        index.Should().NotBeNull();

        var instrumentRepo = new InstrumentRepo(database);
        foreach (var stockCode in index.Instruments)
        {
            Console.WriteLine($"===================={stockCode}=========================");
            var instrument = instrumentRepo.GetInstrument(stockCode);
            instrument.Should().NotBeNull();

            var stockCollection = database.GetCollection<StockCandle>(instrument.CollectionNameForDailyData());
            var stockCandles = stockCollection.Find(FilterDefinition<StockCandle>.Empty).SortBy(x => x.DateTime).ToList();

            var data = new StockData2(instrument, database);
            for (var i = 0; i < stockCandles.Count; i++)
            {
                data.Candles = stockCandles.GetRange(0, i + 1);
                var runner = new VivekSMAStrategy(data);
                runner.Run();
            }
        }
    }
}
