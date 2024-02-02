using KiteConnect;

using MongoDB.Driver;

namespace AstTrader.DbSeeder.StockInstrumentDL
{
    public class InstrumentRepo
    {
        private readonly IMongoDatabase _database;
        private readonly string _collectionName = InstrumentDbConfig.CollectionName;

        public InstrumentRepo(IMongoDatabase database)
        {
            _database = database;
        }

        public Instrument GetInstrument(string stockName)
        {
            var symbol = stockName.Split(":");
            var tbInstrument = _database.GetCollection<Instrument>(_collectionName);
            var instrument = tbInstrument.Find(x => x.Exchange == symbol[0] && x.Symbol == symbol[1]).SingleOrDefault();
            return instrument ?? throw new InputException($"Stock not found: {stockName}");
        }

        public IReadOnlyList<Instrument> GetAllStockInstruments()
        {
            var tbInstrument = _database.GetCollection<Instrument>(_collectionName);
            var instrumentList = tbInstrument.Find(x => x.LotSize == 1).ToList();
            return instrumentList;
        }
    }
}
