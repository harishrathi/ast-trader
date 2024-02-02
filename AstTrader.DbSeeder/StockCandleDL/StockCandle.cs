using AstTrader.DbSeeder.Utils;

namespace AstTrader.DbSeeder.StockCandleDL
{
    public class StockCandle : DbCollectionBase
    {
        public DateTime DateTime { get; set; } = DateTime.MinValue;
        public decimal Open { get; set; } = decimal.Zero;
        public decimal High { get; set; } = decimal.Zero;
        public decimal Low { get; set; } = decimal.Zero;
        public decimal Close { get; set; } = decimal.Zero;
        public decimal Volume { get; set; } = decimal.Zero;

    }
}
