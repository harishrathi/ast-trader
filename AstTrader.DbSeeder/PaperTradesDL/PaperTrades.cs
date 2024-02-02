namespace AstTrader.DbSeeder.PaperTradesDL;

public class PaperTrade
{
    public static readonly string CollectionName = "PaperTrade";

    public string StrategyName { get; set; } = string.Empty;
    public string RunId { get; set; } = string.Empty;
    public string StockName { get; set; } = string.Empty;
    public bool IsBase { get; set; } = true;
    public DateTime BuyDate { get; set; } = DateTime.MinValue;
    public decimal BuyPrice { get; set; }
    public int Quantity { get; set; }
    public DateTime SellDate { get; set; } = DateTime.MinValue;
    public decimal SellPrice { get; set; }
    public int DaysDiff { get; set; }
    public decimal CAGR { get; set; }
}
