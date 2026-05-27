// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Text.RegularExpressions;
using FinnHub.MCP.Server.Application.Models;

namespace FinnHub.MCP.Server.Tools.Insiders;

/// <summary>Validation helpers for the <c>get-insider-signal</c> tool parameters.</summary>
internal static partial class InsidersInputValidator
{
    /// <summary>Default lookback in days when no <c>from</c> is supplied.</summary>
    internal const int DefaultLookbackDays = 30;

    /// <summary>Maximum allowed window in days (matches the news/earnings tier — keeps payloads bounded).</summary>
    internal const int MaxWindowDays = 90;

    [GeneratedRegex(@"^[A-Z][A-Z0-9.\-]{0,19}$", RegexOptions.Compiled)]
    private static partial Regex SymbolRegex();

    public static string ValidateSymbol(string? symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
        {
            throw new ArgumentException("Symbol cannot be empty.", nameof(symbol));
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

    public static (DateOnly From, DateOnly To) ValidateWindow(string? from, string? to, DateOnly today)
    {
        var toDate = ParseOrDefault(to, today, nameof(to));
        var fromDate = ParseOrDefault(from, toDate.AddDays(-DefaultLookbackDays), nameof(from));

        if (toDate < fromDate)
        {
            throw new ArgumentException(
                $"'to' ({toDate:yyyy-MM-dd}) must be on or after 'from' ({fromDate:yyyy-MM-dd}).",
                nameof(to));
        }

        var span = toDate.DayNumber - fromDate.DayNumber;
        if (span > MaxWindowDays)
        {
            throw new ArgumentException(
                $"Window must not exceed {MaxWindowDays} days (requested {span}).",
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
