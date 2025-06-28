// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of  FinnHub MCP project and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
//  <summary>
//    Service for searching stock symbols using the FinnHub API.
//  </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text;
using System.Text.Json.Serialization;
using MCP.FinnHub.Server.SSE.Application.Features.Search.Queries;
using MCP.FinnHub.Server.SSE.Models;
using MCP.FinnHub.Server.SSE.Options;
using Microsoft.Extensions.Options;

namespace MCP.FinnHub.Server.SSE.Application.Features.Search.Services;

public sealed class SearchService(
    IHttpClientFactory httpClientFactory,
    IOptions<FinnHubOptions> options,
    ILogger<SearchService> logger)
    : ISearchService, IDisposable
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("FinnHub");
    private readonly FinnHubOptions _finnHubOptions = options.Value;
    private bool _disposed;

    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public async Task<Result<IReadOnlyList<StockSymbol>>> SearchSymbolsAsync(
        SymbolSearchQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        var searchEndpoint = this.GetSearchEndpoint();

        if (searchEndpoint == null)
        {
            logger.LogError("Search symbol endpoint is not configured or inactive");

            return new Result<IReadOnlyList<StockSymbol>>()
                .Failure("Search service is not available", ResultErrorType.ServiceUnavailable);
        }

        var requestUri = this.BuildRequestUri(searchEndpoint, query);

        logger.LogInformation("Requesting search symbols from FinnHub: {RequestUri}", requestUri);

        try
        {
            var symbols = await this.ExecuteSearchRequestAsync(requestUri, cancellationToken);
            logger.LogInformation("Retrieved {Count} symbols from FinnHub", symbols.Count);
            return new Result<IReadOnlyList<StockSymbol>>().Success(symbols);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP request to FinnHub failed for query: {Query}", query.Query);
            return new Result<IReadOnlyList<StockSymbol>>()
                .Failure("Service temporarily unavailable", ResultErrorType.ServiceUnavailable);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            logger.LogWarning(ex, "Request to FinnHub timed out for query: {Query}", query.Query);
            return new Result<IReadOnlyList<StockSymbol>>()
                .Failure("Request timed out", ResultErrorType.Timeout);
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "Failed to deserialize response from FinnHub for query: {Query}", query.Query);
            return new Result<IReadOnlyList<StockSymbol>>()
                .Failure("Invalid response from service", ResultErrorType.InvalidResponse);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while searching symbols for query: {Query}", query.Query);
            throw;
        }
    }

    private string? GetSearchEndpoint()
    {
        return this._finnHubOptions
            .EndPoints
            .FirstOrDefault(x => x is {IsActive: true, Name: "search-symbol"})
            ?.Url;
    }

    private string BuildRequestUri(string searchEndpoint, SymbolSearchQuery qeury)
    {
        var uriBuilder = new StringBuilder()
            .Append(this._finnHubOptions.BaseUrl.TrimEnd('/'))
            .Append('/')
            .Append(searchEndpoint.TrimStart('/'))
            .Append("?q=")
            .Append(Uri.EscapeDataString(qeury.Query));

        if (!string.IsNullOrWhiteSpace(qeury.Exchange))
        {
            uriBuilder
                .Append("&exchange=")
                .Append(Uri.EscapeDataString(qeury.Exchange));
        }

        return uriBuilder.ToString();
    }

    private async Task<IReadOnlyList<StockSymbol>> ExecuteSearchRequestAsync(
        string requestUri,
        CancellationToken cancellationToken)
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
        requestMessage.Headers.Add("X-FinnHub-Token", this._finnHubOptions.ApiKey);

        using var response = await this._httpClient.SendAsync(requestMessage, cancellationToken);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(content))
        {
            logger.LogWarning("Received empty response from FinnHub");

            return [];
        }

        var symbols = JsonSerializer.Deserialize<List<StockSymbol>>(content, this._options);

        return symbols?.AsReadOnly() ?? Array.Empty<StockSymbol>().AsReadOnly();
    }

    public void Dispose()
    {
        if (this._disposed)
        {
            return;
        }

        this._httpClient?.Dispose();
        this._disposed = true;
    }
}
