// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using System.Text.Json;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Options;
using FinnHub.MCP.Server.Application.Peers.Clients;
using FinnHub.MCP.Server.Application.Peers.Features.GetPeers;
using FinnHub.MCP.Server.Infrastructure.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinnHub.MCP.Server.Infrastructure.Clients.Peers;

/// <summary>
/// HTTP client for the Finnhub <c>/stock/peers</c> endpoint. The upstream response
/// is a flat JSON array of ticker strings; this client normalises that into a
/// <see cref="GetPeersResponse"/> and translates HTTP failures into typed exceptions.
/// </summary>
public sealed class FinnHubPeersApiClient : IPeersApiClient
{
    private const string EndpointName = "peers";

    private readonly HttpClient _httpClient;
    private readonly FinnHubOptions _options;
    private readonly ILogger<FinnHubPeersApiClient> _logger;

    /// <summary>
    /// Initialises a new <see cref="FinnHubPeersApiClient"/>.
    /// </summary>
    public FinnHubPeersApiClient(
        HttpClient httpClient,
        IOptions<FinnHubOptions> options,
        ILogger<FinnHubPeersApiClient> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        this._httpClient = httpClient;
        this._options = options.Value ?? throw new ArgumentException("FinnHub options cannot be null.", nameof(options));
        this._logger = logger;
    }

    /// <inheritdoc />
    public async Task<GetPeersResponse> GetPeersAsync(GetPeersQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var endpoint = this._options.EndPoints.FirstOrDefault(e => e is { IsActive: true, Name: EndpointName })?.Url
                       ?? throw new ArgumentException("Peers endpoint is not configured or inactive");

        var groupingParam = MapGrouping(query.Grouping);
        var requestUri = $"{endpoint.TrimStart('/')}?symbol={Uri.EscapeDataString(query.Symbol)}&grouping={groupingParam}";

        this._logger.LogInformation("Requesting peers from FinnHub: {RequestUri}", requestUri);

        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

        HttpResponseMessage response;
        try
        {
            response = await this._httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            this._logger.LogError(ex, "HTTP request to FinnHub peers failed: {Uri}", requestUri);
            throw new ApiClientHttpException($"HTTP request to FinnHub peers failed: {requestUri}", HttpStatusCode.InternalServerError);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            this._logger.LogError(ex, "Peers request timed out: {Uri}", requestUri);
            throw new ApiClientTimeoutException($"Peers request timed out: {requestUri}", ex);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            this._logger.LogWarning("Peers request cancelled: {Uri}", requestUri);
            throw new ApiClientCancelledException($"Peers request cancelled: {requestUri}");
        }

        await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            await this.HandleErrorAsync(response, contentStream, cancellationToken).ConfigureAwait(false);
        }

        IReadOnlyList<string> tickers;
        try
        {
            tickers = await JsonSerializer
                .DeserializeAsync(contentStream, FinnHubJsonContext.Default.StringArray, cancellationToken)
                .ConfigureAwait(false) ?? [];
        }
        catch (JsonException ex)
        {
            this._logger.LogError(ex, "Failed to deserialize peers response: {Uri}", requestUri);
            throw new ApiClientDeserializationException(
                $"Failed to deserialize peers response: {requestUri}",
                innerException: ex);
        }

        return new GetPeersResponse
        {
            Peers = tickers,
            Grouping = groupingParam
        };
    }

    private async Task HandleErrorAsync(HttpResponseMessage response, Stream contentStream, CancellationToken ct)
    {
        var statusCode = response.StatusCode;

        using var reader = new StreamReader(contentStream);
        var errorBody = await reader.ReadToEndAsync(ct).ConfigureAwait(false);

        if (statusCode == HttpStatusCode.Forbidden)
        {
            var endpoint = response.RequestMessage?.RequestUri?.AbsolutePath ?? "(unknown)";
            this._logger.LogWarning("Premium-locked peers endpoint: {Endpoint} - {Content}", endpoint, errorBody);
            throw new ApiClientPremiumRequiredException(endpoint, errorBody);
        }

        this._logger.Log(
            (int)statusCode >= 500 ? LogLevel.Error : LogLevel.Warning,
            "Peers API error: {StatusCode} - {Content}", statusCode, errorBody);

        throw new ApiClientHttpException(
            $"FinnHub peers returned {statusCode}.", statusCode, errorBody);
    }

    private static string MapGrouping(PeersGrouping grouping) => grouping switch
    {
        PeersGrouping.Industry => "industry",
        PeersGrouping.SubIndustry => "subIndustry",
        PeersGrouping.Sector => "sector",
        _ => throw new ArgumentOutOfRangeException(nameof(grouping), grouping, "Unknown peer grouping")
    };
}
