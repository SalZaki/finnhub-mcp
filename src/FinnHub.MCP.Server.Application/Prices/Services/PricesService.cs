// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Caching;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Prices.Clients;
using FinnHub.MCP.Server.Application.Prices.Features.GetPriceSummary;
using Microsoft.Extensions.Logging;

namespace FinnHub.MCP.Server.Application.Prices.Services;

/// <summary>
/// Default <see cref="IPricesService"/> wrapping <see cref="IPricesApiClient"/> with
/// hybrid caching and exception-to-result translation.
/// </summary>
public sealed class PricesService(
    IPricesApiClient apiClient,
    IFinnHubCache cache,
    ILogger<PricesService> logger)
    : IPricesService
{
    /// <inheritdoc />
    public async Task<Result<GetPriceSummaryResponse>> GetSummaryAsync(
        GetPriceSummaryQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        try
        {
            var cacheKey = BuildCacheKey(query);

            var response = await cache.GetOrCreateAsync(
                cacheKey,
                CacheTier.Quote,
                async ct => await apiClient.GetSummaryAsync(query, ct),
                cancellationToken);

            logger.LogInformation(
                "Retrieved price summary for {Symbol} period={Period} candles={Count}",
                query.Symbol, query.Period, response.CandleCount);

            return response.CandleCount > 0
                ? new Result<GetPriceSummaryResponse>().Success(response)
                : new Result<GetPriceSummaryResponse>().Failure(
                    $"No price data for {query.Symbol} over {query.Period}.",
                    ResultErrorType.NotFound);
        }
        catch (ApiClientPremiumRequiredException ex)
        {
            logger.LogWarning(ex, "Premium-only candle endpoint for {Symbol}", query.Symbol);
            return new Result<GetPriceSummaryResponse>().Failure(ex.Message, ResultErrorType.PremiumRequired);
        }
        catch (ApiClientHttpException ex)
        {
            logger.LogError(ex, "HTTP error fetching candles for {Symbol} (status: {Status})", query.Symbol, ex.StatusCode);
            return new Result<GetPriceSummaryResponse>().Failure(ex.Message, ResultErrorType.ServiceUnavailable);
        }
        catch (ApiClientTimeoutException ex)
        {
            logger.LogWarning(ex, "Candle request timed out for {Symbol}", query.Symbol);
            return new Result<GetPriceSummaryResponse>().Failure("Request timed out", ResultErrorType.Timeout);
        }
        catch (ApiClientDeserializationException ex)
        {
            logger.LogError(ex, "Failed to deserialize candle response for {Symbol}", query.Symbol);
            return new Result<GetPriceSummaryResponse>().Failure("Invalid response from service", ResultErrorType.InvalidResponse);
        }
        catch (ApiClientCancelledException)
        {
            // Caller-initiated cancellation — surface as a typed cancel rather than
            // demoting to the catch-all "Unknown" failure that the base ApiClientException
            // arm below produces.
            throw;
        }
        catch (ApiClientException ex)
        {
            logger.LogError(ex, "Unexpected candle failure for {Symbol}", query.Symbol);
            return new Result<GetPriceSummaryResponse>().Failure("Price summary failed unexpectedly");
        }
    }

    private static string BuildCacheKey(GetPriceSummaryQuery query) =>
        $"price-summary:s={query.Symbol.ToUpperInvariant()}:p={query.Period}:c={query.IncludeCandles}";
}
