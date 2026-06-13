// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Search.Clients;
using FinnHub.MCP.Server.Application.Search.Features.SearchSymbol;
using FinnHub.MCP.Server.Application.Symbols;
using FinnHub.MCP.Server.Application.Tests.Unit.TestDoubles;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace FinnHub.MCP.Server.Application.Tests.Unit.Symbols;

public sealed class SymbolResolverTests
{
    private readonly ISearchApiClient _searchApiClient = Substitute.For<ISearchApiClient>();
    private readonly FakeFinnHubCache _cache = new();
    private readonly SymbolResolver _resolver;

    public SymbolResolverTests()
    {
        ILogger<SymbolResolver> logger = Substitute.For<ILogger<SymbolResolver>>();
        this._resolver = new SymbolResolver(this._searchApiClient, this._cache, logger);
    }

    [Fact]
    public async Task ResolveAsync_BareTicker_ReturnsFastPathWithConfidenceOne()
    {
        var result = await this._resolver.ResolveAsync("AAPL");

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal("AAPL", result.Data.Canonical);
        Assert.Equal("AAPL", result.Data.Display);
        Assert.Null(result.Data.Exchange);
        Assert.Equal(1.0d, result.Data.Confidence);
        Assert.Empty(result.Data.Candidates);

        await this._searchApiClient.DidNotReceive().SearchSymbolAsync(
            Arg.Any<SearchSymbolQuery>(), Arg.Any<CancellationToken>());
        Assert.Equal(0, this._cache.FactoryInvocationCount);
    }

