// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using FinnHub.MCP.Server.Application.RateLimiting;
using Microsoft.Extensions.Logging;

namespace FinnHub.MCP.Server.Infrastructure.RateLimiting;

/// <summary>
/// Reads Finnhub's <c>X-Ratelimit-Remaining</c> / <c>X-Ratelimit-Reset</c>
/// headers off every upstream response and feeds them into
/// <see cref="IRateLimitTracker"/>. Records throttle events on HTTP 429.
/// </summary>
/// <remarks>
/// <para>
/// Registered as transient and attached to the typed <c>HttpClient</c> chain
/// before Polly's retry handlers so it sees the response Polly ultimately
/// returns to the caller (after any retries), not the intermediate failures.
/// </para>
/// <para>
/// The handler never throws on header oddities — defensive parsing returns
/// <c>null</c> for missing / unparseable values so an upstream response with
/// no rate-limit headers (free tier omits them on some endpoints) flows
/// through unchanged.
/// </para>
/// </remarks>
internal sealed class RateLimitHeaderHandler(
    IRateLimitTracker tracker,
    ILogger<RateLimitHeaderHandler> logger) : DelegatingHandler
{
    private const string RemainingHeader = "X-Ratelimit-Remaining";
    private const string ResetHeader = "X-Ratelimit-Reset";
    private const int ThrottledStatusCode = 429;

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

        var remaining = TryReadInt(response, RemainingHeader);
        var resetAt = TryReadResetAt(response, ResetHeader);

        if (remaining is not null || resetAt is not null)
        {
            tracker.Update(remaining, resetAt);
            logger.LogDebug(
                "rate-limit update remaining={Remaining} reset_at={ResetAt:O} status={Status}",
                remaining,
                resetAt,
                (int)response.StatusCode);
        }

        if ((int)response.StatusCode == ThrottledStatusCode)
        {
            tracker.RecordThrottled();
            logger.LogWarning(
                "rate-limit throttled status=429 remaining={Remaining} reset_at={ResetAt:O}",
                remaining,
                resetAt);
        }

        return response;
    }

    private static int? TryReadInt(HttpResponseMessage response, string headerName)
    {
        if (!response.Headers.TryGetValues(headerName, out var values))
        {
            return null;
        }

        var raw = values.FirstOrDefault();
        if (raw is null)
        {
            return null;
        }

        return int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed)
            ? parsed
            : null;
    }

    private static DateTimeOffset? TryReadResetAt(HttpResponseMessage response, string headerName)
    {
        if (!response.Headers.TryGetValues(headerName, out var values))
        {
            return null;
        }

        var raw = values.FirstOrDefault();
        if (raw is null)
        {
            return null;
        }

        // Unix epoch seconds is the most common shape for X-Ratelimit-Reset
        // (matches GitHub/Twitter conventions). Try that first; fall back to
        // ISO-8601 / RFC-3339 for upstreams that emit a timestamp string.
        if (long.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var unixSeconds))
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixSeconds);
        }

        if (DateTimeOffset.TryParse(
                raw,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                out var iso))
        {
            return iso;
        }

        return null;
    }
}
