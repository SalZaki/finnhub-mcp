// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Calendar.Features.GetCalendar;

/// <summary>
/// Query parameters for the <c>get-calendar</c> tool — a dispatched lookup
/// against one of Finnhub's calendar endpoints (earnings, IPO, economic).
/// </summary>
public sealed class GetCalendarQuery
{
    /// <summary>Per-invocation correlation id; passed through for logging only.</summary>
    public required string QueryId { get; init; }

    /// <summary>Which calendar feed to dispatch to.</summary>
    public required CalendarKind Kind { get; init; }

    /// <summary>Inclusive start of the date window (UTC calendar day).</summary>
    public required DateOnly From { get; init; }

    /// <summary>Inclusive end of the date window (UTC calendar day).</summary>
    public required DateOnly To { get; init; }

    /// <summary>
    /// Optional uppercase ticker filter. When <c>null</c> the upstream returns the
    /// full calendar for the window; when set, results are filtered to that symbol.
    /// </summary>
    public string? Symbol { get; init; }

    /// <summary>
    /// Optional ISO 3166-1 alpha-2 country code (or Finnhub pseudo-code <c>EU</c>/<c>WW</c>).
    /// Only relevant when <see cref="Kind"/> is <see cref="CalendarKind.Economic"/>; the
    /// upstream does not accept a country filter, so <c>CalendarService</c> applies it
    /// server-side after the unfiltered window fetch.
    /// </summary>
    public string? Country { get; init; }
}
