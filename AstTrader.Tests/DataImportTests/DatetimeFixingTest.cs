using AstTrader.DbSeeder.StockInstrumentDL;
using AstTrader.DbSeeder.Utils;
using AstTrader.Tests.TestUtils;

namespace AstTrader.Tests.DataImportTests;

[TestFixture]
public class DatetimeFixingTest
{
    [Test, Explicit]
    public void FixForInstrument()
    {
        var appSetting = GlobalTestUtils.GetAppConnectionString();

        var mongoDb = new MongoDbConnect(appSetting);
        var database = mongoDb.GetDatabase();

        var instrumentRepo = new InstrumentRepo(database);
        var instruments = instrumentRepo.GetAllStockInstruments().ToList();

        var instrumentCollection = database.GetCollection<Instrument>(InstrumentDbConfig.CollectionName);
        instruments.ForEach(x =>
        {
            x.DataFetchedOn = x.DataFetchedOn.ToLocalTime();
            x.DailyFrom = x.DailyFrom.ToLocalTime();
            x.DailyTo = x.DailyTo.ToLocalTime();
            x.ListingDate = x.ListingDate.ToLocalTime();

            //var filter = Builders<Instrument>.Filter.Eq(x => x.Id, x.Id);
            //instrumentCollection.ReplaceOne(filter, x);
        });
    }
}
