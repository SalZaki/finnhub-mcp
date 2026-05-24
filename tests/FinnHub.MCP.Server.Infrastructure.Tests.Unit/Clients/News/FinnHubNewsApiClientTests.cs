// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Options;
using FinnHub.MCP.Server.Infrastructure.Clients.News;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace FinnHub.MCP.Server.Infrastructure.Tests.Unit.Clients.News;

public sealed class FinnHubNewsApiClientTests : IDisposable
{
    private const string SentimentPayload = """
                                            {
                                              "symbol": "AAPL",
                                              "companyNewsScore": 0.84,
                                              "sentiment": { "bullishPercent": 0.85, "bearishPercent": 0.15 }
                                            }
                                            """;

    private const string CompanyNewsPayload = """
                                              [
                                                { "headline": "h1", "url": "u1", "source": "s1", "datetime": 1700000000 },
                                                { "headline": "h2", "url": "u2", "source": "s2", "datetime": 1700086400 }
                                              ]
                                              """;

    private readonly MockHttpMessageHandler _handler = new();
    private readonly HttpClient _httpClient;
    private readonly FinnHubNewsApiClient _sut;

    public FinnHubNewsApiClientTests()
    {
        this._httpClient = new HttpClient(this._handler) { BaseAddress = new Uri("https://finnhub.io/api/v1/") };

        var options = Substitute.For<IOptions<FinnHubOptions>>();
        options.Value.Returns(new FinnHubOptions
        {
            BaseUrl = "https://finnhub.io/api/v1",
            ApiKey = "test-key",
            EndPoints =
            [
                new FinnHubEndPoint { Name = "company-news", Url = "company-news", IsActive = true },
                new FinnHubEndPoint { Name = "news-sentiment", Url = "news-sentiment", IsActive = true }
            ]
        });

        this._sut = new FinnHubNewsApiClient(this._httpClient, options, NullLogger<FinnHubNewsApiClient>.Instance);
    }

    [Fact]
    public async Task GetSentimentAsync_Success_ParsesBreakdown()
    {
        this._handler.SetResponse(HttpStatusCode.OK, SentimentPayload);

        var result = await this._sut.GetSentimentAsync("AAPL", CancellationToken.None);

        Assert.Equal(0.84, result.CompanyNewsScore);
        Assert.Equal(0.85, result.BullishPercent);
        Assert.Equal(0.15, result.BearishPercent);
    }

    [Fact]
    public async Task GetSentimentAsync_Forbidden_ThrowsPremiumRequired()
    {
        this._handler.SetResponse(HttpStatusCode.Forbidden, "premium");

        await Assert.ThrowsAsync<ApiClientPremiumRequiredException>(() =>
            this._sut.GetSentimentAsync("AAPL", CancellationToken.None));
    }

    /// <summary>
    /// Regression: catches future URL-construction drift on /news-sentiment. See
    /// FinnHubProfilesApiClientTests for the bug class this protects against.
    /// </summary>
    [Fact]
    public async Task GetSentimentAsync_HitsApiV1NewsSentimentEndpoint()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "{}");

        await this._sut.GetSentimentAsync("AAPL", CancellationToken.None);

        Assert.NotNull(this._handler.LastRequest?.RequestUri);
        Assert.Equal(
            "https://finnhub.io/api/v1/news-sentiment?symbol=AAPL",
            this._handler.LastRequest!.RequestUri!.AbsoluteUri);
    }

    /// <summary>
    /// Regression: catches future URL-construction drift on /company-news. The from/to
    /// date params are also pinned because they previously had off-by-one issues elsewhere.
    /// </summary>
    [Fact]
    public async Task GetCompanyNewsAsync_HitsApiV1CompanyNewsEndpointWithDateRange()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "[]");

        await this._sut.GetCompanyNewsAsync(
            "AAPL",
            new DateOnly(2026, 5, 16),
            new DateOnly(2026, 5, 23),
            CancellationToken.None);

        Assert.NotNull(this._handler.LastRequest?.RequestUri);
        Assert.Equal(
            "https://finnhub.io/api/v1/company-news?symbol=AAPL&from=2026-05-16&to=2026-05-23",
            this._handler.LastRequest!.RequestUri!.AbsoluteUri);
    }

    [Fact]
    public async Task GetCompanyNewsAsync_Success_MapsArticles()
    {
        this._handler.SetResponse(HttpStatusCode.OK, CompanyNewsPayload);

        var result = await this._sut.GetCompanyNewsAsync(
            "AAPL",
            new DateOnly(2026, 5, 1),
            new DateOnly(2026, 5, 7),
            CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Equal("h1", result[0].Headline);
        Assert.Equal("u1", result[0].Url);
        Assert.Contains("from=2026-05-01", this._handler.LastRequest!.RequestUri!.Query, StringComparison.Ordinal);
        Assert.Contains("to=2026-05-07", this._handler.LastRequest!.RequestUri!.Query, StringComparison.Ordinal);
    }

    [Fact]
    public async Task GetCompanyNewsAsync_Empty_ReturnsEmptyList()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "[]");

        var result = await this._sut.GetCompanyNewsAsync(
            "UNK",
            new DateOnly(2026, 5, 1),
            new DateOnly(2026, 5, 7),
            CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetCompanyNewsAsync_BlankHeadline_Filtered()
    {
        this._handler.SetResponse(HttpStatusCode.OK,
            "[{\"headline\":\"\",\"url\":\"u\",\"source\":\"s\",\"datetime\":1700000000},{\"headline\":\"h2\",\"url\":\"u2\",\"source\":\"s2\",\"datetime\":1700000000}]");

        var result = await this._sut.GetCompanyNewsAsync(
            "AAPL",
            new DateOnly(2026, 5, 1),
            new DateOnly(2026, 5, 7),
            CancellationToken.None);

        Assert.Single(result);
        Assert.Equal("h2", result[0].Headline);
    }

    public void Dispose()
    {
        this._handler.Dispose();
        this._httpClient.Dispose();
    }
}
