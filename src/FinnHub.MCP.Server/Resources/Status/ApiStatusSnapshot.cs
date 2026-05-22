// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Resources.Status;

/// <summary>
/// Wire shape for <c>finnhub://resources/api-status</c>.
/// </summary>
/// <param name="Remaining">Requests remaining in the current Finnhub quota window, or <c>null</c> before any upstream call.</param>
/// <param name="ResetAt">When the current quota window resets, or <c>null</c> before any upstream call.</param>
/// <param name="RecentThrottledCount">429 responses observed since the current quota window started.</param>
/// <remarks>
/// Distinct from <c>RateLimitInfo</c> on the tool envelope because this resource also
/// surfaces the rolling throttle count. The envelope field only carries the headers.
/// </remarks>
public sealed record ApiStatusSnapshot(
    [property: JsonPropertyName("remaining")] int? Remaining,
    [property: JsonPropertyName("reset_at")] DateTimeOffset? ResetAt,
    [property: JsonPropertyName("recent_throttled_count")] int RecentThrottledCount);
