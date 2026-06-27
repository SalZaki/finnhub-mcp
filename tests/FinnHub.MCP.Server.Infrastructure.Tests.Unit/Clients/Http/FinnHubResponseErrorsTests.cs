// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using System.Text;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Infrastructure.Clients.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace FinnHub.MCP.Server.Infrastructure.Tests.Unit.Clients.Http;

public sealed class FinnHubResponseErrorsTests
{
    private static MemoryStream Body(string text = "error-body") => new(Encoding.UTF8.GetBytes(text));

    private static HttpResponseMessage Response(HttpStatusCode status, string? path = "/api/v1/quote")
    {
        var response = new HttpResponseMessage(status);
        if (path is not null)
        {
            response.RequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://finnhub.io" + path);
        }

        return response;
    }

    [Fact]
    public async Task ThrowForStatusAsync_Forbidden_ThrowsPremiumRequiredWithEndpointAndBody()
    {
        using var response = Response(HttpStatusCode.Forbidden);

        var ex = await Assert.ThrowsAsync<ApiClientPremiumRequiredException>(() =>
            FinnHubResponseErrors.ThrowForStatusAsync(response, Body(), NullLogger.Instance, "quote", default));

        Assert.Equal("/api/v1/quote", ex.Endpoint);
        Assert.Equal("error-body", ex.ResponseContent);
    }

    [Fact]
    public async Task ThrowForStatusAsync_Unauthorized_WithFlag_ThrowsPremiumRequired()
    {
        using var response = Response(HttpStatusCode.Unauthorized);

        await Assert.ThrowsAsync<ApiClientPremiumRequiredException>(() =>
            FinnHubResponseErrors.ThrowForStatusAsync(
                response, Body(), NullLogger.Instance, "exchange-symbols", default, treatUnauthorizedAsPremium: true));
    }

    [Fact]
    public async Task ThrowForStatusAsync_Unauthorized_DefaultFlag_ThrowsHttp()
    {
        using var response = Response(HttpStatusCode.Unauthorized);

        var ex = await Assert.ThrowsAsync<ApiClientHttpException>(() =>
            FinnHubResponseErrors.ThrowForStatusAsync(response, Body(), NullLogger.Instance, "quote", default));

        Assert.Equal(HttpStatusCode.Unauthorized, ex.StatusCode);
    }

    [Theory]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.BadGateway)]
    public async Task ThrowForStatusAsync_OtherErrors_ThrowHttpWithStatusBodyAndResourceMessage(HttpStatusCode status)
    {
        using var response = Response(status);

        var ex = await Assert.ThrowsAsync<ApiClientHttpException>(() =>
            FinnHubResponseErrors.ThrowForStatusAsync(response, Body(), NullLogger.Instance, "quote", default));

        Assert.Equal(status, ex.StatusCode);
        Assert.Equal("error-body", ex.ResponseContent);
        Assert.Equal($"FinnHub quote returned {status}.", ex.Message);
    }

    [Fact]
    public async Task ThrowForStatusAsync_Forbidden_NullRequestMessage_EndpointIsUnknown()
    {
        using var response = Response(HttpStatusCode.Forbidden, path: null);

        var ex = await Assert.ThrowsAsync<ApiClientPremiumRequiredException>(() =>
            FinnHubResponseErrors.ThrowForStatusAsync(response, Body(), NullLogger.Instance, "quote", default));

        Assert.Equal("(unknown)", ex.Endpoint);
    }
}
