using AstTrader.DbSeeder.StockCandleDL;
using AstTrader.DbSeeder.StockInstrumentDL;
using AstTrader.DbSeeder.Utils;
using AstTrader.DbSeeder.Zerodha;
using AstTrader.Tests.TestUtils;

namespace AstTrader.Tests.DataImportTests;

[TestFixture]
public class UpdateStockEodDataTest
{
    [Test, Explicit]
    public void UpdateDataDailyEodTest()
    {
        var appSetting = AppSettingTest.GetAppSettings();

        Console.WriteLine(appSetting.ConnectionStrings.KiteAuthToken);

        var kite = new KiteWrapper(appSetting);

        var mongoDb = new MongoDbConnect(appSetting);
        var database = mongoDb.GetDatabase();

        var instrumentRepo = new InstrumentRepo(database);
        var instruments = instrumentRepo.GetAllStockInstruments();
        var instrumentCollection = database.GetCollection<Instrument>(InstrumentDbConfig.CollectionName);
        instruments = instruments.Where(x => DateOnly.FromDateTime(x.DataFetchedOn) != DateOnly.FromDateTime(DateTime.Today)).ToList();

        foreach (var instr in instruments)
        {
            TestContext.Progress.WriteLine($"Getting data for => {instr.GetStockCode()}");
            Console.WriteLine(instr.GetStockCode());
            if (DateOnly.FromDateTime(instr.DataFetchedOn) == DateOnly.FromDateTime(DateTime.Today))
            {
                Console.WriteLine("data already fetched, as per dates");
                continue;
            }

            try
            {
                var candlesList = kite.GetData4Daily(instr, instr.DailyTo.AddDays(1), DateTime.Today);
                if (!candlesList.Any())
                {
                    Console.WriteLine("no new data from kite");
                    continue;
                }
                var candleList2 = candlesList.Select(x => x.ToEodCandle()).ToList();

                var stockCandleCollection = database.GetCollection<StockCandle>(instr.CollectionNameForDailyData());
                var records = candleList2.Select(x => new InsertOneModel<StockCandle>(x)).ToList();
                stockCandleCollection.BulkWrite(records);

                instr.UpdateForlatest(candleList2);
                var filter = Builders<Instrument>.Filter.Eq(x => x.Id, instr.Id);
                instrumentCollection.ReplaceOne(filter, instr);

                Thread.Sleep(100);
            }
            catch (KiteConnect.TokenException)
            {
                throw;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }
    }
}
