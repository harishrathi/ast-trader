using AstTrader.DbSeeder.PaperTradesDL;
using AstTrader.DbSeeder.StockCandleDL;
using AstTrader.DbSeeder.StockInstrumentDL;
using MongoDB.Driver;

namespace AstTrader.Strategy;

public class StockData2
{
    private readonly string _runId;
    public Instrument Stock { get; }
    public IMongoCollection<PaperTrade> _collection { get; }
    public StockData2(Instrument stock, IMongoDatabase database)
    {
        Stock = stock;
        _collection = database.GetCollection<PaperTrade>(PaperTrade.CollectionName);
        _runId = DateTime.Now.ToString("s");
    }

    public List<StockCandle> Candles { get; set; } = new List<StockCandle>();
    public StockCandle TodayCandle => Candles.Last();

    public StockTrade BaseTrade { get; set; } = StockTrade.NULL;
    public StockTrade AvgTrade { get; set; } = StockTrade.NULL;

    public TradeBySmaStrategy BaseStrategy { get; set; } = TradeBySmaStrategy.NULL!;
    public TradeByPriceRangeStrategy AvgStrategy { get; set; } = TradeByPriceRangeStrategy.NULL;

    public void SaveTrade(bool isBase)
    {
        var trade = isBase ? BaseTrade : AvgTrade;
        var paperTrade = new PaperTrade
        {
            StockName = Stock.GetStockCode(),
            BuyDate = trade.BuyDate,
            BuyPrice = trade.BuyPrice,
            CAGR = trade.ComputeCagr(),
            DaysDiff = (trade.SellDate - trade.BuyDate).Days,
            SellDate = trade.SellDate,
            SellPrice = trade.SellPrice,
            IsBase = isBase,
            StrategyName = "SMA",
            RunId = _runId,
            Quantity = Convert.ToInt32(Math.Round(100000 / trade.BuyPrice)),
        };

        _collection.InsertOne(paperTrade);
    }
}
