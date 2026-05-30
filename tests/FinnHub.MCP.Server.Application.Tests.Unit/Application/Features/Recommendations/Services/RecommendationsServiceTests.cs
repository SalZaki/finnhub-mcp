// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Recommendations.Clients;
using FinnHub.MCP.Server.Application.Recommendations.Features.GetRecommendations;
using FinnHub.MCP.Server.Application.Recommendations.Services;
using FinnHub.MCP.Server.Application.Tests.Unit.TestDoubles;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace FinnHub.MCP.Server.Application.Tests.Unit.Application.Features.Recommendations.Services;

public sealed class RecommendationsServiceTests
{
    private readonly IRecommendationsApiClient _apiClient = Substitute.For<IRecommendationsApiClient>();
    private readonly FakeFinnHubCache _cache = new();
    private readonly RecommendationsService _sut;

    public RecommendationsServiceTests()
    {
        this._sut = new RecommendationsService(this._apiClient, this._cache, NullLogger<RecommendationsService>.Instance);
    }

    private static GetRecommendationsQuery Query(string symbol = "AAPL") => new()
    {
        QueryId = "q1",
        Symbol = symbol
    };

    private static RecommendationSnapshot Snap(
        int yearOffset,
        int strongBuy = 15,
        int buy = 24,
        int hold = 13,
        int sell = 2,
        int strongSell = 0) => new()
        {
            Period = new DateOnly(2026, 5, 1).AddMonths(-yearOffset),
            StrongBuy = strongBuy,
            Buy = buy,
            Hold = hold,
            Sell = sell,
            StrongSell = strongSell
        };

