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
/// Wire response for <c>get-calendar</c>.
/// </summary>
/// <remarks>
/// The wire envelope is flat across kinds — only the populated event list depends
/// on <see cref="Kind"/>. v1 carries <see cref="EarningsEvents"/> only; follow-up
/// stories add IPO and economic event lists alongside it.
/// </remarks>
[ExcludeFromCodeCoverage]
public sealed class GetCalendarResponse
{
    /// <summary>Echo of the dispatched calendar kind.</summary>
    [JsonPropertyName("kind")]
    public required string Kind { get; init; }

    /// <summary>Inclusive start of the requested window.</summary>
    [JsonPropertyName("from")]
    public required DateOnly From { get; init; }

    /// <summary>Inclusive end of the requested window.</summary>
    [JsonPropertyName("to")]
    public required DateOnly To { get; init; }

    /// <summary>Echo of the symbol filter, when one was requested.</summary>
    [JsonPropertyName("symbol")]
    public string? Symbol { get; init; }

    /// <summary>Number of events in the response.</summary>
    [JsonPropertyName("total_count")]
    public int TotalCount { get; init; }

    /// <summary>
    /// Earnings releases in the requested window, ordered by <see cref="EarningsEvent.Date"/> ascending.
    /// Populated only when <see cref="Kind"/> is <c>earnings</c>.
    /// </summary>
    [JsonPropertyName("earnings_events")]
    public IReadOnlyList<EarningsEvent>? EarningsEvents { get; init; }
}
