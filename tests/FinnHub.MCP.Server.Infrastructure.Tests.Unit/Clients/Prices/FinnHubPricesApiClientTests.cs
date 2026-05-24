// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Options;
using FinnHub.MCP.Server.Application.Prices.Features.GetPriceSummary;
using FinnHub.MCP.Server.Infrastructure.Clients.Prices;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace FinnHub.MCP.Server.Infrastructure.Tests.Unit.Clients.Prices;

public sealed class FinnHubPricesApiClientTests : IDisposable
{
    private const string SamplePayload = """
                                         {
                                           "c": [100.0, 102.0, 105.0, 108.0, 110.0],
                                           "h": [101.0, 103.0, 106.0, 109.0, 111.0],
                                           "l": [99.0, 101.0, 104.0, 107.0, 109.0],
                                           "o": [99.5, 101.5, 104.5, 107.5, 109.5],
                                           "v": [10000, 12000, 11000, 13000, 14000],
                                           "t": [1700000000, 1700086400, 1700172800, 1700259200, 1700345600],
                                           "s": "ok"
                                         }
                                         """;

    private readonly MockHttpMessageHandler _handler = new();
    private readonly HttpClient _httpClient;
    private readonly FinnHubPricesApiClient _sut;

    public FinnHubPricesApiClientTests()
    {
        this._httpClient = new HttpClient(this._handler) { BaseAddress = new Uri("https://finnhub.io/api/v1/") };

        var options = Substitute.For<IOptions<FinnHubOptions>>();
        options.Value.Returns(new FinnHubOptions
        {
            BaseUrl = "https://finnhub.io/api/v1",
            ApiKey = "test-key",
            EndPoints = [new FinnHubEndPoint { Name = "candle", Url = "stock/candle", IsActive = true }]
        });

        this._sut = new FinnHubPricesApiClient(
            this._httpClient,
            options,
            NullLogger<FinnHubPricesApiClient>.Instance);
    }

    [Fact]
    public async Task GetSummaryAsync_AggregatesStats()
    {
        this._handler.SetResponse(HttpStatusCode.OK, SamplePayload);

        var result = await this._sut.GetSummaryAsync(
            new GetPriceSummaryQuery { QueryId = "q1", Symbol = "AAPL" },
            CancellationToken.None);

        Assert.Equal(5, result.CandleCount);
        Assert.Equal(99.0, result.Min);
        Assert.Equal(111.0, result.Max);
        Assert.Equal(105.0, result.Mean);
        Assert.Equal(10.0, result.ReturnPct);
        Assert.NotNull(result.Latest);
        Assert.Equal(110.0, result.Latest!.Close);
        Assert.Null(result.Candles);
    }

    [Fact]
    public async Task GetSummaryAsync_IncludeCandles_PopulatesCandles()
    {
        this._handler.SetResponse(HttpStatusCode.OK, SamplePayload);

        var result = await this._sut.GetSummaryAsync(
            new GetPriceSummaryQuery { QueryId = "q1", Symbol = "AAPL", IncludeCandles = true },
            CancellationToken.None);

        Assert.NotNull(result.Candles);
        Assert.Equal(5, result.Candles!.Close.Count);
    }

    [Fact]
    public async Task GetSummaryAsync_NoData_ReturnsZeroCandles()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "{\"s\":\"no_data\"}");

        var result = await this._sut.GetSummaryAsync(
            new GetPriceSummaryQuery { QueryId = "q1", Symbol = "UNK" },
            CancellationToken.None);

        Assert.Equal(0, result.CandleCount);
        Assert.Null(result.Min);
        Assert.Null(result.Mean);
    }

    [Fact]
    public async Task GetSummaryAsync_Forbidden_ThrowsPremiumRequired()
    {
        this._handler.SetResponse(HttpStatusCode.Forbidden, "premium");

        await Assert.ThrowsAsync<ApiClientPremiumRequiredException>(() => this._sut.GetSummaryAsync(
            new GetPriceSummaryQuery { QueryId = "q1", Symbol = "AAPL" },
            CancellationToken.None));
    }

    /// <summary>
    /// Regression: catches future URL-construction drift. See FinnHubProfilesApiClientTests
    /// for the bug class this protects against. The 1y period asserts in another test;
    /// here we pin the default 30d shape.
    /// </summary>
    [Fact]
    public async Task GetSummaryAsync_HitsApiV1StockCandleEndpointWithSymbolAndResolution()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "{\"s\":\"no_data\"}");

        await this._sut.GetSummaryAsync(
            new GetPriceSummaryQuery { QueryId = "q1", Symbol = "AAPL" },
            CancellationToken.None);

        Assert.NotNull(this._handler.LastRequest?.RequestUri);
        var uri = this._handler.LastRequest!.RequestUri!.AbsoluteUri;
        Assert.StartsWith("https://finnhub.io/api/v1/stock/candle?symbol=AAPL&resolution=D&from=", uri, StringComparison.Ordinal);
        Assert.Contains("&to=", uri, StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetSummaryAsync_OneYearPeriod_UsesWeeklyResolution()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "{\"s\":\"no_data\"}");

        var result = await this._sut.GetSummaryAsync(
            new GetPriceSummaryQuery { QueryId = "q1", Symbol = "AAPL", Period = PricePeriod.OneYear },
            CancellationToken.None);

        Assert.Equal("W", result.Resolution);
        Assert.Equal("1y", result.Period);
        Assert.Contains("resolution=W", this._handler.LastRequest!.RequestUri!.Query, StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetSummaryAsync_HttpRequestException_ThrowsHttpException()
    {
        this._handler.SetException(new HttpRequestException("network unreachable"));

        await Assert.ThrowsAsync<ApiClientHttpException>(() => this._sut.GetSummaryAsync(
            new GetPriceSummaryQuery { QueryId = "q1", Symbol = "AAPL" },
            CancellationToken.None));
    }

    [Fact]
    public async Task GetSummaryAsync_TaskCanceledWithTimeout_ThrowsTimeoutException()
    {
        this._handler.SetException(new TaskCanceledException("slow", new TimeoutException()));

        await Assert.ThrowsAsync<ApiClientTimeoutException>(() => this._sut.GetSummaryAsync(
            new GetPriceSummaryQuery { QueryId = "q1", Symbol = "AAPL" },
            CancellationToken.None));
    }

    [Fact]
    public async Task GetSummaryAsync_TokenAlreadyCancelled_ThrowsCancelledException()
    {
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        await Assert.ThrowsAsync<ApiClientCancelledException>(() => this._sut.GetSummaryAsync(
            new GetPriceSummaryQuery { QueryId = "q1", Symbol = "AAPL" },
            cts.Token));
    }

    public void Dispose()
    {
        this._handler.Dispose();
        this._httpClient.Dispose();
    }
}
