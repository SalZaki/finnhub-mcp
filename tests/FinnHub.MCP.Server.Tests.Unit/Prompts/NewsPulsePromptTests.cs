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

public sealed class NewsPulsePromptTests
{
    [Fact]
    public void NewsPulse_RenderedTwice_IsByteIdentical()
    {
        var first = NewsPulsePrompt.NewsPulse("AAPL");
        var second = NewsPulsePrompt.NewsPulse("AAPL");

        // The AC requires byte-identical output, so assert at the UTF-8 byte level explicitly.
        Assert.Equal(Encoding.UTF8.GetBytes(first), Encoding.UTF8.GetBytes(second));
    }

    [Fact]
    public void NewsPulse_ReferencesNewsPulseToolAndFramesWeekOverWeekSentiment()
    {
        var text = NewsPulsePrompt.NewsPulse("AAPL");

        Assert.Contains("get-news-pulse", text, StringComparison.Ordinal);
        // The AC requires a comparison vs last week plus a sentiment narrative.
        Assert.Contains("last week", text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("sentiment", text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void NewsPulse_LowercaseSymbol_NormalisesToUppercase()
    {
        var text = NewsPulsePrompt.NewsPulse("aapl");

        Assert.Contains("\"AAPL\"", text, StringComparison.Ordinal);
        Assert.DoesNotContain("aapl", text, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("AAPL")]
    [InlineData("BRK.A")]
    [InlineData("RDS-A")]
    public void NewsPulse_ValidSymbol_EmbedsTheSymbol(string symbol)
    {
        var text = NewsPulsePrompt.NewsPulse(symbol);

        Assert.Contains(symbol, text, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("A B")]
    [InlineData("A/B")]
    [InlineData("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA")] // 40 chars — exceeds the 32-char bound
    public void NewsPulse_InvalidSymbol_Throws(string? symbol)
    {
        Assert.Throws<ArgumentException>(() => NewsPulsePrompt.NewsPulse(symbol!));
    }
}
