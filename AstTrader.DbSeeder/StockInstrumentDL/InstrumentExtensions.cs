using AstTrader.DbSeeder.StockCandleDL;

namespace AstTrader.DbSeeder.StockInstrumentDL
{
    public static class InstrumentExtensions
    {
        public static Instrument ToTbInstrument(this KiteConnect.Instrument instrument)
        {
            return new Instrument
            {
                Exchange = instrument.Exchange,
                Name = instrument.Name,
                Symbol = instrument.TradingSymbol,
                ZerodhaId = instrument.InstrumentToken.ToString(),
                LotSize = instrument.LotSize,
                TickSize = instrument.TickSize
            };
        }

        public static void UpdateForData(this Instrument instrument, IReadOnlyList<StockCandle> candles)
        {
            instrument.DataFetchedOn = DateTime.Today;
            if (candles.Any())
            {
                instrument.DailyFrom = candles.Min(x => x.DateTime);
                instrument.DailyTo = candles.Max(x => x.DateTime);
            }
        }

        public static void UpdateForlatest(this Instrument instrument, IReadOnlyList<StockCandle> candles)
        {
            instrument.DataFetchedOn = DateTime.Today;
            if (candles.Any())
            {
                instrument.DailyTo = candles.Max(x => x.DateTime);
            }
        }

        public static string CollectionNameForDailyData(this Instrument x) => $"{x.Exchange}_{x.Symbol}_Daily";
        public static string GetStockCode(this Instrument x) => $"{x.Exchange}:{x.Symbol}";
    }
}
