// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Application.Recommendations.Features.GetRecommendations;

/// <summary>
/// Wire response for <c>get-recommendations</c>.
/// </summary>
/// <remarks>
/// <see cref="Consensus"/> is a human-readable label derived from the weighted analyst
/// score (Strong Buy = +2, Buy = +1, Hold = 0, Sell = −1, Strong Sell = −2; mean across
/// total coverage). <see cref="ChangeVsPrev"/> is <c>null</c> when Finnhub returns only
/// one period for the symbol.
/// </remarks>
[ExcludeFromCodeCoverage]
public sealed class GetRecommendationsResponse
{
    /// <summary>Echo of the queried ticker symbol.</summary>
    [JsonPropertyName("symbol")]
    public required string Symbol { get; init; }

    /// <summary>Period of the latest snapshot (first day of the month, UTC).</summary>
    [JsonPropertyName("period")]
    public required DateOnly Period { get; init; }

    /// <summary>
    /// Single-label consensus: <c>Strong Buy</c>, <c>Buy</c>, <c>Hold</c>, <c>Sell</c>,
    /// or <c>Strong Sell</c>. Derived from the weighted average rating (range −2..+2).
    /// </summary>
    [JsonPropertyName("consensus")]
    public required string Consensus { get; init; }

    /// <summary>Number of analysts at <c>Strong Buy</c> in the latest period.</summary>
    [JsonPropertyName("strong_buy")]
    public required int StrongBuy { get; init; }

    /// <summary>Number of analysts at <c>Buy</c> in the latest period.</summary>
    [JsonPropertyName("buy")]
    public required int Buy { get; init; }

    /// <summary>Number of analysts at <c>Hold</c> in the latest period.</summary>
    [JsonPropertyName("hold")]
    public required int Hold { get; init; }

    /// <summary>Number of analysts at <c>Sell</c> in the latest period.</summary>
    [JsonPropertyName("sell")]
    public required int Sell { get; init; }

    /// <summary>Number of analysts at <c>Strong Sell</c> in the latest period.</summary>
    [JsonPropertyName("strong_sell")]
    public required int StrongSell { get; init; }

    /// <summary>Total analyst coverage in the latest period.</summary>
    [JsonPropertyName("total")]
    public required int Total { get; init; }

    /// <summary>
    /// Per-bucket delta vs the prior period in the same upstream payload; <c>null</c> when
    /// Finnhub returns only one period.
    /// </summary>
    [JsonPropertyName("change_vs_prev")]
    public RecommendationChange? ChangeVsPrev { get; init; }

    /// <summary>
    /// Full history of period snapshots returned by Finnhub (most-recent first).
    /// Populated only when the tool is invoked with <c>view='full'</c>.
    /// </summary>
    [JsonPropertyName("snapshots")]
    public IReadOnlyList<RecommendationSnapshot>? Snapshots { get; init; }
}
