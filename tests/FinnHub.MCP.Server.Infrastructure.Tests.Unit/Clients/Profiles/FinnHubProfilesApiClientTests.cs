// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Options;
using FinnHub.MCP.Server.Application.Profiles.Features.GetCompanyProfile;
using FinnHub.MCP.Server.Infrastructure.Clients.Profiles;
using FinnHub.MCP.Server.Infrastructure.Tests.Unit.Fixtures;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace FinnHub.MCP.Server.Infrastructure.Tests.Unit.Clients.Profiles;

public sealed class FinnHubProfilesApiClientTests : IDisposable
{
    private const string SamplePayload = """
                                         {
                                           "ticker": "AAPL",
                                           "name": "Apple Inc",
                                           "country": "US",
                                           "currency": "USD",
                                           "exchange": "NASDAQ NMS - GLOBAL MARKET",
                                           "ipo": "1980-12-12",
                                           "marketCapitalization": 3000000.0,
                                           "shareOutstanding": 15500.0,
                                           "finnhubIndustry": "Technology",
                                           "logo": "https://logo.example/aapl.png",
                                           "phone": "+1-408-996-1010",
                                           "weburl": "https://www.apple.com"
                                         }
                                         """;

    private readonly MockHttpMessageHandler _handler = new();
    private readonly HttpClient _httpClient;
    private readonly FinnHubProfilesApiClient _sut;

    public FinnHubProfilesApiClientTests()
    {
        this._httpClient = new HttpClient(this._handler) { BaseAddress = new Uri("https://finnhub.io/api/v1/") };

        var options = Substitute.For<IOptions<FinnHubOptions>>();
        options.Value.Returns(new FinnHubOptions
        {
            BaseUrl = "https://finnhub.io/api/v1",
            ApiKey = "test-key",
            EndPoints = [new FinnHubEndPoint { Name = "profile", Url = "stock/profile2", IsActive = true }]
        });

        this._sut = new FinnHubProfilesApiClient(this._httpClient, options, NullLogger<FinnHubProfilesApiClient>.Instance);
    }

    [Fact]
    public async Task GetProfileAsync_SummaryView_DropsCosmeticFields()
    {
        this._handler.SetResponse(HttpStatusCode.OK, SamplePayload);

        var result = await this._sut.GetProfileAsync(
            new GetCompanyProfileQuery { QueryId = "q1", Symbol = "AAPL", IncludeCosmeticFields = false },
            CancellationToken.None);

        Assert.Equal("Apple Inc", result.Name);
        Assert.Equal("Technology", result.Industry);
        Assert.Null(result.Logo);
        Assert.Null(result.Phone);
        Assert.Null(result.WebUrl);
    }

    [Fact]
    public async Task GetProfileAsync_StandardView_IncludesCosmeticFields()
    {
        this._handler.SetResponse(HttpStatusCode.OK, SamplePayload);

        var result = await this._sut.GetProfileAsync(
            new GetCompanyProfileQuery { QueryId = "q1", Symbol = "AAPL", IncludeCosmeticFields = true },
            CancellationToken.None);

        Assert.Equal("https://logo.example/aapl.png", result.Logo);
        Assert.Equal("+1-408-996-1010", result.Phone);
        Assert.Equal("https://www.apple.com", result.WebUrl);
    }

    [Fact]
    public async Task GetProfileAsync_EmptyResponse_ReturnsTickerFallback()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "{}");

        var result = await this._sut.GetProfileAsync(
            new GetCompanyProfileQuery { QueryId = "q1", Symbol = "UNKN" },
            CancellationToken.None);

        Assert.Equal("UNKN", result.Ticker);
        Assert.Null(result.Name);
    }

    /// <summary>
    /// Regression: previously this client constructed relative URIs that, combined with a
    /// slash-less BaseAddress, sent requests to /api/stock/profile2 (dropping /v1) — surfaced
    /// as InvalidResponse from HTML deserialization. Any future drift in URL construction
    /// will fail this assertion before it hits production.
    /// </summary>
    [Fact]
    public async Task GetProfileAsync_HitsApiV1StockProfile2Endpoint()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "{}");

        await this._sut.GetProfileAsync(
            new GetCompanyProfileQuery { QueryId = "q1", Symbol = "AAPL" },
            CancellationToken.None);

        Assert.NotNull(this._handler.LastRequest?.RequestUri);
        Assert.Equal(
            "https://finnhub.io/api/v1/stock/profile2?symbol=AAPL",
            this._handler.LastRequest!.RequestUri!.AbsoluteUri);
    }

    [Fact]
    public async Task GetProfileAsync_Forbidden_ThrowsPremiumRequired()
    {
        this._handler.SetResponse(HttpStatusCode.Forbidden, "premium");

        await Assert.ThrowsAsync<ApiClientPremiumRequiredException>(() => this._sut.GetProfileAsync(
            new GetCompanyProfileQuery { QueryId = "q1", Symbol = "AAPL" },
            CancellationToken.None));
    }

    [Fact]
    public async Task GetProfileAsync_HttpRequestException_ThrowsHttpException()
    {
        this._handler.SetException(new HttpRequestException("network unreachable"));

        await Assert.ThrowsAsync<ApiClientHttpException>(() => this._sut.GetProfileAsync(
            new GetCompanyProfileQuery { QueryId = "q1", Symbol = "AAPL" },
            CancellationToken.None));
    }

    [Fact]
    public async Task GetProfileAsync_TaskCanceledWithTimeout_ThrowsTimeoutException()
    {
        this._handler.SetException(new TaskCanceledException("slow", new TimeoutException()));

        await Assert.ThrowsAsync<ApiClientTimeoutException>(() => this._sut.GetProfileAsync(
            new GetCompanyProfileQuery { QueryId = "q1", Symbol = "AAPL" },
            CancellationToken.None));
    }

    [Fact]
    public async Task GetProfileAsync_TokenAlreadyCancelled_ThrowsCancelledException()
    {
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        await Assert.ThrowsAsync<ApiClientCancelledException>(() => this._sut.GetProfileAsync(
            new GetCompanyProfileQuery { QueryId = "q1", Symbol = "AAPL" },
            cts.Token));
    }

    /// <summary>
    /// Real captured Finnhub /stock/profile2 response for AAPL. Proves the parser
    /// handles the actual wire shape including fields the synthetic SamplePayload
    /// doesn't carry (e.g. estimateCurrency). Catches Finnhub shape drift on the
    /// next fixture refresh — see CLAUDE.md "Don't ship synthetic-payload-only client tests".
    /// </summary>
    [Fact]
    public async Task GetProfileAsync_RealAaplFixture_MapsAllFields()
    {
        this._handler.SetResponse(HttpStatusCode.OK, Fixture.LoadFinnHub("profile-AAPL"));

        var result = await this._sut.GetProfileAsync(
            new GetCompanyProfileQuery { QueryId = "q1", Symbol = "AAPL" },
            CancellationToken.None);

        Assert.Equal("AAPL", result.Ticker);
        Assert.Equal("US", result.Country);
        Assert.Equal("USD", result.Currency);
        Assert.False(string.IsNullOrEmpty(result.Exchange));
        Assert.False(string.IsNullOrEmpty(result.Industry));
        Assert.True(result.MarketCap > 0);
    }

    public void Dispose()
    {
        this._handler.Dispose();
        this._httpClient.Dispose();
    }
}
