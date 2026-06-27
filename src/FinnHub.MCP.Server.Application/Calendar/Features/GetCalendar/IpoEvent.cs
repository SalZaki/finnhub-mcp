// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Application.Calendar.Features.GetCalendar;

/// <summary>
/// A single IPO record from Finnhub's <c>/calendar/ipo</c> feed.
/// </summary>
/// <remarks>
/// All fields except <see cref="Date"/> and <see cref="Name"/> are nullable —
/// Finnhub returns <c>null</c> for <see cref="Symbol"/>/<see cref="Exchange"/>/
/// <see cref="Price"/> on withdrawn or filed-but-not-yet-priced offerings.
/// <see cref="Price"/> is exposed as a numeric value parsed from the upstream
/// string (e.g. <c>"4.00"</c>); upstream values that fail parsing degrade to
/// <c>null</c> rather than failing the whole response.
/// </remarks>
[ExcludeFromCodeCoverage]
public sealed class IpoEvent
{
    /// <summary>Ticker assigned to the listing; <c>null</c> for withdrawn or unpriced offerings.</summary>
    [JsonPropertyName("symbol")]
    public string? Symbol { get; init; }

    /// <summary>Issuer name.</summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>Effective or expected listing date.</summary>
    [JsonPropertyName("date")]
    public required DateOnly Date { get; init; }

    /// <summary>Listing exchange (e.g. <c>NASDAQ Capital</c>, <c>NYSE</c>); <c>null</c> when not yet assigned.</summary>
    [JsonPropertyName("exchange")]
    public string? Exchange { get; init; }

    /// <summary>Offer price per share in the listing currency; <c>null</c> when not yet priced.</summary>
    [JsonPropertyName("price")]
    public double? Price { get; init; }

    /// <summary>Number of shares offered; <c>null</c> when not yet disclosed.</summary>
    [JsonPropertyName("number_of_shares")]
    public long? NumberOfShares { get; init; }

    /// <summary>Total offering value (<c>price * shares</c>); <c>null</c> when either component is missing.</summary>
    [JsonPropertyName("total_shares_value")]
    public double? TotalSharesValue { get; init; }

    /// <summary>Lifecycle status (e.g. <c>priced</c>, <c>filed</c>, <c>withdrawn</c>, <c>expected</c>).</summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }
}
