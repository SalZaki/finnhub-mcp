// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Calendar.Features.GetCalendar;

namespace FinnHub.MCP.Server.Application.Calendar.Clients;

/// <summary>
/// Infrastructure contract for the Finnhub <c>/calendar/*</c> endpoint family.
/// </summary>
public interface ICalendarApiClient
{
    /// <summary>
    /// Fetches earnings releases scheduled within the supplied window, optionally
    /// filtered to a single symbol.
    /// </summary>
    Task<IReadOnlyList<EarningsEvent>> GetEarningsCalendarAsync(
        DateOnly from,
        DateOnly to,
        string? symbol,
        CancellationToken cancellationToken);
}
