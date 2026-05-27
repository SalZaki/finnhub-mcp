// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Insiders.Features.GetInsiderSignal;

namespace FinnHub.MCP.Server.Application.Insiders.Clients;

/// <summary>
/// Infrastructure contract for the Finnhub <c>/stock/insider-transactions</c> endpoint.
/// </summary>
public interface IInsidersApiClient
{
    /// <summary>
    /// Fetches insider transactions for <paramref name="symbol"/> within the supplied window.
    /// </summary>
    Task<IReadOnlyList<InsiderTransaction>> GetInsiderTransactionsAsync(
        string symbol,
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken);
}
