// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Infrastructure.Dtos;

/// <summary>
/// Wire DTO for a single entry in Finnhub's <c>/calendar/ipo</c> response.
/// </summary>
/// <remarks>
/// All fields are nullable: Finnhub returns <c>null</c> for <c>symbol</c>,
/// <c>exchange</c>, <c>numberOfShares</c>, <c>price</c>, and
/// <c>totalSharesValue</c> on withdrawn or filed-but-not-yet-priced offerings.
/// <c>price</c> is a JSON string on the wire (e.g. <c>"4.00"</c>) — the
/// application-layer mapper parses it into a nullable double. Property names
/// are camelCase, overriding the snake_case default in
/// <see cref="Serialization.FinnHubJsonContext"/>.
/// </remarks>
[ExcludeFromCodeCoverage]
public sealed class FinnHubIpoEntry
{
    /// <summary>Ticker; <c>null</c> for withdrawn / unpriced offerings.</summary>
    [JsonPropertyName("symbol")]
    public string? Symbol { get; init; }

    /// <summary>Issuer name.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>Effective or expected listing date as ISO <c>yyyy-MM-dd</c>.</summary>
    [JsonPropertyName("date")]
    public string? Date { get; init; }

    /// <summary>Listing exchange (e.g. <c>NASDAQ Capital</c>, <c>NYSE</c>); <c>null</c> when unassigned.</summary>
    [JsonPropertyName("exchange")]
    public string? Exchange { get; init; }

    /// <summary>Offer price per share as a JSON string (e.g. <c>"4.00"</c>); <c>null</c> when unpriced.</summary>
    [JsonPropertyName("price")]
    public string? Price { get; init; }

    /// <summary>Number of shares offered; <c>null</c> when undisclosed.</summary>
    [JsonPropertyName("numberOfShares")]
    public long? NumberOfShares { get; init; }

    /// <summary>Total offering value; <c>null</c> when undisclosed.</summary>
    [JsonPropertyName("totalSharesValue")]
    public double? TotalSharesValue { get; init; }

    /// <summary>Lifecycle status (e.g. <c>priced</c>, <c>filed</c>, <c>withdrawn</c>, <c>expected</c>).</summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }
}
