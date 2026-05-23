// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.RegularExpressions;
using FinnHub.MCP.Server.Application.Models;

namespace FinnHub.MCP.Server.Tools.News;

/// <summary>Validation helpers for the <c>get-news-pulse</c> tool parameters.</summary>
internal static partial class NewsInputValidator
{
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
