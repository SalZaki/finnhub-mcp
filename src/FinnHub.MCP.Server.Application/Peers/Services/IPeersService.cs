// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Peers.Features.GetPeers;

namespace FinnHub.MCP.Server.Application.Peers.Services;

/// <summary>
/// Application-level entry point for peer lookups. Translates infrastructure
/// exceptions into typed <see cref="Result{T}"/> shapes the tool can consume.
/// </summary>
public interface IPeersService
{
    /// <summary>
    /// Executes a peer lookup and returns a categorized result.
    /// </summary>
    Task<Result<GetPeersResponse>> GetPeersAsync(GetPeersQuery query, CancellationToken cancellationToken = default);
}
