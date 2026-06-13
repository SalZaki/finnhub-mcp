// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Tools;
using FinnHub.MCP.Server.Tools.Insiders;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Tools.Insiders;

public sealed class InsidersInputValidatorTests
{
    private static readonly DateOnly s_today = new(2026, 5, 27);

    [Theory]
    [InlineData("aapl", "AAPL")]
    [InlineData("BRK.A", "BRK.A")]
    [InlineData("rds-a", "RDS-A")]
    [InlineData("  msft  ", "MSFT")]
    public void ValidateSymbol_Valid_NormalisesToUppercase(string input, string expected)
    {
        Assert.Equal(expected, CommonInputValidators.ValidateSymbol(input));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ValidateSymbol_Empty_Throws(string? symbol)
    {
        Assert.Throws<ArgumentException>(() => CommonInputValidators.ValidateSymbol(symbol));
    }

    [Theory]
    [InlineData("!!!")]
    [InlineData("1AAPL")]
    [InlineData("WAY-TOO-LONG-A-SYMBOL")]
    public void ValidateSymbol_Invalid_Throws(string symbol)
    {
        Assert.Throws<ArgumentException>(() => CommonInputValidators.ValidateSymbol(symbol));
    }

    [Fact]
    public void ValidateWindow_BothNull_DefaultsToTodayMinus30()
    {
        var (from, to) = InsidersInputValidator.ValidateWindow(null, null, s_today);
        Assert.Equal(s_today, to);
        Assert.Equal(s_today.AddDays(-InsidersInputValidator.DefaultLookbackDays), from);
    }

    [Fact]
    public void ValidateWindow_OnlyToProvided_DefaultsFromToToMinus30()
    {
        var (from, to) = InsidersInputValidator.ValidateWindow(null, "2026-05-20", s_today);
        Assert.Equal(new DateOnly(2026, 5, 20), to);
        Assert.Equal(new DateOnly(2026, 5, 20).AddDays(-InsidersInputValidator.DefaultLookbackDays), from);
    }

    [Fact]
    public void ValidateWindow_OnlyFromProvided_DefaultsToToToday()
    {
        var (from, to) = InsidersInputValidator.ValidateWindow("2026-04-01", null, s_today);
        Assert.Equal(new DateOnly(2026, 4, 1), from);
        Assert.Equal(s_today, to);
    }

    [Fact]
    public void ValidateWindow_ValidPair_Returned()
    {
        var (from, to) = InsidersInputValidator.ValidateWindow("2026-05-01", "2026-05-15", s_today);
        Assert.Equal(new DateOnly(2026, 5, 1), from);
        Assert.Equal(new DateOnly(2026, 5, 15), to);
    }

    [Fact]
    public void ValidateWindow_ToBeforeFrom_Throws()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            InsidersInputValidator.ValidateWindow("2026-05-15", "2026-05-01", s_today));
        Assert.Contains("must be on or after", ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void ValidateWindow_ExceedsMaxSpan_Throws()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            InsidersInputValidator.ValidateWindow("2026-01-01", "2026-06-01", s_today));
        Assert.Contains("must not exceed", ex.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("2026/05/01")]
    [InlineData("01-05-2026")]
    [InlineData("not-a-date")]
    public void ValidateWindow_BadFromFormat_Throws(string from)
    {
        Assert.Throws<ArgumentException>(() => InsidersInputValidator.ValidateWindow(from, "2026-05-15", s_today));
    }

    [Theory]
    [InlineData(null, ToolView.Summary)]
    [InlineData("", ToolView.Summary)]
    [InlineData("summary", ToolView.Summary)]
    [InlineData("Standard", ToolView.Standard)]
    [InlineData("FULL", ToolView.Full)]
    public void ValidateView_AcceptsKnownValuesCaseInsensitive(string? view, ToolView expected)
    {
        Assert.Equal(expected, CommonInputValidators.ValidateView(view));
    }

    [Fact]
    public void ValidateView_Unknown_Throws()
    {
        Assert.Throws<ArgumentException>(() => CommonInputValidators.ValidateView("brief"));
    }
}
