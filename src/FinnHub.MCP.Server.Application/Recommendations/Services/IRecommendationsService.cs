// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Recommendations.Features.GetRecommendations;

namespace FinnHub.MCP.Server.Application.Recommendations.Services;

/// <summary>Application-level entry point for analyst recommendation lookups.</summary>
public interface IRecommendationsService
{
    /// <summary>Returns the latest recommendation snapshot plus the change vs the prior period.</summary>
    Task<Result<GetRecommendationsResponse>> GetRecommendationsAsync(
        GetRecommendationsQuery query,
        CancellationToken cancellationToken = default);
}
