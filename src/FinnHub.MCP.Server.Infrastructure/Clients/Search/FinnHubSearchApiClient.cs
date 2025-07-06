// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using FinnHub.MCP.Server.Application.Options;
using FinnHub.MCP.Server.Application.Search.Clients;
using FinnHub.MCP.Server.Application.Search.Features.SearchSymbol;
using FinnHub.MCP.Server.Infrastructure.Dtos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinnHub.MCP.Server.Infrastructure.Clients.Search;

/// <summary>
/// Provides HTTP client implementation for searching financial symbols via the FinnHub API.
/// </summary>
/// <remarks>
/// This class implements the <see cref="ISearchClient"/> interface to provide symbol search functionality using the
/// FinnHub REST API. It handles HTTP communication, error mapping, response deserialization, and proper resource
/// disposal. The client supports configurable endpoints and includes comprehensive error handling and logging.
/// </remarks>
public sealed class FinnHubSearchApiClient : ISearchClient, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly FinnHubOptions _finnHubOptions;
    private readonly ILogger<FinnHubSearchApiClient> _logger;
    private readonly JsonSerializerOptions _jsonOptions;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="FinnHubSearchApiClient"/> class.
    /// </summary>
    /// <param name="httpClientFactory">The HTTP client factory for creating HTTP clients.</param>
    /// <param name="options">The FinnHub configuration options.</param>
    /// <param name="logger">The logger for recording operations and errors.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when any of the required parameters are <c>null</c>.
    /// </exception>
    public FinnHubSearchApiClient(
        IHttpClientFactory httpClientFactory,
        IOptions<FinnHubOptions> options,
        ILogger<FinnHubSearchApiClient> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClientFactory);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        this._httpClient = httpClientFactory.CreateClient("FinnHub");
        this._finnHubOptions = options.Value ?? throw new ArgumentException("FinnHub options cannot be null.", nameof(options));
        this._logger = logger;

        this._jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true
        };
    }

    /// <summary>
    /// Searches for financial symbols asynchronously based on the provided query parameters.
    /// </summary>
    /// <param name="query">The search query containing the search criteria and parameters.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the asynchronous operation.</param>
    /// <returns>
    /// A task that represents the asynchronous search operation. The task result contains
    /// a <see cref="SearchSymbolResponse"/> with the search results or error information.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="query"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the search symbol endpoint is not configured or inactive.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// Thrown when the client has been disposed.
    /// </exception>
    /// <exception cref="HttpRequestException">
    /// Thrown when the HTTP request fails.
    /// </exception>
    /// <exception cref="TaskCanceledException">
    /// Thrown when the request times out.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Thrown when the operation is canceled via the <paramref name="cancellationToken"/>.
    /// </exception>
    public async Task<SearchSymbolResponse> SearchSymbolAsync(SearchSymbolQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        ObjectDisposedException.ThrowIf(this._disposed, this);

        var stopwatch = Stopwatch.StartNew();
        var searchTimestamp = DateTime.UtcNow;

        // Get the search endpoint
        var searchEndpoint = this.GetSearchEndpoint();

        if (searchEndpoint == null)
        {
            this._logger.LogError("Search symbol endpoint is not configured or inactive");
            throw new ArgumentException("Search symbol endpoint is not configured or inactive");
        }

        var requestUri = this.BuildRequestUri(searchEndpoint, query);
        this._logger.LogInformation("Requesting search symbols from FinnHub Api: {RequestUri}", requestUri);

        try
        {
            this._logger.LogInformation("Starting symbol search for query: {Query} with ID: {QueryId}", query.Query, query.QueryId);

            var stockSymbols = await this.ExecuteSearchRequestAsync(requestUri, cancellationToken);

            return new SearchSymbolResponse
            {
                Symbols = stockSymbols.Select(MapToSymbolResult).ToList().AsReadOnly(),
                Query = query.Query,
                SearchDuration = stopwatch.Elapsed,
                SearchTimestamp = searchTimestamp,
                Source = "FinnHub",
                IsFromCache = false
            };
        }

        catch (HttpRequestException ex)
        {
            this._logger.LogError(ex, "HTTP request failed for symbol search: {Query}", query.Query);

            throw;
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            this._logger.LogWarning(ex, "Search request timed out for query: {Query}", query.Query);

            throw;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            this._logger.LogInformation("Search operation was cancelled for query: {Query}", query.Query);

            throw;
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, "Unexpected error during symbol search: {Query}", query.Query);
            throw;
        }
    }

    /// <summary>
    /// Gets the active search endpoint URL from the configuration.
    /// </summary>
    /// <returns>The search endpoint URL if configured and active; otherwise, <c>null</c>.</returns>
    private string? GetSearchEndpoint()
    {
        return this._finnHubOptions
            .EndPoints
            .FirstOrDefault(x => x is { IsActive: true, Name: "search-symbol" })
            ?.Url;
    }

    /// <summary>
    /// Builds the complete request URI for the symbol search API call.
    /// </summary>
    /// <param name="searchEndpoint">The search endpoint path.</param>
    /// <param name="symbolSearchQuery">The search query parameters.</param>
    /// <returns>The complete request URI with query parameters.</returns>
    private string BuildRequestUri(string searchEndpoint, SearchSymbolQuery symbolSearchQuery)
    {
        var uriBuilder = new StringBuilder()
            .Append(this._finnHubOptions.BaseUrl.TrimEnd('/'))
            .Append('/')
            .Append(searchEndpoint.TrimStart('/'))
            .Append("?q=")
            .Append(Uri.EscapeDataString(symbolSearchQuery.Query));

        if (!string.IsNullOrWhiteSpace(symbolSearchQuery.Exchange))
        {
            uriBuilder
                .Append("&exchange=")
                .Append(Uri.EscapeDataString(symbolSearchQuery.Exchange));
        }

        // Add the API token if configured
        if (!string.IsNullOrWhiteSpace(this._finnHubOptions.ApiKey))
        {
            uriBuilder
                .Append("&token=")
                .Append(Uri.EscapeDataString(this._finnHubOptions.ApiKey));
        }

        return uriBuilder.ToString();
    }

    /// <summary>
    /// Releases all resources used by the <see cref="FinnHubSearchApiClient"/>.
    /// </summary>
    public void Dispose()
    {
        if (this._disposed)
        {
            return;
        }

        this._disposed = true;
        this._httpClient.Dispose();

        this._logger.LogDebug("SearchClient disposed");
    }

    /// <summary>
    /// Executes the HTTP request to search for symbols and deserializes the response.
    /// </summary>
    /// <param name="requestUri">The complete request URI including query parameters.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the asynchronous operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains
    /// a read-only list of <see cref="FinnHubSymbolResult"/> objects.
    /// </returns>
    /// <exception cref="HttpRequestException">
    /// Thrown when the HTTP request fails or returns an unsuccessful status code.
    /// </exception>
    /// <exception cref="JsonException">
    /// Thrown when the response content cannot be deserialized.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Thrown when the operation is canceled via the <paramref name="cancellationToken"/>.
    /// </exception>
    private async Task<IReadOnlyList<FinnHubSymbolResult>> ExecuteSearchRequestAsync(
        string requestUri,
        CancellationToken cancellationToken)
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
        requestMessage.Headers.Add("X-FinnHub-Token", this._finnHubOptions.ApiKey);

        using var response = await this._httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        var finnHubSearchResponse = JsonSerializer.Deserialize<FinnHubSearchResponse>(content, this._jsonOptions);

        if (finnHubSearchResponse is not { Count: > 0 })
        {
            this._logger.LogWarning("Received empty response from FinnHub Api");
            return Array.Empty<FinnHubSymbolResult>().AsReadOnly();
        }

        return finnHubSearchResponse
            .Result
            .Select(x => new FinnHubSymbolResult
            {
                Symbol = x.Symbol,
                Description = x.Description,
                DisplaySymbol = x.DisplaySymbol,
                Type = x.Type
            })
            .ToList()
            .AsReadOnly();
    }

    /// <summary>
    /// Maps a FinnHub API symbol result to the application symbol result model.
    /// </summary>
    /// <param name="finnHubSymbolResult">The API symbol result to map.</param>
    /// <returns>A <see cref="StockSymbol"/> object mapped from the FinnHub symbol result.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="finnHubSymbolResult"/> is <c>null</c>.
    /// </exception>
    private static StockSymbol MapToSymbolResult(FinnHubSymbolResult finnHubSymbolResult)
    {
        return new StockSymbol
        {
            Symbol = finnHubSymbolResult.Symbol ?? string.Empty,
            Description = finnHubSymbolResult.Description ?? string.Empty,
            DisplaySymbol = finnHubSymbolResult.DisplaySymbol ?? finnHubSymbolResult.Symbol ?? string.Empty,
            Type = finnHubSymbolResult.Type ?? string.Empty,
        };
    }
}
