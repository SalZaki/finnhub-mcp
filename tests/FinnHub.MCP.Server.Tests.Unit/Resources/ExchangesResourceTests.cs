// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.Json;
using FinnHub.MCP.Server.Application.Exchanges;
using FinnHub.MCP.Server.Application.Exchanges.Features.GetAllExchanges;
using FinnHub.MCP.Server.Resources.Exchanges;
using NSubstitute;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Resources;

public sealed class ExchangesResourceTests
{
    private static Exchange MakeExchange(string code, string name, string? url) => new()
    {
        ExchangeCode = code,
        ExchangeName = name,
        MicCode = "XTST",
        TimeZone = "Europe/London",
        TradingHours = "08:00-16:30",
        CountryCode = "GB",
        CountryName = "United Kingdom",
        Url = url
    };

    [Fact]
    public void GetExchanges_SerializesCatalogEntries_WithEnvelopeCounts()
    {
        var catalog = Substitute.For<IExchangeCatalog>();
        IReadOnlyList<Exchange> data =
        [
            MakeExchange("L", "London Stock Exchange", "https://example.com/lse"),
            MakeExchange("US", "US exchanges", url: null)
        ];
        catalog.Exchanges.Returns(data);

        var json = new ExchangesResource(catalog).GetExchanges();

        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;
        var exchanges = root.GetProperty("exchanges");

        Assert.Equal(JsonValueKind.Array, exchanges.ValueKind);
        Assert.Equal(2, exchanges.GetArrayLength());
        Assert.Equal(2, root.GetProperty("total_count").GetInt32());
        Assert.True(root.GetProperty("has_results").GetBoolean());

        Assert.Equal("L", exchanges[0].GetProperty("code").GetString());
        Assert.Equal("London Stock Exchange", exchanges[0].GetProperty("name").GetString());
        Assert.Equal("https://example.com/lse", exchanges[0].GetProperty("url").GetString());

        // Venues Finnhub lists without a reference URL serialize url as null, not omitted.
        Assert.Equal(JsonValueKind.Null, exchanges[1].GetProperty("url").ValueKind);
    }

    [Fact]
    public void GetExchanges_EmptyCatalog_ReturnsEmptyArrayAndFalseHasResults()
    {
        var catalog = Substitute.For<IExchangeCatalog>();
        catalog.Exchanges.Returns([]);

        var json = new ExchangesResource(catalog).GetExchanges();

        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        Assert.Equal(0, root.GetProperty("exchanges").GetArrayLength());
        Assert.Equal(0, root.GetProperty("total_count").GetInt32());
        Assert.False(root.GetProperty("has_results").GetBoolean());
    }
}
