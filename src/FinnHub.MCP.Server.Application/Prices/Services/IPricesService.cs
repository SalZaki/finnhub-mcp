// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Prices.Features.GetPriceSummary;

namespace FinnHub.MCP.Server.Application.Prices.Services;

/// <summary>
/// Application-level entry point for price-summary lookups.
/// </summary>
public interface IPricesService
{
    /// <summary>
    /// Executes a price summary lookup and returns a categorised result.
    /// </summary>
    Task<Result<GetPriceSummaryResponse>> GetSummaryAsync(
        GetPriceSummaryQuery query,
        CancellationToken cancellationToken = default);
}
