// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;

namespace FinnHub.MCP.Server.Application.Symbols;

/// <summary>
/// Resolves any of Finnhub's accepted symbol forms to a canonical bare ticker
/// plus auxiliary metadata.
/// </summary>
/// <remarks>
/// <para>
/// The implementation short-circuits three syntactic shapes without an upstream call:
/// </para>
/// <list type="bullet">
///   <item><description>Bare ticker — <c>^[A-Z]{1,8}$</c>, case-insensitive (<c>aapl</c> → <c>AAPL</c>).</description></item>
///   <item><description>Suffix form — <c>^[A-Z]{1,8}\.[A-Z]{1,4}$</c> (<c>AAPL.US</c> → canonical <c>AAPL</c>, exchange <c>US</c>).</description></item>
///   <item><description>Colon-prefixed — <c>^[A-Z]+:[A-Z]{1,8}$</c> (<c>NASDAQ:AAPL</c> → canonical <c>AAPL</c>, exchange <c>NASDAQ</c>).</description></item>
/// </list>
/// <para>
/// Anything else (company name, ISIN, etc.) is routed through <c>ISearchApiClient.SearchSymbolAsync</c>
/// via <c>IFinnHubCache.GetOrCreateAsync</c> at <c>CacheTier.Profile</c> (24h default). The top result
/// becomes the canonical pick; up to five total candidates are surfaced.
/// </para>
/// </remarks>
public interface ISymbolResolver
{
    /// <summary>
    /// Resolves the supplied input to a canonical symbol and ranked candidate list.
    /// </summary>
    /// <param name="input">
    /// Non-null, non-whitespace string up to 500 chars. The resolver uppercases the input internally
    /// for the syntactic short-circuit checks and lower-cases it for the cache key.
    /// </param>
    /// <param name="cancellationToken">Cancels the lookup and any underlying upstream call.</param>
    /// <returns>
    /// <c>Success</c> with a populated <see cref="ResolvedSymbol"/> when the input maps to a canonical
    /// form (fast-path) or when the upstream returns at least one candidate (ambiguous path).
    /// <c>Failure</c> with <c>NotFound</c> when the upstream returns no matches for an ambiguous input.
    /// Upstream HTTP / timeout / deserialisation failures surface as their existing
    /// <see cref="ResultErrorType"/> categories per the <c>SearchService</c> precedent.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="input"/> is null, whitespace, or longer than 500 characters.
    /// </exception>
    Task<Result<ResolvedSymbol>> ResolveAsync(string input, CancellationToken cancellationToken = default);
}
