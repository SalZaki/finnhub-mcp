// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Recommendations.Features.GetRecommendations;

namespace FinnHub.MCP.Server.Application.Recommendations.Clients;

/// <summary>
/// Infrastructure contract for the Finnhub <c>/stock/recommendation</c> endpoint.
/// </summary>
public interface IRecommendationsApiClient
{
    /// <summary>
    /// Fetches the analyst-consensus history for <paramref name="symbol"/>. Finnhub
    /// returns the most recent N monthly snapshots in a single call — the service
    /// uses index [0] as the current period and [1] as the previous-period baseline.
    /// </summary>
    Task<IReadOnlyList<RecommendationSnapshot>> GetRecommendationsAsync(
        string symbol,
        CancellationToken cancellationToken);
}
