// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Exchanges.Features.GetExchangeSymbols;
using FinnHub.MCP.Server.Application.Models;

namespace FinnHub.MCP.Server.Application.Exchanges.Services;

/// <summary>
/// Returns the aggregated symbol listing for an exchange.
/// </summary>
public interface IExchangeSymbolsService
{
    /// <summary>
    /// Gets the token-conscious symbol aggregate for the exchange named in <paramref name="query"/>.
    /// </summary>
    /// <param name="query">The exchange-symbols query.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>A <see cref="Result{T}"/> wrapping the aggregate, or a typed failure.</returns>
    Task<Result<GetExchangeSymbolsResponse>> GetExchangeSymbolsAsync(
        GetExchangeSymbolsQuery query,
        CancellationToken cancellationToken = default);
}
