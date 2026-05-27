// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Text.RegularExpressions;
using FinnHub.MCP.Server.Application.Calendar.Features.GetCalendar;
using FinnHub.MCP.Server.Application.Models;

namespace FinnHub.MCP.Server.Tools.Calendar;

/// <summary>Validation helpers for the <c>get-calendar</c> tool parameters.</summary>
internal static partial class CalendarInputValidator
{
    /// <summary>Maximum window for earnings calendars (90 days).</summary>
    internal const int MaxEarningsWindowDays = 90;

    /// <summary>Maximum window for IPO calendars (365 days). IPOs are sparser than earnings releases,
    /// so a wider lookahead stays within token budget while remaining useful for analyst workflows.</summary>
    internal const int MaxIpoWindowDays = 365;

    [GeneratedRegex(@"^[A-Z][A-Z0-9.\-]{0,19}$", RegexOptions.Compiled)]
    private static partial Regex SymbolRegex();

    /// <summary>Returns the maximum allowed window in days for the supplied <see cref="CalendarKind"/>.</summary>
    public static int MaxWindowDaysFor(CalendarKind kind) => kind switch
    {
        CalendarKind.Earnings => MaxEarningsWindowDays,
        CalendarKind.Ipo => MaxIpoWindowDays,
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
            "economic" => throw new ArgumentException(
                $"Calendar kind '{kind}' is not yet supported. Available kinds: earnings, ipo.",
                nameof(kind)),
            _ => throw new ArgumentException(
                "Kind must be one of: earnings, ipo.",
                nameof(kind))
        };
    }

    /// <summary>
    /// Validates that <paramref name="symbol"/> is either omitted or, when supplied,
    /// permitted by the dispatched <paramref name="kind"/>. IPO calendar rejects
    /// symbol — the upstream does not filter by ticker and silently dropping it
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

        return validated;
    }

    public static string? ValidateSymbol(string? symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
        {
            return null;
        }

        var normalised = symbol.Trim().ToUpperInvariant();

        if (!SymbolRegex().IsMatch(normalised))
        {
            throw new ArgumentException(
                "Symbol must be 1-20 chars, start with A-Z, and contain only A-Z, 0-9, '.', '-'.",
                nameof(symbol));
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

    public static ToolView ValidateView(string? view)
    {
        if (string.IsNullOrWhiteSpace(view))
        {
            return ToolView.Summary;
        }

        return view.Trim().ToLowerInvariant() switch
        {
            "summary" => ToolView.Summary,
            "standard" => ToolView.Standard,
            "full" => ToolView.Full,
            _ => throw new ArgumentException("View must be one of: summary, standard, full.", nameof(view))
        };
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
