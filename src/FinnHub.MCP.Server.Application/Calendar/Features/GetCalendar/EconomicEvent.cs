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
/// A single macro release from Finnhub's <c>/calendar/economic</c> feed.
/// </summary>
/// <remarks>
/// <see cref="Actual"/>, <see cref="Estimate"/>, and <see cref="Prev"/> are
/// nullable: Finnhub returns <c>null</c> for <see cref="Actual"/> on future
/// events, and for <see cref="Estimate"/>/<see cref="Prev"/> on releases
/// without consensus or prior-period history (one-off events, elections,
/// holidays). <see cref="Time"/> is the upstream's UTC scheduled time
/// (<c>yyyy-MM-dd HH:mm:ss</c>) parsed into a UTC <see cref="DateTime"/>.
/// </remarks>
[ExcludeFromCodeCoverage]
public sealed class EconomicEvent
{
    /// <summary>ISO 3166-1 alpha-2 country code (e.g. <c>US</c>, <c>GB</c>) or Finnhub pseudo-code (<c>EU</c>, <c>WW</c>).</summary>
    [JsonPropertyName("country")]
    public required string Country { get; init; }

    /// <summary>Human-readable release name (e.g. <c>CPI YoY</c>, <c>FOMC Economic Projections</c>).</summary>
    [JsonPropertyName("event_name")]
    public required string EventName { get; init; }

    /// <summary>Scheduled UTC time of the release.</summary>
    [JsonPropertyName("time_utc")]
    public required DateTime Time { get; init; }

    /// <summary>Market impact classification: <c>low</c>, <c>medium</c>, or <c>high</c>; <c>null</c> when unclassified.</summary>
    [JsonPropertyName("impact")]
    public string? Impact { get; init; }

    /// <summary>Reported value; <c>null</c> for future events.</summary>
    [JsonPropertyName("actual")]
    public double? Actual { get; init; }

    /// <summary>Analyst-consensus estimate; <c>null</c> when no consensus is published.</summary>
    [JsonPropertyName("estimate")]
    public double? Estimate { get; init; }

    /// <summary>Prior-period value; <c>null</c> for one-off events with no history.</summary>
    [JsonPropertyName("prev")]
    public double? Prev { get; init; }

    /// <summary>Unit of the numeric fields (e.g. <c>%</c>, <c>$</c>, <c>€</c>); empty string for non-numeric events.</summary>
    [JsonPropertyName("unit")]
    public string? Unit { get; init; }
}
