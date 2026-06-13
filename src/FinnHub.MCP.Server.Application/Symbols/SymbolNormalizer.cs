// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Symbols;

/// <summary>
/// The single canonical symbol-casing rule shared by every cache-key builder. Centralising it
/// removes the drift where most services upper-cased without trimming and only one trimmed first.
/// </summary>
internal static class SymbolNormalizer
{
    /// <summary>
    /// Trims surrounding whitespace then upper-cases (invariant) a ticker or exchange code, so
    /// <c>" aapl "</c> and <c>"AAPL"</c> resolve to the same cache slot.
    /// </summary>
    /// <param name="symbol">The raw symbol or exchange code.</param>
    /// <returns>The trimmed, upper-invariant form.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="symbol"/> is <c>null</c>.</exception>
    public static string Normalize(string symbol)
    {
        ArgumentNullException.ThrowIfNull(symbol);
        return symbol.Trim().ToUpperInvariant();
    }

    /// <summary>
    /// Like <see cref="Normalize"/>, but maps a <c>null</c> symbol to the literal <c>"all"</c> sentinel
    /// used by the calendar feeds for an all-symbols query. Branches on <c>null</c> only — an empty or
    /// whitespace symbol normalises like any other value, matching the prior calendar behaviour exactly.
    /// </summary>
    /// <param name="symbol">The optional symbol.</param>
    /// <returns><c>"all"</c> when <paramref name="symbol"/> is <c>null</c>; otherwise the normalised symbol.</returns>
    public static string NormalizeOrAll(string? symbol) =>
        symbol is null ? "all" : Normalize(symbol);
}
