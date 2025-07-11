// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics;
using System.Net;
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
/// Implements <see cref="ISearchClient"/> to offer search functionality via FinnHub’s REST API.
/// This includes endpoint configuration, error handling, deserialization, and logging.
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
    /// <param name="httpClientFactory">Factory to create named HTTP clients.</param>
    /// <param name="options">Configuration options for the FinnHub API.</param>
    /// <param name="logger">Logger for diagnostics and monitoring.</param>
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
    ///
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="SearchSymbolUnexpectedException"></exception>
    public async Task<SearchSymbolResponse> SearchSymbolAsync(
        SearchSymbolQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);
        ObjectDisposedException.ThrowIf(this._disposed, this);

        var stopwatch = Stopwatch.StartNew();
        var searchTimestamp = DateTime.UtcNow;

        this._logger.LogInformation("Starting symbol search for query: {Query} with ID: {QueryId}", query.Query, query.QueryId);

        try
        {
            var searchEndpoint = this.GetSearchEndpoint();
            ValidateEndpoint(searchEndpoint);

            var requestUri = this.BuildRequestUri(searchEndpoint, query);
            this._logger.LogInformation("Requesting search symbols from FinnHub API: {RequestUri}", requestUri);

            var stockSymbols = await this.ExecuteSearchRequestAsync(requestUri, cancellationToken).ConfigureAwait(false);

            return CreateSuccessResponse(query, stopwatch.Elapsed, searchTimestamp, stockSymbols);
        }
        catch (Exception ex) when (ex is not (
                     SearchSymbolHttpException or
                     SearchSymbolDeserializationException or
                     SearchSymbolTimeoutException or
                     SearchSymbolCancelledException or
                     SearchSymbolUnexpectedException or
                     ArgumentException))
        {
            this._logger.LogError(ex, "Unexpected error during symbol search for query: {Query}", query.Query);
            throw new SearchSymbolUnexpectedException($"Unexpected error during symbol search: {query.Query}", ex);
        }
    }

    /// <summary>
    /// Gets the active search endpoint URL from the configuration.
    /// </summary>
    /// <returns>The search endpoint URL, or <c>null</c> if none is active.</returns>
    private string? GetSearchEndpoint()
    {
        return this._finnHubOptions
            .EndPoints
            .FirstOrDefault(x => x is { IsActive: true, Name: "search-symbol" })
            ?.Url;
    }

    /// <summary>
    /// Validates that the search endpoint is properly configured.
    /// </summary>
    /// <param name="searchEndpoint">The endpoint to validate.</param>
    /// <exception cref="ArgumentException">Thrown if endpoint is null or whitespace.</exception>
    private static void ValidateEndpoint(string? searchEndpoint)
    {
        if (string.IsNullOrWhiteSpace(searchEndpoint) || string.IsNullOrEmpty(searchEndpoint))
        {
            throw new ArgumentException("Search symbol endpoint is not configured or inactive");
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (this._disposed)
        {
            return;
        }

        this._disposed = true;
        this._httpClient.Dispose();

        this._logger.Log(LogLevel.Debug, "FinnHubSearchApiClient disposed.");
    }

    /// <summary>
    /// Executes the HTTP request to the FinnHub API and processes the response.
    /// </summary>
    /// <param name="requestUri">The full request URI.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of search results.</returns>
    private async Task<IReadOnlyList<FinnHubSymbolResult>> ExecuteSearchRequestAsync(
        string requestUri,
        CancellationToken cancellationToken)
    {
        using var requestMessage = this.CreateHttpRequest(requestUri);

        var response = await this.SendRequestAsync(requestMessage, requestUri, cancellationToken).ConfigureAwait(false);

        return await this.ProcessResponseAsync(response, requestUri, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Creates an HTTP request with the FinnHub API key.
    /// </summary>
    /// <param name="requestUri">The request URI.</param>
    /// <returns>The configured <see cref="HttpRequestMessage"/>.</returns>
    private HttpRequestMessage CreateHttpRequest(string requestUri)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

        if (!string.IsNullOrWhiteSpace(this._finnHubOptions.ApiKey))
        {
            request.Headers.Add("X-FinnHub-Token", this._finnHubOptions.ApiKey);
        }

        return request;
    }

    /// <summary>
    /// Sends an HTTP request and wraps known failure cases into domain-specific exceptions.
    /// </summary>
    /// <param name="requestMessage">The HTTP request message.</param>
    /// <param name="requestUri">The originating URI for logging context.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The HTTP response message.</returns>
    private async Task<HttpResponseMessage> SendRequestAsync(
        HttpRequestMessage requestMessage,
        string requestUri,
        CancellationToken cancellationToken)
    {
        try
        {
            return await this._httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            this._logger.LogError(ex, "HTTP request to FinnHub API failed. URI: {RequestUri}", requestUri);
            throw new SearchSymbolHttpException($"HTTP request to FinnHub API failed: {requestUri}", HttpStatusCode.InternalServerError);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            this._logger.LogError(ex, "Symbol search request timed out: {RequestUri}", requestUri);
            throw new SearchSymbolTimeoutException($"Symbol search timed out: {requestUri}", ex);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            this. _logger.LogWarning("Symbol search operation was cancelled: {RequestUri}", requestUri);
            throw new SearchSymbolCancelledException($"Symbol search cancelled: {requestUri}");
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="finnHubSymbolResult">The raw result from the FinnHub API.</param>
    /// <returns>A <see cref="StockSymbol"/> mapped result.</returns>
    private static StockSymbol MapToSymbolResult(FinnHubSymbolResult finnHubSymbolResult)
    {
        return new StockSymbol
        {
            Symbol = finnHubSymbolResult.Symbol ?? string.Empty,
            Description = finnHubSymbolResult.Description ?? string.Empty,
            DisplaySymbol = finnHubSymbolResult.DisplaySymbol ?? finnHubSymbolResult.Symbol ?? string.Empty,
            Type = finnHubSymbolResult.Type ?? string.Empty
        };
    }

    /// <summary>
    /// Builds the final success response object.
    /// </summary>
    /// <param name="query">The original query submitted by the client.</param>
    /// <param name="duration">How long the search took to execute.</param>
    /// <param name="timestamp">The UTC timestamp when search began.</param>
    /// <param name="stockSymbols">The raw symbol results.</param>
    /// <returns>A <see cref="SearchSymbolResponse"/> object to return to the MCP client.</returns>
    private static SearchSymbolResponse CreateSuccessResponse(
        SearchSymbolQuery query,
        TimeSpan duration,
        DateTime timestamp,
        IReadOnlyList<FinnHubSymbolResult> stockSymbols)
    {
        return new SearchSymbolResponse
        {
            Query = query.Query,
            QueryId = query.QueryId,
            SearchDuration = duration,
            SearchTimestamp = timestamp,
            Source = "finnhub-api",
            Symbols = stockSymbols
                .Select(MapToSymbolResult)
                .ToList()
                .AsReadOnly(),
        };
    }

    /// <summary>
    /// Validates the HTTP response, throws meaningful exceptions on failure,
    /// and deserializes valid responses into a list of <see cref="FinnHubSymbolResult"/>.
    /// </summary>
    /// <param name="response">The HTTP response message from FinnHub API.</param>
    /// <param name="requestUri">The originating request URI (for logging and debugging).</param>
    /// <param name="cancellationToken">The token used to monitor for cancellation requests.</param>
    /// <returns>A list of <see cref="FinnHubSymbolResult"/> if response is successful.</returns>
    /// <exception cref="SearchSymbolHttpException">When the API response status is not successful.</exception>
    /// <exception cref="SearchSymbolDeserializationException">When the API response cannot be parsed.</exception>
    private async Task<IReadOnlyList<FinnHubSymbolResult>> ProcessResponseAsync(
        HttpResponseMessage response,
        string requestUri,
        CancellationToken cancellationToken)
    {
        await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            await this.HandleErrorResponseAsync(response, contentStream, cancellationToken);
        }

        return await this.DeserializeResponseAsync(contentStream, requestUri, cancellationToken).ConfigureAwait(false);
    }

     /// <summary>
    /// Handles HTTP responses with non-success status codes by reading the body and logging an appropriate message.
    /// </summary>
    /// <param name="response">The response with error status.</param>
    /// <param name="contentStream">The stream containing response body.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <exception cref="SearchSymbolHttpException">Thrown with enriched details about the error.</exception>
    private async Task HandleErrorResponseAsync(
        HttpResponseMessage response,
        Stream contentStream,
        CancellationToken cancellationToken)
    {
        var statusCode = response.StatusCode;
        contentStream.Position = 0;

        using var reader = new StreamReader(contentStream);
        var errorBody = await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);

        if ((int)statusCode >= 400 && (int)statusCode < 500)
        {
            this._logger.LogWarning("Client error from FinnHub API: {StatusCode} - {Content}", statusCode, errorBody);
        }
        else
        {
            this._logger.LogError("Server error from FinnHub API: {StatusCode} - {Content}", statusCode, errorBody);
        }

        throw new SearchSymbolHttpException(
            $"FinnHub API returned error status {statusCode}. See logs for more detail.",
            statusCode,
            errorBody);
    }

    /// <summary>
    /// Deserializes a successful JSON response into a strongly typed result object.
    /// </summary>
    /// <param name="contentStream">The response content stream.</param>
    /// <param name="requestUri">The request URI (used for logging context).</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of <see cref="FinnHubSymbolResult"/> or an empty list.</returns>
    /// <exception cref="SearchSymbolDeserializationException">If deserialization fails.</exception>
    private async Task<IReadOnlyList<FinnHubSymbolResult>> DeserializeResponseAsync(
        Stream contentStream,
        string requestUri,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await JsonSerializer
                .DeserializeAsync<FinnHubSearchResponse>(contentStream, this._jsonOptions, cancellationToken)
                .ConfigureAwait(false);

            if (response?.Count == 0 || response?.Result == null || response.Result.Count == 0)
            {
                this._logger.LogInformation("FinnHub returned no results for request: {RequestUri}", requestUri);
                return Array.Empty<FinnHubSymbolResult>().AsReadOnly();
            }

            return response.Result
                .Select(result => new FinnHubSymbolResult
                {
                    Symbol = result.Symbol,
                    Description = result.Description,
                    DisplaySymbol = result.DisplaySymbol,
                    Type = result.Type
                })
                .ToList()
                .AsReadOnly();
        }
        catch (JsonException ex)
        {
            this._logger.LogError(ex, "Failed to deserialize FinnHub API response.");
            throw new SearchSymbolDeserializationException("Invalid JSON returned from FinnHub API.", ex);
        }
    }

    /// <summary>
    /// Constructs the full request URI for the FinnHub symbol search endpoint.
    /// </summary>
    /// <param name="searchEndpoint">The endpoint path to be used for symbol search.</param>
    /// <param name="query">The search query containing symbol and exchange information.</param>
    /// <returns>A fully qualified request URI string with query parameters.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="searchEndpoint"/> or <paramref name="query"/> is null.</exception>
    private string BuildRequestUri(string? searchEndpoint, SearchSymbolQuery query)
    {
        ArgumentNullException.ThrowIfNull(searchEndpoint);
        ArgumentNullException.ThrowIfNull(query);

        var baseUrl = this._finnHubOptions.BaseUrl.TrimEnd('/');
        var endpoint = searchEndpoint.TrimStart('/');
        var queryParam = Uri.EscapeDataString(query.Query);

        var uriBuilder = new StringBuilder()
            .Append(baseUrl)
            .Append('/')
            .Append(endpoint)
            .Append("?q=")
            .Append(queryParam);

        if (!string.IsNullOrWhiteSpace(query.Exchange))
        {
            uriBuilder
                .Append("&exchange=")
                .Append(Uri.EscapeDataString(query.Exchange));
        }

        return uriBuilder.ToString();
    }
}
