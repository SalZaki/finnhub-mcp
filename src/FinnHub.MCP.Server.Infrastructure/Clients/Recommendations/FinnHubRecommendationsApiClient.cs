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
using FinnHub.MCP.Server.Application.Options;
using FinnHub.MCP.Server.Application.Recommendations.Clients;
using FinnHub.MCP.Server.Application.Recommendations.Features.GetRecommendations;
using FinnHub.MCP.Server.Infrastructure.Dtos;
using FinnHub.MCP.Server.Infrastructure.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinnHub.MCP.Server.Infrastructure.Clients.Recommendations;

/// <summary>HTTP client for the Finnhub <c>/stock/recommendation</c> endpoint.</summary>
public sealed class FinnHubRecommendationsApiClient : IRecommendationsApiClient
{
    private const string EndpointName = "recommendation";

    private readonly HttpClient _httpClient;
    private readonly FinnHubOptions _options;
    private readonly ILogger<FinnHubRecommendationsApiClient> _logger;

    /// <summary>Initialises a new <see cref="FinnHubRecommendationsApiClient"/>.</summary>
    public FinnHubRecommendationsApiClient(
        HttpClient httpClient,
        IOptions<FinnHubOptions> options,
        ILogger<FinnHubRecommendationsApiClient> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        this._httpClient = httpClient;
        this._options = options.Value ?? throw new ArgumentException("FinnHub options cannot be null.", nameof(options));
        this._logger = logger;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<RecommendationSnapshot>> GetRecommendationsAsync(
        string symbol,
        CancellationToken cancellationToken)
    {
        var endpoint = this._options.EndPoints.FirstOrDefault(e => e is { IsActive: true, Name: EndpointName })?.Url
                       ?? throw new ArgumentException("Recommendation endpoint is not configured or inactive");

        var requestUri = $"{endpoint.TrimStart('/')}?symbol={Uri.EscapeDataString(symbol)}";

        this._logger.LogInformation("Requesting recommendations from FinnHub: {RequestUri}", requestUri);

        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

        HttpResponseMessage response;
        try
        {
            response = await this._httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            this._logger.LogError(ex, "HTTP request to FinnHub recommendations failed: {Uri}", requestUri);
            throw new ApiClientHttpException(
                $"HTTP request to FinnHub recommendations failed: {requestUri}",
                HttpStatusCode.ServiceUnavailable,
                innerException: ex);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            this._logger.LogError(ex, "Recommendations request timed out: {Uri}", requestUri);
            throw new ApiClientTimeoutException($"Recommendations request timed out: {requestUri}", ex);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            this._logger.LogWarning("Recommendations request cancelled: {Uri}", requestUri);
            throw new ApiClientCancelledException($"Recommendations request cancelled: {requestUri}");
        }

        await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            await this.HandleErrorAsync(response, contentStream, cancellationToken).ConfigureAwait(false);
        }

        FinnHubRecommendationEntry[]? dto;
        try
        {
            dto = await JsonSerializer
                .DeserializeAsync(contentStream, FinnHubJsonContext.Default.FinnHubRecommendationEntryArray, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (JsonException ex)
        {
            this._logger.LogError(ex, "Failed to deserialize recommendations response: {Uri}", requestUri);
            throw new ApiClientDeserializationException(
                $"Failed to deserialize recommendations response: {requestUri}",
                innerException: ex);
        }

        if (dto is null || dto.Length == 0)
        {
            return [];
        }

        var snapshots = new List<RecommendationSnapshot>(dto.Length);
        foreach (var entry in dto)
        {
            // Period + at least one bucket value is required. Skip malformed rows.
            if (string.IsNullOrWhiteSpace(entry.Period))
            {
                continue;
            }

            if (!DateOnly.TryParseExact(entry.Period, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var period))
            {
                continue;
            }

            snapshots.Add(new RecommendationSnapshot
            {
                Period = period,
                StrongBuy = entry.StrongBuy ?? 0,
                Buy = entry.Buy ?? 0,
                Hold = entry.Hold ?? 0,
                Sell = entry.Sell ?? 0,
                StrongSell = entry.StrongSell ?? 0
            });
        }

        return snapshots;
    }

    private async Task HandleErrorAsync(HttpResponseMessage response, Stream contentStream, CancellationToken ct)
    {
        var statusCode = response.StatusCode;

        using var reader = new StreamReader(contentStream);
        var errorBody = await reader.ReadToEndAsync(ct).ConfigureAwait(false);

        if (statusCode == HttpStatusCode.Forbidden)
        {
            var endpoint = response.RequestMessage?.RequestUri?.AbsolutePath ?? "(unknown)";
            this._logger.LogWarning("Premium-locked recommendations endpoint: {Endpoint} - {Content}", endpoint, errorBody);
            throw new ApiClientPremiumRequiredException(endpoint, errorBody);
        }

        this._logger.Log(
            (int)statusCode >= 500 ? LogLevel.Error : LogLevel.Warning,
            "Recommendations API error: {StatusCode} - {Content}", statusCode, errorBody);

        throw new ApiClientHttpException($"FinnHub recommendations returned {statusCode}.", statusCode, errorBody);
    }
}
