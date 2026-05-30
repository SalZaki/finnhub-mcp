// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Tools.Recommendations;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Tools.Recommendations;

public sealed class RecommendationsInputValidatorTests
{
    [Theory]
    [InlineData("aapl", "AAPL")]
    [InlineData("BRK.A", "BRK.A")]
    [InlineData("rds-a", "RDS-A")]
    [InlineData("  msft  ", "MSFT")]
    public void ValidateSymbol_Valid_NormalisesToUppercase(string input, string expected)
    {
        Assert.Equal(expected, RecommendationsInputValidator.ValidateSymbol(input));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ValidateSymbol_Empty_Throws(string? symbol)
    {
        Assert.Throws<ArgumentException>(() => RecommendationsInputValidator.ValidateSymbol(symbol));
    }

    [Theory]
    [InlineData("!!!")]
    [InlineData("1AAPL")]
    [InlineData("WAY-TOO-LONG-A-SYMBOL")]
    public void ValidateSymbol_Invalid_Throws(string symbol)
    {
        Assert.Throws<ArgumentException>(() => RecommendationsInputValidator.ValidateSymbol(symbol));
    }

    [Theory]
    [InlineData(null, ToolView.Summary)]
    [InlineData("", ToolView.Summary)]
    [InlineData("summary", ToolView.Summary)]
    [InlineData("Standard", ToolView.Standard)]
    [InlineData("FULL", ToolView.Full)]
    public void ValidateView_AcceptsKnownValuesCaseInsensitive(string? view, ToolView expected)
    {
        Assert.Equal(expected, RecommendationsInputValidator.ValidateView(view));
    }

    [Fact]
    public void ValidateView_Unknown_Throws()
    {
        Assert.Throws<ArgumentException>(() => RecommendationsInputValidator.ValidateView("brief"));
    }
}
