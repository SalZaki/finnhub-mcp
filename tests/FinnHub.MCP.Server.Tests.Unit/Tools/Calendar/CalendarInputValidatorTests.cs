// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Calendar.Features.GetCalendar;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Tools.Calendar;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Tools.Calendar;

public sealed class CalendarInputValidatorTests
{
    private static readonly DateOnly s_today = new(2026, 5, 27);

    [Theory]
    [InlineData("earnings")]
    [InlineData("EARNINGS")]
    [InlineData("  Earnings  ")]
    public void ValidateKind_AcceptsEarningsCaseInsensitive(string kind)
    {
        Assert.Equal(CalendarKind.Earnings, CalendarInputValidator.ValidateKind(kind));
    }

    [Theory]
    [InlineData("ipo")]
    [InlineData("IPO")]
    [InlineData("  ipo  ")]
    public void ValidateKind_AcceptsIpoCaseInsensitive(string kind)
    {
        Assert.Equal(CalendarKind.Ipo, CalendarInputValidator.ValidateKind(kind));
    }

    [Fact]
    public void ValidateKind_Economic_ThrowsWithUpgradeHint()
    {
        var ex = Assert.Throws<ArgumentException>(() => CalendarInputValidator.ValidateKind("economic"));
        Assert.Contains("not yet supported", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void ValidateKind_Empty_Throws(string? kind)
    {
        Assert.Throws<ArgumentException>(() => CalendarInputValidator.ValidateKind(kind));
    }

    [Fact]
    public void ValidateKind_UnknownToken_Throws()
    {
        Assert.Throws<ArgumentException>(() => CalendarInputValidator.ValidateKind("dividends"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ValidateSymbol_NullOrWhitespace_ReturnsNull(string? symbol)
    {
        Assert.Null(CalendarInputValidator.ValidateSymbol(symbol));
    }

    [Theory]
    [InlineData("aapl", "AAPL")]
    [InlineData("BRK.A", "BRK.A")]
    [InlineData("rds-a", "RDS-A")]
    public void ValidateSymbol_Valid_NormalisesToUppercase(string input, string expected)
    {
        Assert.Equal(expected, CalendarInputValidator.ValidateSymbol(input));
    }

    [Theory]
    [InlineData("!!!")]
    [InlineData("1AAPL")]
    [InlineData("WAY-TOO-LONG-A-SYMBOL")]
    public void ValidateSymbol_Invalid_Throws(string symbol)
    {
        Assert.Throws<ArgumentException>(() => CalendarInputValidator.ValidateSymbol(symbol));
    }

    [Fact]
    public void ValidateWindow_BothNull_DefaultsToTodayAndPlus90Days()
    {
        var (from, to) = CalendarInputValidator.ValidateWindow(null, null, s_today);
        Assert.Equal(s_today, from);
        Assert.Equal(s_today.AddDays(CalendarInputValidator.MaxEarningsWindowDays), to);
    }

    [Fact]
    public void ValidateWindow_OnlyFromProvided_DefaultsToToFromPlusMax()
    {
        var (from, to) = CalendarInputValidator.ValidateWindow("2026-06-01", null, s_today);
        Assert.Equal(new DateOnly(2026, 6, 1), from);
        Assert.Equal(new DateOnly(2026, 6, 1).AddDays(CalendarInputValidator.MaxEarningsWindowDays), to);
    }

    [Fact]
    public void ValidateWindow_ValidPair_Returned()
    {
        var (from, to) = CalendarInputValidator.ValidateWindow("2026-05-01", "2026-06-01", s_today);
        Assert.Equal(new DateOnly(2026, 5, 1), from);
        Assert.Equal(new DateOnly(2026, 6, 1), to);
    }

    [Fact]
    public void ValidateWindow_ToBeforeFrom_Throws()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            CalendarInputValidator.ValidateWindow("2026-06-01", "2026-05-01", s_today));
        Assert.Contains("must be on or after", ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void ValidateWindow_ExceedsMaxSpan_Throws()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            CalendarInputValidator.ValidateWindow("2026-05-01", "2026-09-01", s_today));
        Assert.Contains("must not exceed", ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void ValidateWindow_Ipo_AllowsUpTo365Days()
    {
        var (from, to) = CalendarInputValidator.ValidateWindow(
            "2026-06-01", "2026-12-31", s_today, CalendarKind.Ipo);
        Assert.Equal(new DateOnly(2026, 6, 1), from);
        Assert.Equal(new DateOnly(2026, 12, 31), to);
    }

    [Fact]
    public void ValidateWindow_IpoExceeds365Days_Throws()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            CalendarInputValidator.ValidateWindow("2026-01-01", "2027-06-01", s_today, CalendarKind.Ipo));
        Assert.Contains("must not exceed 365 days", ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void ValidateWindow_EarningsExceeds90Days_ErrorMessageNamesKind()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            CalendarInputValidator.ValidateWindow("2026-05-01", "2026-09-01", s_today, CalendarKind.Earnings));
        Assert.Contains("'Earnings'", ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void ValidateSymbolForKind_IpoWithSymbol_Throws()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            CalendarInputValidator.ValidateSymbolForKind("AAPL", CalendarKind.Ipo));
        Assert.Contains("does not accept a symbol filter", ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void ValidateSymbolForKind_IpoWithoutSymbol_ReturnsNull()
    {
        Assert.Null(CalendarInputValidator.ValidateSymbolForKind(null, CalendarKind.Ipo));
        Assert.Null(CalendarInputValidator.ValidateSymbolForKind("", CalendarKind.Ipo));
    }

    [Fact]
    public void ValidateSymbolForKind_EarningsWithSymbol_NormalisesAndReturns()
    {
        Assert.Equal("AAPL", CalendarInputValidator.ValidateSymbolForKind("aapl", CalendarKind.Earnings));
    }

    [Theory]
    [InlineData(CalendarKind.Earnings, 90)]
    [InlineData(CalendarKind.Ipo, 365)]
    public void MaxWindowDaysFor_ReturnsKindSpecificCap(CalendarKind kind, int expected)
    {
        Assert.Equal(expected, CalendarInputValidator.MaxWindowDaysFor(kind));
    }

    [Theory]
    [InlineData("2026/05/01")]
    [InlineData("01-05-2026")]
    [InlineData("not-a-date")]
    public void ValidateWindow_BadFromFormat_Throws(string from)
    {
        Assert.Throws<ArgumentException>(() => CalendarInputValidator.ValidateWindow(from, "2026-06-01", s_today));
    }

    [Fact]
    public void ValidateWindow_SameDay_IsValid()
    {
        var (from, to) = CalendarInputValidator.ValidateWindow("2026-05-27", "2026-05-27", s_today);
        Assert.Equal(from, to);
    }

    [Theory]
    [InlineData(null, ToolView.Summary)]
    [InlineData("", ToolView.Summary)]
    [InlineData("summary", ToolView.Summary)]
    [InlineData("Standard", ToolView.Standard)]
    [InlineData("FULL", ToolView.Full)]
    public void ValidateView_AcceptsKnownValuesCaseInsensitive(string? view, ToolView expected)
    {
        Assert.Equal(expected, CalendarInputValidator.ValidateView(view));
    }

    [Fact]
    public void ValidateView_Unknown_Throws()
    {
        Assert.Throws<ArgumentException>(() => CalendarInputValidator.ValidateView("brief"));
    }
}
