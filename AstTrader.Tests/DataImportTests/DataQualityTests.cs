using AstTrader.DbSeeder.StockInstrumentDL;
using AstTrader.DbSeeder.Utils;
using AstTrader.Tests.TestUtils;
using MongoDB.Driver.Linq;

namespace AstTrader.Tests.DataImportTests;

[TestFixture]
public class DataQualityTests
{
    [Test, Explicit]
    public void CheckInactiveStocks()
    {
        var appSetting = AppSettingTest.GetAppSettings();

        var mongoDb = new MongoDbConnect(appSetting);
        var database = mongoDb.GetDatabase();

        var instrumentRepo = new InstrumentRepo(database);
        var instruments = instrumentRepo.GetAllStockInstruments();

        //check if no trades in 2024
        var inactive = instruments.Where(x => x.DailyTo.Year != 2024).ToList();
        if (inactive.Count == 0)
        {
            Assert.Pass();
        }

        var instrumentCollection = database.GetCollection<Instrument>(InstrumentDbConfig.CollectionName);
        inactive.ForEach(x =>
        {
            Console.WriteLine(x.GetStockCode());

            x.Status = "Inactive";
            x.StatusReason = "NoTradesIn2024";

            var filter = Builders<Instrument>.Filter.Eq(x => x.Id, x.Id);
            instrumentCollection.ReplaceOne(filter, x);
        });
    }
}
