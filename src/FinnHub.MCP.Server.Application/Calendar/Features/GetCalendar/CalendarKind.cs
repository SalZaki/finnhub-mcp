// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Calendar.Features.GetCalendar;

/// <summary>
/// Discriminator for the parameter-dispatched <c>get-calendar</c> tool.
/// </summary>
/// <remarks>
/// The tool exposes one entry point that fans out to distinct Finnhub
/// calendar endpoints. v1 ships <see cref="Earnings"/> only; IPO and economic
/// calendars are added by follow-up stories that extend the validator and
/// service switch.
/// </remarks>
public enum CalendarKind
{
    /// <summary>Corporate earnings releases (<c>/calendar/earnings</c>).</summary>
    Earnings
}
