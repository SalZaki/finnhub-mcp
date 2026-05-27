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
/// Wire DTO for a single entry in Finnhub's <c>/calendar/economic</c> response.
/// </summary>
/// <remarks>
/// <c>actual</c>, <c>estimate</c>, and <c>prev</c> are numeric and nullable;
/// future events arrive with <c>actual=null</c>, and one-off events (elections,
/// holidays) carry no consensus or history so all three may be null. <c>time</c>
/// is the upstream's scheduled timestamp as <c>yyyy-MM-dd HH:mm:ss</c> (UTC) and
/// is parsed into a <see cref="DateTime"/> at the application layer. Property
/// names are camelCase on the wire, overriding the snake_case default in
/// <see cref="Serialization.FinnHubJsonContext"/>.
/// </remarks>
[ExcludeFromCodeCoverage]
public sealed class FinnHubEconomicEntry
{
    /// <summary>ISO 3166-1 alpha-2 country code, or Finnhub pseudo-code (<c>EU</c>, <c>WW</c>).</summary>
    [JsonPropertyName("country")]
    public string? Country { get; init; }

    /// <summary>Release name (e.g. <c>CPI YoY</c>).</summary>
    [JsonPropertyName("event")]
    public string? Event { get; init; }

    /// <summary>Scheduled UTC time as <c>yyyy-MM-dd HH:mm:ss</c>.</summary>
    [JsonPropertyName("time")]
    public string? Time { get; init; }

    /// <summary>Market-impact tier: <c>low</c>, <c>medium</c>, or <c>high</c>.</summary>
    [JsonPropertyName("impact")]
    public string? Impact { get; init; }

    /// <summary>Reported value; <c>null</c> for future events.</summary>
    [JsonPropertyName("actual")]
    public double? Actual { get; init; }

    /// <summary>Analyst-consensus estimate; <c>null</c> when no consensus is published.</summary>
    [JsonPropertyName("estimate")]
    public double? Estimate { get; init; }

    /// <summary>Prior-period value; <c>null</c> for events with no history.</summary>
    [JsonPropertyName("prev")]
    public double? Prev { get; init; }

    /// <summary>Unit of the numeric fields (e.g. <c>%</c>, <c>$</c>); empty string for non-numeric events.</summary>
    [JsonPropertyName("unit")]
    public string? Unit { get; init; }
}
