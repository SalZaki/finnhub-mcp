// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using System.Text.Json;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Exchanges.Clients;
using FinnHub.MCP.Server.Application.Exchanges.Features.GetExchangeSymbols;
using FinnHub.MCP.Server.Application.Options;
using FinnHub.MCP.Server.Infrastructure.Clients.Http;
using FinnHub.MCP.Server.Infrastructure.Dtos;
using FinnHub.MCP.Server.Infrastructure.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinnHub.MCP.Server.Infrastructure.Clients.Exchanges;

/// <summary>
/// HTTP client for the Finnhub <c>/stock/symbol</c> endpoint. The upstream response is a JSON array
/// of symbol rows; this client maps it to <see cref="ExchangeSymbol"/> and translates HTTP failures
/// into typed exceptions.
/// </summary>
/// <remarks>
/// Finnhub answers <c>/stock/symbol?exchange=US</c> with a 302 to a signed
/// <c>static2.finnhub.io</c> CDN URL; the shared <c>SocketsHttpHandler</c> follows that redirect
/// automatically (auto-redirect is left at its default of <c>true</c>, the <c>X-Finnhub-Token</c>
/// header survives the cross-host hop, and the signed URL carries its own auth). Premium-locked
/// exchanges are gated with HTTP <c>401</c> (not <c>403</c>) and a body of
/// "You don't have access to this resource." — both <c>401</c> and <c>403</c> are mapped to
/// <see cref="ApiClientPremiumRequiredException"/> here.
/// </remarks>
public sealed class FinnHubExchangesApiClient : IExchangesApiClient
{
    private const string EndpointName = "exchange-symbols";

    private readonly HttpClient _httpClient;
    private readonly FinnHubOptions _options;
    private readonly ILogger<FinnHubExchangesApiClient> _logger;

    /// <summary>
    /// Initialises a new <see cref="FinnHubExchangesApiClient"/>.
    /// </summary>
    public FinnHubExchangesApiClient(
        HttpClient httpClient,
        IOptions<FinnHubOptions> options,
        ILogger<FinnHubExchangesApiClient> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        this._httpClient = httpClient;
        this._options = options.Value ?? throw new ArgumentException("FinnHub options cannot be null.", nameof(options));
        this._logger = logger;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ExchangeSymbol>> GetSymbolsAsync(string exchange, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(exchange);

        var endpoint = this._options.EndPoints.FirstOrDefault(e => e is { IsActive: true, Name: EndpointName })?.Url
                       ?? throw new ArgumentException("Exchange-symbols endpoint is not configured or inactive");

        var requestUri = $"{endpoint.TrimStart('/')}?exchange={Uri.EscapeDataString(exchange)}";

        this._logger.LogInformation("Requesting exchange symbols from FinnHub: {RequestUri}", requestUri);

        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

        HttpResponseMessage response;
        try
        {
            response = await this._httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            this._logger.LogError(ex, "HTTP request to FinnHub exchange-symbols failed: {Uri}", requestUri);
            throw new ApiClientHttpException(
                $"HTTP request to FinnHub exchange-symbols failed: {requestUri}",
                HttpStatusCode.ServiceUnavailable,
                innerException: ex);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            this._logger.LogError(ex, "Exchange-symbols request timed out: {Uri}", requestUri);
            throw new ApiClientTimeoutException($"Exchange-symbols request timed out: {requestUri}", ex);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            this._logger.LogWarning("Exchange-symbols request cancelled: {Uri}", requestUri);
            throw new ApiClientCancelledException($"Exchange-symbols request cancelled: {requestUri}");
        }

        await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            await FinnHubResponseErrors.ThrowForStatusAsync(response, contentStream, this._logger, "exchange-symbols", cancellationToken, treatUnauthorizedAsPremium: true).ConfigureAwait(false);
        }

        FinnHubSymbolRow[] rows;
        try
        {
            rows = await JsonSerializer
                .DeserializeAsync(contentStream, FinnHubJsonContext.Default.FinnHubSymbolRowArray, cancellationToken)
                .ConfigureAwait(false) ?? [];
        }
        catch (JsonException ex)
        {
            this._logger.LogError(ex, "Failed to deserialize exchange-symbols response: {Uri}", requestUri);
            throw new ApiClientDeserializationException(
                $"Failed to deserialize exchange-symbols response: {requestUri}",
                innerException: ex);
        }

        return rows
            .Where(r => !string.IsNullOrWhiteSpace(r.Symbol))
            .Select(r => new ExchangeSymbol
            {
                Symbol = r.Symbol!,
                DisplaySymbol = r.DisplaySymbol,
                Description = r.Description,
                Type = r.Type
            })
            .ToList()
            .AsReadOnly();
    }
}
