// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Tools.Exchanges;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Tools.Exchanges;

public sealed class ExchangeSymbolsInputValidatorTests
{
    [Theory]
    [InlineData("us", "US")]
    [InlineData("  l  ", "L")]
    [InlineData("DFM", "DFM")]
    [InlineData("TOOLONGX", "TOOLONGX")] // 8 letters is the upper bound
    public void ValidateExchange_Valid_NormalisesToUppercase(string input, string expected)
    {
        Assert.Equal(expected, ExchangeSymbolsInputValidator.ValidateExchange(input));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ValidateExchange_Empty_Throws(string? exchange)
    {
        Assert.Throws<ArgumentException>(() => ExchangeSymbolsInputValidator.ValidateExchange(exchange));
    }

    [Theory]
    [InlineData("US1")]      // digits not allowed
    [InlineData("U.S")]      // punctuation not allowed
    [InlineData("US-")]      // dashes not allowed
    [InlineData("TOOLONGXX")] // 9 chars — exceeds the 8-letter bound
    public void ValidateExchange_InvalidChars_Throws(string exchange)
    {
        Assert.Throws<ArgumentException>(() => ExchangeSymbolsInputValidator.ValidateExchange(exchange));
    }

    [Theory]
    [InlineData(null, ToolView.Summary)]
    [InlineData("summary", ToolView.Summary)]
    [InlineData("Standard", ToolView.Standard)]
    [InlineData("FULL", ToolView.Full)]
    public void ValidateView_AcceptsKnownValuesCaseInsensitive(string? view, ToolView expected)
    {
        Assert.Equal(expected, ExchangeSymbolsInputValidator.ValidateView(view));
    }

    [Fact]
    public void ValidateView_Unknown_Throws()
    {
        Assert.Throws<ArgumentException>(() => ExchangeSymbolsInputValidator.ValidateView("brief"));
    }
}
