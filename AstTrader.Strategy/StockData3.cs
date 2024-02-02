using AstTrader.DbSeeder.PaperTradesDL;
using AstTrader.DbSeeder.StockCandleDL;
using AstTrader.DbSeeder.StockInstrumentDL;
using MongoDB.Driver;

namespace AstTrader.Strategy;

public class StockData3
{
    private readonly string _runId;
    public Instrument Stock { get; }
    public IMongoCollection<PaperTrade> _collection { get; }
    public StockData3(Instrument stock, IMongoDatabase database)
    {
        Stock = stock;
        _collection = database.GetCollection<PaperTrade>(PaperTrade.CollectionName);
        _runId = DateTime.Now.ToString("s");
    }

    public List<StockCandle> Candles { get; set; } = new List<StockCandle>();
    public StockCandle TodayCandle => Candles.Last();

    public StockTrade BaseTrade { get; set; } = StockTrade.NULL;
    public TradeByPriceRangeStrategy2 BaseStrategy { get; set; } = TradeByPriceRangeStrategy2.NULL;

    public void SaveTrade()
    {
        var paperTrade = new PaperTrade
        {
            StockName = Stock.GetStockCode(),
            BuyDate = BaseTrade.BuyDate,
            BuyPrice = BaseTrade.BuyPrice,
            CAGR = BaseTrade.ComputeCagr(),
            DaysDiff = (BaseTrade.SellDate - BaseTrade.BuyDate).Days,
            SellDate = BaseTrade.SellDate,
            SellPrice = BaseTrade.SellPrice,
            IsBase = true,
            StrategyName = "3Xin3Y",
            RunId = _runId,
            Quantity = Convert.ToInt32(Math.Round(100000 / BaseTrade.BuyPrice)),
        };

        _collection.InsertOne(paperTrade);
    }
}
