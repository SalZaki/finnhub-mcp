// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Caching;

/// <summary>
/// Mutability tier of an upstream Finnhub endpoint, used by <c>IFinnHubCache</c>
/// to look up the appropriate TTL for a cached entry.
/// </summary>
/// <remarks>
/// Ordered shortest-TTL to longest-TTL. The enum is internal labeling only —
/// it is never serialized to the wire. Actual TTL durations live in
/// <see cref="FinnHub.MCP.Server.Application.Options.CacheOptions"/> and are
/// resolved via <c>CacheOptions.GetTtl(tier)</c>.
/// </remarks>
public enum CacheTier
{
    /// <summary>
    /// Live market data (quotes, last trade). Default TTL ~10s.
    /// </summary>
    Quote,

    /// <summary>
    /// News articles, sentiment, and symbol search results. Default TTL ~60s.
    /// </summary>
    News,

    /// <summary>
    /// Reported financials, KPI snapshots, basic metrics. Default TTL ~1 hour.
    /// </summary>
    Financials,

    /// <summary>
    /// Company profiles, peer lists, sector classification. Default TTL ~24 hours.
    /// </summary>
    Profile,

    /// <summary>
    /// Stock exchange catalogues and other near-static reference data. Default TTL ~7 days.
    /// </summary>
    Exchanges
}
