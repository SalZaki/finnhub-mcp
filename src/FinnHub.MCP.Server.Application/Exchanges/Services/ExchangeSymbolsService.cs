// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Caching;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Exchanges.Clients;
using FinnHub.MCP.Server.Application.Exchanges.Features.GetExchangeSymbols;
using FinnHub.MCP.Server.Application.Models;
using Microsoft.Extensions.Logging;

namespace FinnHub.MCP.Server.Application.Exchanges.Services;

/// <summary>
/// Default <see cref="IExchangeSymbolsService"/> — fetches the full symbol list for an exchange
/// once per <see cref="CacheTier.Exchanges"/> window and caches a slim, token-conscious aggregate.
/// </summary>
/// <remarks>
/// The cache factory fetches the complete upstream list (tens of thousands of rows for a major
/// exchange) and immediately reduces it to <see cref="GetExchangeSymbolsResponse"/> — the true total
/// count, a per-type breakdown, and a bounded sample — so the cached payload stays well under the
/// hybrid-cache size limit and a single upstream call serves every view for the whole TTL window.
/// </remarks>
public sealed class ExchangeSymbolsService(
    IExchangesApiClient apiClient,
    IFinnHubCache cache,
    ILogger<ExchangeSymbolsService> logger) : IExchangeSymbolsService
{
    /// <summary>Largest sample retained in the cached aggregate; the tool projects this down per view.</summary>
    private const int SampleSize = 100;

    /// <inheritdoc />
    public async Task<Result<GetExchangeSymbolsResponse>> GetExchangeSymbolsAsync(
        GetExchangeSymbolsQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        try
        {
            var cacheKey = $"exchange-symbols:ex={query.Exchange.ToUpperInvariant()}";

            var response = await cache.GetOrCreateAsync(
                cacheKey,
                CacheTier.Exchanges,
                async ct => BuildResponse(query.Exchange, await apiClient.GetSymbolsAsync(query.Exchange, ct).ConfigureAwait(false)),
                cancellationToken).ConfigureAwait(false);

            return response.TotalCount > 0
                ? Result<GetExchangeSymbolsResponse>.Success(response)
                : Result<GetExchangeSymbolsResponse>.Failure(
                    $"No symbols found for exchange '{query.Exchange}'.", ResultErrorType.NotFound);
        }
        catch (ApiClientPremiumRequiredException ex)
        {
            logger.LogWarning(ex, "Exchange symbols premium-locked for {Exchange}.", query.Exchange);
            return Result<GetExchangeSymbolsResponse>.Failure(ex.Message, ResultErrorType.PremiumRequired);
        }
        catch (ApiClientHttpException ex)
        {
            logger.LogError(ex, "Exchange symbols HTTP error for {Exchange}.", query.Exchange);
            return Result<GetExchangeSymbolsResponse>.Failure(ex.Message, ResultErrorType.ServiceUnavailable);
        }
        catch (ApiClientTimeoutException ex)
        {
            logger.LogError(ex, "Exchange symbols request timed out for {Exchange}.", query.Exchange);
            return Result<GetExchangeSymbolsResponse>.Failure("Request timed out", ResultErrorType.Timeout);
        }
        catch (ApiClientDeserializationException ex)
        {
            logger.LogError(ex, "Exchange symbols response could not be parsed for {Exchange}.", query.Exchange);
            return Result<GetExchangeSymbolsResponse>.Failure("Invalid response from service", ResultErrorType.InvalidResponse);
        }
        catch (ApiClientCancelledException)
        {
            throw;
        }
        catch (ApiClientException ex)
        {
            logger.LogError(ex, "Exchange symbols lookup failed unexpectedly for {Exchange}.", query.Exchange);
            return Result<GetExchangeSymbolsResponse>.Failure("Exchange symbols lookup failed unexpectedly");
        }
    }

    private static GetExchangeSymbolsResponse BuildResponse(string exchange, IReadOnlyList<ExchangeSymbol> symbols)
    {
        var typeBreakdown = symbols
            .GroupBy(s => string.IsNullOrWhiteSpace(s.Type) ? "Unknown" : s.Type!, StringComparer.Ordinal)
            .OrderByDescending(g => g.Count())
            .ThenBy(g => g.Key, StringComparer.Ordinal)
            .ToDictionary(g => g.Key, g => g.Count(), StringComparer.Ordinal);

        return new GetExchangeSymbolsResponse
        {
            Exchange = exchange,
            TotalCount = symbols.Count,
            TypeBreakdown = typeBreakdown,
            Symbols = symbols.Take(SampleSize).ToList().AsReadOnly()
        };
    }
}
