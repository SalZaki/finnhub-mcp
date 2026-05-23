// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Application.Peers.Features.GetPeers;

/// <summary>
/// Wire response for <c>get-peers</c>. Carries the peer ticker list and the grouping
/// strategy that produced it.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class GetPeersResponse
{
    /// <summary>Peer ticker symbols, ordered as returned by Finnhub.</summary>
    [JsonPropertyName("peers")]
    public IReadOnlyList<string> Peers { get; init; } = [];

    /// <summary>Echo of the grouping that produced the list (industry|subindustry|sector).</summary>
    [JsonPropertyName("grouping")]
    public required string Grouping { get; init; }

    /// <summary>Number of peers returned.</summary>
    [JsonPropertyName("total_count")]
    public int TotalCount => this.Peers.Count;

    /// <summary>Whether any peers were returned.</summary>
    [JsonPropertyName("has_results")]
    public bool HasResults => this.TotalCount > 0;
}
