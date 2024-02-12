using AstTrader.DbSeeder.StockCandleDL;
using AstTrader.DbSeeder.StockInstrumentDL;
using AstTrader.DbSeeder.Utils;

namespace AstTrader.Strategy.VivekSinghal;

public class VivekV20Strategy
{
    private readonly StockData _info;
    public VivekV20Strategy(StockData info)
    {
        _info = info;
    }

    public void Run()
    {
        CheckForPattern();
        RunTheTrade();
    }

    public void CheckForPattern()
    {
        if (_info.BaseTrade != StockTrade.NULL)
            return;

        if (_info.Candles.Count == 0 || !_info.TodayCandle.IsGreen())
            return;

        var lastRed = _info.Candles.OrderByDescending(x => x.DateTime).FirstOrDefault(x => !x.IsGreen());
        var firstGreen = lastRed == null ? 0 : _info.Candles.IndexOf(lastRed) + 1;

        decimal buyPrice = _info.Candles[firstGreen].Low, sellPrice = _info.TodayCandle.High;
        var patternFound = SystemExtensions.Percentage(buyPrice, sellPrice) > 20;
        if (!patternFound)
            return;

        _info.BaseStrategy = new TradeByPriceRangeStrategy
        {
            BuyBelowPrice = buyPrice,
            SellAbovePrice = sellPrice,
            TriggeredAt = _info.TodayCandle.DateTime
        };

        _info.AvgStrategy = new TradeByPriceRangeStrategy
        {
            BuyBelowPrice = _info.BaseStrategy.BuyBelowPrice / 10 * 9,
            SellAbovePrice = _info.BaseStrategy.BuyBelowPrice
        };

        var hadPattern = patternFound ? "Pattern found" : "Pattern overridden";
        Console.WriteLine($"{_info.Stock.GetStockCode()}: {hadPattern} on ${_info.BaseStrategy.TriggeredAt}, " +
            $"prices: {_info.BaseStrategy.BuyBelowPrice}/{_info.BaseStrategy.SellAbovePrice}");
    }

    public void RunTheTrade()
    {
        if (_info.BaseStrategy == TradeByPriceRangeStrategy.NULL)
            return;

        // base - trade
        if (_info.BaseTrade == StockTrade.NULL && _info.BaseStrategy.CanBuyToday(_info.TodayCandle))
        {
            _info.BaseTrade = StockTrade.Buy(_info.TodayCandle);
        }
        if (_info.BaseTrade != StockTrade.NULL && _info.BaseStrategy.CanSellToday(_info.TodayCandle, _info.BaseTrade))
        {
            _info.BaseTrade.Sell(_info.TodayCandle);
            _info.BaseTrade = StockTrade.NULL;
        }

        // avg - trade
        if (_info.BaseTrade != StockTrade.NULL && _info.AvgTrade == StockTrade.NULL && _info.AvgStrategy.CanBuyToday(_info.TodayCandle))
        {
            _info.AvgTrade = StockTrade.Buy(_info.TodayCandle);
        }
        if (_info.AvgTrade != StockTrade.NULL && _info.AvgStrategy.CanSellToday(_info.TodayCandle, _info.AvgTrade))
        {
            _info.AvgTrade.Sell(_info.TodayCandle);
            _info.AvgTrade = StockTrade.NULL;
        }
    }
}
