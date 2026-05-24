// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Tools.Quotes;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Tools.Quotes;

public sealed class QuotesInputValidatorTests
{
    [Theory]
    [InlineData("AAPL", "AAPL")]
    [InlineData("aapl", "AAPL")]
    [InlineData(" AAPL ", "AAPL")]
    [InlineData("BRK.B", "BRK.B")]
    [InlineData("RDS-A", "RDS-A")]
    public void ValidateSymbol_Valid_NormalisesToUppercase(string input, string expected) =>
        Assert.Equal(expected, QuotesInputValidator.ValidateSymbol(input));

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("!!!")]
    [InlineData("1AAPL")]
    [InlineData("AAPL@INVALID")]
    [InlineData("TOOLONGSYMBOLNAME1234567890")]
    public void ValidateSymbol_Invalid_Throws(string? input) =>
        Assert.Throws<ArgumentException>(() => QuotesInputValidator.ValidateSymbol(input));

    [Theory]
    [InlineData(null, ToolView.Summary)]
    [InlineData("", ToolView.Summary)]
    [InlineData("   ", ToolView.Summary)]
    [InlineData("summary", ToolView.Summary)]
    [InlineData("SUMMARY", ToolView.Summary)]
    [InlineData("standard", ToolView.Standard)]
    [InlineData("full", ToolView.Full)]
    public void ValidateView_KnownValues_MapToEnum(string? input, ToolView expected) =>
        Assert.Equal(expected, QuotesInputValidator.ValidateView(input));

    [Fact]
    public void ValidateView_UnknownValue_Throws() =>
        Assert.Throws<ArgumentException>(() => QuotesInputValidator.ValidateView("nonsense"));
}
