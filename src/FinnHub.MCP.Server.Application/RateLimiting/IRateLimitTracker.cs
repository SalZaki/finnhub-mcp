// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;

namespace FinnHub.MCP.Server.Application.RateLimiting;

/// <summary>
/// Most-recent Finnhub upstream rate-limit observation, shared between the
/// HTTP delegating handler that writes it and the consumers (middleware,
/// status resource) that read it.
/// </summary>
/// <remarks>
/// Registered as a DI singleton — the underlying HTTP message handlers have
/// finite lifetime (per <c>SetHandlerLifetime</c> on the typed client) so the
/// tracker must outlive any single handler instance.
/// </remarks>
public interface IRateLimitTracker
{
    /// <summary>
    /// Returns the most-recent observed rate-limit snapshot, or <c>null</c>
    /// when no upstream response has yet been seen (cold start).
    /// </summary>
    /// <remarks>
    /// Returning <c>null</c> rather than a <see cref="RateLimitInfo"/> with two
    /// <c>null</c> members lets the envelope's <c>rate_limit</c> slot stay
    /// <c>null</c> until the first observation — matching the P1 envelope
    /// contract.
    /// </remarks>
    RateLimitInfo? Snapshot();

    /// <summary>
    /// Records the latest observed values for the upstream's quota counters.
    /// Either parameter may be <c>null</c> if the upstream emitted only one of
    /// the two headers; a call with both <c>null</c> is a no-op.
    /// </summary>
    /// <remarks>
    /// When <paramref name="resetAt"/> advances past the previously stored
    /// reset boundary, the throttled counter is reset to zero — the
    /// <see cref="RecentThrottledCount"/> tracks throttles within the current
    /// quota window only.
    /// </remarks>
    void Update(int? remaining, DateTimeOffset? resetAt);

    /// <summary>
    /// Increments the throttled counter. Called whenever the upstream
    /// returns HTTP 429, independent of whether the rate-limit headers were
    /// present on that response.
    /// </summary>
    void RecordThrottled();

    /// <summary>
    /// Count of HTTP 429 responses observed since the start of the current
    /// quota window (resets when <c>Update</c> sees a fresher
    /// <c>ResetAt</c>).
    /// </summary>
    int RecentThrottledCount { get; }
}
