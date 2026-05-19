// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Caching;

/// <summary>
/// Tiered response cache fronting upstream Finnhub calls.
/// </summary>
/// <remarks>
/// <para>
/// Concrete implementation lives in <c>Infrastructure</c> behind
/// <c>Microsoft.Extensions.Caching.Hybrid.HybridCache</c>. Services call
/// <see cref="GetOrCreateAsync{T}"/> with a logical cache key and a factory; the cache
/// short-circuits identical requests within the tier's configured TTL and propagates
/// upstream exceptions through the factory delegate without poisoning the slot.
/// </para>
/// <para>
/// Cache keys are namespaced internally with a <c>tenant</c> segment. v1 uses the literal
/// <c>"shared"</c> tenant per <c>.planning/specs/01-product-surface.md</c> §4 cross-cutting
/// concerns; a future BYOK milestone flips the tenant to <c>sha256(api-key)</c> without a
/// key-shape migration.
/// </para>
/// <para>
/// The cached payload type must be JSON-serialisable by whatever serializer
/// <c>HybridCache</c> is configured with; in this project that is
/// <c>FinnHubJsonContext</c>'s source-generated metadata.
/// </para>
/// </remarks>
public interface IFinnHubCache
{
    /// <summary>
    /// Returns the cached value for <paramref name="key"/> in the supplied
    /// <paramref name="tier"/>, invoking <paramref name="factory"/> on miss.
    /// </summary>
    /// <typeparam name="T">JSON-serialisable payload type.</typeparam>
    /// <param name="key">Caller-shaped logical key. The implementation namespaces this with the tenant and tier.</param>
    /// <param name="tier">Mutability tier controlling the TTL.</param>
    /// <param name="factory">Invoked on cache miss to produce a fresh value. Cancellation propagates into the factory.</param>
    /// <param name="cancellationToken">Cancels both the cache lookup and the factory invocation.</param>
    /// <returns>The cached value on hit, or the freshly produced value on miss.</returns>
    ValueTask<T> GetOrCreateAsync<T>(
        string key,
        CacheTier tier,
        Func<CancellationToken, ValueTask<T>> factory,
        CancellationToken cancellationToken = default);
}
