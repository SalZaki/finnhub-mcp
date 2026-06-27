// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text;
using FinnHub.MCP.Server.Prompts;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Prompts;

public sealed class ResearchTickerPromptTests
{
    [Fact]
    public void ResearchTicker_RenderedTwice_IsByteIdentical()
    {
        var first = ResearchTickerPrompt.ResearchTicker("AAPL");
        var second = ResearchTickerPrompt.ResearchTicker("AAPL");

        // The AC requires byte-identical output, so assert at the UTF-8 byte level explicitly.
        Assert.Equal(Encoding.UTF8.GetBytes(first), Encoding.UTF8.GetBytes(second));
    }

    [Fact]
    public void ResearchTicker_ReferencesResolverAndAggregationTools()
    {
        var text = ResearchTickerPrompt.ResearchTicker("AAPL");

        Assert.Contains("search-symbol", text, StringComparison.Ordinal);
        Assert.Contains("get-price-summary", text, StringComparison.Ordinal);
        Assert.Contains("get-financials-snapshot", text, StringComparison.Ordinal);
        Assert.Contains("get-news-pulse", text, StringComparison.Ordinal);
    }

    [Fact]
    public void ResearchTicker_LowercaseSymbol_NormalisesToUppercase()
    {
        var text = ResearchTickerPrompt.ResearchTicker("aapl");

        Assert.Contains("\"AAPL\"", text, StringComparison.Ordinal);
        Assert.DoesNotContain("aapl", text, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("AAPL")]
    [InlineData("BRK.A")]
    [InlineData("RDS-A")]
    public void ResearchTicker_ValidSymbol_EmbedsTheSymbol(string symbol)
    {
        var text = ResearchTickerPrompt.ResearchTicker(symbol);

        Assert.Contains(symbol, text, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("A B")]
    [InlineData("A/B")]
    [InlineData("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA")] // 40 chars — exceeds the 32-char bound
    public void ResearchTicker_InvalidSymbol_Throws(string? symbol)
    {
        Assert.Throws<ArgumentException>(() => ResearchTickerPrompt.ResearchTicker(symbol!));
    }
}
