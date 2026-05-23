// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Peers.Features.GetPeers;

/// <summary>
/// Query parameters for the <c>get-peers</c> tool — a Finnhub <c>/stock/peers</c> lookup
/// for a symbol with optional grouping (industry, subindustry, sector).
/// </summary>
public sealed class GetPeersQuery
{
    /// <summary>Per-invocation correlation id; passed through for logging only.</summary>
    public required string QueryId { get; init; }

    /// <summary>Uppercase ticker symbol the peer set is requested for.</summary>
    public required string Symbol { get; init; }

    /// <summary>Grouping strategy: <c>industry</c> (default), <c>subindustry</c>, or <c>sector</c>.</summary>
    public PeersGrouping Grouping { get; init; } = PeersGrouping.Industry;
}
