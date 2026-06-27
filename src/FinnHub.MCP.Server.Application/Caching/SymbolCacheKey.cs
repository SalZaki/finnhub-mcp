// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text;

namespace FinnHub.MCP.Server.Application.Caching;

/// <summary>
/// Builds the logical cache-key segment a service owns, e.g. <c>"quote:s=AAPL"</c> — a prefix
/// followed by ordered <c>key=value</c> parts joined with <c>':'</c>.
/// </summary>
/// <remarks>
/// Deliberately distinct from <see cref="CacheKey"/>, which adds the outer
/// <c>finnhub:tenant=…:tier=…</c> namespacing in the Infrastructure cache layer. Routing every
/// service's <c>BuildCacheKey</c> through here keeps the symbol segment, part ordering, and joining
/// rules from drifting between services. Callers pass already-formatted string values (dates with
/// <c>InvariantCulture</c>), so this builder performs no culture-sensitive formatting of its own.
/// </remarks>
internal static class SymbolCacheKey
{
    /// <summary>
    /// Composes a logical key from a <paramref name="prefix"/> and ordered <paramref name="parts"/>.
    /// </summary>
    /// <param name="prefix">The leading segment (e.g. "quote", "news-pulse").</param>
    /// <param name="parts">Ordered <c>(key, value)</c> pairs appended as <c>:key=value</c>.</param>
    /// <returns>The joined logical key, e.g. <c>"profile:s=AAPL:cosmetic=True"</c>.</returns>
    public static string For(string prefix, params (string Key, string Value)[] parts)
    {
        var builder = new StringBuilder(prefix);
        foreach (var (key, value) in parts)
        {
            builder.Append(':').Append(key).Append('=').Append(value);
        }

        return builder.ToString();
    }
}
