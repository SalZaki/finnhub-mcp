// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Calendar.Features.GetCalendar;
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

    [Theory]
    [InlineData("economic")]
    [InlineData("ECONOMIC")]
    [InlineData("  Economic  ")]
    public void ValidateKind_AcceptsEconomicCaseInsensitive(string kind)
    {
        Assert.Equal(CalendarKind.Economic, CalendarInputValidator.ValidateKind(kind));
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
    [InlineData(CalendarKind.Economic, 90)]
    public void MaxWindowDaysFor_ReturnsKindSpecificCap(CalendarKind kind, int expected)
    {
        Assert.Equal(expected, CalendarInputValidator.MaxWindowDaysFor(kind));
    }

    [Fact]
    public void ValidateWindow_Economic_DefaultsToFromPlus90Days()
    {
        var (from, to) = CalendarInputValidator.ValidateWindow(
            "2026-06-01", null, s_today, CalendarKind.Economic);
        Assert.Equal(new DateOnly(2026, 6, 1), from);
        Assert.Equal(new DateOnly(2026, 6, 1).AddDays(CalendarInputValidator.MaxEconomicWindowDays), to);
    }

    [Fact]
    public void ValidateWindow_EconomicExceeds90Days_Throws()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            CalendarInputValidator.ValidateWindow("2026-01-01", "2026-06-01", s_today, CalendarKind.Economic));
        Assert.Contains("'Economic'", ex.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ValidateCountry_NullOrWhitespace_ReturnsNull(string? country)
    {
        Assert.Null(CalendarInputValidator.ValidateCountry(country));
    }

    [Theory]
    [InlineData("us", "US")]
    [InlineData("GB", "GB")]
    [InlineData("  de  ", "DE")]
    [InlineData("eu", "EU")]
    [InlineData("ww", "WW")]
    public void ValidateCountry_KnownCode_NormalisesToUppercase(string input, string expected)
    {
        Assert.Equal(expected, CalendarInputValidator.ValidateCountry(input));
    }

    [Fact]
    public void ValidateCountry_UnknownCode_Throws()
    {
        var ex = Assert.Throws<ArgumentException>(() => CalendarInputValidator.ValidateCountry("XX"));
        Assert.Contains("Unknown country code", ex.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("USA")]
    [InlineData("U")]
    [InlineData("US1")]
    [InlineData("U-S")]
    public void ValidateCountry_BadFormat_Throws(string country)
    {
        var ex = Assert.Throws<ArgumentException>(() => CalendarInputValidator.ValidateCountry(country));
        Assert.Contains("2-letter ISO", ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void ValidateCountryForKind_EconomicWithKnownCountry_NormalisesAndReturns()
    {
        Assert.Equal("US", CalendarInputValidator.ValidateCountryForKind("us", CalendarKind.Economic));
    }

    [Fact]
    public void ValidateCountryForKind_EconomicWithoutCountry_ReturnsNull()
    {
        Assert.Null(CalendarInputValidator.ValidateCountryForKind(null, CalendarKind.Economic));
        Assert.Null(CalendarInputValidator.ValidateCountryForKind("", CalendarKind.Economic));
    }

    [Theory]
    [InlineData(CalendarKind.Earnings)]
    [InlineData(CalendarKind.Ipo)]
    public void ValidateCountryForKind_NonEconomicWithCountry_Throws(CalendarKind kind)
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            CalendarInputValidator.ValidateCountryForKind("US", kind));
        Assert.Contains("only valid when 'kind' is 'economic'", ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void ValidateSymbolForKind_EconomicWithSymbol_Throws()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            CalendarInputValidator.ValidateSymbolForKind("AAPL", CalendarKind.Economic));
        Assert.Contains("does not accept a symbol filter", ex.Message, StringComparison.Ordinal);
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
}
