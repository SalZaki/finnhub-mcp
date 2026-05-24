// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Caching;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Quotes.Clients;
using FinnHub.MCP.Server.Application.Quotes.Features.GetQuote;
using Microsoft.Extensions.Logging;

namespace FinnHub.MCP.Server.Application.Quotes.Services;

/// <summary>
/// Default <see cref="IQuotesService"/> wrapping <see cref="IQuotesApiClient"/> with
/// hybrid caching (Quote tier — 10s TTL) and exception-to-result translation.
/// </summary>
public sealed class QuotesService(
    IQuotesApiClient apiClient,
    IFinnHubCache cache,
    ILogger<QuotesService> logger)
    : IQuotesService
{
    /// <inheritdoc />
    public async Task<Result<GetQuoteResponse>> GetQuoteAsync(
        GetQuoteQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        try
        {
            var cacheKey = $"quote:s={query.Symbol.ToUpperInvariant()}";

            var response = await cache.GetOrCreateAsync(
                cacheKey,
                CacheTier.Quote,
                async ct => await apiClient.GetQuoteAsync(query, ct),
                cancellationToken);

            logger.LogInformation(
                "Retrieved quote for {Symbol}: current={Current} change={Change}",
                query.Symbol, response.Current, response.Change);

            // Finnhub returns all-zero/null for unknown symbols. Anything with a
            // non-zero current price OR a real timestamp is treated as a live quote.
            return (response.Current is > 0) || response.TimestampUtc is not null
                ? new Result<GetQuoteResponse>().Success(response)
                : new Result<GetQuoteResponse>().Failure(
                    $"No live quote for {query.Symbol}.",
                    ResultErrorType.NotFound);
        }
        catch (ApiClientPremiumRequiredException ex)
        {
            logger.LogWarning(ex, "Premium-only quote endpoint for {Symbol}", query.Symbol);
            return new Result<GetQuoteResponse>().Failure(ex.Message, ResultErrorType.PremiumRequired);
        }
        catch (ApiClientHttpException ex)
        {
            logger.LogError(ex, "HTTP error fetching quote for {Symbol} (status: {Status})", query.Symbol, ex.StatusCode);
            return new Result<GetQuoteResponse>().Failure(ex.Message, ResultErrorType.ServiceUnavailable);
        }
        catch (ApiClientTimeoutException ex)
        {
            logger.LogWarning(ex, "Quote request timed out for {Symbol}", query.Symbol);
            return new Result<GetQuoteResponse>().Failure("Request timed out", ResultErrorType.Timeout);
        }
        catch (ApiClientDeserializationException ex)
        {
            logger.LogError(ex, "Failed to deserialize quote response for {Symbol}", query.Symbol);
            return new Result<GetQuoteResponse>().Failure("Invalid response from service", ResultErrorType.InvalidResponse);
        }
        catch (ApiClientException ex)
        {
            logger.LogError(ex, "Unexpected quote failure for {Symbol}", query.Symbol);
            return new Result<GetQuoteResponse>().Failure("Quote lookup failed unexpectedly");
        }
    }
}
