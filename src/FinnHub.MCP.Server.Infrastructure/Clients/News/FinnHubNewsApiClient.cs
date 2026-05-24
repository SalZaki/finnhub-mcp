// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Net;
using System.Text.Json;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.News.Clients;
using FinnHub.MCP.Server.Application.News.Features.GetNewsPulse;
using FinnHub.MCP.Server.Application.Options;
using FinnHub.MCP.Server.Infrastructure.Dtos;
using FinnHub.MCP.Server.Infrastructure.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinnHub.MCP.Server.Infrastructure.Clients.News;

/// <summary>
/// HTTP client for Finnhub's news endpoints (<c>/news-sentiment</c> and <c>/company-news</c>).
/// </summary>
public sealed class FinnHubNewsApiClient : INewsApiClient
{
    private const string CompanyNewsEndpoint = "company-news";
    private const string SentimentEndpoint = "news-sentiment";

    private readonly HttpClient _httpClient;
    private readonly FinnHubOptions _options;
    private readonly ILogger<FinnHubNewsApiClient> _logger;

    /// <summary>Initialises a new <see cref="FinnHubNewsApiClient"/>.</summary>
    public FinnHubNewsApiClient(
        HttpClient httpClient,
        IOptions<FinnHubOptions> options,
        ILogger<FinnHubNewsApiClient> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        this._httpClient = httpClient;
        this._options = options.Value ?? throw new ArgumentException("FinnHub options cannot be null.", nameof(options));
        this._logger = logger;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<CompanyNewsArticle>> GetCompanyNewsAsync(
        string symbol,
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken)
    {
        var endpoint = this.ResolveEndpoint(CompanyNewsEndpoint);
        var requestUri = $"{endpoint.TrimStart('/')}?symbol={Uri.EscapeDataString(symbol)}" +
                         $"&from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}";

        await using var stream = await this.GetStreamAsync(requestUri, cancellationToken).ConfigureAwait(false);

        FinnHubCompanyNewsArticle[]? articles;
        try
        {
            articles = await JsonSerializer
                .DeserializeAsync(stream, FinnHubJsonContext.Default.FinnHubCompanyNewsArticleArray, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (JsonException ex)
        {
            this._logger.LogError(ex, "Failed to deserialize company-news response: {Uri}", requestUri);
            throw new ApiClientDeserializationException(
                $"Failed to deserialize company-news response: {requestUri}",
                innerException: ex);
        }

        return articles is null
            ? []
            : articles
                .Where(a => !string.IsNullOrEmpty(a.Headline))
                .Select(a => new CompanyNewsArticle(
                    a.Headline ?? string.Empty,
                    a.Url ?? string.Empty,
                    a.Source ?? string.Empty,
                    a.Datetime is { } ts and > 0
                        ? DateTimeOffset.FromUnixTimeSeconds(ts)
                        : DateTimeOffset.MinValue))
                .ToList()
                .AsReadOnly();
    }

    /// <inheritdoc />
    public async Task<NewsSentiment> GetSentimentAsync(string symbol, CancellationToken cancellationToken)
    {
        var endpoint = this.ResolveEndpoint(SentimentEndpoint);
        var requestUri = $"{endpoint.TrimStart('/')}?symbol={Uri.EscapeDataString(symbol)}";

        await using var stream = await this.GetStreamAsync(requestUri, cancellationToken).ConfigureAwait(false);

        FinnHubNewsSentimentResponse? dto;
        try
        {
            dto = await JsonSerializer
                .DeserializeAsync(stream, FinnHubJsonContext.Default.FinnHubNewsSentimentResponse, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (JsonException ex)
        {
            this._logger.LogError(ex, "Failed to deserialize sentiment response: {Uri}", requestUri);
            throw new ApiClientDeserializationException(
                $"Failed to deserialize sentiment response: {requestUri}",
                innerException: ex);
        }

        return new NewsSentiment(
            dto?.CompanyNewsScore,
            dto?.Sentiment?.BullishPercent,
            dto?.Sentiment?.BearishPercent);
    }

    private string ResolveEndpoint(string name) =>
        this._options.EndPoints.FirstOrDefault(e => e.IsActive && string.Equals(e.Name, name, StringComparison.Ordinal))?.Url
        ?? throw new ArgumentException(string.Create(CultureInfo.InvariantCulture, $"Endpoint '{name}' is not configured or inactive"));

    private async Task<Stream> GetStreamAsync(string requestUri, CancellationToken cancellationToken)
    {
        this._logger.LogInformation("Requesting news from FinnHub: {RequestUri}", requestUri);

        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

        HttpResponseMessage response;
        try
        {
            response = await this._httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            this._logger.LogError(ex, "HTTP request to FinnHub news failed: {Uri}", requestUri);
            throw new ApiClientHttpException($"HTTP request to FinnHub news failed: {requestUri}", HttpStatusCode.InternalServerError);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            this._logger.LogError(ex, "News request timed out: {Uri}", requestUri);
            throw new ApiClientTimeoutException($"News request timed out: {requestUri}", ex);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            this._logger.LogWarning("News request cancelled: {Uri}", requestUri);
            throw new ApiClientCancelledException($"News request cancelled: {requestUri}");
        }

        var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            await HandleErrorAsync(response, contentStream, cancellationToken, this._logger).ConfigureAwait(false);
        }

        return contentStream;
    }

    private static async Task HandleErrorAsync(
        HttpResponseMessage response,
        Stream contentStream,
        CancellationToken ct,
        ILogger logger)
    {
        var statusCode = response.StatusCode;

        using var reader = new StreamReader(contentStream);
        var errorBody = await reader.ReadToEndAsync(ct).ConfigureAwait(false);

        if (statusCode == HttpStatusCode.Forbidden)
        {
            var endpoint = response.RequestMessage?.RequestUri?.AbsolutePath ?? "(unknown)";
            logger.LogWarning("Premium-locked news endpoint: {Endpoint} - {Content}", endpoint, errorBody);
            throw new ApiClientPremiumRequiredException(endpoint, errorBody);
        }

        logger.Log(
            (int)statusCode >= 500 ? LogLevel.Error : LogLevel.Warning,
            "News API error: {StatusCode} - {Content}", statusCode, errorBody);

        throw new ApiClientHttpException($"FinnHub news returned {statusCode}.", statusCode, errorBody);
    }
}
