// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Recommendations.Features.GetRecommendations;

/// <summary>
/// Query parameters for the <c>get-recommendations</c> tool — a Finnhub
/// <c>/stock/recommendation</c> lookup that returns the latest analyst-consensus
/// snapshot for a ticker, with the change vs the prior period computed in-memory.
/// </summary>
public sealed class GetRecommendationsQuery
{
    /// <summary>Per-invocation correlation id; passed through for logging only.</summary>
    public required string QueryId { get; init; }

    /// <summary>Uppercase ticker symbol the recommendation snapshot is requested for.</summary>
    public required string Symbol { get; init; }
}
