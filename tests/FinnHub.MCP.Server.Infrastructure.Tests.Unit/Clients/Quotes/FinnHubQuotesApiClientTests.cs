// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Options;
using FinnHub.MCP.Server.Application.Quotes.Features.GetQuote;
using FinnHub.MCP.Server.Infrastructure.Clients.Quotes;
using FinnHub.MCP.Server.Infrastructure.Tests.Unit.Fixtures;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace FinnHub.MCP.Server.Infrastructure.Tests.Unit.Clients.Quotes;

public sealed class FinnHubQuotesApiClientTests : IDisposable
{
    private readonly MockHttpMessageHandler _handler = new();
    private readonly HttpClient _httpClient;
    private readonly FinnHubQuotesApiClient _sut;

    public FinnHubQuotesApiClientTests()
    {
        this._httpClient = new HttpClient(this._handler) { BaseAddress = new Uri("https://finnhub.io/api/v1/") };

        var options = Substitute.For<IOptions<FinnHubOptions>>();
        options.Value.Returns(new FinnHubOptions
        {
            BaseUrl = "https://finnhub.io/api/v1",
            ApiKey = "test-key",
            EndPoints = [new FinnHubEndPoint { Name = "quote", Url = "quote", IsActive = true }]
        });

        this._sut = new FinnHubQuotesApiClient(this._httpClient, options, NullLogger<FinnHubQuotesApiClient>.Instance);
    }

    [Fact]
    public async Task GetQuoteAsync_RealAaplResponse_MapsAllFields()
    {
        this._handler.SetResponse(HttpStatusCode.OK, Fixture.LoadFinnHub("quote-AAPL"));

        var result = await this._sut.GetQuoteAsync(
            new GetQuoteQuery { QueryId = "q1", Symbol = "AAPL" },
            CancellationToken.None);

        Assert.Equal("AAPL", result.Symbol);
        Assert.NotNull(result.Current);
        Assert.NotNull(result.Change);
        Assert.NotNull(result.PercentChange);
        Assert.NotNull(result.TimestampUtc);
        Assert.True(result.Current > 0);
    }

    /// <summary>
    /// Regression: Finnhub returns <c>{"c":0,"d":null,"dp":null,"h":0,...,"t":0}</c> for
    /// unknown symbols. With non-nullable double DTO fields this threw a JsonException
    /// at deserialization time and surfaced to clients as InvalidResponse. With
    /// nullable fields the parser tolerates the nulls and the service layer maps the
    /// all-zero/null shape to a NotFound result.
    /// </summary>
    [Fact]
    public async Task GetQuoteAsync_UnknownSymbol_ParsesNullChangeFieldsWithoutThrowing()
    {
        this._handler.SetResponse(HttpStatusCode.OK, Fixture.LoadFinnHub("quote-unknown"));

        var result = await this._sut.GetQuoteAsync(
            new GetQuoteQuery { QueryId = "q1", Symbol = "ZZZNOTASYMBOL" },
            CancellationToken.None);

        Assert.Equal(0, result.Current);
        Assert.Null(result.Change);
        Assert.Null(result.PercentChange);
        Assert.Null(result.TimestampUtc);
    }

    /// <summary>
    /// Regression: catches future URL-construction drift. See FinnHubProfilesApiClientTests
    /// for the bug class this protects against.
    /// </summary>
    [Fact]
    public async Task GetQuoteAsync_HitsApiV1QuoteEndpoint()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "{}");

        await this._sut.GetQuoteAsync(
            new GetQuoteQuery { QueryId = "q1", Symbol = "AAPL" },
            CancellationToken.None);

        Assert.NotNull(this._handler.LastRequest?.RequestUri);
        Assert.Equal(
            "https://finnhub.io/api/v1/quote?symbol=AAPL",
            this._handler.LastRequest!.RequestUri!.AbsoluteUri);
    }

    [Fact]
    public async Task GetQuoteAsync_Forbidden_ThrowsPremiumRequired()
    {
        this._handler.SetResponse(HttpStatusCode.Forbidden, "premium");

        await Assert.ThrowsAsync<ApiClientPremiumRequiredException>(() => this._sut.GetQuoteAsync(
            new GetQuoteQuery { QueryId = "q1", Symbol = "AAPL" },
            CancellationToken.None));
    }

    [Fact]
    public async Task GetQuoteAsync_InvalidJson_ThrowsDeserialization()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "not-json");

        await Assert.ThrowsAsync<ApiClientDeserializationException>(() => this._sut.GetQuoteAsync(
            new GetQuoteQuery { QueryId = "q1", Symbol = "AAPL" },
            CancellationToken.None));
    }

    [Fact]
    public async Task GetQuoteAsync_HttpRequestException_ThrowsHttpException()
    {
        this._handler.SetException(new HttpRequestException("network unreachable"));

        await Assert.ThrowsAsync<ApiClientHttpException>(() => this._sut.GetQuoteAsync(
            new GetQuoteQuery { QueryId = "q1", Symbol = "AAPL" },
            CancellationToken.None));
    }

    [Fact]
    public async Task GetQuoteAsync_TaskCanceledWithTimeout_ThrowsTimeoutException()
    {
        this._handler.SetException(new TaskCanceledException("slow", new TimeoutException()));

        await Assert.ThrowsAsync<ApiClientTimeoutException>(() => this._sut.GetQuoteAsync(
            new GetQuoteQuery { QueryId = "q1", Symbol = "AAPL" },
            CancellationToken.None));
    }

    [Fact]
    public async Task GetQuoteAsync_TokenAlreadyCancelled_ThrowsCancelledException()
    {
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        await Assert.ThrowsAsync<ApiClientCancelledException>(() => this._sut.GetQuoteAsync(
            new GetQuoteQuery { QueryId = "q1", Symbol = "AAPL" },
            cts.Token));
    }

    public void Dispose()
    {
        this._handler.Dispose();
        this._httpClient.Dispose();
    }
}
