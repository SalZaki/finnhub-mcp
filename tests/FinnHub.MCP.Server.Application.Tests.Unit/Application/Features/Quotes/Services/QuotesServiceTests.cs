// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using FinnHub.MCP.Server.Application.Caching;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Quotes.Clients;
using FinnHub.MCP.Server.Application.Quotes.Features.GetQuote;
using FinnHub.MCP.Server.Application.Quotes.Services;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace FinnHub.MCP.Server.Application.Tests.Unit.Application.Features.Quotes.Services;

public sealed class QuotesServiceTests
{
    private readonly IQuotesApiClient _apiClient = Substitute.For<IQuotesApiClient>();
    private readonly IFinnHubCache _cache = Substitute.For<IFinnHubCache>();
    private readonly QuotesService _sut;

    public QuotesServiceTests()
    {
        this._cache
            .GetOrCreateAsync(
                Arg.Any<string>(),
                Arg.Any<CacheTier>(),
                Arg.Any<Func<CancellationToken, ValueTask<GetQuoteResponse>>>(),
                Arg.Any<CancellationToken>())
            .Returns(call => call.Arg<Func<CancellationToken, ValueTask<GetQuoteResponse>>>()(CancellationToken.None));

        this._sut = new QuotesService(this._apiClient, this._cache, NullLogger<QuotesService>.Instance);
    }

    [Fact]
    public async Task GetQuoteAsync_NullQuery_Throws()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => this._sut.GetQuoteAsync(null!));
    }

    [Fact]
    public async Task GetQuoteAsync_LivePrice_ReturnsSuccess()
    {
        var query = new GetQuoteQuery { QueryId = "q1", Symbol = "AAPL" };
        this._apiClient.GetQuoteAsync(query, Arg.Any<CancellationToken>())
            .Returns(new GetQuoteResponse { Symbol = "AAPL", Current = 261.74, Change = 0.4 });

        var result = await this._sut.GetQuoteAsync(query);

        Assert.True(result.IsSuccess);
        Assert.Equal(261.74, result.Data!.Current);
    }

    [Fact]
    public async Task GetQuoteAsync_ZeroPrice_ReturnsNotFound()
    {
        var query = new GetQuoteQuery { QueryId = "q1", Symbol = "UNKN" };
        this._apiClient.GetQuoteAsync(query, Arg.Any<CancellationToken>())
            .Returns(new GetQuoteResponse { Symbol = "UNKN", Current = 0 });

        var result = await this._sut.GetQuoteAsync(query);

        Assert.False(result.IsSuccess);
        Assert.Equal("NotFound", result.ErrorType);
    }

    [Fact]
    public async Task GetQuoteAsync_HttpError_MapsToServiceUnavailable()
    {
        var query = new GetQuoteQuery { QueryId = "q1", Symbol = "AAPL" };
        this._apiClient.GetQuoteAsync(query, Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientHttpException("boom", HttpStatusCode.BadGateway));

        var result = await this._sut.GetQuoteAsync(query);

        Assert.False(result.IsSuccess);
        Assert.Equal("ServiceUnavailable", result.ErrorType);
    }

    [Fact]
    public async Task GetQuoteAsync_Cancelled_RethrowsTypedException_DoesNotDemoteToUnknown()
    {
        // Regression: previously the catch (ApiClientException ex) catch-all
        // turned ApiClientCancelledException into ResultErrorType.Unknown with
        // a generic "lookup failed unexpectedly" message — indistinguishable
        // from a real upstream failure. Now the typed catch arm rethrows.
        var query = new GetQuoteQuery { QueryId = "q1", Symbol = "AAPL" };
        this._apiClient.GetQuoteAsync(query, Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientCancelledException("cancelled by caller"));

        var ex = await Assert.ThrowsAsync<ApiClientCancelledException>(
            () => this._sut.GetQuoteAsync(query));
        Assert.Equal("cancelled by caller", ex.Message);
    }
}
