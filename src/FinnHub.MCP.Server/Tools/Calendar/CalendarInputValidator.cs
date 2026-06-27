// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Text.RegularExpressions;
using FinnHub.MCP.Server.Application.Calendar.Features.GetCalendar;

namespace FinnHub.MCP.Server.Tools.Calendar;

/// <summary>Validation helpers for the <c>get-calendar</c> tool parameters.</summary>
internal static partial class CalendarInputValidator
{
    /// <summary>Maximum window for earnings calendars (90 days).</summary>
    internal const int MaxEarningsWindowDays = 90;

    /// <summary>Maximum window for IPO calendars (365 days). IPOs are sparser than earnings releases,
    /// so a wider lookahead stays within token budget while remaining useful for analyst workflows.</summary>
    internal const int MaxIpoWindowDays = 365;

    /// <summary>Maximum window for economic calendars (90 days). Macro events are dense — a typical
    /// month surfaces ~1300 entries globally — so the window is tighter than IPO to keep cached
    /// payloads bounded.</summary>
    internal const int MaxEconomicWindowDays = 90;

    [GeneratedRegex(@"^[A-Z]{2}$", RegexOptions.Compiled)]
    private static partial Regex CountryRegex();

    /// <summary>
    /// Accepted country codes — ISO 3166-1 alpha-2 plus Finnhub pseudo-codes (<c>EU</c>, <c>WW</c>).
    /// Snapshot derived from the captured economic-calendar fixture; extend when Finnhub starts
    /// returning a new code we don't yet recognise.
    /// </summary>
    private static readonly HashSet<string> s_acceptedCountries = new(StringComparer.Ordinal)
    {
        "AE", "AL", "AM", "AO", "AR", "AT", "AU", "AZ", "BD", "BE", "BG", "BO", "BR", "BW", "BY",
        "CA", "CG", "CH", "CL", "CN", "CO", "CR", "CV", "CY", "CZ", "DE", "DK", "DO", "DZ", "EC",
        "EE", "EG", "ES", "ET", "EU", "FI", "FR", "GB", "GE", "GH", "GM", "GR", "HK", "HR", "HU",
        "ID", "IE", "IL", "IN", "IQ", "IR", "IS", "IT", "JO", "JP", "KE", "KG", "KM", "KR", "KW",
        "KZ", "LB", "LK", "LT", "LU", "LV", "LY", "MA", "MD", "ME", "MK", "MN", "MT", "MU", "MV",
        "MX", "MY", "MZ", "NA", "NG", "NL", "NO", "NZ", "OM", "PE", "PG", "PH", "PK", "PL", "PS",
        "PT", "PY", "RO", "RS", "RU", "RW", "SA", "SC", "SE", "SG", "SI", "SK", "SN", "SV", "TH",
        "TN", "TR", "TW", "TZ", "UA", "UG", "US", "UY", "UZ", "VE", "VN", "WW", "XK", "ZA"
    };

    /// <summary>Returns the maximum allowed window in days for the supplied <see cref="CalendarKind"/>.</summary>
    public static int MaxWindowDaysFor(CalendarKind kind) => kind switch
    {
        CalendarKind.Earnings => MaxEarningsWindowDays,
        CalendarKind.Ipo => MaxIpoWindowDays,
        CalendarKind.Economic => MaxEconomicWindowDays,
        _ => MaxEarningsWindowDays
    };

    public static CalendarKind ValidateKind(string? kind)
    {
        if (string.IsNullOrWhiteSpace(kind))
        {
            throw new ArgumentException("Kind cannot be empty.", nameof(kind));
        }

        return kind.Trim().ToLowerInvariant() switch
        {
            "earnings" => CalendarKind.Earnings,
            "ipo" => CalendarKind.Ipo,
            "economic" => CalendarKind.Economic,
            _ => throw new ArgumentException(
                "Kind must be one of: earnings, ipo, economic.",
                nameof(kind))
        };
    }

