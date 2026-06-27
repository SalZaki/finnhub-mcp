// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Exchanges.Features.GetExchangeSymbols;

namespace FinnHub.MCP.Server.Application.Exchanges.Clients;

/// <summary>
/// Infrastructure contract for the Finnhub <c>/stock/symbol</c> endpoint.
/// </summary>
public interface IExchangesApiClient
{
    /// <summary>
    /// Gets every symbol listed on the given exchange.
    /// </summary>
    /// <param name="exchange">The exchange code, e.g. <c>"US"</c>.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The symbols listed on the exchange; an empty list when none are returned.</returns>
    Task<IReadOnlyList<ExchangeSymbol>> GetSymbolsAsync(string exchange, CancellationToken cancellationToken);
}
