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
/// One analyst-consensus snapshot from Finnhub's <c>/stock/recommendation</c> feed.
/// Each entry covers a single month (<see cref="Period"/> is the first calendar day).
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class RecommendationSnapshot
{
    /// <summary>Period the snapshot covers (first calendar day of the month, UTC).</summary>
    [JsonPropertyName("period")]
    public required DateOnly Period { get; init; }

    /// <summary>Number of analysts at <c>Strong Buy</c>.</summary>
    [JsonPropertyName("strong_buy")]
    public required int StrongBuy { get; init; }

    /// <summary>Number of analysts at <c>Buy</c>.</summary>
    [JsonPropertyName("buy")]
    public required int Buy { get; init; }

    /// <summary>Number of analysts at <c>Hold</c>.</summary>
    [JsonPropertyName("hold")]
    public required int Hold { get; init; }

    /// <summary>Number of analysts at <c>Sell</c>.</summary>
    [JsonPropertyName("sell")]
    public required int Sell { get; init; }

    /// <summary>Number of analysts at <c>Strong Sell</c>.</summary>
    [JsonPropertyName("strong_sell")]
    public required int StrongSell { get; init; }

    /// <summary>Sum of all rating buckets — total analyst coverage in this period.</summary>
    [JsonPropertyName("total")]
    public int Total => this.StrongBuy + this.Buy + this.Hold + this.Sell + this.StrongSell;
}
