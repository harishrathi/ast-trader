using AstTrader.DbSeeder.StockCandleDL;
using AstTrader.DbSeeder.Utils;

namespace AstTrader.Strategy;

public class StockTrade
{
    public static readonly StockTrade NULL = new StockTrade(new StockCandle { Close = 0, DateTime = DateTime.MinValue });

    public int Quantity { get; }
    public decimal BuyPrice { get; }
    public DateTime BuyDate { get; }
    public decimal SellPrice { get; private set; }
    public DateTime SellDate { get; private set; } = DateTime.MinValue;
    public bool IsOpen => SellDate == DateTime.MinValue;

    private StockTrade(StockCandle candle)
    {
        BuyPrice = candle.Close;
        Quantity = 1;
        BuyDate = candle.DateTime.Date;
    }

    public static StockTrade Buy(StockCandle candle)
    {
        var trade = new StockTrade(candle);
        Console.WriteLine($"Traded On: {candle.DateTime} at {candle.Close}");
        return trade;
    }

    public decimal ComputeCagr()
    {
        var percentage = SystemExtensions.Percentage(BuyPrice, SellPrice);
        var noOfDays = SellDate - BuyDate;
        return SystemExtensions.CaculateCAGR(percentage, noOfDays.Days);
    }

    public void Sell(StockCandle candle)
    {
        SellPrice = candle.Open;
        SellDate = candle.DateTime.Date;
        var cagr = ComputeCagr();
        var sold = cagr <= 0 ? "Sold (loss)" : "Sold";
        Console.WriteLine($"== {sold} : Price: {SellPrice}, with CAGR: {ComputeCagr()}... date {SellDate.ToShortDateString()}");
    }
}
