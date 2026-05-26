// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Quotes.Clients;
using FinnHub.MCP.Server.Application.Quotes.Features.GetQuote;
using FinnHub.MCP.Server.Application.Quotes.Services;
using FinnHub.MCP.Server.Application.Tests.Unit.TestDoubles;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace FinnHub.MCP.Server.Application.Tests.Unit.Application.Features.Quotes.Services;

public sealed class QuotesServiceTests
{
    private readonly IQuotesApiClient _apiClient = Substitute.For<IQuotesApiClient>();
    private readonly FakeFinnHubCache _cache = new();
    private readonly QuotesService _sut;

    public QuotesServiceTests()
    {

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
    public async Task GetQuoteAsync_TwoIdenticalCalls_HitsApiClientExactlyOnce()
    {
        // Real cache semantics — FakeFinnHubCache returns the cached value on
        // the second call instead of re-invoking the factory. This locks in
        // the "two consecutive identical calls hit upstream exactly once"
        // contract from .planning/specs/01-product-surface.md §3 P2 — the
        // contract every Wave A/B service was supposed to satisfy but only
        // SearchService had a regression test for.
        var query = new GetQuoteQuery { QueryId = "q1", Symbol = "AAPL" };
        this._apiClient.GetQuoteAsync(query, Arg.Any<CancellationToken>())
            .Returns(new GetQuoteResponse { Symbol = "AAPL", Current = 261.74 });

        await this._sut.GetQuoteAsync(query);
        await this._sut.GetQuoteAsync(query);

        Assert.Equal(1, this._cache.FactoryInvocationCount);
        await this._apiClient.Received(1).GetQuoteAsync(query, Arg.Any<CancellationToken>());
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
}
