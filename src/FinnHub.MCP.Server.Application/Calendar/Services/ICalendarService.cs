// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Calendar.Features.GetCalendar;
using FinnHub.MCP.Server.Application.Models;

namespace FinnHub.MCP.Server.Application.Calendar.Services;

/// <summary>
/// Application-level entry point for calendar lookups. Dispatches by
/// <see cref="GetCalendarQuery.Kind"/> to the appropriate upstream feed.
/// </summary>
public interface ICalendarService
{
    /// <summary>Executes the dispatched calendar lookup and returns a categorised result.</summary>
    Task<Result<GetCalendarResponse>> GetCalendarAsync(
        GetCalendarQuery query,
        CancellationToken cancellationToken = default);
}
