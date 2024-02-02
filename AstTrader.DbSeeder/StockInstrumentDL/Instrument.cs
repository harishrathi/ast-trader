using AstTrader.DbSeeder.Utils;

namespace AstTrader.DbSeeder.StockInstrumentDL
{
    public class Instrument : DbCollectionBase
    {
        public string Exchange { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ZerodhaId { get; set; } = string.Empty;
        public uint LotSize { get; set; }
        public decimal TickSize { get; set; }

        public DateTime DataFetchedOn { get; set; } = DateTime.MinValue;
        public DateTime DailyFrom { get; set; } = DateTime.MinValue;
        public DateTime DailyTo { get; set; } = DateTime.MinValue;

        public string Status { get; set; } = "Active";
        public string StatusReason { get; set; } = string.Empty;

        public DateTime ListingDate { get; set; } = DateTime.MinValue;
    }
}
