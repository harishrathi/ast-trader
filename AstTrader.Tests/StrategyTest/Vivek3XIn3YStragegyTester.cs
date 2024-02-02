using AstTrader.DbSeeder.StockCandleDL;
using AstTrader.DbSeeder.StockInstrumentDL;
using AstTrader.DbSeeder.Utils;
using AstTrader.Tests.TestUtils;
using AstTrader.Strategy;
using AstTrader.DbSeeder.StockIndexDL;
using AstTrader.Strategy.VivekSinghal;

namespace AstTrader.Tests.StrategyTest;

[TestFixture]
public class Vivek3XIn3YStragegyTester
{
    [Test, Explicit]
    public void Test_3XIn3Y_OnV40()
    {
        var appSetting = AppSettingTest.GetAppSettings();

        var mongoDb = new MongoDbConnect(appSetting);
        var database = mongoDb.GetDatabase();

        var indexCollection = database.GetCollection<StockIndex>(StockIndexDbConfig.CollectionName);
        var index = indexCollection.Find(x => x.IndexName == "NSE500").FirstOrDefault();
        index.Should().NotBeNull();

        var instrumentRepo = new InstrumentRepo(database);

        foreach (var stockCode in index.Instruments)
        {
            Console.WriteLine($"===================={stockCode}=========================");
            var instrument = instrumentRepo.GetInstrument(stockCode);
            instrument.Should().NotBeNull();

            var stockCollection = database.GetCollection<StockCandle>(instrument.CollectionNameForDailyData());
            var stockCandles = stockCollection.Find(FilterDefinition<StockCandle>.Empty).SortBy(x => x.DateTime).ToList();

            var data = new StockData3(instrument, database);
            for (var i = 0; i < stockCandles.Count; i++)
            {
                data.Candles = stockCandles.GetRange(0, i + 1);
                var runner = new Vivek3XIn3YearsStrategy(data);
                runner.Run();
            }
        }
    }
}
