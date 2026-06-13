// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.RegularExpressions;
using FinnHub.MCP.Server.Application.Models;

namespace FinnHub.MCP.Server.Tools;

/// <summary>
/// Input validators shared by every MCP tool. The single home for the symbol and view rules that
/// had drifted into a private copy inside each tool's <c>&lt;Tool&gt;InputValidator</c>.
/// </summary>
internal static partial class CommonInputValidators
{
    [GeneratedRegex(@"^[A-Z][A-Z0-9.\-]{0,19}$", RegexOptions.Compiled)]
    private static partial Regex SymbolRegex();

    /// <summary>
    /// Validates a required ticker symbol: trims and upper-cases it, throwing on blank or malformed input.
    /// </summary>
    /// <exception cref="ArgumentException">The symbol is blank or does not match the allowed shape.</exception>
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

    /// <summary>
    /// Like <see cref="ValidateSymbol"/>, but returns <c>null</c> on blank input — for feeds (e.g. the
    /// calendar) where an absent symbol means "all". A non-blank symbol is validated identically.
    /// </summary>
    /// <exception cref="ArgumentException">A non-blank symbol does not match the allowed shape.</exception>
    public static string? ValidateOptionalSymbol(string? symbol)
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

    /// <summary>
    /// Maps a view string to <see cref="ToolView"/>. Blank defaults to <see cref="ToolView.Summary"/>;
    /// an unrecognised value throws.
    /// </summary>
    /// <exception cref="ArgumentException">The view is not one of summary, standard, full.</exception>
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
}
