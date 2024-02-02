using AstTrader.DbSeeder.StockCandleDL;
using AstTrader.DbSeeder.StockInstrumentDL;

namespace AstTrader.Strategy;

public class StockData
{
    public Instrument Stock { get; }
    public StockData(Instrument stock)
    {
        Stock = stock;
    }

    public IList<StockCandle> Candles { get; set; } = new List<StockCandle>();
    public StockCandle TodayCandle => Candles.Last();

    public StockTrade BaseTrade { get; set; } = StockTrade.NULL;
    public StockTrade AvgTrade { get; set; } = StockTrade.NULL;

    public TradeByPriceRangeStrategy BaseStrategy { get; set; } = TradeByPriceRangeStrategy.NULL!;
    public TradeByPriceRangeStrategy AvgStrategy { get; set; } = TradeByPriceRangeStrategy.NULL;
}
