// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;

namespace FinnHub.MCP.Server.Application.RateLimiting;

/// <summary>
/// Default <see cref="IRateLimitTracker"/>. Stores the most-recent observed
/// upstream rate-limit headers behind a single lock — writers come from the
/// delegating handler (one per upstream response), readers from the tool
/// invocation middleware and the status resource.
/// </summary>
public sealed class RateLimitTracker : IRateLimitTracker
{
    private readonly Lock _gate = new();
    private int? _remaining;
    private DateTimeOffset? _resetAt;
    private int _throttledCount;

    /// <inheritdoc />
    public int RecentThrottledCount
    {
        get
        {
            lock (this._gate)
            {
                return this._throttledCount;
            }
        }
    }

    /// <inheritdoc />
    public RateLimitInfo? Snapshot()
    {
        lock (this._gate)
        {
            if (this._remaining is null && this._resetAt is null)
            {
                return null;
            }

            return new RateLimitInfo(this._remaining, this._resetAt);
        }
    }

    /// <inheritdoc />
    public void Update(int? remaining, DateTimeOffset? resetAt)
    {
        if (remaining is null && resetAt is null)
        {
            return;
        }

        lock (this._gate)
        {
            if (resetAt is { } newReset && this._resetAt is { } oldReset && newReset > oldReset)
            {
                this._throttledCount = 0;
            }

            if (remaining is not null)
            {
                this._remaining = remaining;
            }

            if (resetAt is not null)
            {
                this._resetAt = resetAt;
            }
        }
    }

    /// <inheritdoc />
    public void RecordThrottled()
    {
        lock (this._gate)
        {
            this._throttledCount++;
        }
    }
}