    [Fact]
    public async Task ResolveAsync_LowercaseTicker_UppercasedOnFastPath()
    {
        var result = await this._resolver.ResolveAsync("aapl");

        Assert.True(result.IsSuccess);
        Assert.Equal("AAPL", result.Data!.Canonical);
        Assert.Equal(1.0d, result.Data.Confidence);
        await this._searchApiClient.DidNotReceive().SearchSymbolAsync(
            Arg.Any<SearchSymbolQuery>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ResolveAsync_SuffixForm_SplitsTickerAndExchange()
    {
        var result = await this._resolver.ResolveAsync("AAPL.US");

        Assert.True(result.IsSuccess);
        Assert.Equal("AAPL", result.Data!.Canonical);
        Assert.Equal("AAPL.US", result.Data.Display);
        Assert.Equal("US", result.Data.Exchange);
        Assert.Equal(1.0d, result.Data.Confidence);
        await this._searchApiClient.DidNotReceive().SearchSymbolAsync(
            Arg.Any<SearchSymbolQuery>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ResolveAsync_ColonForm_SplitsExchangeAndTicker()
    {
        var result = await this._resolver.ResolveAsync("NASDAQ:AAPL");

        Assert.True(result.IsSuccess);
        Assert.Equal("AAPL", result.Data!.Canonical);
        Assert.Equal("NASDAQ:AAPL", result.Data.Display);
        Assert.Equal("NASDAQ", result.Data.Exchange);
        Assert.Equal(1.0d, result.Data.Confidence);
        await this._searchApiClient.DidNotReceive().SearchSymbolAsync(
            Arg.Any<SearchSymbolQuery>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ResolveAsync_AmbiguousInput_CallsUpstreamAndRanksByConfidence()
    {
        this._searchApiClient
            .SearchSymbolAsync(Arg.Any<SearchSymbolQuery>(), Arg.Any<CancellationToken>())
            .Returns(new SearchSymbolResponse
            {
                Symbols =
                [
                    Stock("AAPN", 0.40),
                    Stock("AAPL", 0.92),
                    Stock("APLE", 0.70)
                ]
            });

        var result = await this._resolver.ResolveAsync("apple inc");

        Assert.True(result.IsSuccess);
        Assert.Equal("AAPL", result.Data!.Canonical);
        Assert.Equal(0.92d, result.Data.Confidence);
        Assert.Equal(3, result.Data.Candidates.Count);
        Assert.Equal("AAPL", result.Data.Candidates[0].Canonical);
        Assert.Equal("APLE", result.Data.Candidates[1].Canonical);
        Assert.Equal("AAPN", result.Data.Candidates[2].Canonical);
        Assert.Empty(result.Data.Candidates[0].Candidates);
        Assert.Equal(1, this._cache.FactoryInvocationCount);
    }

    [Fact]
    public async Task ResolveAsync_AmbiguousInput_CapsCandidatesAtFive()
    {
        this._searchApiClient
            .SearchSymbolAsync(Arg.Any<SearchSymbolQuery>(), Arg.Any<CancellationToken>())
            .Returns(new SearchSymbolResponse
            {
                Symbols =
                [
                    Stock("S1", 0.9), Stock("S2", 0.8), Stock("S3", 0.7),
                    Stock("S4", 0.6), Stock("S5", 0.5), Stock("S6", 0.4),
                    Stock("S7", 0.3), Stock("S8", 0.2)
                ]
            });

        var result = await this._resolver.ResolveAsync("something");

        Assert.True(result.IsSuccess);
        Assert.Equal(5, result.Data!.Candidates.Count);
    }

    [Fact]
    public async Task ResolveAsync_AmbiguousInput_NoUpstreamMatch_ReturnsNotFound()
    {
        this._searchApiClient
            .SearchSymbolAsync(Arg.Any<SearchSymbolQuery>(), Arg.Any<CancellationToken>())
            .Returns(new SearchSymbolResponse { Symbols = [] });

        var result = await this._resolver.ResolveAsync("no such company");

        Assert.False(result.IsSuccess);
        Assert.Equal("NotFound", result.ErrorType);
        Assert.Contains("no such company", result.ErrorMessage);
    }

    [Fact]
    public async Task ResolveAsync_AmbiguousInput_CacheShortCircuitsSecondCall()
    {
        this._searchApiClient
            .SearchSymbolAsync(Arg.Any<SearchSymbolQuery>(), Arg.Any<CancellationToken>())
            .Returns(new SearchSymbolResponse { Symbols = [Stock("AAPL", 0.95)] });

        await this._resolver.ResolveAsync("apple inc");
        await this._resolver.ResolveAsync("APPLE INC");
        await this._resolver.ResolveAsync("Apple Inc");

        Assert.Equal(1, this._cache.FactoryInvocationCount);
        await this._searchApiClient.Received(1).SearchSymbolAsync(
            Arg.Any<SearchSymbolQuery>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ResolveAsync_AmbiguousInput_DifferentInputsHitUpstreamPerCall()
    {
        this._searchApiClient
            .SearchSymbolAsync(Arg.Any<SearchSymbolQuery>(), Arg.Any<CancellationToken>())
            .Returns(new SearchSymbolResponse { Symbols = [Stock("X", 0.9)] });

        await this._resolver.ResolveAsync("apple inc");
        await this._resolver.ResolveAsync("microsoft corp");

        Assert.Equal(2, this._cache.FactoryInvocationCount);
    }

    [Fact]
    public async Task ResolveAsync_UpstreamThrows_DoesNotPoisonCache()
    {
        var callCount = 0;
        this._searchApiClient
            .SearchSymbolAsync(Arg.Any<SearchSymbolQuery>(), Arg.Any<CancellationToken>())
            .Returns(_ =>
            {
                callCount++;
                if (callCount == 1)
                {
                    throw new ApiClientHttpException("boom", HttpStatusCode.ServiceUnavailable);
                }
                return new SearchSymbolResponse { Symbols = [Stock("AAPL", 0.95)] };
            });

        var first = await this._resolver.ResolveAsync("apple inc");
        var second = await this._resolver.ResolveAsync("apple inc");

        Assert.False(first.IsSuccess);
        Assert.Equal("ServiceUnavailable", first.ErrorType);
        Assert.True(second.IsSuccess);
        Assert.Equal("AAPL", second.Data!.Canonical);
        Assert.Equal(2, callCount);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ResolveAsync_NullOrWhitespaceInput_Throws(string? input)
    {
        // ArgumentException.ThrowIfNullOrWhiteSpace throws ArgumentNullException for null
        // and ArgumentException for whitespace/empty — both derive from ArgumentException.
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            this._resolver.ResolveAsync(input!));
    }

    [Fact]
    public async Task ResolveAsync_TrimmedInputExceedsMaxLength_Throws()
    {
        // 501 non-whitespace chars: the trimmed value is over the 500-char bound.
        var input = new string('a', 501);

        await Assert.ThrowsAsync<ArgumentException>(() => this._resolver.ResolveAsync(input));
    }

    [Fact]
    public async Task ResolveAsync_RawOverLimitButTrimmedWithinLimit_PassesLengthGuard()
    {
        // Raw length 504 (> 500) but trimmed length 500 (within bound). The length guard must
        // measure the trimmed value, so this proceeds to the ambiguous path instead of throwing.
        // Pre-fix it threw, because the bound was checked against the raw input.
        this._searchApiClient
            .SearchSymbolAsync(Arg.Any<SearchSymbolQuery>(), Arg.Any<CancellationToken>())
            .Returns(new SearchSymbolResponse { Symbols = [Stock("AAPL", 0.95)] });

        var input = "  " + new string('a', 500) + "  ";

        var result = await this._resolver.ResolveAsync(input);

        Assert.True(result.IsSuccess);
        Assert.Equal("AAPL", result.Data!.Canonical);
    }

    [Fact]
    public async Task ResolveAsync_BareTickerWithDigits_FallsThroughToAmbiguousPath()
    {
        // Canonical regex excludes digits — "2330" routes through the ambiguous path.
        // Documents the regex constraint and the P-future cleanup noted in the resolver's XML doc.
        this._searchApiClient
            .SearchSymbolAsync(Arg.Any<SearchSymbolQuery>(), Arg.Any<CancellationToken>())
            .Returns(new SearchSymbolResponse { Symbols = [Stock("2330.TW", 0.99)] });

        var result = await this._resolver.ResolveAsync("2330");

        Assert.True(result.IsSuccess);
        Assert.Equal("2330.TW", result.Data!.Canonical);
        Assert.Equal(1, this._cache.FactoryInvocationCount);
    }

    private static StockSymbol Stock(string symbol, double confidence) => new()
    {
        Symbol = symbol,
        DisplaySymbol = symbol,
        Description = symbol,
        Type = "Common Stock",
        Exchange = "US",
        ConfidenceScore = confidence
    };
}
