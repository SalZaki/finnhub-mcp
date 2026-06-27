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
/// on <see cref="Kind"/>.
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

    /// <summary>Echo of the country filter, when one was requested (economic kind only).</summary>
    [JsonPropertyName("country")]
    public string? Country { get; init; }

    /// <summary>Number of events in the response.</summary>
    [JsonPropertyName("total_count")]
    public int TotalCount { get; init; }

    /// <summary>
    /// Earnings releases in the requested window, ordered by <see cref="EarningsEvent.Date"/> ascending.
    /// Populated only when <see cref="Kind"/> is <c>earnings</c>.
    /// </summary>
    [JsonPropertyName("earnings_events")]
    public IReadOnlyList<EarningsEvent>? EarningsEvents { get; init; }

    /// <summary>
    /// IPO listings in the requested window, ordered by <see cref="IpoEvent.Date"/> descending
    /// (most recent first). Populated only when <see cref="Kind"/> is <c>ipo</c>.
    /// </summary>
    [JsonPropertyName("ipo_events")]
    public IReadOnlyList<IpoEvent>? IpoEvents { get; init; }

    /// <summary>
    /// Economic releases in the requested window, ordered by <see cref="EconomicEvent.Time"/> ascending.
    /// Populated only when <see cref="Kind"/> is <c>economic</c>.
    /// </summary>
    [JsonPropertyName("economic_events")]
    public IReadOnlyList<EconomicEvent>? EconomicEvents { get; init; }
}
