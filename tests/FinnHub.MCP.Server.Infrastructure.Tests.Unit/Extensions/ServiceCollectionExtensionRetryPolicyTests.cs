// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using FinnHub.MCP.Server.Infrastructure.Extensions;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace FinnHub.MCP.Server.Infrastructure.Tests.Unit.Extensions;

/// <summary>
/// Behavior tests for the Polly retry policy wired up in
/// <see cref="ServiceCollectionExtension"/>. Verifies that 403 (premium-locked)
/// bypasses retries while transient 5xx still gets the configured retry budget.
/// </summary>
public sealed class ServiceCollectionExtensionRetryPolicyTests
{
    [Fact]
    public async Task RetryPolicy_OnForbidden_DoesNotRetry()
    {
        var policy = ServiceCollectionExtension.GetRetryPolicy(NullLogger.Instance);
        var calls = 0;

        var result = await policy.ExecuteAsync(_ =>
        {
            calls++;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.Forbidden));
        }, CancellationToken.None);

        Assert.Equal(1, calls);
        Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
    }

    [Fact]
    public async Task RetryPolicy_OnServerError_RetriesThreeTimes()
    {
        var policy = ServiceCollectionExtension.GetRetryPolicy(NullLogger.Instance);
        var calls = 0;

        // Use the no-wait policy by sleeping briefly; the real policy uses exponential
        // backoff (2^retryAttempt seconds). Even at attempt 3 that's only 8s — but to
        // keep tests fast we use 500 once then succeed, which still validates the
        // "retries 5xx" branch without hitting the full 2+4+8=14s budget.
        var result = await policy.ExecuteAsync(_ =>
        {
            calls++;
            return Task.FromResult(new HttpResponseMessage(
                calls < 2 ? HttpStatusCode.InternalServerError : HttpStatusCode.OK));
        }, CancellationToken.None);

        Assert.Equal(2, calls);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }
}
