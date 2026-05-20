// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Caching;
using FinnHub.MCP.Server.Application.Options;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinnHub.MCP.Server.Infrastructure.Caching;

/// <summary>
/// <see cref="IFinnHubCache"/> backed by <see cref="HybridCache"/>.
/// </summary>
/// <remarks>
/// Builds the namespaced cache key via <see cref="CacheKey.Build"/>, resolves the
/// per-tier TTL from <see cref="CacheOptions"/>, and delegates to
/// <see cref="HybridCache"/>'s stateless <c>GetOrCreateAsync</c> overload. Hit/miss outcome is tracked via a
/// captured flag inside the factory delegate and emitted as a single structured log
/// line per call. Upstream factory exceptions propagate to the caller without
/// leaving a poisoned cache slot — only successful upstream results are stored.
/// </remarks>
public sealed class FinnHubHybridCache(
    HybridCache hybridCache,
    IOptions<CacheOptions> options,
    ILogger<FinnHubHybridCache> logger) : IFinnHubCache
{
    private const string SharedTenant = "shared";
    private const int KeySuffixLength = 40;

    /// <inheritdoc />
    public async ValueTask<T> GetOrCreateAsync<T>(
        string key,
        CacheTier tier,
        Func<CancellationToken, ValueTask<T>> factory,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(factory);

        var prefixedKey = CacheKey.Build(SharedTenant, tier, key);
        var ttl = options.Value.GetTtl(tier);
        var entryOptions = new HybridCacheEntryOptions
        {
            Expiration = ttl,
            LocalCacheExpiration = ttl
        };

        var wasFactoryInvoked = false;

        var result = await hybridCache.GetOrCreateAsync(
            prefixedKey,
            async ct =>
            {
                wasFactoryInvoked = true;
                return await factory(ct).ConfigureAwait(false);
            },
            options: entryOptions,
            tags: null,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        var outcome = wasFactoryInvoked ? "miss" : "hit";
        var keySuffix = prefixedKey.Length <= KeySuffixLength
            ? prefixedKey
            : prefixedKey[^KeySuffixLength..];

        logger.LogInformation(
            "cache {Outcome} tier={Tier} ttl_ms={TtlMs} key_suffix={KeySuffix}",
            outcome,
            tier,
            (long)ttl.TotalMilliseconds,
            keySuffix);

        return result;
    }
}
