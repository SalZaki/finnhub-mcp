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
/// Wire DTO for a single entry in Finnhub's <c>/calendar/earnings</c> response.
/// </summary>
/// <remarks>
/// All numeric fields are nullable: Finnhub returns <c>null</c> for
/// <c>epsActual</c>/<c>revenueActual</c> on future-dated entries and for
/// <c>epsEstimate</c>/<c>revenueEstimate</c> on symbols with no analyst coverage.
/// Property names are camelCase on the wire (overriding the snake_case default
/// in <see cref="Serialization.FinnHubJsonContext"/>) to match the upstream shape.
/// </remarks>
[ExcludeFromCodeCoverage]
public sealed class FinnHubEarningsEntry
{
    /// <summary>Reporting ticker.</summary>
    [JsonPropertyName("symbol")]
    public string? Symbol { get; init; }

    /// <summary>Scheduled report date as ISO <c>yyyy-MM-dd</c>.</summary>
    [JsonPropertyName("date")]
    public string? Date { get; init; }

    /// <summary>Session timing: <c>bmo</c>, <c>amc</c>, <c>dmh</c>, or <c>null</c>.</summary>
    [JsonPropertyName("hour")]
    public string? Hour { get; init; }

    /// <summary>Fiscal quarter (1–4).</summary>
    [JsonPropertyName("quarter")]
    public int? Quarter { get; init; }

    /// <summary>Fiscal year.</summary>
    [JsonPropertyName("year")]
    public int? Year { get; init; }

    /// <summary>Reported EPS; <c>null</c> for future entries.</summary>
    [JsonPropertyName("epsActual")]
    public double? EpsActual { get; init; }

    /// <summary>Analyst-consensus EPS estimate; <c>null</c> when no coverage.</summary>
    [JsonPropertyName("epsEstimate")]
    public double? EpsEstimate { get; init; }

    /// <summary>Reported revenue; <c>null</c> for future entries.</summary>
    [JsonPropertyName("revenueActual")]
    public double? RevenueActual { get; init; }

    /// <summary>Analyst-consensus revenue estimate; <c>null</c> when no coverage.</summary>
    [JsonPropertyName("revenueEstimate")]
    public double? RevenueEstimate { get; init; }
}
