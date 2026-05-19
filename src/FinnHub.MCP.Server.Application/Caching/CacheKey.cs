// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.RegularExpressions;

namespace FinnHub.MCP.Server.Application.Caching;

/// <summary>
/// Builds the namespaced cache key emitted by <see cref="IFinnHubCache"/> implementations.
/// </summary>
/// <remarks>
/// Format: <c>finnhub:tenant={tenant}:tier={tier_lowercase}:{logicalKey}</c>.
/// The tenant segment is the literal <c>"shared"</c> for v1; the BYOK milestone swaps it
/// for the sha256 of the per-request API key without changing the key shape.
/// </remarks>
internal static partial class CacheKey
{
    private const int MaxLogicalKeyLength = 256;

    [GeneratedRegex(@"^[a-z0-9_-]{1,64}$", RegexOptions.Compiled)]
    private static partial Regex TenantRegex();

    /// <summary>
    /// Builds a fully-namespaced cache key.
    /// </summary>
    /// <param name="tenant">Per-deployment tenant slug. Must match <c>[a-z0-9_-]{1,64}</c>.</param>
    /// <param name="tier">Mutability tier; lowercased into the key for readability.</param>
    /// <param name="logicalKey">Caller-shaped key segment. 1–256 chars.</param>
    /// <exception cref="ArgumentException">Either input violates the documented constraints.</exception>
    public static string Build(string tenant, CacheTier tier, string logicalKey)
    {
        if (string.IsNullOrWhiteSpace(tenant) || !TenantRegex().IsMatch(tenant))
        {
            throw new ArgumentException(
                "Tenant must match [a-z0-9_-]{1,64}.",
                nameof(tenant));
        }

        if (string.IsNullOrWhiteSpace(logicalKey))
        {
            throw new ArgumentException(
                "Logical key cannot be empty or whitespace.",
                nameof(logicalKey));
        }

        if (logicalKey.Length > MaxLogicalKeyLength)
        {
            throw new ArgumentException(
                $"Logical key must be at most {MaxLogicalKeyLength} chars.",
                nameof(logicalKey));
        }

        var tierName = tier.ToString().ToLowerInvariant();
        return $"finnhub:tenant={tenant}:tier={tierName}:{logicalKey}";
    }
}
