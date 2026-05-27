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
/// calendar endpoints — earnings, IPO, and economic.
/// </remarks>
public enum CalendarKind
{
    /// <summary>Corporate earnings releases (<c>/calendar/earnings</c>).</summary>
    Earnings,

    /// <summary>Initial public offerings (<c>/calendar/ipo</c>).</summary>
    Ipo,

    /// <summary>
    /// Macro releases and central-bank events (<c>/calendar/economic</c>). The
    /// upstream does not accept a country filter; <c>CalendarService</c> applies
    /// it server-side after the unfiltered window fetch.
    /// </summary>
    Economic
}
