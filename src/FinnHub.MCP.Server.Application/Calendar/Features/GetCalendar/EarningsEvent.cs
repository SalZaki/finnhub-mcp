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
/// A single corporate-earnings release from Finnhub's <c>/calendar/earnings</c> feed.
/// </summary>
/// <remarks>
/// All financial fields are nullable: Finnhub returns <c>null</c> for
/// <c>epsActual</c>/<c>revenueActual</c> on future-dated entries and for
/// <c>epsEstimate</c>/<c>revenueEstimate</c> on symbols with no analyst coverage.
/// </remarks>
[ExcludeFromCodeCoverage]
public sealed class EarningsEvent
{
    /// <summary>Ticker symbol of the reporting company.</summary>
    [JsonPropertyName("symbol")]
    public required string Symbol { get; init; }

    /// <summary>Scheduled report date (calendar day, no time component).</summary>
    [JsonPropertyName("date")]
    public required DateOnly Date { get; init; }

    /// <summary>
    /// Session timing relative to the trading day: <c>bmo</c> (before market open),
    /// <c>amc</c> (after market close), or <c>dmh</c> (during market hours). May be
    /// <c>null</c> when Finnhub has not classified the release.
    /// </summary>
    [JsonPropertyName("hour")]
    public string? Hour { get; init; }

    /// <summary>Fiscal quarter (1–4) the release covers.</summary>
    [JsonPropertyName("quarter")]
    public int? Quarter { get; init; }

    /// <summary>Fiscal year the release covers.</summary>
    [JsonPropertyName("year")]
    public int? Year { get; init; }

    /// <summary>Reported EPS, or <c>null</c> if the release has not happened yet.</summary>
    [JsonPropertyName("eps_actual")]
    public double? EpsActual { get; init; }

    /// <summary>Analyst-consensus EPS estimate, or <c>null</c> if no coverage.</summary>
    [JsonPropertyName("eps_estimate")]
    public double? EpsEstimate { get; init; }

    /// <summary>Reported revenue, or <c>null</c> if the release has not happened yet.</summary>
    [JsonPropertyName("revenue_actual")]
    public double? RevenueActual { get; init; }

    /// <summary>Analyst-consensus revenue estimate, or <c>null</c> if no coverage.</summary>
    [JsonPropertyName("revenue_estimate")]
    public double? RevenueEstimate { get; init; }
}
