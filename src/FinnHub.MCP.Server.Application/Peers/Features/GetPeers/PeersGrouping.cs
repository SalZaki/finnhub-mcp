// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Peers.Features.GetPeers;

/// <summary>
/// Grouping strategy the Finnhub <c>/stock/peers</c> endpoint uses to determine
/// "peer" companies for a given ticker.
/// </summary>
public enum PeersGrouping
{
    /// <summary>Default — peers within the same GICS industry.</summary>
    Industry,

    /// <summary>Peers within the same GICS sub-industry (tighter than industry).</summary>
    SubIndustry,

    /// <summary>Peers within the broader GICS sector (looser than industry).</summary>
    Sector
}
