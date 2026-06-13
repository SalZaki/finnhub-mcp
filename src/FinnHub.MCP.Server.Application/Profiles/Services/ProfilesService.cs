// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Caching;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Profiles.Clients;
using FinnHub.MCP.Server.Application.Profiles.Features.GetCompanyProfile;
using FinnHub.MCP.Server.Application.Symbols;
using Microsoft.Extensions.Logging;

namespace FinnHub.MCP.Server.Application.Profiles.Services;

/// <summary>
/// Default <see cref="IProfilesService"/> wrapping <see cref="IProfilesApiClient"/> with
/// hybrid caching (Profile tier — 24h TTL) and exception-to-result translation.
/// </summary>
public sealed class ProfilesService(
    IProfilesApiClient apiClient,
    IFinnHubCache cache,
    ILogger<ProfilesService> logger)
    : IProfilesService
{
    /// <inheritdoc />
    public async Task<Result<GetCompanyProfileResponse>> GetProfileAsync(
        GetCompanyProfileQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        try
        {
            var cacheKey = SymbolCacheKey.For("profile", ("s", SymbolNormalizer.Normalize(query.Symbol)), ("cosmetic", query.IncludeCosmeticFields.ToString()));

            var response = await cache.GetOrCreateAsync(
                cacheKey,
                CacheTier.Profile,
                async ct => await apiClient.GetProfileAsync(query, ct),
                cancellationToken);

            logger.LogInformation("Retrieved profile for {Symbol}", query.Symbol);

            return string.IsNullOrEmpty(response.Name)
                ? Result<GetCompanyProfileResponse>.Failure(
                    $"No profile found for {query.Symbol}.",
                    ResultErrorType.NotFound)
                : Result<GetCompanyProfileResponse>.Success(response);
        }
        catch (ApiClientPremiumRequiredException ex)
        {
            logger.LogWarning(ex, "Premium-only profile endpoint for {Symbol}", query.Symbol);
            return Result<GetCompanyProfileResponse>.Failure(ex.Message, ResultErrorType.PremiumRequired);
        }
        catch (ApiClientHttpException ex)
        {
            logger.LogError(ex, "HTTP error fetching profile for {Symbol} (status: {Status})", query.Symbol, ex.StatusCode);
            return Result<GetCompanyProfileResponse>.Failure(ex.Message, ResultErrorType.ServiceUnavailable);
        }
        catch (ApiClientTimeoutException ex)
        {
            logger.LogWarning(ex, "Profile request timed out for {Symbol}", query.Symbol);
            return Result<GetCompanyProfileResponse>.Failure("Request timed out", ResultErrorType.Timeout);
        }
        catch (ApiClientDeserializationException ex)
        {
            logger.LogError(ex, "Failed to deserialize profile response for {Symbol}", query.Symbol);
            return Result<GetCompanyProfileResponse>.Failure("Invalid response from service", ResultErrorType.InvalidResponse);
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
            logger.LogError(ex, "Unexpected profile failure for {Symbol}", query.Symbol);
            return Result<GetCompanyProfileResponse>.Failure("Profile lookup failed unexpectedly");
        }
    }
}
