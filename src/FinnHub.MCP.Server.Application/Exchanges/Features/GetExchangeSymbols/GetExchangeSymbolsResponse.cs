// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Application.Exchanges.Features.GetExchangeSymbols;

/// <summary>
/// Wire response for <c>get-exchange-symbols</c> — an aggregated, token-conscious view of an
/// exchange's symbol list: the total count, a breakdown by security type, and a capped sample.
/// </summary>
/// <remarks>
/// The raw Finnhub <c>/stock/symbol</c> payload for a major exchange is tens of thousands of rows
/// (the US exchange returns ~30,000). Returning that wholesale would blow the per-view token budget
/// and exceed the cache payload limit, so the service caches this slim aggregate instead. Use
/// <c>search-symbol</c> to resolve a specific ticker on the exchange.
/// </remarks>
[ExcludeFromCodeCoverage]
public sealed class GetExchangeSymbolsResponse
{
    /// <summary>Echo of the requested exchange code.</summary>
    [JsonPropertyName("exchange")]
    public required string Exchange { get; init; }

    /// <summary>Total number of symbols listed on the exchange (the true upstream count, not the sample size).</summary>
    [JsonPropertyName("total_count")]
    public int TotalCount { get; init; }

    /// <summary>Count of symbols per security type, most common first.</summary>
    [JsonPropertyName("type_breakdown")]
    public IReadOnlyDictionary<string, int> TypeBreakdown { get; init; } =
        new Dictionary<string, int>(StringComparer.Ordinal);

    /// <summary>
    /// A capped sample of symbols. Omitted in <c>summary</c> view, 25 rows in <c>standard</c>,
    /// up to 100 in <c>full</c>. Not the complete list — use <c>search-symbol</c> to find a specific ticker.
    /// </summary>
    [JsonPropertyName("symbols")]
    public IReadOnlyList<ExchangeSymbol>? Symbols { get; init; }

    /// <summary>Whether the exchange listed any symbols.</summary>
    [JsonPropertyName("has_results")]
    public bool HasResults => this.TotalCount > 0;
}
