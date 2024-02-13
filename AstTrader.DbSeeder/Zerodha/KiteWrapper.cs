using AstTrader.DbSeeder.Utils;

using KiteConnect;

namespace AstTrader.DbSeeder.Zerodha
{
    public class KiteWrapper
    {
        public Kite kite { get; private set; }

        public KiteWrapper(ApplicationSettings appSettings)
        {
            kite = new Kite(string.Empty, appSettings.ConnectionStrings.KiteAuthToken);
        }

        public List<Instrument> GetSymbolsForNSE()
        {
            return kite.GetInstruments("NSE");
        }

        public List<Historical> GetData4Daily(StockInstrumentDL.Instrument stock, DateTime from, DateTime to)
        {
            return kite.GetHistoricalData(stock.ZerodhaId, from, to, Constants.INTERVAL_DAY);
        }
    }
}
