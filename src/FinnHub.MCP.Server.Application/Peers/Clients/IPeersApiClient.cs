// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Peers.Features.GetPeers;

namespace FinnHub.MCP.Server.Application.Peers.Clients;

/// <summary>
/// Infrastructure contract for the Finnhub <c>/stock/peers</c> endpoint.
/// Implementations handle HTTP transport, deserialization, and exception translation.
/// </summary>
public interface IPeersApiClient
{
    /// <summary>
    /// Fetches the peer ticker list for a symbol.
    /// </summary>
    Task<GetPeersResponse> GetPeersAsync(GetPeersQuery query, CancellationToken cancellationToken);
}
