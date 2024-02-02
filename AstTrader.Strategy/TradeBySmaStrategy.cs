using AstTrader.DbSeeder.StockCandleDL;

namespace AstTrader.Strategy;

public class TradeBySmaStrategy
{
    public static readonly TradeBySmaStrategy NULL = new();

    public DateTime TriggeredAt { get; set; } = DateTime.MinValue;

    public bool CanBuyToday(StockCandle todayCandle)
    {
        return todayCandle.DateTime > TriggeredAt;
    }

    public bool CanSellToday(List<StockCandle> candles)
    {
        decimal sma20 = candles.GetSMA(20),
            sma50 = candles.GetSMA(50),
            sma200 = candles.GetSMA(200);
        return sma200 <= sma50 &&
            sma50 <= sma20 &&
            sma20 <= candles.Last().Close;
    }
}
