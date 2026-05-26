// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using System.Text.Json;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Financials.Clients;
using FinnHub.MCP.Server.Application.Financials.Features.GetFinancialsSnapshot;
using FinnHub.MCP.Server.Application.Options;
using FinnHub.MCP.Server.Infrastructure.Dtos;
using FinnHub.MCP.Server.Infrastructure.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinnHub.MCP.Server.Infrastructure.Clients.Financials;

/// <summary>
/// HTTP client for the Finnhub <c>/stock/metric</c> endpoint. Projects the upstream
/// metric dictionary into the curated <see cref="GetFinancialsSnapshotResponse"/>
/// KPIs and optionally retains the raw dictionary.
/// </summary>
public sealed class FinnHubFinancialsApiClient : IFinancialsApiClient
{
    private const string EndpointName = "financials-metric";

    private readonly HttpClient _httpClient;
    private readonly FinnHubOptions _options;
    private readonly ILogger<FinnHubFinancialsApiClient> _logger;

    /// <summary>Initialises a new <see cref="FinnHubFinancialsApiClient"/>.</summary>
    public FinnHubFinancialsApiClient(
        HttpClient httpClient,
        IOptions<FinnHubOptions> options,
        ILogger<FinnHubFinancialsApiClient> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        this._httpClient = httpClient;
        this._options = options.Value ?? throw new ArgumentException("FinnHub options cannot be null.", nameof(options));
        this._logger = logger;
    }

    /// <inheritdoc />
    public async Task<GetFinancialsSnapshotResponse> GetSnapshotAsync(
        GetFinancialsSnapshotQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var endpoint = this._options.EndPoints.FirstOrDefault(e => e is { IsActive: true, Name: EndpointName })?.Url
                       ?? throw new ArgumentException("Financials metric endpoint is not configured or inactive");

        var requestUri = $"{endpoint.TrimStart('/')}?symbol={Uri.EscapeDataString(query.Symbol)}&metric=all";

        this._logger.LogInformation("Requesting financials from FinnHub: {RequestUri}", requestUri);

        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

        HttpResponseMessage response;
        try
        {
            response = await this._httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            this._logger.LogError(ex, "HTTP request to FinnHub financials failed: {Uri}", requestUri);
            throw new ApiClientHttpException(
                $"HTTP request to FinnHub financials failed: {requestUri}",
                HttpStatusCode.ServiceUnavailable,
                innerException: ex);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            this._logger.LogError(ex, "Financials request timed out: {Uri}", requestUri);
            throw new ApiClientTimeoutException($"Financials request timed out: {requestUri}", ex);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            this._logger.LogWarning("Financials request cancelled: {Uri}", requestUri);
            throw new ApiClientCancelledException($"Financials request cancelled: {requestUri}");
        }

        await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            await this.HandleErrorAsync(response, contentStream, cancellationToken).ConfigureAwait(false);
        }

        FinnHubFinancialsResponse? dto;
        try
        {
            dto = await JsonSerializer
                .DeserializeAsync(contentStream, FinnHubJsonContext.Default.FinnHubFinancialsResponse, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (JsonException ex)
        {
            this._logger.LogError(ex, "Failed to deserialize financials response: {Uri}", requestUri);
            throw new ApiClientDeserializationException(
                $"Failed to deserialize financials response: {requestUri}",
                innerException: ex);
        }

        return Project(query, dto?.Metric);
    }

    private static GetFinancialsSnapshotResponse Project(GetFinancialsSnapshotQuery query, Dictionary<string, JsonElement>? metric)
    {
        double? Get(string key) =>
            metric is not null
            && metric.TryGetValue(key, out var value)
            && value.ValueKind == JsonValueKind.Number
            && value.TryGetDouble(out var d)
                ? d
                : null;

        return new GetFinancialsSnapshotResponse
        {
            Symbol = query.Symbol,
            MarketCap = Get("marketCapitalization"),
            PeTtm = Get("peTTM"),
            PbAnnual = Get("pbAnnual"),
            EpsTtm = Get("epsTTM"),
            DividendYield = Get("dividendYieldIndicatedAnnual"),
            Week52High = Get("52WeekHigh"),
            Week52Low = Get("52WeekLow"),
            Week52PriceReturnPct = Get("52WeekPriceReturnDaily"),
            Beta = Get("beta"),
            RevenuePerShareTtm = Get("revenuePerShareTTM"),
            Raw = query.IncludeRaw && metric is not null
                ? BuildRaw(metric)
                : null
        };
    }

    /// <summary>
    /// Builds the numeric-only subset of the raw upstream metric dictionary.
    /// Non-numeric values (date strings, etc.) are skipped so the wire shape
    /// stays <c>Dictionary&lt;string, double?&gt;</c>.
    /// </summary>
    private static Dictionary<string, double?> BuildRaw(Dictionary<string, JsonElement> metric)
    {
        var raw = new Dictionary<string, double?>(metric.Count, StringComparer.Ordinal);
        foreach (var (key, value) in metric)
        {
            if (value.ValueKind == JsonValueKind.Number && value.TryGetDouble(out var d))
            {
                raw[key] = d;
            }
        }
        return raw;
    }

    private async Task HandleErrorAsync(HttpResponseMessage response, Stream contentStream, CancellationToken ct)
    {
        var statusCode = response.StatusCode;

        using var reader = new StreamReader(contentStream);
        var errorBody = await reader.ReadToEndAsync(ct).ConfigureAwait(false);

        if (statusCode == HttpStatusCode.Forbidden)
        {
            var endpoint = response.RequestMessage?.RequestUri?.AbsolutePath ?? "(unknown)";
            this._logger.LogWarning("Premium-locked financials endpoint: {Endpoint} - {Content}", endpoint, errorBody);
            throw new ApiClientPremiumRequiredException(endpoint, errorBody);
        }

        this._logger.Log(
            (int)statusCode >= 500 ? LogLevel.Error : LogLevel.Warning,
            "Financials API error: {StatusCode} - {Content}", statusCode, errorBody);

        throw new ApiClientHttpException($"FinnHub financials returned {statusCode}.", statusCode, errorBody);
    }
}
