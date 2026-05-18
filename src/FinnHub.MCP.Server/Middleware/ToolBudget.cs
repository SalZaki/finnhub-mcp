// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Common;

namespace FinnHub.MCP.Server.Middleware;

/// <summary>
/// Maps a <see cref="ToolView"/> to its per-response token ceiling.
/// </summary>
/// <remarks>
/// <see cref="ToolView.Full"/> is uncapped. Summary and standard ceilings live on
/// <see cref="Constants.Envelope"/> so they're discoverable in one place and can be
/// tuned without touching the middleware.
/// </remarks>
internal static class ToolBudget
{
    /// <summary>
    /// Returns the token ceiling for the supplied view, or <see cref="int.MaxValue"/> for <see cref="ToolView.Full"/>.
    /// </summary>
    public static int CeilingFor(ToolView view) => view switch
    {
        ToolView.Summary => Constants.Envelope.SummaryTokenCeiling,
        ToolView.Standard => Constants.Envelope.StandardTokenCeiling,
        ToolView.Full => int.MaxValue,
        _ => Constants.Envelope.SummaryTokenCeiling
    };
}
