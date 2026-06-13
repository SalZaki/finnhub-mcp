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
using FinnHub.MCP.Server.Application.Insiders.Clients;
using FinnHub.MCP.Server.Application.Insiders.Features.GetInsiderSignal;
using FinnHub.MCP.Server.Application.Options;
using FinnHub.MCP.Server.Infrastructure.Clients.Http;
using FinnHub.MCP.Server.Infrastructure.Dtos;
using FinnHub.MCP.Server.Infrastructure.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinnHub.MCP.Server.Infrastructure.Clients.Insiders;

/// <summary>HTTP client for the Finnhub <c>/stock/insider-transactions</c> endpoint.</summary>
public sealed class FinnHubInsidersApiClient : IInsidersApiClient
{
    private const string EndpointName = "insider-transactions";

    private readonly HttpClient _httpClient;
    private readonly FinnHubOptions _options;
    private readonly ILogger<FinnHubInsidersApiClient> _logger;

    /// <summary>Initialises a new <see cref="FinnHubInsidersApiClient"/>.</summary>
    public FinnHubInsidersApiClient(
        HttpClient httpClient,
        IOptions<FinnHubOptions> options,
        ILogger<FinnHubInsidersApiClient> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        this._httpClient = httpClient;
        this._options = options.Value ?? throw new ArgumentException("FinnHub options cannot be null.", nameof(options));
        this._logger = logger;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<InsiderTransaction>> GetInsiderTransactionsAsync(
        string symbol,
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken)
    {
        var endpoint = this._options.EndPoints.FirstOrDefault(e => e is { IsActive: true, Name: EndpointName })?.Url
                       ?? throw new ArgumentException("Insider transactions endpoint is not configured or inactive");

        var requestUri = BuildRequestUri(endpoint, symbol, from, to);

        this._logger.LogInformation("Requesting insider transactions from FinnHub: {RequestUri}", requestUri);

        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

        HttpResponseMessage response;
        try
        {
            response = await this._httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            this._logger.LogError(ex, "HTTP request to FinnHub insider transactions failed: {Uri}", requestUri);
            throw new ApiClientHttpException(
                $"HTTP request to FinnHub insider transactions failed: {requestUri}",
                HttpStatusCode.ServiceUnavailable,
                innerException: ex);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            this._logger.LogError(ex, "Insider transactions request timed out: {Uri}", requestUri);
            throw new ApiClientTimeoutException($"Insider transactions request timed out: {requestUri}", ex);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            this._logger.LogWarning("Insider transactions request cancelled: {Uri}", requestUri);
            throw new ApiClientCancelledException($"Insider transactions request cancelled: {requestUri}");
        }

        await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            await FinnHubResponseErrors.ThrowForStatusAsync(response, contentStream, this._logger, "insider transactions", cancellationToken).ConfigureAwait(false);
        }

        FinnHubInsiderTransactionsResponse? dto;
        try
        {
            dto = await JsonSerializer
                .DeserializeAsync(contentStream, FinnHubJsonContext.Default.FinnHubInsiderTransactionsResponse, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (JsonException ex)
        {
            this._logger.LogError(ex, "Failed to deserialize insider transactions response: {Uri}", requestUri);
            throw new ApiClientDeserializationException(
                $"Failed to deserialize insider transactions response: {requestUri}",
                innerException: ex);
        }

        if (dto?.Data is null || dto.Data.Count == 0)
        {
            return [];
        }

        var transactions = new List<InsiderTransaction>(dto.Data.Count);
        foreach (var entry in dto.Data)
        {
            // Name + change + transaction date are the minimum needed to be useful;
            // skip malformed rows rather than failing the whole window.
            if (string.IsNullOrWhiteSpace(entry.Name)
                || entry.Change is null
                || string.IsNullOrWhiteSpace(entry.TransactionDate))
            {
                continue;
            }

            if (!DateOnly.TryParseExact(entry.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var txDate))
            {
                continue;
            }

            DateOnly? filingDate = null;
            if (!string.IsNullOrWhiteSpace(entry.FilingDate)
                && DateOnly.TryParseExact(entry.FilingDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var fDate))
            {
                filingDate = fDate;
            }

            transactions.Add(new InsiderTransaction
            {
                Name = entry.Name,
                Change = entry.Change.Value,
                Share = entry.Share,
                TransactionDate = txDate,
                FilingDate = filingDate,
                TransactionPrice = entry.TransactionPrice,
                TransactionCode = string.IsNullOrWhiteSpace(entry.TransactionCode) ? null : entry.TransactionCode,
                IsDerivative = entry.IsDerivative,
                Currency = string.IsNullOrWhiteSpace(entry.Currency) ? null : entry.Currency
            });
        }

        return transactions;
    }

    private static string BuildRequestUri(string endpoint, string symbol, DateOnly from, DateOnly to)
    {
        var trimmed = endpoint.TrimStart('/');
        var fromStr = from.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var toStr = to.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        return string.Create(
            CultureInfo.InvariantCulture,
            $"{trimmed}?symbol={Uri.EscapeDataString(symbol)}&from={fromStr}&to={toStr}");
    }
}
