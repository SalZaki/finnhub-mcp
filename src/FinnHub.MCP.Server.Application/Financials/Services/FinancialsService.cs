// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Caching;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Financials.Clients;
using FinnHub.MCP.Server.Application.Financials.Features.GetFinancialsSnapshot;
using FinnHub.MCP.Server.Application.Models;
using Microsoft.Extensions.Logging;

namespace FinnHub.MCP.Server.Application.Financials.Services;

/// <summary>
/// Default <see cref="IFinancialsService"/> wrapping <see cref="IFinancialsApiClient"/>
/// with hybrid caching and exception-to-result translation.
/// </summary>
public sealed class FinancialsService(
    IFinancialsApiClient apiClient,
    IFinnHubCache cache,
    ILogger<FinancialsService> logger)
    : IFinancialsService
{
    /// <inheritdoc />
    public async Task<Result<GetFinancialsSnapshotResponse>> GetSnapshotAsync(
        GetFinancialsSnapshotQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        try
        {
            var cacheKey = BuildCacheKey(query);

            var response = await cache.GetOrCreateAsync(
                cacheKey,
                CacheTier.Financials,
                async ct => await apiClient.GetSnapshotAsync(query, ct),
                cancellationToken);

            logger.LogInformation("Retrieved financials snapshot for {Symbol}", query.Symbol);

            return new Result<GetFinancialsSnapshotResponse>().Success(response);
        }
        catch (ApiClientPremiumRequiredException ex)
        {
            logger.LogWarning(ex, "Premium-only financials endpoint for {Symbol}", query.Symbol);
            return new Result<GetFinancialsSnapshotResponse>().Failure(ex.Message, ResultErrorType.PremiumRequired);
        }
        catch (ApiClientHttpException ex)
        {
            logger.LogError(ex, "HTTP error fetching financials for {Symbol} (status: {Status})", query.Symbol, ex.StatusCode);
            return new Result<GetFinancialsSnapshotResponse>().Failure(ex.Message, ResultErrorType.ServiceUnavailable);
        }
        catch (ApiClientTimeoutException ex)
        {
            logger.LogWarning(ex, "Financials request timed out for {Symbol}", query.Symbol);
            return new Result<GetFinancialsSnapshotResponse>().Failure("Request timed out", ResultErrorType.Timeout);
        }
        catch (ApiClientDeserializationException ex)
        {
            logger.LogError(ex, "Failed to deserialize financials response for {Symbol}", query.Symbol);
            return new Result<GetFinancialsSnapshotResponse>().Failure("Invalid response from service", ResultErrorType.InvalidResponse);
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
            logger.LogError(ex, "Unexpected financials failure for {Symbol}", query.Symbol);
            return new Result<GetFinancialsSnapshotResponse>().Failure("Financials lookup failed unexpectedly");
        }
    }

    private static string BuildCacheKey(GetFinancialsSnapshotQuery query) =>
        $"financials:s={query.Symbol.ToUpperInvariant()}:raw={query.IncludeRaw}";
}
