using AstTrader.DbSeeder.StockCandleDL;
using Skender.Stock.Indicators;

namespace AstTrader.Strategy;
public static class StockCandleExtensions
{
    public static Quote ToStockQuote(this StockCandle candle)
    {
        return new Quote
        {
            Close = candle.Close,
            Date = candle.DateTime.ToLocalTime(),
            High = candle.High,
            Low = candle.Low,
            Open = candle.Open,
            Volume = candle.Volume
        };
    }

    public static List<Quote> ToStockQuote(this IEnumerable<StockCandle> candles)
    {
        return candles.Select(x => x.ToStockQuote()).ToList();
    }

    public static decimal GetSMA(this List<StockCandle> candles, int count)
    {
        var quotes = candles.GetRange(candles.Count - count, count).ToStockQuote();
        var result = quotes.GetSma(count);
        return Convert.ToDecimal(result.Last().Sma ?? 0);
    }
}
