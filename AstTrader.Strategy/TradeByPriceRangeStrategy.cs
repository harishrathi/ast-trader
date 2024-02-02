using AstTrader.DbSeeder.StockCandleDL;

namespace AstTrader.Strategy;

public class TradeByPriceRangeStrategy
{
    public static readonly TradeByPriceRangeStrategy NULL = new();

    public DateTime TriggeredAt { get; set; } = DateTime.MinValue;
    public decimal BuyBelowPrice { get; set; } = decimal.Zero;
    public decimal SellAbovePrice { get; set; } = decimal.Zero;

    public bool CanBuyToday(StockCandle candle)
    {
        return candle.DateTime > TriggeredAt && candle.Close < BuyBelowPrice;
    }

    public bool CanSellToday(StockCandle candle, StockTrade trade)
    {
        return candle.DateTime > trade.BuyDate && candle.Close >= SellAbovePrice;
    }
}
