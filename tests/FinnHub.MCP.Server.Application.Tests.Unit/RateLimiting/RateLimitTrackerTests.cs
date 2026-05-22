// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.RateLimiting;
using Xunit;

namespace FinnHub.MCP.Server.Application.Tests.Unit.RateLimiting;

public sealed class RateLimitTrackerTests
{
    [Fact]
    public void Snapshot_ColdStart_ReturnsNull()
    {
        var tracker = new RateLimitTracker();

        Assert.Null(tracker.Snapshot());
        Assert.Equal(0, tracker.RecentThrottledCount);
    }

    [Fact]
    public void Update_SetsBothFields_AndSnapshotReturnsPopulatedInfo()
    {
        var tracker = new RateLimitTracker();
        var reset = DateTimeOffset.UtcNow.AddSeconds(60);

        tracker.Update(42, reset);

        var snap = tracker.Snapshot();
        Assert.NotNull(snap);
        Assert.Equal(42, snap.Remaining);
        Assert.Equal(reset, snap.ResetAt);
    }

    [Fact]
    public void Update_PartialFields_LeavesOtherUnchanged()
    {
        var tracker = new RateLimitTracker();
        var reset = DateTimeOffset.UtcNow.AddSeconds(60);

        tracker.Update(42, reset);
        tracker.Update(remaining: 10, resetAt: null);

        var snap = tracker.Snapshot();
        Assert.NotNull(snap);
        Assert.Equal(10, snap.Remaining);
        Assert.Equal(reset, snap.ResetAt);
    }

    [Fact]
    public void Update_BothNull_IsNoOp()
    {
        var tracker = new RateLimitTracker();

        tracker.Update(null, null);

        Assert.Null(tracker.Snapshot());
    }

    [Fact]
    public void RecordThrottled_IncrementsCounter()
    {
        var tracker = new RateLimitTracker();

        tracker.RecordThrottled();
        tracker.RecordThrottled();
        tracker.RecordThrottled();

        Assert.Equal(3, tracker.RecentThrottledCount);
    }

    [Fact]
    public void Update_ResetAtAdvances_ResetsThrottledCounter()
    {
        var tracker = new RateLimitTracker();
        var firstReset = DateTimeOffset.UtcNow;
        var secondReset = firstReset.AddSeconds(60);

        tracker.Update(50, firstReset);
        tracker.RecordThrottled();
        tracker.RecordThrottled();
        Assert.Equal(2, tracker.RecentThrottledCount);

        tracker.Update(60, secondReset);

        Assert.Equal(0, tracker.RecentThrottledCount);
        Assert.Equal(60, tracker.Snapshot()!.Remaining);
        Assert.Equal(secondReset, tracker.Snapshot()!.ResetAt);
    }

    [Fact]
    public void Update_ResetAtUnchanged_PreservesThrottledCounter()
    {
        var tracker = new RateLimitTracker();
        var reset = DateTimeOffset.UtcNow;

        tracker.Update(50, reset);
        tracker.RecordThrottled();
        tracker.Update(40, reset);

        Assert.Equal(1, tracker.RecentThrottledCount);
        Assert.Equal(40, tracker.Snapshot()!.Remaining);
    }

    [Fact]
    public async Task Update_ConcurrentWriters_DoNotCorruptState()
    {
        var tracker = new RateLimitTracker();
        var reset = DateTimeOffset.UtcNow;

        // 100 concurrent Update + RecordThrottled pairs. The lock-based impl must
        // produce a snapshot with values from one of the Updates (no torn read) and
        // a throttled count of exactly 100.
        var tasks = Enumerable.Range(0, 100).Select(i => Task.Run(() =>
        {
            tracker.Update(i, reset);
            tracker.RecordThrottled();
        }));

        await Task.WhenAll(tasks);

        Assert.Equal(100, tracker.RecentThrottledCount);
        var snap = tracker.Snapshot();
        Assert.NotNull(snap);
        Assert.InRange(snap.Remaining!.Value, 0, 99);
    }
}
