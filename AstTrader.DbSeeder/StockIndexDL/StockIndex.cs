using AstTrader.DbSeeder.Utils;

namespace AstTrader.DbSeeder.StockIndexDL
{
    public class StockIndex : DbCollectionBase
    {
        public string IndexName { get; set; } = string.Empty;
        public List<string> Instruments { get; set; } = new List<string>();
    }
}
