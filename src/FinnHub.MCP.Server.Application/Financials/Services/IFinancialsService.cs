// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Financials.Features.GetFinancialsSnapshot;
using FinnHub.MCP.Server.Application.Models;

namespace FinnHub.MCP.Server.Application.Financials.Services;

/// <summary>
/// Application-level entry point for financials-snapshot lookups. Translates
/// infrastructure exceptions into typed <see cref="Result{T}"/> shapes.
/// </summary>
public interface IFinancialsService
{
    /// <summary>
    /// Executes a financials snapshot lookup and returns a categorised result.
    /// </summary>
    Task<Result<GetFinancialsSnapshotResponse>> GetSnapshotAsync(
        GetFinancialsSnapshotQuery query,
        CancellationToken cancellationToken = default);
}
