// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Application.Insiders.Features.GetInsiderSignal;

/// <summary>
/// Wire response for <c>get-insider-signal</c>.
/// </summary>
/// <remarks>
/// <see cref="NetBuySell30d"/> is the signed sum of <see cref="InsiderTransaction.Change"/>
/// across the requested window — positive when insiders are net acquirers, negative when
/// they are net sellers. <see cref="NotableNames"/> ranks unique insiders by absolute
/// trade volume so the most-active executives surface first.
/// </remarks>
[ExcludeFromCodeCoverage]
public sealed class GetInsiderSignalResponse
{
    /// <summary>Echo of the queried ticker symbol.</summary>
    [JsonPropertyName("symbol")]
    public required string Symbol { get; init; }

    /// <summary>Inclusive start of the lookup window.</summary>
    [JsonPropertyName("from")]
    public required DateOnly From { get; init; }

    /// <summary>Inclusive end of the lookup window.</summary>
    [JsonPropertyName("to")]
    public required DateOnly To { get; init; }

    /// <summary>
    /// Net signed share change across the window. Positive = net acquisition,
    /// negative = net disposition. <c>0</c> when there are no transactions.
    /// </summary>
    [JsonPropertyName("net_buy_sell_30d")]
    public long NetBuySell30d { get; init; }

    /// <summary>Top 5 insider names ranked by absolute trade volume (most active first).</summary>
    [JsonPropertyName("notable_names")]
    public required IReadOnlyList<string> NotableNames { get; init; }

    /// <summary>Total number of insider transactions in the window.</summary>
    [JsonPropertyName("total_count")]
    public int TotalCount { get; init; }

    /// <summary>
    /// The most-recent transaction in the window (by <see cref="InsiderTransaction.TransactionDate"/>,
    /// then <see cref="InsiderTransaction.FilingDate"/>). <c>null</c> when there are no transactions.
    /// </summary>
    [JsonPropertyName("latest")]
    public InsiderTransaction? Latest { get; init; }

    /// <summary>
    /// Full transaction list post-DTO mapping, ordered most-recent first. Populated
    /// only when the tool is invoked with <c>view='full'</c>.
    /// </summary>
    [JsonPropertyName("transactions")]
    public IReadOnlyList<InsiderTransaction>? Transactions { get; init; }
}
