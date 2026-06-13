// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Prices.Features.GetPriceSummary;
using FinnHub.MCP.Server.Tools;
using FinnHub.MCP.Server.Tools.Prices;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Tools.Prices;

public sealed class PricesInputValidatorTests
{
    [Theory]
    [InlineData("AAPL", "AAPL")]
    [InlineData("aapl", "AAPL")]
    public void ValidateSymbol_Valid_Normalises(string input, string expected) =>
        Assert.Equal(expected, CommonInputValidators.ValidateSymbol(input));

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("!!!")]
    public void ValidateSymbol_Invalid_Throws(string? input) =>
        Assert.Throws<ArgumentException>(() => CommonInputValidators.ValidateSymbol(input));

    [Theory]
    [InlineData(null, PricePeriod.ThirtyDays)]
    [InlineData("", PricePeriod.ThirtyDays)]
    [InlineData("7d", PricePeriod.SevenDays)]
    [InlineData("30d", PricePeriod.ThirtyDays)]
    [InlineData("90d", PricePeriod.NinetyDays)]
    [InlineData("1y", PricePeriod.OneYear)]
    [InlineData("1Y", PricePeriod.OneYear)]
    public void ValidatePeriod_KnownValues_MapToEnum(string? input, PricePeriod expected) =>
        Assert.Equal(expected, PricesInputValidator.ValidatePeriod(input));

    [Theory]
    [InlineData("5h")]
    [InlineData("2y")]
    [InlineData("nonsense")]
    public void ValidatePeriod_Unknown_Throws(string input) =>
        Assert.Throws<ArgumentException>(() => PricesInputValidator.ValidatePeriod(input));

    [Theory]
    [InlineData(null, ToolView.Summary)]
    [InlineData("summary", ToolView.Summary)]
    [InlineData("standard", ToolView.Standard)]
    [InlineData("full", ToolView.Full)]
    public void ValidateView_KnownValues_MapToEnum(string? input, ToolView expected) =>
        Assert.Equal(expected, CommonInputValidators.ValidateView(input));

    [Fact]
    public void ValidateView_Unknown_Throws() =>
        Assert.Throws<ArgumentException>(() => CommonInputValidators.ValidateView("nonsense"));
}
