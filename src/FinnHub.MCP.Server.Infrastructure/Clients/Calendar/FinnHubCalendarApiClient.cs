// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Net;
using System.Text.Json;
using FinnHub.MCP.Server.Application.Calendar.Clients;
using FinnHub.MCP.Server.Application.Calendar.Features.GetCalendar;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Options;
using FinnHub.MCP.Server.Infrastructure.Clients.Http;
using FinnHub.MCP.Server.Infrastructure.Dtos;
using FinnHub.MCP.Server.Infrastructure.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinnHub.MCP.Server.Infrastructure.Clients.Calendar;

/// <summary>HTTP client for the Finnhub <c>/calendar/*</c> endpoint family.</summary>
public sealed class FinnHubCalendarApiClient : ICalendarApiClient
{
    private const string EarningsEndpointName = "calendar-earnings";
    private const string IpoEndpointName = "calendar-ipo";
    private const string EconomicEndpointName = "calendar-economic";

    private readonly HttpClient _httpClient;
    private readonly FinnHubOptions _options;
    private readonly ILogger<FinnHubCalendarApiClient> _logger;

    /// <summary>Initialises a new <see cref="FinnHubCalendarApiClient"/>.</summary>
    public FinnHubCalendarApiClient(
        HttpClient httpClient,
        IOptions<FinnHubOptions> options,
        ILogger<FinnHubCalendarApiClient> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        this._httpClient = httpClient;
        this._options = options.Value ?? throw new ArgumentException("FinnHub options cannot be null.", nameof(options));
        this._logger = logger;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<EarningsEvent>> GetEarningsCalendarAsync(
        DateOnly from,
        DateOnly to,
        string? symbol,
        CancellationToken cancellationToken)
    {
        var endpoint = this._options.EndPoints.FirstOrDefault(e => e is { IsActive: true, Name: EarningsEndpointName })?.Url
                       ?? throw new ArgumentException("Calendar earnings endpoint is not configured or inactive");

        var requestUri = BuildEarningsUri(endpoint, from, to, symbol);

        this._logger.LogInformation("Requesting earnings calendar from FinnHub: {RequestUri}", requestUri);

        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

        HttpResponseMessage response;
        try
        {
            response = await this._httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            this._logger.LogError(ex, "HTTP request to FinnHub earnings calendar failed: {Uri}", requestUri);
            throw new ApiClientHttpException(
                $"HTTP request to FinnHub earnings calendar failed: {requestUri}",
                HttpStatusCode.ServiceUnavailable,
                innerException: ex);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            this._logger.LogError(ex, "Earnings calendar request timed out: {Uri}", requestUri);
            throw new ApiClientTimeoutException($"Earnings calendar request timed out: {requestUri}", ex);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            this._logger.LogWarning("Earnings calendar request cancelled: {Uri}", requestUri);
            throw new ApiClientCancelledException($"Earnings calendar request cancelled: {requestUri}");
        }

        await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            await FinnHubResponseErrors.ThrowForStatusAsync(response, contentStream, this._logger, "calendar", cancellationToken).ConfigureAwait(false);
        }

        FinnHubEarningsCalendarResponse? dto;
        try
        {
            dto = await JsonSerializer
                .DeserializeAsync(contentStream, FinnHubJsonContext.Default.FinnHubEarningsCalendarResponse, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (JsonException ex)
        {
            this._logger.LogError(ex, "Failed to deserialize earnings calendar response: {Uri}", requestUri);
            throw new ApiClientDeserializationException(
                $"Failed to deserialize earnings calendar response: {requestUri}",
                innerException: ex);
        }

        if (dto?.EarningsCalendar is null || dto.EarningsCalendar.Count == 0)
        {
            return [];
        }

        var events = new List<EarningsEvent>(dto.EarningsCalendar.Count);
        foreach (var entry in dto.EarningsCalendar)
        {
            if (string.IsNullOrWhiteSpace(entry.Symbol) || string.IsNullOrWhiteSpace(entry.Date))
            {
                continue;
            }

            if (!DateOnly.TryParseExact(entry.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                continue;
            }

            events.Add(new EarningsEvent
            {
                Symbol = entry.Symbol,
                Date = date,
                Hour = entry.Hour,
                Quarter = entry.Quarter,
                Year = entry.Year,
                EpsActual = entry.EpsActual,
                EpsEstimate = entry.EpsEstimate,
                RevenueActual = entry.RevenueActual,
                RevenueEstimate = entry.RevenueEstimate
            });
        }

        return events;
    }

    private static string BuildEarningsUri(string endpoint, DateOnly from, DateOnly to, string? symbol)
    {
        var trimmed = endpoint.TrimStart('/');
        var fromStr = from.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var toStr = to.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        return string.IsNullOrWhiteSpace(symbol)
            ? string.Create(CultureInfo.InvariantCulture, $"{trimmed}?from={fromStr}&to={toStr}")
            : string.Create(CultureInfo.InvariantCulture, $"{trimmed}?from={fromStr}&to={toStr}&symbol={Uri.EscapeDataString(symbol)}");
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<IpoEvent>> GetIpoCalendarAsync(
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken)
    {
        var endpoint = this._options.EndPoints.FirstOrDefault(e => e is { IsActive: true, Name: IpoEndpointName })?.Url
                       ?? throw new ArgumentException("Calendar IPO endpoint is not configured or inactive");

        var requestUri = BuildWindowUri(endpoint, from, to);

        this._logger.LogInformation("Requesting IPO calendar from FinnHub: {RequestUri}", requestUri);

        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

        HttpResponseMessage response;
        try
        {
            response = await this._httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            this._logger.LogError(ex, "HTTP request to FinnHub IPO calendar failed: {Uri}", requestUri);
            throw new ApiClientHttpException(
                $"HTTP request to FinnHub IPO calendar failed: {requestUri}",
                HttpStatusCode.ServiceUnavailable,
                innerException: ex);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            this._logger.LogError(ex, "IPO calendar request timed out: {Uri}", requestUri);
            throw new ApiClientTimeoutException($"IPO calendar request timed out: {requestUri}", ex);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            this._logger.LogWarning("IPO calendar request cancelled: {Uri}", requestUri);
            throw new ApiClientCancelledException($"IPO calendar request cancelled: {requestUri}");
        }

        await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            await FinnHubResponseErrors.ThrowForStatusAsync(response, contentStream, this._logger, "calendar", cancellationToken).ConfigureAwait(false);
        }

        FinnHubIpoCalendarResponse? dto;
        try
        {
            dto = await JsonSerializer
                .DeserializeAsync(contentStream, FinnHubJsonContext.Default.FinnHubIpoCalendarResponse, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (JsonException ex)
        {
            this._logger.LogError(ex, "Failed to deserialize IPO calendar response: {Uri}", requestUri);
            throw new ApiClientDeserializationException(
                $"Failed to deserialize IPO calendar response: {requestUri}",
                innerException: ex);
        }

        if (dto?.IpoCalendar is null || dto.IpoCalendar.Count == 0)
        {
            return [];
        }

        var events = new List<IpoEvent>(dto.IpoCalendar.Count);
        foreach (var entry in dto.IpoCalendar)
        {
            // Date + name are the only two fields required to be useful — withdrawn
            // offerings have null symbol/exchange/price but still carry both.
            if (string.IsNullOrWhiteSpace(entry.Name) || string.IsNullOrWhiteSpace(entry.Date))
            {
                continue;
            }

            if (!DateOnly.TryParseExact(entry.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                continue;
            }

            events.Add(new IpoEvent
            {
                Symbol = string.IsNullOrWhiteSpace(entry.Symbol) ? null : entry.Symbol,
                Name = entry.Name,
                Date = date,
                Exchange = string.IsNullOrWhiteSpace(entry.Exchange) ? null : entry.Exchange,
                Price = ParsePrice(entry.Price),
                NumberOfShares = entry.NumberOfShares,
                TotalSharesValue = entry.TotalSharesValue,
                Status = string.IsNullOrWhiteSpace(entry.Status) ? null : entry.Status
            });
        }

        return events;
    }

    private static string BuildWindowUri(string endpoint, DateOnly from, DateOnly to)
    {
        var trimmed = endpoint.TrimStart('/');
        var fromStr = from.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var toStr = to.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        return string.Create(CultureInfo.InvariantCulture, $"{trimmed}?from={fromStr}&to={toStr}");
    }

    private static double? ParsePrice(string? raw) =>
        !string.IsNullOrWhiteSpace(raw)
            && double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed)
            ? parsed
            : null;

    /// <inheritdoc />
    public async Task<IReadOnlyList<EconomicEvent>> GetEconomicCalendarAsync(
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken)
    {
        var endpoint = this._options.EndPoints.FirstOrDefault(e => e is { IsActive: true, Name: EconomicEndpointName })?.Url
                       ?? throw new ArgumentException("Calendar economic endpoint is not configured or inactive");

        var requestUri = BuildWindowUri(endpoint, from, to);

        this._logger.LogInformation("Requesting economic calendar from FinnHub: {RequestUri}", requestUri);

        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

        HttpResponseMessage response;
        try
        {
            response = await this._httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            this._logger.LogError(ex, "HTTP request to FinnHub economic calendar failed: {Uri}", requestUri);
            throw new ApiClientHttpException(
                $"HTTP request to FinnHub economic calendar failed: {requestUri}",
                HttpStatusCode.ServiceUnavailable,
                innerException: ex);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            this._logger.LogError(ex, "Economic calendar request timed out: {Uri}", requestUri);
            throw new ApiClientTimeoutException($"Economic calendar request timed out: {requestUri}", ex);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            this._logger.LogWarning("Economic calendar request cancelled: {Uri}", requestUri);
            throw new ApiClientCancelledException($"Economic calendar request cancelled: {requestUri}");
        }

        await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            await FinnHubResponseErrors.ThrowForStatusAsync(response, contentStream, this._logger, "calendar", cancellationToken).ConfigureAwait(false);
        }

        FinnHubEconomicCalendarResponse? dto;
        try
        {
            dto = await JsonSerializer
                .DeserializeAsync(contentStream, FinnHubJsonContext.Default.FinnHubEconomicCalendarResponse, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (JsonException ex)
        {
            this._logger.LogError(ex, "Failed to deserialize economic calendar response: {Uri}", requestUri);
            throw new ApiClientDeserializationException(
                $"Failed to deserialize economic calendar response: {requestUri}",
                innerException: ex);
        }

        if (dto?.EconomicCalendar is null || dto.EconomicCalendar.Count == 0)
        {
            return [];
        }

        var events = new List<EconomicEvent>(dto.EconomicCalendar.Count);
        foreach (var entry in dto.EconomicCalendar)
        {
            // Country + event name + parseable timestamp are required to be useful.
            // Skip malformed entries rather than failing the whole window.
            if (string.IsNullOrWhiteSpace(entry.Country)
                || string.IsNullOrWhiteSpace(entry.Event)
                || string.IsNullOrWhiteSpace(entry.Time))
            {
                continue;
            }

            if (!DateTime.TryParseExact(
                    entry.Time,
                    "yyyy-MM-dd HH:mm:ss",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                    out var time))
            {
                continue;
            }

            events.Add(new EconomicEvent
            {
                Country = entry.Country,
                EventName = entry.Event,
                Time = time,
                Impact = string.IsNullOrWhiteSpace(entry.Impact) ? null : entry.Impact,
                Actual = entry.Actual,
                Estimate = entry.Estimate,
                Prev = entry.Prev,
                Unit = entry.Unit
            });
        }

        return events;
    }
}
