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
using MCP.FinnHub.Server.SSE.Options;
using Microsoft.Extensions.Options;

namespace MCP.FinnHub.Server.SSE.Services;

/// <summary>
/// Concrete implementation of <see cref="IFinnHubService"/> that interacts with the Finnhub API
/// to retrieve stock market data such as supported stock symbols.
/// </summary>
/// <remarks>
/// This service uses a named <c>HttpClient</c> ("finnhub") and expects a configured <see cref="FinnHubOptions"/> object
/// injected via <see cref="IOptions{T}"/>. Logging is provided via <see cref="ILogger{IFinnHubService}"/>.
/// </remarks>
public sealed class FinnHubService(
    IHttpClientFactory httpClientFactory,
    IOptions<FinnHubOptions> options,
    ILogger<IFinnHubService> logger)
    : IFinnHubService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("finnhub");
    private readonly FinnHubOptions _finnHubOptions = options.Value;
    private readonly ILogger<IFinnHubService> _logger = logger;

    /// <summary>
    /// Retrieves a list of stock symbols traded on U.S. exchanges by calling the Finnhub API.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token used to cancel the request if needed.</param>
    /// <returns>
    /// A read-only list of <see cref="StockSymbol"/> objects representing companies traded on U.S. exchanges.
    /// </returns>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request to the Finnhub API fails.</exception>
    /// <exception cref="TaskCanceledException">Thrown when the request is canceled or times out.</exception>
    public async Task<IReadOnlyList<StockSymbol>> GetUsStockSymbolsAsync(CancellationToken cancellationToken = default)
    {
        var endPoint = this._finnHubOptions.BaseUrl + "/stock/symbol?exchange=US";
        this._logger.LogInformation("Requesting stock symbols from Finnhub: {Endpoint}", endPoint);

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, endPoint);
            request.Headers.Add("X-Finnhub-Token", this._finnHubOptions.ApiKey);
            var response = await this._httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
            var symbols = await response.Content.ReadFromJsonAsync<List<StockSymbol>>(cancellationToken: cancellationToken);
            this._logger.LogInformation("Retrieved {Count} stock symbols from Finnhub.", symbols?.Count ?? 0);
            return symbols ?? [];
        }
        catch (HttpRequestException ex)
        {
            this._logger.LogError(ex, "HTTP request to Finnhub failed.");
            throw;
        }
        catch (TaskCanceledException ex)
        {
            this._logger.LogWarning(ex, "Request to Finnhub timed out.");
            throw;
        }
    }
}
