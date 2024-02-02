using AstTrader.DbSeeder.Utils;

namespace AstTrader.Strategy.VivekSinghal;

public class Vivek3XIn3YearsStrategy
{
    private readonly StockData3 _info;
    public Vivek3XIn3YearsStrategy(StockData3 info)
    {
        _info = info;
    }

    public void Run()
    {
        CheckForPattern();
        RunTheTrade();
    }

    private void RunTheTrade()
    {
        if (_info.BaseStrategy != TradeByPriceRangeStrategy2.NULL)
            return;


        var highCandle = _info.Candles.OrderBy(x => x.Close).Last();
        var lowCandle = _info.Candles.Where(x => x.DateTime > highCandle.DateTime).OrderBy(x => x.Close).FirstOrDefault();
        if (lowCandle == null || lowCandle.Close == 0)
            return;

        var lowPct = SystemExtensions.Percentage(highCandle.Close, lowCandle.Close);
        if (highCandle.Close < (lowCandle.Close * 3))
            return;

        if (_info.TodayCandle.Close < (lowCandle.Close * 1.4M))
            return;

        Console.WriteLine($"High: {highCandle.Close}, Low: {lowCandle.Close}, Today: {_info.TodayCandle.Close}");

        _info.BaseStrategy = new TradeByPriceRangeStrategy2
        {
            TriggeredAt = _info.TodayCandle.DateTime,
            SellAbovePrice = _info.TodayCandle.Close * 3,
        };
    }

    private void CheckForPattern()
    {
        if (_info.BaseStrategy == TradeByPriceRangeStrategy2.NULL)
            return;

        if (_info.BaseTrade == StockTrade.NULL && _info.BaseStrategy.CanBuyToday(_info.TodayCandle))
        {
            _info.BaseTrade = StockTrade.Buy(_info.TodayCandle);
        }
        if (_info.BaseTrade != StockTrade.NULL && _info.BaseStrategy.CanSellToday(_info.TodayCandle, _info.BaseTrade))
        {
            _info.BaseTrade.Sell(_info.TodayCandle);
            _info.SaveTrade();
            _info.BaseTrade = StockTrade.NULL;
            _info.BaseStrategy = TradeByPriceRangeStrategy2.NULL;
        }
    }
}
