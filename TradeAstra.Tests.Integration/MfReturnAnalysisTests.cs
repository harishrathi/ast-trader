using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using System.Globalization;

namespace TradeAstra.Tests.Integration;

public class MfReturnAnalysisTests
{
    private List<NavDataConfig.NavData> GetData()
    {
        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
            ShouldSkipRecord = record => record.Row[0].StartsWith("#")
        };
        using var reader = new StreamReader(NavDataConfig.FilePath);
        using var csv = new CsvReader(reader, csvConfig);
        var records = csv.GetRecords<NavDataConfig.NavData>();
        return records.ToList();
    }

    [Test]
    public void ReadCsv()
    {
        var data = GetData();
        data.Count.Should().BeGreaterThan(100);

        var returns = NavDataConfig.Convert(data).ToList();
        returns.Count.Should().Be(data.Count - 1);
    }
}

