// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Caching;

namespace FinnHub.MCP.Server.Application.Tests.Unit.TestDoubles;

/// <summary>
/// In-process <see cref="IFinnHubCache"/> double for service-layer tests.
/// </summary>
/// <remarks>
/// Mirrors production semantics around the boundaries that matter for the
/// <c>SearchService</c> caching contract: identical <c>(tier, key)</c> pairs
/// invoke the factory once; distinct pairs invoke it per-call; factory
/// exceptions propagate unwrapped and the cache slot remains empty.
/// TTL expiry is not modelled — tests that need expiry semantics belong in
/// <c>FinnHubHybridCacheTests</c>.
/// </remarks>
internal sealed class FakeFinnHubCache : IFinnHubCache
{
    private readonly Dictionary<string, object?> _entries = new(StringComparer.Ordinal);

    public int FactoryInvocationCount { get; private set; }

    /// <summary>Every logical key passed to <see cref="GetOrCreateAsync{T}"/>, in call order.</summary>
    public List<string> ObservedKeys { get; } = [];

    public async ValueTask<T> GetOrCreateAsync<T>(
        string key,
        CacheTier tier,
        Func<CancellationToken, ValueTask<T>> factory,
        CancellationToken cancellationToken = default)
    {
        this.ObservedKeys.Add(key);
        var compositeKey = $"{tier}:{key}";

        if (this._entries.TryGetValue(compositeKey, out var cached))
        {
            return (T)cached!;
        }

        this.FactoryInvocationCount++;
        var value = await factory(cancellationToken);
        this._entries[compositeKey] = value;
        return value;
    }
}
