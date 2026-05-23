// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Caching;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.News.Clients;
using FinnHub.MCP.Server.Application.News.Features.GetNewsPulse;
using FinnHub.MCP.Server.Application.News.Services;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace FinnHub.MCP.Server.Application.Tests.Unit.Application.Features.News.Services;

public sealed class NewsServiceTests
{
    private readonly INewsApiClient _apiClient = Substitute.For<INewsApiClient>();
    private readonly IFinnHubCache _cache = Substitute.For<IFinnHubCache>();
    private readonly NewsService _sut;

    public NewsServiceTests()
    {
        // Cache delegates passthrough for both list and sentiment payloads.
        this._cache
            .GetOrCreateAsync(
                Arg.Any<string>(),
                Arg.Any<CacheTier>(),
                Arg.Any<Func<CancellationToken, ValueTask<IReadOnlyList<CompanyNewsArticle>>>>(),
                Arg.Any<CancellationToken>())
            .Returns(call => call.Arg<Func<CancellationToken, ValueTask<IReadOnlyList<CompanyNewsArticle>>>>()(CancellationToken.None));

        this._cache
            .GetOrCreateAsync(
                Arg.Any<string>(),
                Arg.Any<CacheTier>(),
                Arg.Any<Func<CancellationToken, ValueTask<NewsSentiment>>>(),
                Arg.Any<CancellationToken>())
            .Returns(call => call.Arg<Func<CancellationToken, ValueTask<NewsSentiment>>>()(CancellationToken.None));

        this._sut = new NewsService(this._apiClient, this._cache, NullLogger<NewsService>.Instance);
    }

    [Fact]
    public async Task GetPulseAsync_NullQuery_Throws()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => this._sut.GetPulseAsync(null!));
    }

    [Fact]
    public async Task GetPulseAsync_WithSentimentAndArticles_ReturnsFullResponse()
    {
        var query = new GetNewsPulseQuery { QueryId = "q1", Symbol = "AAPL" };
        this._apiClient.GetSentimentAsync(query.Symbol, Arg.Any<CancellationToken>())
            .Returns(new NewsSentiment(0.8, 0.7, 0.1));
        var now = DateTimeOffset.UtcNow;
        this._apiClient.GetCompanyNewsAsync(query.Symbol, Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .Returns(_ =>
            {
                return (IReadOnlyList<CompanyNewsArticle>)
                [
                    new CompanyNewsArticle("h1", "u1", "src", now),
                    new CompanyNewsArticle("h2", "u2", "src", now.AddHours(-1))
                ];
            });

        var result = await this._sut.GetPulseAsync(query);

        Assert.True(result.IsSuccess);
        Assert.Equal(0.8, result.Data!.SentimentScore);
        Assert.Equal("finnhub", result.Data.SentimentSource);
        Assert.Equal(2, result.Data.Count);
        Assert.Equal(0, result.Data.DeltaVsPrevWeek);
        Assert.Equal(2, result.Data.TopHeadlines.Count);
    }

    [Fact]
    public async Task GetPulseAsync_PremiumSentiment_DegradesGracefully()
    {
        var query = new GetNewsPulseQuery { QueryId = "q1", Symbol = "AAPL" };
        this._apiClient.GetSentimentAsync(query.Symbol, Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientPremiumRequiredException("/api/v1/news-sentiment"));

        var now = DateTimeOffset.UtcNow;
        this._apiClient.GetCompanyNewsAsync(query.Symbol, Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .Returns(_ => (IReadOnlyList<CompanyNewsArticle>)[new CompanyNewsArticle("h1", "u1", "src", now)]);

        var result = await this._sut.GetPulseAsync(query);

        Assert.True(result.IsSuccess);
        Assert.Null(result.Data!.SentimentScore);
        Assert.Null(result.Data.SentimentSource);
        Assert.Equal(1, result.Data.Count);
    }

    [Fact]
    public async Task GetPulseAsync_NoArticles_ReturnsNotFound()
    {
        var query = new GetNewsPulseQuery { QueryId = "q1", Symbol = "UNKN" };
        this._apiClient.GetSentimentAsync(query.Symbol, Arg.Any<CancellationToken>())
            .Returns(new NewsSentiment(null, null, null));
        this._apiClient.GetCompanyNewsAsync(query.Symbol, Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .Returns(_ => (IReadOnlyList<CompanyNewsArticle>)[]);

        var result = await this._sut.GetPulseAsync(query);

        Assert.False(result.IsSuccess);
        Assert.Equal("NotFound", result.ErrorType);
    }

    [Fact]
    public async Task GetPulseAsync_TopHeadlinesCappedAtFive()
    {
        var query = new GetNewsPulseQuery { QueryId = "q1", Symbol = "AAPL" };
        this._apiClient.GetSentimentAsync(query.Symbol, Arg.Any<CancellationToken>())
            .Returns(new NewsSentiment(null, null, null));
        var now = DateTimeOffset.UtcNow;
        var many = Enumerable.Range(0, 10)
            .Select(i => new CompanyNewsArticle($"h{i}", $"u{i}", "src", now.AddMinutes(-i)))
            .ToList();
        this._apiClient.GetCompanyNewsAsync(query.Symbol, Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .Returns(_ => (IReadOnlyList<CompanyNewsArticle>)many);

        var result = await this._sut.GetPulseAsync(query);

        Assert.True(result.IsSuccess);
        Assert.Equal(5, result.Data!.TopHeadlines.Count);
        Assert.Equal("h0", result.Data.TopHeadlines[0].Headline);
    }

    [Fact]
    public async Task GetPulseAsync_IncludeAllHeadlines_ReturnsAll()
    {
        var query = new GetNewsPulseQuery { QueryId = "q1", Symbol = "AAPL", IncludeAllHeadlines = true };
        this._apiClient.GetSentimentAsync(query.Symbol, Arg.Any<CancellationToken>())
            .Returns(new NewsSentiment(null, null, null));
        var now = DateTimeOffset.UtcNow;
        var many = Enumerable.Range(0, 8)
            .Select(i => new CompanyNewsArticle($"h{i}", $"u{i}", "src", now.AddMinutes(-i)))
            .ToList();
        this._apiClient.GetCompanyNewsAsync(query.Symbol, Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .Returns(_ => (IReadOnlyList<CompanyNewsArticle>)many);

        var result = await this._sut.GetPulseAsync(query);

        Assert.Equal(8, result.Data!.TopHeadlines.Count);
    }
}
