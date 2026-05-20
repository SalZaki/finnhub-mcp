// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Symbols;

/// <summary>
/// Output of <c>ISymbolResolver.ResolveAsync</c>.
/// </summary>
/// <param name="Canonical">
/// Bare ticker (e.g. <c>"AAPL"</c>) — the form Finnhub's <c>/search</c> endpoint emits and most other
/// endpoints accept. Spec §7 Q4 decision (2026-05-18) standardises on this shape across the milestone.
/// </param>
/// <param name="Display">
/// User-facing form (e.g. <c>"AAPL.US"</c>, <c>"NASDAQ:AAPL"</c>, or the canonical itself when the
/// resolver short-circuits a bare-ticker input).
/// </param>
/// <param name="Exchange">
/// Exchange code when known (e.g. <c>"US"</c>, <c>"NASDAQ"</c>); <c>null</c> for canonical short-circuit
/// or when the upstream omits it.
/// </param>
/// <param name="Confidence">
/// Score on <c>[0.0, 1.0]</c>. The three syntactic fast-paths emit <c>1.0</c> (user supplied a structurally
/// complete identifier). Ambiguous-input resolution passes Finnhub's <c>StockSymbol.ConfidenceScore</c>
/// through verbatim.
/// </param>
/// <param name="Candidates">
/// Ranked list (highest confidence first). The top-level resolved symbol carries up to five candidates;
/// each child carries an empty list to avoid a self-referential serialisation cycle.
/// </param>
/// <remarks>
/// JSON property names defer to <c>FinnHubJsonContext</c>'s snake-case naming policy.
/// </remarks>
public sealed record ResolvedSymbol(
    string Canonical,
    string Display,
    string? Exchange,
    double Confidence,
    IReadOnlyList<ResolvedSymbol> Candidates);
