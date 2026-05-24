// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using FinnHub.MCP.Server.Application.Exceptions;
using Xunit;

namespace FinnHub.MCP.Server.Application.Tests.Unit.Exceptions;

/// <summary>
/// Constructor + property coverage for every concrete <see cref="ApiClientException"/>
/// subclass. Bumps coverage from ~50% line to 100% and pins the wire <c>ErrorCode</c>
/// strings that downstream consumers (logging, error-type mapping) depend on.
/// </summary>
public sealed class ApiClientExceptionTests
{
    [Fact]
    public void ApiClientHttpException_PreservesAllConstructorArgs()
    {
        var inner = new InvalidOperationException("inner");
        var ex = new ApiClientHttpException("boom", HttpStatusCode.BadGateway, "body", "https://x/y", inner);

        Assert.Equal("boom", ex.Message);
        Assert.Equal(HttpStatusCode.BadGateway, ex.StatusCode);
        Assert.Equal("body", ex.ResponseContent);
        Assert.Equal("https://x/y", ex.RequestUri);
        Assert.Same(inner, ex.InnerException);
        Assert.Equal("API_CLIENT_HTTP", ex.ErrorCode);
    }

    [Fact]
    public void ApiClientHttpException_OptionalArgs_DefaultToNull()
    {
        var ex = new ApiClientHttpException("boom", HttpStatusCode.InternalServerError);

        Assert.Null(ex.ResponseContent);
        Assert.Null(ex.RequestUri);
        Assert.Null(ex.InnerException);
    }

    [Fact]
    public void ApiClientDeserializationException_PreservesResponseContentAndInner()
    {
        var inner = new System.Text.Json.JsonException("bad json");
        var ex = new ApiClientDeserializationException("boom", "<html>", inner);

        Assert.Equal("boom", ex.Message);
        Assert.Equal("<html>", ex.ResponseContent);
        Assert.Same(inner, ex.InnerException);
        Assert.Equal("API_CLIENT_DESERIALIZATION", ex.ErrorCode);
    }

    [Fact]
    public void ApiClientPremiumRequiredException_PreservesEndpointAndContent()
    {
        var ex = new ApiClientPremiumRequiredException("/api/v1/stock/candle", "premium-only");

        Assert.Equal("/api/v1/stock/candle", ex.Endpoint);
        Assert.Equal("premium-only", ex.ResponseContent);
        Assert.Contains("premium plan", ex.Message);
        Assert.Equal("API_CLIENT_PREMIUM_REQUIRED", ex.ErrorCode);
    }

    [Fact]
    public void ApiClientTimeoutException_HasExpectedErrorCode()
    {
        var inner = new TimeoutException();
        var ex = new ApiClientTimeoutException("timed out", inner);

        Assert.Equal("timed out", ex.Message);
        Assert.Same(inner, ex.InnerException);
        Assert.Equal("API_CLIENT_TIMEOUT", ex.ErrorCode);
    }

    [Fact]
    public void ApiClientCancelledException_HasExpectedErrorCode()
    {
        var ex = new ApiClientCancelledException("cancelled");

        Assert.Equal("cancelled", ex.Message);
        Assert.Equal("API_CLIENT_CANCELLED", ex.ErrorCode);
    }

    [Fact]
    public void ApiClientUnexpectedException_HasExpectedErrorCode()
    {
        var inner = new Exception("root cause");
        var ex = new ApiClientUnexpectedException("oops", inner);

        Assert.Equal("oops", ex.Message);
        Assert.Same(inner, ex.InnerException);
        Assert.Equal("API_CLIENT_UNEXPECTED", ex.ErrorCode);
    }

    [Fact]
    public void ApiClientException_BaseClass_CorrelationAndSourceServiceSettable()
    {
        var ex = new ApiClientHttpException("boom", HttpStatusCode.OK)
        {
            CorrelationId = "req-123",
            SourceService = "finnhub"
        };

        Assert.Equal("req-123", ex.CorrelationId);
        Assert.Equal("finnhub", ex.SourceService);
        Assert.True(ex.OccurredAtUtc <= DateTimeOffset.UtcNow);
    }
}
