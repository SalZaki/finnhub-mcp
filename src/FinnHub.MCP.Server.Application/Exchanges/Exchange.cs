// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Application.Exchanges;

/// <summary>
/// Represents detailed information about a single stock exchange.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class Exchange
{
    /// <summary>
    /// Gets the exchange code or symbol (e.g. "NYSE", "AB").
    /// </summary>
    [JsonPropertyName("code")]
    public required string ExchangeCode { get; init; }

    /// <summary>
    /// Gets the full human-readable name of the exchange.
    /// </summary>
    [JsonPropertyName("name")]
    public required string ExchangeName { get; init; }

    /// <summary>
    /// Gets the Market Identifier Code (MIC), per ISO 10383.
    /// </summary>
    [JsonPropertyName("mic")]
    public string? MicCode { get; init; }

    /// <summary>
    /// Gets the IANA time zone identifier for the exchange's location.
    /// </summary>
    [JsonPropertyName("time_zone")]
    public string? TimeZone { get; init; }

    /// <summary>
    /// Gets the scheduled start time of the pre‑market session.
    /// </summary>
    [JsonPropertyName("pre_market_hours")]
    public string? PreMarketHours { get; init; }

    /// <summary>
    /// Gets the regular market trading hours.
    /// </summary>
    [JsonPropertyName("trading_hours")]
    public string? TradingHours { get; init; }

    /// <summary>
    /// Gets the scheduled hours of the post‑market session.
    /// </summary>
    [JsonPropertyName("post_market_hours")]
    public string? PostMarketHours { get; init; }

    /// <summary>
    /// Gets the date or time when the exchange is closed (if applicable).
    /// </summary>
    [JsonPropertyName("close_date")]
    public string? CloseDate { get; init; }

    /// <summary>
    /// Gets the ISO 2‑letter country code where the exchange is located.
    /// </summary>
    [JsonPropertyName("country_code")]
    public required string CountryCode { get; init; }

    /// <summary>
    /// Gets the full country name where the exchange is located.
    /// </summary>
    [JsonPropertyName("country_name")]
    public required string CountryName { get; init; }

    /// <summary>
    /// Gets the URL for more information about the exchange.
    /// </summary>
    [JsonPropertyName("url")]
    public required string Url { get; init; }
}
