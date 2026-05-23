// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Caching;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Peers.Clients;
using FinnHub.MCP.Server.Application.Peers.Features.GetPeers;
using Microsoft.Extensions.Logging;

namespace FinnHub.MCP.Server.Application.Peers.Services;

/// <summary>
/// Default <see cref="IPeersService"/> wrapping <see cref="IPeersApiClient"/> with
/// hybrid caching and exception-to-result translation.
/// </summary>
public sealed class PeersService(
    IPeersApiClient apiClient,
    IFinnHubCache cache,
    ILogger<PeersService> logger)
    : IPeersService
{
    /// <inheritdoc />
    public async Task<Result<GetPeersResponse>> GetPeersAsync(
        GetPeersQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        try
        {
            var cacheKey = BuildCacheKey(query);

            var response = await cache.GetOrCreateAsync(
                cacheKey,
                CacheTier.Profile,
                async ct => await apiClient.GetPeersAsync(query, ct),
                cancellationToken);

            logger.LogInformation(
                "Retrieved {Count} peers for {Symbol} (grouping={Grouping})",
                response.TotalCount, query.Symbol, query.Grouping);

            return response.HasResults
                ? new Result<GetPeersResponse>().Success(response)
                : new Result<GetPeersResponse>().Failure(
                    $"No peers found for {query.Symbol}.",
                    ResultErrorType.NotFound);
        }
        catch (ApiClientPremiumRequiredException ex)
        {
            logger.LogWarning(ex, "Premium-only peers endpoint for {Symbol}", query.Symbol);
            return new Result<GetPeersResponse>().Failure(ex.Message, ResultErrorType.PremiumRequired);
        }
        catch (ApiClientHttpException ex)
        {
            logger.LogError(ex, "HTTP error fetching peers for {Symbol} (status: {Status})", query.Symbol, ex.StatusCode);
            return new Result<GetPeersResponse>().Failure(ex.Message, ResultErrorType.ServiceUnavailable);
        }
        catch (ApiClientTimeoutException ex)
        {
            logger.LogWarning(ex, "Peers request timed out for {Symbol}", query.Symbol);
            return new Result<GetPeersResponse>().Failure("Request timed out", ResultErrorType.Timeout);
        }
        catch (ApiClientDeserializationException ex)
        {
            logger.LogError(ex, "Failed to deserialize peers response for {Symbol}", query.Symbol);
            return new Result<GetPeersResponse>().Failure("Invalid response from service", ResultErrorType.InvalidResponse);
        }
        catch (ApiClientException ex)
        {
            logger.LogError(ex, "Unexpected peers failure for {Symbol}", query.Symbol);
            return new Result<GetPeersResponse>().Failure("Peers lookup failed unexpectedly");
        }
    }

    private static string BuildCacheKey(GetPeersQuery query) =>
        $"peers:s={query.Symbol.ToUpperInvariant()}:g={query.Grouping}";
}
