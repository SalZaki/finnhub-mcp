// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.News.Features.GetNewsPulse;

namespace FinnHub.MCP.Server.Application.News.Services;

/// <summary>
/// Application-level entry point for the news-pulse aggregation. Orchestrates
/// the sentiment + current-week + prior-week calls into a single result.
/// </summary>
public interface INewsService
{
    /// <summary>
    /// Executes the news-pulse aggregation and returns a categorised result.
    /// </summary>
    Task<Result<GetNewsPulseResponse>> GetPulseAsync(
        GetNewsPulseQuery query,
        CancellationToken cancellationToken = default);
}
