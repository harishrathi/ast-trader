using CsvHelper.Configuration.Attributes;

namespace TradeAstra.Tests.Integration;

public static class NavDataConfig
{
    public static readonly string FilePath = "DummyData\\Sample_MfNav_Data.csv";

    private static NavAnalysis Convert(this NavData current, NavData previous)
    {
        return new NavAnalysis(current, previous);
    }

    public static IEnumerable<NavAnalysis> Convert(List<NavData> data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            if (i == 0)
            {
                continue;
            }

            yield return data[i].Convert(data[i - 1]);
        }
    }

    public class NavData
    {
        [Format("dd-MM-yyyy")]
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
    }

    public class NavAnalysis
    {
        public NavAnalysis(NavData current, NavData previous)
        {
            Date = current.Date;
            Price = current.Price;
            Return = Math.Round((Price - previous.Price) * 100 / previous.Price, 2);
        }

        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public decimal Return { get; set; }
    }
}
