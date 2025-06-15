// --------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of the MCP FinnHub project and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
//  <summary>
//    Add summary.
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using MCP.FinnHub.Server.SSE.Models;

namespace MCP.FinnHub.Server.SSE.Services;

/// <summary>
/// Provides access to Finnhub API endpoints for retrieving market data.
/// </summary>
public interface IFinnHubService
{
    /// <summary>
    /// Retrieves a list of stock symbols traded on U.S. exchanges from the Finnhub API.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token used to cancel the request.</param>
    /// <returns>
    /// A read-only list of <see cref="StockSymbol"/> instances representing supported U.S. stocks.
    /// </returns>
    /// <exception cref="System.Net.Http.HttpRequestException">Thrown if the HTTP request fails.</exception>
    /// <exception cref="System.Threading.Tasks.TaskCanceledException">Thrown if the request is canceled or times out.</exception>
    Task<IReadOnlyList<StockSymbol>> GetUsStockSymbolsAsync(CancellationToken cancellationToken);
}
