using AstTrader.DbSeeder.StockCandleDL;
using AstTrader.DbSeeder.StockInstrumentDL;
using AstTrader.DbSeeder.Utils;
using AstTrader.Tests.TestUtils;
using AstTrader.Strategy;
using AstTrader.DbSeeder.StockIndexDL;
using AstTrader.Strategy.VivekSinghal;

namespace AstTrader.Tests.StrategyTest;

[TestFixture]
public class VivekV20StragegyTester
{
    [Test, Explicit]
    public void V20_Gillete_Test()
    {
        var appSetting = AppSettingTest.GetAppSettings();

        var mongoDb = new MongoDbConnect(appSetting);
        var database = mongoDb.GetDatabase();

        var instrumentRepo = new InstrumentRepo(database);
        var instrument = instrumentRepo.GetInstrument("NSE:GILLETTE");
        instrument.Should().NotBeNull();

        var stockCollection = database.GetCollection<StockCandle>(instrument.CollectionNameForDailyData());
        var stockCandles = stockCollection
            .Find(x => x.DateTime > new DateTime(2020, 03, 15))
            .SortBy(x => x.DateTime)
            .ToList();

        var data = new StockData(instrument);
        for (var i = 0; i < stockCandles.Count; i++)
        {
            data.Candles = stockCandles.GetRange(0, i + 1);
            var runner = new VivekV20Strategy(data);
            runner.Run();
        }
    }

    [Test, Explicit]
    public void V20_Test_OnV40()
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
            var stockCandles = stockCollection.Find(x => x.DateTime > DateTime.Parse("2021-01-01")).SortBy(x => x.DateTime).ToList();

            var data = new StockData(instrument);
            for (var i = 0; i < stockCandles.Count; i++)
            {
                data.Candles = stockCandles.GetRange(0, i + 1);
                var runner = new VivekV20Strategy(data);
                runner.Run();
            }
        }
    }
}