    [Fact]
    public async Task GetRecommendationsAsync_NullQuery_Throws()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => this._sut.GetRecommendationsAsync(null!));
    }

    [Fact]
    public async Task GetRecommendationsAsync_Empty_ReturnsNotFound()
    {
        this._apiClient
            .GetRecommendationsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns([]);

        var result = await this._sut.GetRecommendationsAsync(Query());

        Assert.False(result.IsSuccess);
        Assert.Equal("NotFound", result.ErrorType);
    }

    [Fact]
    public async Task GetRecommendationsAsync_WithCurrentAndPrevious_PopulatesChangeVsPrev()
    {
        RecommendationSnapshot[] upstream =
        [
            Snap(0, strongBuy: 15, buy: 24, hold: 13, sell: 2),
            Snap(1, strongBuy: 14, buy: 23, hold: 15, sell: 2)
        ];
        this._apiClient
            .GetRecommendationsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(upstream);

        var result = await this._sut.GetRecommendationsAsync(Query());

        Assert.True(result.IsSuccess);
        var change = result.Data!.ChangeVsPrev;
        Assert.NotNull(change);
        Assert.Equal(new DateOnly(2026, 4, 1), change!.PrevPeriod);
        Assert.Equal(1, change.StrongBuyDelta);
        Assert.Equal(1, change.BuyDelta);
        Assert.Equal(-2, change.HoldDelta);
        Assert.Equal(0, change.SellDelta);
    }

    [Fact]
    public async Task GetRecommendationsAsync_OnlyOnePeriod_ChangeVsPrevIsNull()
    {
        this._apiClient
            .GetRecommendationsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns([Snap(0)]);

        var result = await this._sut.GetRecommendationsAsync(Query());

        Assert.True(result.IsSuccess);
        Assert.Null(result.Data!.ChangeVsPrev);
    }

    [Fact]
    public async Task GetRecommendationsAsync_OrdersUpstreamByPeriodDescending()
    {
        // Provide unordered upstream — service should still pick the latest as current.
        RecommendationSnapshot[] unordered =
        [
            Snap(2, strongBuy: 5),
            Snap(0, strongBuy: 15),
            Snap(1, strongBuy: 10)
        ];
        this._apiClient
            .GetRecommendationsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(unordered);

        var result = await this._sut.GetRecommendationsAsync(Query());

        Assert.True(result.IsSuccess);
        Assert.Equal(new DateOnly(2026, 5, 1), result.Data!.Period);
        Assert.Equal(15, result.Data.StrongBuy);
        Assert.Equal(new DateOnly(2026, 4, 1), result.Data.ChangeVsPrev!.PrevPeriod);
    }

    [Fact]
    public async Task GetRecommendationsAsync_TwoIdenticalCalls_HitsApiClientExactlyOnceAndNoExtraUpstreamForChange()
    {
        // AC: change_vs_prev must come from the single cached payload — no extra upstream call.
        this._apiClient
            .GetRecommendationsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns([Snap(0), Snap(1)]);

        await this._sut.GetRecommendationsAsync(Query());
        await this._sut.GetRecommendationsAsync(Query());

        Assert.Equal(1, this._cache.FactoryInvocationCount);
        await this._apiClient.Received(1).GetRecommendationsAsync(
            Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData(50, 0, 0, 0, 0, "Strong Buy")] // pure strong buy → score = +2
    [InlineData(0, 50, 0, 0, 0, "Buy")]        // pure buy → score = +1
    [InlineData(0, 0, 50, 0, 0, "Hold")]       // pure hold → score = 0
    [InlineData(0, 0, 0, 50, 0, "Sell")]       // pure sell → score = −1
    [InlineData(0, 0, 0, 0, 50, "Strong Sell")] // pure strong sell → score = −2
    public void DeriveConsensus_PureBuckets_MapsToLabel(int sb, int b, int h, int s, int ss, string expected)
    {
        var snap = new RecommendationSnapshot
        {
            Period = new DateOnly(2026, 5, 1),
            StrongBuy = sb,
            Buy = b,
            Hold = h,
            Sell = s,
            StrongSell = ss
        };

        Assert.Equal(expected, RecommendationsService.DeriveConsensus(snap));
    }

    [Fact]
    public void DeriveConsensus_AllZero_ReturnsHold()
    {
        // Defensive: divide-by-zero guard treats no-coverage as neutral.
        var snap = new RecommendationSnapshot
        {
            Period = new DateOnly(2026, 5, 1),
            StrongBuy = 0,
            Buy = 0,
            Hold = 0,
            Sell = 0,
            StrongSell = 0
        };

        Assert.Equal("Hold", RecommendationsService.DeriveConsensus(snap));
    }

    [Fact]
    public async Task GetRecommendationsAsync_BullishShift_LabelsMoreBullish()
    {
        // Current is strong buy-leaning, previous is hold-leaning.
        RecommendationSnapshot[] upstream =
        [
            Snap(0, strongBuy: 30, buy: 10, hold: 5, sell: 0, strongSell: 0),
            Snap(1, strongBuy: 5, buy: 10, hold: 25, sell: 5, strongSell: 0)
        ];
        this._apiClient
            .GetRecommendationsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(upstream);

        var result = await this._sut.GetRecommendationsAsync(Query());

        Assert.True(result.IsSuccess);
        Assert.Equal("more bullish", result.Data!.ChangeVsPrev!.ConsensusShift);
    }

    [Fact]
    public async Task GetRecommendationsAsync_BearishShift_LabelsMoreBearish()
    {
        RecommendationSnapshot[] upstream =
        [
            Snap(0, strongBuy: 5, buy: 10, hold: 25, sell: 10, strongSell: 5),
            Snap(1, strongBuy: 30, buy: 10, hold: 5, sell: 0, strongSell: 0)
        ];
        this._apiClient
            .GetRecommendationsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(upstream);

        var result = await this._sut.GetRecommendationsAsync(Query());

        Assert.True(result.IsSuccess);
        Assert.Equal("more bearish", result.Data!.ChangeVsPrev!.ConsensusShift);
    }

    [Fact]
    public async Task GetRecommendationsAsync_NoMeaningfulShift_LabelsNoChange()
    {
        RecommendationSnapshot[] upstream =
        [
            Snap(0, strongBuy: 15, buy: 24, hold: 13, sell: 2),
            Snap(1, strongBuy: 15, buy: 24, hold: 13, sell: 2)
        ];
        this._apiClient
            .GetRecommendationsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(upstream);

        var result = await this._sut.GetRecommendationsAsync(Query());

        Assert.True(result.IsSuccess);
        Assert.Equal("no change", result.Data!.ChangeVsPrev!.ConsensusShift);
    }

    [Fact]
    public async Task GetRecommendationsAsync_HttpError_MapsToServiceUnavailable()
    {
        this._apiClient
            .GetRecommendationsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientHttpException("boom", HttpStatusCode.BadGateway));

        var result = await this._sut.GetRecommendationsAsync(Query());

        Assert.False(result.IsSuccess);
        Assert.Equal("ServiceUnavailable", result.ErrorType);
    }

    [Fact]
    public async Task GetRecommendationsAsync_Premium_MapsToPremiumRequired()
    {
        this._apiClient
            .GetRecommendationsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientPremiumRequiredException("/stock/recommendation", "premium"));

        var result = await this._sut.GetRecommendationsAsync(Query());

        Assert.False(result.IsSuccess);
        Assert.Equal("PremiumRequired", result.ErrorType);
    }

    [Fact]
    public async Task GetRecommendationsAsync_Timeout_MapsToTimeout()
    {
        this._apiClient
            .GetRecommendationsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientTimeoutException("slow"));

        var result = await this._sut.GetRecommendationsAsync(Query());

        Assert.False(result.IsSuccess);
        Assert.Equal("Timeout", result.ErrorType);
    }

    [Fact]
    public async Task GetRecommendationsAsync_InvalidResponse_MapsToInvalidResponse()
    {
        this._apiClient
            .GetRecommendationsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientDeserializationException("bad json"));

        var result = await this._sut.GetRecommendationsAsync(Query());

        Assert.False(result.IsSuccess);
        Assert.Equal("InvalidResponse", result.ErrorType);
    }

    [Fact]
    public async Task GetRecommendationsAsync_Cancelled_RethrowsTypedException()
    {
        this._apiClient
            .GetRecommendationsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientCancelledException("cancelled"));

        await Assert.ThrowsAsync<ApiClientCancelledException>(() => this._sut.GetRecommendationsAsync(Query()));
    }

    [Fact]
    public async Task GetRecommendationsAsync_UnknownApiClientException_MapsToUnknown()
    {
        this._apiClient
            .GetRecommendationsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientUnexpectedException("weird"));

        var result = await this._sut.GetRecommendationsAsync(Query());

        Assert.False(result.IsSuccess);
        Assert.Equal("Unknown", result.ErrorType);
    }
}
