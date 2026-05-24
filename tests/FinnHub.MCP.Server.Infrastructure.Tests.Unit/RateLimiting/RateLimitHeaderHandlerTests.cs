// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Net;
using FinnHub.MCP.Server.Application.RateLimiting;
using FinnHub.MCP.Server.Infrastructure.RateLimiting;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Xunit;

namespace FinnHub.MCP.Server.Infrastructure.Tests.Unit.RateLimiting;

public sealed class RateLimitHeaderHandlerTests
{
    [Fact]
    public async Task SendAsync_BothHeadersPresent_UpdatesTracker()
    {
        var tracker = Substitute.For<IRateLimitTracker>();
        var resetSeconds = DateTimeOffset.UtcNow.AddMinutes(1).ToUnixTimeSeconds();

        await SendOnceAsync(tracker, response =>
        {
            response.Headers.TryAddWithoutValidation("X-Ratelimit-Remaining", "42");
            response.Headers.TryAddWithoutValidation("X-Ratelimit-Reset", resetSeconds.ToString(CultureInfo.InvariantCulture));
        });

        tracker.Received(1).Update(42, Arg.Is<DateTimeOffset?>(d => d == DateTimeOffset.FromUnixTimeSeconds(resetSeconds)));
        tracker.DidNotReceive().RecordThrottled();
    }

    [Fact]
    public async Task SendAsync_NoHeaders_DoesNotCallUpdate()
    {
        var tracker = Substitute.For<IRateLimitTracker>();

        await SendOnceAsync(tracker, _ => { });

        tracker.DidNotReceive().Update(Arg.Any<int?>(), Arg.Any<DateTimeOffset?>());
        tracker.DidNotReceive().RecordThrottled();
    }

    [Fact]
    public async Task SendAsync_OnlyRemainingHeader_UpdatesWithNullResetAt()
    {
        var tracker = Substitute.For<IRateLimitTracker>();

        await SendOnceAsync(tracker, r =>
            r.Headers.TryAddWithoutValidation("X-Ratelimit-Remaining", "15"));

        tracker.Received(1).Update(15, null);
    }

    [Fact]
    public async Task SendAsync_UnparseableHeader_LeavesTrackerAlone()
    {
        var tracker = Substitute.For<IRateLimitTracker>();

        await SendOnceAsync(tracker, r =>
            r.Headers.TryAddWithoutValidation("X-Ratelimit-Remaining", "not-an-int"));

        tracker.DidNotReceive().Update(Arg.Any<int?>(), Arg.Any<DateTimeOffset?>());
    }

    [Fact]
    public async Task SendAsync_ResetAtAsIso8601_ParsesAsFallback()
    {
        var tracker = Substitute.For<IRateLimitTracker>();
        const string Iso = "2026-12-31T23:59:59Z";

        await SendOnceAsync(tracker, r =>
        {
            r.Headers.TryAddWithoutValidation("X-Ratelimit-Remaining", "1");
            r.Headers.TryAddWithoutValidation("X-Ratelimit-Reset", Iso);
        });

        var expected = DateTimeOffset.Parse(Iso, CultureInfo.InvariantCulture).ToUniversalTime();
        tracker.Received(1).Update(1, expected);
    }

    [Fact]
    public async Task SendAsync_Status429_RecordsThrottled()
    {
        var tracker = Substitute.For<IRateLimitTracker>();

        await SendOnceAsync(tracker, _ => { }, HttpStatusCode.TooManyRequests);

        tracker.Received(1).RecordThrottled();
    }

    [Fact]
    public async Task SendAsync_Status429_WithHeaders_RecordsBoth()
    {
        var tracker = Substitute.For<IRateLimitTracker>();

        await SendOnceAsync(tracker, r =>
            r.Headers.TryAddWithoutValidation("X-Ratelimit-Remaining", "0"),
            HttpStatusCode.TooManyRequests);

        tracker.Received(1).Update(0, null);
        tracker.Received(1).RecordThrottled();
    }

    [Fact]
    public async Task SendAsync_UnparseableResetHeader_LeavesResetAtNull()
    {
        var tracker = Substitute.For<IRateLimitTracker>();

        await SendOnceAsync(tracker, r =>
        {
            r.Headers.TryAddWithoutValidation("X-Ratelimit-Remaining", "3");
            r.Headers.TryAddWithoutValidation("X-Ratelimit-Reset", "not-a-timestamp");
        });

        tracker.Received(1).Update(3, null);
    }

    [Fact]
    public async Task SendAsync_HeaderPresentWithNullValue_TreatedAsMissing()
    {
        // Defends the `raw is null` branches in TryReadInt / TryReadResetAt — TryGetValues
        // returns true but FirstOrDefault yields null when the header was stored as null.
        var tracker = Substitute.For<IRateLimitTracker>();

        await SendOnceAsync(tracker, r =>
        {
            r.Headers.TryAddWithoutValidation("X-Ratelimit-Remaining", new string?[] { null });
            r.Headers.TryAddWithoutValidation("X-Ratelimit-Reset", new string?[] { null });
        });

        tracker.DidNotReceive().Update(Arg.Any<int?>(), Arg.Any<DateTimeOffset?>());
    }

    private static async Task SendOnceAsync(
        IRateLimitTracker tracker,
        Action<HttpResponseMessage> configureResponse,
        HttpStatusCode status = HttpStatusCode.OK)
    {
        // Build the handler chain locally and dispose each link in declared order.
        // Using HttpMessageInvoker rather than HttpClient avoids CA2000's
        // false-positive on inline handler construction.
        using var inner = new StubHandler(status, configureResponse);
        using var handler = new RateLimitHeaderHandler(tracker, NullLogger<RateLimitHeaderHandler>.Instance)
        {
            InnerHandler = inner
        };
        using var invoker = new HttpMessageInvoker(handler, disposeHandler: false);
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://example/");
        using var response = await invoker.SendAsync(request, CancellationToken.None);
    }

    private sealed class StubHandler(HttpStatusCode status, Action<HttpResponseMessage> configure) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
        {
            var response = new HttpResponseMessage(status);
            configure(response);
            return Task.FromResult(response);
        }
    }
}