    /// <summary>
    /// Validates that <paramref name="symbol"/> is either omitted or, when supplied,
    /// permitted by the dispatched <paramref name="kind"/>. IPO and economic calendars
    /// reject symbol — the upstreams do not filter by ticker and silently dropping it
    /// would produce results the user did not ask for.
    /// </summary>
    public static string? ValidateSymbolForKind(string? symbol, CalendarKind kind)
    {
        var validated = ValidateSymbol(symbol);

        if (validated is not null && kind == CalendarKind.Ipo)
        {
            throw new ArgumentException(
                "IPO calendar does not accept a symbol filter — omit 'symbol' when 'kind' is 'ipo'.",
                nameof(symbol));
        }

        if (validated is not null && kind == CalendarKind.Economic)
        {
            throw new ArgumentException(
                "Economic calendar does not accept a symbol filter — use 'country' instead when 'kind' is 'economic'.",
                nameof(symbol));
        }

        return validated;
    }

    private static string? ValidateSymbol(string? symbol) => CommonInputValidators.ValidateOptionalSymbol(symbol);

    /// <summary>
    /// Validates that <paramref name="country"/> is either omitted or, when supplied,
    /// permitted by the dispatched <paramref name="kind"/> AND a recognised ISO 3166-1
    /// alpha-2 code (or Finnhub pseudo-code <c>EU</c>/<c>WW</c>). Country is only
    /// meaningful for the economic calendar.
    /// </summary>
    public static string? ValidateCountryForKind(string? country, CalendarKind kind)
    {
        var validated = ValidateCountry(country);

        if (validated is not null && kind != CalendarKind.Economic)
        {
            throw new ArgumentException(
                "Country filter is only valid when 'kind' is 'economic'.",
                nameof(country));
        }

        return validated;
    }

    public static string? ValidateCountry(string? country)
    {
        if (string.IsNullOrWhiteSpace(country))
        {
            return null;
        }

        var normalised = country.Trim().ToUpperInvariant();

        if (!CountryRegex().IsMatch(normalised))
        {
            throw new ArgumentException(
                "Country must be a 2-letter ISO 3166-1 alpha-2 code (e.g. 'US', 'GB') or Finnhub pseudo-code ('EU', 'WW').",
                nameof(country));
        }

        if (!s_acceptedCountries.Contains(normalised))
        {
            throw new ArgumentException(
                $"Unknown country code '{normalised}'. Expected an ISO 3166-1 alpha-2 code (e.g. 'US', 'GB', 'DE') or Finnhub pseudo-code ('EU', 'WW').",
                nameof(country));
        }

        return normalised;
    }

    public static (DateOnly From, DateOnly To) ValidateWindow(
        string? from,
        string? to,
        DateOnly today,
        CalendarKind kind = CalendarKind.Earnings)
    {
        var maxWindow = MaxWindowDaysFor(kind);
        var fromDate = ParseOrDefault(from, today, nameof(from));
        var toDate = ParseOrDefault(to, fromDate.AddDays(maxWindow), nameof(to));

        if (toDate < fromDate)
        {
            throw new ArgumentException(
                $"'to' ({toDate:yyyy-MM-dd}) must be on or after 'from' ({fromDate:yyyy-MM-dd}).",
                nameof(to));
        }

        var span = toDate.DayNumber - fromDate.DayNumber;
        if (span > maxWindow)
        {
            throw new ArgumentException(
                $"Window must not exceed {maxWindow} days for kind '{kind}' (requested {span}).",
                nameof(to));
        }

        return (fromDate, toDate);
    }

    private static DateOnly ParseOrDefault(string? raw, DateOnly fallback, string paramName)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return fallback;
        }

        if (!DateOnly.TryParseExact(raw.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
        {
            throw new ArgumentException(
                $"'{paramName}' must be an ISO date in 'yyyy-MM-dd' format (received: '{raw}').",
                paramName);
        }

        return parsed;
    }
}
