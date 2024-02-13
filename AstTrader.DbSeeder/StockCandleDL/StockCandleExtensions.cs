using AstTrader.DbSeeder.Utils;

using KiteConnect;

namespace AstTrader.DbSeeder.StockCandleDL
{
    public static class StockCandleExtensions
    {
        public static IReadOnlyList<DateTime> NseDateRanges => new List<DateTime> {
            new DateTime(1995,1,1),
            new DateTime(2000,1,1),
            new DateTime(2005,1,1),
            new DateTime(2010,1,1),
            new DateTime(2015,1,1),
            new DateTime(2020,1,1),
            DateTime.Today
        };

        public static StockCandle ToEodCandle(this Historical candle)
        {
            return new StockCandle
            {
                Close = candle.Close,
                High = candle.High,
                Low = candle.Low,
                Open = candle.Open,
                Volume = candle.Volume,
                DateTime = candle.TimeStamp.RemoveTime()
            };
        }

        public static bool IsGreen(this StockCandle h)
        {
            return h.Close >= h.Open;
        }
    }
}
