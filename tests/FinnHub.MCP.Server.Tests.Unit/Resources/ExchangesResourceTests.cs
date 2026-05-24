// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.Json;
using FinnHub.MCP.Server.Resources.Exchanges;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Resources;

public sealed class ExchangesResourceTests
{
    [Fact]
    public void GetExchanges_ReturnsStubPayloadWithLondonStockExchange()
    {
        var json = new ExchangesResource().GetExchanges();

        using var document = JsonDocument.Parse(json);
        var exchanges = document.RootElement.GetProperty("exchanges");

        Assert.Equal(JsonValueKind.Array, exchanges.ValueKind);
        Assert.Equal(1, exchanges.GetArrayLength());

        var lse = exchanges[0];
        Assert.Equal("L", lse.GetProperty("code").GetString());
        Assert.Equal("London Stock Exchange", lse.GetProperty("name").GetString());
        Assert.Equal("XLON", lse.GetProperty("mic").GetString());
        Assert.Equal("GB", lse.GetProperty("country_code").GetString());
        Assert.Equal("Europe/London", lse.GetProperty("time_zone").GetString());
        Assert.Equal("08:00-16:30", lse.GetProperty("trading_hours").GetString());
    }
}
