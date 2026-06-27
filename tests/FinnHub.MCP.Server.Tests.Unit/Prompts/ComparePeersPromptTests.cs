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

public sealed class ComparePeersPromptTests
{
    [Fact]
    public void ComparePeers_RenderedTwice_IsByteIdentical()
    {
        var first = ComparePeersPrompt.ComparePeers("AAPL");
        var second = ComparePeersPrompt.ComparePeers("AAPL");

        // The AC requires byte-identical output, so assert at the UTF-8 byte level explicitly.
        Assert.Equal(Encoding.UTF8.GetBytes(first), Encoding.UTF8.GetBytes(second));
    }

    [Fact]
    public void ComparePeers_ReferencesPeersAndFinancialsFanOut()
    {
        var text = ComparePeersPrompt.ComparePeers("AAPL");

        Assert.Contains("get-peers", text, StringComparison.Ordinal);
        Assert.Contains("get-financials-snapshot", text, StringComparison.Ordinal);
        // The AC requires a per-peer fan-out instruction.
        Assert.Contains("each peer", text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ComparePeers_LowercaseSymbol_NormalisesToUppercase()
    {
        var text = ComparePeersPrompt.ComparePeers("aapl");

        Assert.Contains("\"AAPL\"", text, StringComparison.Ordinal);
        Assert.DoesNotContain("aapl", text, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("AAPL")]
    [InlineData("BRK.A")]
    [InlineData("RDS-A")]
    public void ComparePeers_ValidSymbol_EmbedsTheSymbol(string symbol)
    {
        var text = ComparePeersPrompt.ComparePeers(symbol);

        Assert.Contains(symbol, text, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("A B")]
    [InlineData("A/B")]
    [InlineData("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA")] // 40 chars — exceeds the 32-char bound
    public void ComparePeers_InvalidSymbol_Throws(string? symbol)
    {
        Assert.Throws<ArgumentException>(() => ComparePeersPrompt.ComparePeers(symbol!));
    }
}
