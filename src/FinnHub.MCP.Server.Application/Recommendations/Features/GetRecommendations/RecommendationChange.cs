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
/// Per-bucket delta between the current and previous recommendation snapshots.
/// </summary>
/// <remarks>
/// A positive delta on a bullish bucket (<c>StrongBuyDelta</c>, <c>BuyDelta</c>)
/// or a negative delta on a bearish bucket (<c>SellDelta</c>, <c>StrongSellDelta</c>)
/// indicates sentiment is moving more bullish. <see cref="ConsensusShift"/> rolls the
/// per-bucket motion up into a single label.
/// </remarks>
[ExcludeFromCodeCoverage]
public sealed class RecommendationChange
{
    /// <summary>Period of the snapshot being compared against.</summary>
    [JsonPropertyName("prev_period")]
    public required DateOnly PrevPeriod { get; init; }

    /// <summary>Delta in <c>Strong Buy</c> count vs the previous period.</summary>
    [JsonPropertyName("strong_buy_delta")]
    public required int StrongBuyDelta { get; init; }

    /// <summary>Delta in <c>Buy</c> count vs the previous period.</summary>
    [JsonPropertyName("buy_delta")]
    public required int BuyDelta { get; init; }

    /// <summary>Delta in <c>Hold</c> count vs the previous period.</summary>
    [JsonPropertyName("hold_delta")]
    public required int HoldDelta { get; init; }

    /// <summary>Delta in <c>Sell</c> count vs the previous period.</summary>
    [JsonPropertyName("sell_delta")]
    public required int SellDelta { get; init; }

    /// <summary>Delta in <c>Strong Sell</c> count vs the previous period.</summary>
    [JsonPropertyName("strong_sell_delta")]
    public required int StrongSellDelta { get; init; }

    /// <summary>
    /// Single-label summary of the consensus shift: <c>more bullish</c>,
    /// <c>more bearish</c>, or <c>no change</c>.
    /// </summary>
    [JsonPropertyName("consensus_shift")]
    public required string ConsensusShift { get; init; }
}
