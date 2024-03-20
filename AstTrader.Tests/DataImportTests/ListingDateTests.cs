using System.Globalization;

using AstTrader.DbSeeder.StockCandleDL;
using AstTrader.DbSeeder.StockInstrumentDL;
using AstTrader.DbSeeder.Utils;
using AstTrader.Tests.TestUtils;

using CsvHelper;
using CsvHelper.Configuration;

namespace AstTrader.Tests.DataImportTests
{
    internal class ListingDateTests
    {
        [Test, Explicit]
        public void MissingListingDate()
        {
            var appSetting = GlobalTestUtils.GetAppConnectionString();

            var mongoDb = new MongoDbConnect(appSetting);
            var database = mongoDb.GetDatabase();

            var instrumentRepo = new InstrumentRepo(database);
            var instruments2 = instrumentRepo.GetAllStockInstruments();

            var missingDates = instruments2.Where(x => x.ListingDate.Year < 1900).ToList();
            var instrumentCollection = database.GetCollection<Instrument>(InstrumentDbConfig.CollectionName);

            foreach (var instr in missingDates)
            {
                Console.WriteLine($"{instr.GetStockCode()} does not have listing date");

                instr.Status = "Inactive";
                instr.StatusReason = "NoListingDate";

                var filter = Builders<Instrument>.Filter.Eq(x => x.Id, instr.Id);
                instrumentCollection.ReplaceOne(filter, instr);
            }
        }

        public List<ListingInfo> ReadListingDate()
        {
            using (var reader = new StreamReader("C:\\Users\\lenovo\\Downloads\\EQUITY_L.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<ListingInfoMap>();
                return csv.GetRecords<ListingInfo>().ToList();
            }
        }

        [Test, Explicit]
        public void SetListingDate()
        {
            var appSetting = GlobalTestUtils.GetAppConnectionString();

            var mongoDb = new MongoDbConnect(appSetting);
            var database = mongoDb.GetDatabase();

            var instrumentRepo = new InstrumentRepo(database);
            var instruments2 = instrumentRepo.GetAllStockInstruments();

            var instrumentCollection = database.GetCollection<Instrument>(InstrumentDbConfig.CollectionName);
            var listingDates = ReadListingDate();
            foreach (var instr in instruments2)
            {
                var listDate = listingDates.FirstOrDefault(x => x.Symbol == instr.Symbol);
                if (listDate == null || listDate.ListingDate == DateTime.MinValue) continue;

                instr.ListingDate = listDate.ListingDate;
                Console.WriteLine($"{instr.GetStockCode()} was listed on {instr.ListingDate}");

                var filter = Builders<Instrument>.Filter.Eq(x => x.Id, instr.Id);
                instrumentCollection.ReplaceOne(filter, instr);
            }
        }

        [Test, Explicit]
        public void AllActiveShouldHaveData()
        {
            var appSetting = GlobalTestUtils.GetAppConnectionString();

            var mongoDb = new MongoDbConnect(appSetting);
            var database = mongoDb.GetDatabase();

            var instrumentRepo = new InstrumentRepo(database);
            var allInstruments = instrumentRepo.GetAllStockInstruments();

            var activeInstruments = allInstruments.Where(x => x.Status == "Active").ToList();

            var documentCount = new List<Tuple<string, decimal, long, decimal>>();

            foreach (var instr in activeInstruments)
            {
                var daysSinceListed = (decimal)Math.Round((DateTime.Today - instr.ListingDate).TotalDays, 0);
                var stockCandleCollection = database.GetCollection<StockCandle>(instr.CollectionNameForDailyData());
                var candleCount = stockCandleCollection.EstimatedDocumentCount();
                var percentage = SystemExtensions.Percentage(daysSinceListed, candleCount);
                documentCount.Add(Tuple.Create(instr.GetStockCode(), daysSinceListed, candleCount, percentage));
                //Console.WriteLine($"{instr.GetStockCode()}, listed since: {daysSinceListed} and has {candleCount} candles");
            }

            var minimal = documentCount.OrderByDescending(x => x.Item4).Take(50).ToList();
            minimal.ForEach(x =>
            {
                Console.WriteLine($"{x.Item1}, listed since: {x.Item2} and has {x.Item4}% candles");
            });
        }
    }

    public class ListingInfo
    {
        public string Symbol { get; set; } = string.Empty;
        public DateTime ListingDate { get; set; } = DateTime.MinValue;
    }

    public class ListingInfoMap : ClassMap<ListingInfo>
    {
        public ListingInfoMap()
        {
            Map(m => m.Symbol).Name("SYMBOL");
            Map(m => m.ListingDate).Name(" DATE OF LISTING");
        }
    }
}
