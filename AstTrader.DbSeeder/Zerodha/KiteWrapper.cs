using AstTrader.DbSeeder.Utils;

using KiteConnect;
using TradeAstra.Core.Config;

namespace AstTrader.DbSeeder.Zerodha
{
    public class KiteWrapper
    {
        public Kite kite { get; private set; }

        public KiteWrapper(AppConnectionString appConnectionString)
        {
            kite = new Kite(string.Empty, appConnectionString.KiteAuthToken);
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
