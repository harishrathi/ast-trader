using KiteConnect;
using TradeAstra.Core.Config;

namespace TradeAstra.Wrapper.Zerodha;

public class KiteApiWrapper
{
    public Kite kite { get; private set; }

    public KiteApiWrapper(AppConnectionString appConnectionString)
    {
        kite = new Kite(appConnectionString.KiteAuthToken);
    }

    public List<Instrument> GetSymbolsForNSE()
    {
        return kite.GetInstruments("NSE");
    }

    public List<Historical> GetHistoricalEodData(string stockZerodhaId, DateTime from, DateTime to)
    {
        return kite.GetHistoricalData(stockZerodhaId, from, to, Constants.INTERVAL_DAY);
    }
}
