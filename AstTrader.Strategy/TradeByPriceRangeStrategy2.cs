using AstTrader.DbSeeder.StockCandleDL;

namespace AstTrader.Strategy;

public class TradeByPriceRangeStrategy2
{
    public static readonly TradeByPriceRangeStrategy2 NULL = new();

    public DateTime TriggeredAt { get; set; } = DateTime.MinValue;
    public decimal SellAbovePrice { get; set; } = decimal.Zero;

    public bool CanBuyToday(StockCandle candle)
    {
        return candle.DateTime > TriggeredAt;
    }

    public bool CanSellToday(StockCandle today, StockTrade trade)
    {
        if (today.Close < (trade.BuyPrice * 0.5M))
            return true; //sell at loss

        return today.DateTime > trade.BuyDate && today.Close >= SellAbovePrice;
    }
}
