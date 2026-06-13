// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Caching;
using Xunit;

namespace FinnHub.MCP.Server.Application.Tests.Unit.Caching;

public sealed class SymbolCacheKeyTests
{
    [Fact]
    public void For_SinglePart_JoinsWithColonEquals()
    {
        Assert.Equal("quote:s=AAPL", SymbolCacheKey.For("quote", ("s", "AAPL")));
    }

    [Fact]
    public void For_MultiPart_PreservesOrder()
    {
        Assert.Equal(
            "profile:s=AAPL:cosmetic=True",
            SymbolCacheKey.For("profile", ("s", "AAPL"), ("cosmetic", "True")));
    }

    [Fact]
    public void For_DateParts_KeepsInvariantFormattedValues()
    {
        Assert.Equal(
            "calendar-ipo:f=2026-05-16:t=2026-05-23",
            SymbolCacheKey.For("calendar-ipo", ("f", "2026-05-16"), ("t", "2026-05-23")));
    }

    [Fact]
    public void For_PrefixOnly_ReturnsPrefix()
    {
        Assert.Equal("exchanges", SymbolCacheKey.For("exchanges"));
    }
}
