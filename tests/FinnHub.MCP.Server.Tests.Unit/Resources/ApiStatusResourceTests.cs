// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.Json;
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

        var json = resource.GetStatus();
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        Assert.Equal(JsonValueKind.Null, root.GetProperty("remaining").ValueKind);
        Assert.Equal(JsonValueKind.Null, root.GetProperty("reset_at").ValueKind);
        Assert.Equal(0, root.GetProperty("recent_throttled_count").GetInt32());
    }

    [Fact]
    public void GetStatus_AfterUpdate_SurfacesObservedValues()
    {
        var tracker = new RateLimitTracker();
        var reset = DateTimeOffset.UtcNow.AddMinutes(1);
        tracker.Update(25, reset);
        tracker.RecordThrottled();
        tracker.RecordThrottled();

        var json = new ApiStatusResource(tracker).GetStatus();
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        Assert.Equal(25, root.GetProperty("remaining").GetInt32());
        Assert.Equal(reset, root.GetProperty("reset_at").GetDateTimeOffset());
        Assert.Equal(2, root.GetProperty("recent_throttled_count").GetInt32());
    }
}
