// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Net.Mime;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.RateLimiting;

namespace FinnHub.MCP.Server.Resources.Status;

/// <summary>
/// MCP resource that exposes the most-recent observed Finnhub upstream quota state.
/// </summary>
/// <remarks>
/// Reads from the same <see cref="IRateLimitTracker"/> singleton the
/// <c>ToolInvocationMiddleware</c> consults. Pre-first-call the snapshot's numeric
/// fields are <c>null</c> — clients that want to poll without invoking a tool can
/// distinguish "no observation yet" from "quota exhausted".
/// </remarks>
[McpServerResourceType]
public sealed class ApiStatusResource(IRateLimitTracker tracker)
{
    /// <summary>
    /// Returns the current Finnhub rate-limit snapshot wrapped in the standard
    /// application <see cref="Result{T}"/>.
    /// </summary>
    [McpServerResource(
        UriTemplate = "finnhub://resources/api-status",
        Name = "get-api-status",
        Title = "API Status",
        MimeType = MediaTypeNames.Application.Json)]
    [Description("Returns the most-recent Finnhub upstream rate-limit headers and the rolling 429 count.")]
    public Result<ApiStatusSnapshot> GetStatus()
    {
        var snapshot = tracker.Snapshot();
        var payload = new ApiStatusSnapshot(
            Remaining: snapshot?.Remaining,
            ResetAt: snapshot?.ResetAt,
            RecentThrottledCount: tracker.RecentThrottledCount);

        return new Result<ApiStatusSnapshot>().Success(payload);
    }
}
