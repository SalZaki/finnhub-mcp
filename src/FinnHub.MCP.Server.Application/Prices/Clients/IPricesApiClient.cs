// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Prices.Features.GetPriceSummary;

namespace FinnHub.MCP.Server.Application.Prices.Clients;

/// <summary>
/// Infrastructure contract for the Finnhub <c>/stock/candle</c> endpoint.
/// </summary>
public interface IPricesApiClient
{
    /// <summary>
    /// Fetches the candle range for a symbol and aggregates it server-side
    /// into the curated summary stats. <see cref="GetPriceSummaryResponse.Candles"/>
    /// is populated only when <paramref name="query"/> has <c>IncludeCandles = true</c>.
    /// </summary>
    Task<GetPriceSummaryResponse> GetSummaryAsync(GetPriceSummaryQuery query, CancellationToken cancellationToken);
}
