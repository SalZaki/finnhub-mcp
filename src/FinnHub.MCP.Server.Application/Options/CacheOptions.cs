// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using FinnHub.MCP.Server.Application.Caching;

namespace FinnHub.MCP.Server.Application.Options;

/// <summary>
/// Per-tier TTL configuration for the response cache.
/// </summary>
/// <remarks>
/// Bound from the <c>Cache</c> section of <c>appsettings.json</c> via
/// <c>AddOptions&lt;CacheOptions&gt;().Bind(...).ValidateDataAnnotations().ValidateOnStart()</c>.
/// Defaults match the values documented in <c>.planning/specs/01-product-surface.md</c> §3 P2
/// so a missing <c>Cache</c> section still produces correct behaviour. Each tier carries a
/// data-annotation <see cref="RangeAttribute"/> that enforces sensible bounds — a configured
/// TTL of zero or a negative value fails startup validation.
/// </remarks>
public sealed class CacheOptions
{
    /// <summary>
    /// TTL for the <see cref="CacheTier.Quote"/> tier. Default 10 seconds.
    /// </summary>
    [Range(typeof(TimeSpan), "00:00:01", "00:01:00")]
    public TimeSpan QuoteTtl { get; init; } = TimeSpan.FromSeconds(10);

    /// <summary>
    /// TTL for the <see cref="CacheTier.News"/> tier. Default 60 seconds.
    /// </summary>
    [Range(typeof(TimeSpan), "00:00:10", "00:10:00")]
    public TimeSpan NewsTtl { get; init; } = TimeSpan.FromSeconds(60);

    /// <summary>
    /// TTL for the <see cref="CacheTier.Financials"/> tier. Default 1 hour.
    /// </summary>
    [Range(typeof(TimeSpan), "00:01:00", "06:00:00")]
    public TimeSpan FinancialsTtl { get; init; } = TimeSpan.FromHours(1);

    /// <summary>
    /// TTL for the <see cref="CacheTier.Profile"/> tier. Default 24 hours.
    /// </summary>
    [Range(typeof(TimeSpan), "00:10:00", "7.00:00:00")]
    public TimeSpan ProfileTtl { get; init; } = TimeSpan.FromHours(24);

    /// <summary>
    /// TTL for the <see cref="CacheTier.Exchanges"/> tier. Default 7 days.
    /// </summary>
    [Range(typeof(TimeSpan), "01:00:00", "30.00:00:00")]
    public TimeSpan ExchangesTtl { get; init; } = TimeSpan.FromDays(7);

    /// <summary>
    /// Returns the configured TTL for the supplied <paramref name="tier"/>.
    /// </summary>
    /// <param name="tier">The cache tier to resolve.</param>
    /// <exception cref="ArgumentOutOfRangeException">The tier value is not a known enum member.</exception>
    public TimeSpan GetTtl(CacheTier tier) => tier switch
    {
        CacheTier.Quote => this.QuoteTtl,
        CacheTier.News => this.NewsTtl,
        CacheTier.Financials => this.FinancialsTtl,
        CacheTier.Profile => this.ProfileTtl,
        CacheTier.Exchanges => this.ExchangesTtl,
        _ => throw new ArgumentOutOfRangeException(nameof(tier), tier, "Unknown cache tier.")
    };
}
