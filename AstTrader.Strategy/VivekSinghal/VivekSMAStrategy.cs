using AstTrader.DbSeeder.StockInstrumentDL;

namespace AstTrader.Strategy.VivekSinghal;

public class VivekSMAStrategy
{
    private readonly StockData2 _info;
    public VivekSMAStrategy(StockData2 info)
    {
        _info = info;
    }

    public void Run()
    {
        CheckForPattern();
        RunTheTrade();
    }

    private void CheckForPattern()
    {
        if (_info.BaseStrategy != TradeBySmaStrategy.NULL)
            return;

        if (_info.Candles.Count < 200)
            return;

        decimal sma20 = _info.Candles.GetSMA(20),
            sma50 = _info.Candles.GetSMA(50),
            sma200 = _info.Candles.GetSMA(200);
        if (sma200 >= sma50 && sma50 >= sma20 && sma20 >= _info.TodayCandle.Close)
        {
            _info.BaseStrategy = new TradeBySmaStrategy
            {
                TriggeredAt = _info.TodayCandle.DateTime
            };
            _info.AvgStrategy = new TradeByPriceRangeStrategy
            {
                TriggeredAt = _info.TodayCandle.DateTime,
                BuyBelowPrice = _info.TodayCandle.Close / 10 * 9,
                SellAbovePrice = _info.TodayCandle.Close
            };

            Console.WriteLine($"{_info.Stock.GetStockCode()}: Pattern found on ${_info.BaseStrategy.TriggeredAt}, buy @ {_info.TodayCandle.Close}");
        }
    }

    private void RunTheTrade()
    {
        if (_info.BaseStrategy == TradeBySmaStrategy.NULL)
            return;

        // base - trade
        if (_info.BaseTrade == StockTrade.NULL && _info.BaseStrategy.CanBuyToday(_info.TodayCandle))
        {
            _info.BaseTrade = StockTrade.Buy(_info.TodayCandle);
        }
        if (_info.BaseTrade != StockTrade.NULL && _info.BaseStrategy.CanSellToday(_info.Candles))
        {
            _info.BaseTrade.Sell(_info.TodayCandle);
            _info.SaveTrade(true);
            _info.BaseTrade = StockTrade.NULL;
            _info.BaseStrategy = TradeBySmaStrategy.NULL;
            _info.AvgStrategy = TradeByPriceRangeStrategy.NULL;
        }

        // avg - trade
        if (_info.BaseTrade != StockTrade.NULL && _info.AvgTrade == StockTrade.NULL && _info.AvgStrategy.CanBuyToday(_info.TodayCandle))
        {
            _info.AvgTrade = StockTrade.Buy(_info.TodayCandle);
        }
        if (_info.AvgTrade != StockTrade.NULL && _info.AvgStrategy.CanSellToday(_info.TodayCandle, _info.AvgTrade))
        {
            _info.AvgTrade.Sell(_info.TodayCandle);
            _info.SaveTrade(false);
            _info.AvgTrade = StockTrade.NULL;
        }
    }
}
