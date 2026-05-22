// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.RateLimiting;
using FinnHub.MCP.Server.Resources.Status;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Resources;

public sealed class ApiStatusResourceTests
{
    [Fact]
    public void GetStatus_ColdTracker_ReturnsNullsAndZeroThrottled()
    {
        var resource = new ApiStatusResource(new RateLimitTracker());

        var result = resource.GetStatus();

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Null(result.Data.Remaining);
        Assert.Null(result.Data.ResetAt);
        Assert.Equal(0, result.Data.RecentThrottledCount);
    }

    [Fact]
    public void GetStatus_AfterUpdate_SurfacesObservedValues()
    {
        var tracker = new RateLimitTracker();
        var reset = DateTimeOffset.UtcNow.AddMinutes(1);
        tracker.Update(25, reset);
        tracker.RecordThrottled();
        tracker.RecordThrottled();

        var result = new ApiStatusResource(tracker).GetStatus();

        Assert.True(result.IsSuccess);
        Assert.Equal(25, result.Data!.Remaining);
        Assert.Equal(reset, result.Data.ResetAt);
        Assert.Equal(2, result.Data.RecentThrottledCount);
    }
}
