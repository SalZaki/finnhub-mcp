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

    public void Dispose()
    {
        this._handler.Dispose();
        this._httpClient.Dispose();
    }
}
