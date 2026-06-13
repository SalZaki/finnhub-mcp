// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Symbols;
using Xunit;

namespace FinnHub.MCP.Server.Application.Tests.Unit.Symbols;

public sealed class SymbolNormalizerTests
{
    [Theory]
    [InlineData("aapl", "AAPL")]
    [InlineData(" aapl ", "AAPL")]
    [InlineData("AAPL", "AAPL")]
    [InlineData("brk.b", "BRK.B")]
    public void Normalize_TrimsAndUppercases(string input, string expected)
    {
        Assert.Equal(expected, SymbolNormalizer.Normalize(input));
    }

    [Fact]
    public void Normalize_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => SymbolNormalizer.Normalize(null!));
    }

    [Fact]
    public void NormalizeOrAll_Null_ReturnsAll()
    {
        Assert.Equal("all", SymbolNormalizer.NormalizeOrAll(null));
    }

    [Theory]
    [InlineData(" msft ", "MSFT")]
    [InlineData("MSFT", "MSFT")]
    [InlineData("", "")]
    public void NormalizeOrAll_NonNull_NormalizesAndNeverWidensToAll(string input, string expected)
    {
        // Branches on null only: an empty/whitespace (non-null) symbol normalises like any other
        // value, so it never collapses into the "all" slot — matches the prior calendar behaviour.
        Assert.Equal(expected, SymbolNormalizer.NormalizeOrAll(input));
    }
}
