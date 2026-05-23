// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.News.Features.GetNewsPulse;

/// <summary>
/// Query parameters for the <c>get-news-pulse</c> tool — aggregated news sentiment
/// and headline pulse over the past 7 days, with a delta vs the prior 7 days.
/// </summary>
public sealed class GetNewsPulseQuery
{
    /// <summary>Per-invocation correlation id; passed through for logging only.</summary>
    public required string QueryId { get; init; }

    /// <summary>Uppercase ticker symbol the pulse is requested for.</summary>
    public required string Symbol { get; init; }

    /// <summary>Whether the response should carry the full headline list rather than the top 5.</summary>
    public bool IncludeAllHeadlines { get; init; }
}
