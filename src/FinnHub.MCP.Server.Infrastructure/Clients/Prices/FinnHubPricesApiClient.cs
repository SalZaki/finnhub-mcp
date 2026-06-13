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
using FinnHub.MCP.Server.Application.Prices.Clients;
using FinnHub.MCP.Server.Application.Prices.Features.GetPriceSummary;
using FinnHub.MCP.Server.Infrastructure.Clients.Http;
using FinnHub.MCP.Server.Infrastructure.Dtos;
using FinnHub.MCP.Server.Infrastructure.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinnHub.MCP.Server.Infrastructure.Clients.Prices;

/// <summary>
/// HTTP client for the Finnhub <c>/stock/candle</c> endpoint. Aggregates the
/// returned OHLCV arrays into curated summary stats.
/// </summary>
public sealed class FinnHubPricesApiClient : IPricesApiClient
{
    private const string EndpointName = "candle";

    private readonly HttpClient _httpClient;
    private readonly FinnHubOptions _options;
    private readonly ILogger<FinnHubPricesApiClient> _logger;

    /// <summary>Initialises a new <see cref="FinnHubPricesApiClient"/>.</summary>
    public FinnHubPricesApiClient(
        HttpClient httpClient,
        IOptions<FinnHubOptions> options,
        ILogger<FinnHubPricesApiClient> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        this._httpClient = httpClient;
        this._options = options.Value ?? throw new ArgumentException("FinnHub options cannot be null.", nameof(options));
        this._logger = logger;
    }

    /// <inheritdoc />
    public async Task<GetPriceSummaryResponse> GetSummaryAsync(
        GetPriceSummaryQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var endpoint = this._options.EndPoints.FirstOrDefault(e => e is { IsActive: true, Name: EndpointName })?.Url
                       ?? throw new ArgumentException("Candle endpoint is not configured or inactive");

        var (resolution, from, to, periodLabel) = ResolvePeriod(query.Period);

        var requestUri = string.Create(
            CultureInfo.InvariantCulture,
            $"{endpoint.TrimStart('/')}?symbol={Uri.EscapeDataString(query.Symbol)}&resolution={resolution}&from={from}&to={to}");

        this._logger.LogInformation("Requesting candles from FinnHub: {RequestUri}", requestUri);

        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

        HttpResponseMessage response;
        try
        {
            response = await this._httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            this._logger.LogError(ex, "HTTP request to FinnHub candle failed: {Uri}", requestUri);
            throw new ApiClientHttpException(
                $"HTTP request to FinnHub candle failed: {requestUri}",
                HttpStatusCode.ServiceUnavailable,
                innerException: ex);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            this._logger.LogError(ex, "Candle request timed out: {Uri}", requestUri);
            throw new ApiClientTimeoutException($"Candle request timed out: {requestUri}", ex);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            this._logger.LogWarning("Candle request cancelled: {Uri}", requestUri);
            throw new ApiClientCancelledException($"Candle request cancelled: {requestUri}");
        }

        await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            await FinnHubResponseErrors.ThrowForStatusAsync(response, contentStream, this._logger, "candle", cancellationToken).ConfigureAwait(false);
        }

        FinnHubCandleResponse? dto;
        try
        {
            dto = await JsonSerializer
                .DeserializeAsync(contentStream, FinnHubJsonContext.Default.FinnHubCandleResponse, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (JsonException ex)
        {
            this._logger.LogError(ex, "Failed to deserialize candle response: {Uri}", requestUri);
            throw new ApiClientDeserializationException(
                $"Failed to deserialize candle response: {requestUri}",
                innerException: ex);
        }

        return Aggregate(query, resolution, periodLabel, dto);
    }

    private static GetPriceSummaryResponse Aggregate(
        GetPriceSummaryQuery query,
        string resolution,
        string periodLabel,
        FinnHubCandleResponse? dto)
    {
        var noData = dto is null
                     || string.Equals(dto.Status, "no_data", StringComparison.Ordinal)
                     || dto.Close is null
                     || dto.Close.Length == 0;

        if (noData)
        {
            return new GetPriceSummaryResponse
            {
                Symbol = query.Symbol,
                Period = periodLabel,
                Resolution = resolution,
                CandleCount = 0
            };
        }

        var close = dto!.Close!;
        var high = dto.High ?? [];
        var low = dto.Low ?? [];
        var timestamps = dto.Timestamps ?? [];

        var min = low.Length > 0 ? low.Min() : close.Min();
        var max = high.Length > 0 ? high.Max() : close.Max();
        var mean = close.Average();
        var returnPct = (close[^1] - close[0]) / close[0] * 100.0;
        var vol = StandardDeviation(close);
        var latestTs = timestamps.Length > 0 ? timestamps[^1] : 0;

        return new GetPriceSummaryResponse
        {
            Symbol = query.Symbol,
            Period = periodLabel,
            Resolution = resolution,
            Min = min,
            Max = max,
            Mean = mean,
            ReturnPct = returnPct,
            Vol = vol,
            CandleCount = close.Length,
            Latest = new PriceLatest(close[^1], DateTimeOffset.FromUnixTimeSeconds(latestTs)),
            Candles = query.IncludeCandles
                ? new PriceCandles
                {
                    Open = dto.Open ?? [],
                    High = high,
                    Low = low,
                    Close = close,
                    Volume = dto.Volume ?? [],
                    Timestamps = timestamps
                }
                : null
        };
    }

    private static double StandardDeviation(double[] values)
    {
        if (values.Length < 2)
        {
            return 0d;
        }

        var mean = values.Average();
        var sumOfSquares = values.Sum(v => (v - mean) * (v - mean));
        return Math.Sqrt(sumOfSquares / values.Length);
    }

    private static (string Resolution, long From, long To, string Label) ResolvePeriod(PricePeriod period)
    {
        var now = DateTimeOffset.UtcNow;
        return period switch
        {
            PricePeriod.SevenDays => ("D", now.AddDays(-7).ToUnixTimeSeconds(), now.ToUnixTimeSeconds(), "7d"),
            PricePeriod.ThirtyDays => ("D", now.AddDays(-30).ToUnixTimeSeconds(), now.ToUnixTimeSeconds(), "30d"),
            PricePeriod.NinetyDays => ("D", now.AddDays(-90).ToUnixTimeSeconds(), now.ToUnixTimeSeconds(), "90d"),
            PricePeriod.OneYear => ("W", now.AddDays(-365).ToUnixTimeSeconds(), now.ToUnixTimeSeconds(), "1y"),
            _ => throw new ArgumentOutOfRangeException(nameof(period), period, "Unknown period")
        };
    }
}
