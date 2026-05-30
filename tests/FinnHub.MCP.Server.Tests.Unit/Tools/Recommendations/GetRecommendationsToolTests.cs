// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Recommendations.Features.GetRecommendations;
using FinnHub.MCP.Server.Application.Recommendations.Services;
using FinnHub.MCP.Server.Tools.Recommendations;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Tools.Recommendations;

public sealed class GetRecommendationsToolTests
{
    private readonly IRecommendationsService _service = Substitute.For<IRecommendationsService>();
    private readonly GetRecommendationsTool _sut;

    public GetRecommendationsToolTests()
    {
        this._sut = new GetRecommendationsTool(this._service, NullLogger<GetRecommendationsTool>.Instance);
    }

    private static GetRecommendationsResponse Response(bool withSnapshots = true, bool withChange = true) => new()
    {
        Symbol = "AAPL",
        Period = new DateOnly(2026, 5, 1),
        Consensus = "Buy",
        StrongBuy = 15,
        Buy = 24,
        Hold = 13,
        Sell = 2,
        StrongSell = 0,
        Total = 54,
        ChangeVsPrev = withChange
            ? new RecommendationChange
            {
                PrevPeriod = new DateOnly(2026, 4, 1),
                StrongBuyDelta = 1,
                BuyDelta = 1,
                HoldDelta = -2,
                SellDelta = 0,
                StrongSellDelta = 0,
                ConsensusShift = "more bullish"
            }
            : null,
        Snapshots = withSnapshots
            ?
            [
                new RecommendationSnapshot { Period = new DateOnly(2026, 5, 1), StrongBuy = 15, Buy = 24, Hold = 13, Sell = 2, StrongSell = 0 },
                new RecommendationSnapshot { Period = new DateOnly(2026, 4, 1), StrongBuy = 14, Buy = 23, Hold = 15, Sell = 2, StrongSell = 0 }
            ]
            : null
    };

    [Fact]
    public async Task GetRecommendationsAsync_InvalidSymbol_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => this._sut.GetRecommendationsAsync("!!!"));
    }

    [Fact]
    public async Task GetRecommendationsAsync_EmptySymbol_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => this._sut.GetRecommendationsAsync(""));
    }

    [Fact]
    public async Task GetRecommendationsAsync_InvalidView_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            this._sut.GetRecommendationsAsync("AAPL", view: "brief"));
    }

    [Fact]
    public async Task GetRecommendationsAsync_LowercaseSymbol_NormalisesToUppercase()
    {
        this._service.GetRecommendationsAsync(Arg.Any<GetRecommendationsQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetRecommendationsResponse>.Success(Response()));

        await this._sut.GetRecommendationsAsync("aapl");

        await this._service.Received(1).GetRecommendationsAsync(
            Arg.Is<GetRecommendationsQuery>(q => q.Symbol == "AAPL"),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetRecommendationsAsync_SummaryView_DropsSnapshotsArray()
    {
        this._service.GetRecommendationsAsync(Arg.Any<GetRecommendationsQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetRecommendationsResponse>.Success(Response()));

        var envelope = await this._sut.GetRecommendationsAsync("AAPL", view: "summary");

        Assert.True(envelope.IsSuccess);
        Assert.Null(envelope.Data!.Snapshots);
        // Headline snapshot + change preserved.
        Assert.Equal("Buy", envelope.Data.Consensus);
        Assert.NotNull(envelope.Data.ChangeVsPrev);
        Assert.Equal("more bullish", envelope.Data.ChangeVsPrev!.ConsensusShift);
    }

    [Fact]
    public async Task GetRecommendationsAsync_StandardView_DropsSnapshotsArray()
    {
        this._service.GetRecommendationsAsync(Arg.Any<GetRecommendationsQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetRecommendationsResponse>.Success(Response()));

        var envelope = await this._sut.GetRecommendationsAsync("AAPL", view: "standard");

        Assert.Null(envelope.Data!.Snapshots);
    }

    [Fact]
    public async Task GetRecommendationsAsync_FullView_KeepsSnapshotsArray()
    {
        this._service.GetRecommendationsAsync(Arg.Any<GetRecommendationsQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetRecommendationsResponse>.Success(Response()));

        var envelope = await this._sut.GetRecommendationsAsync("AAPL", view: "full");

        Assert.NotNull(envelope.Data!.Snapshots);
        Assert.Equal(2, envelope.Data.Snapshots!.Count);
    }

    [Fact]
    public async Task GetRecommendationsAsync_Success_PopulatesNextActions()
    {
        this._service.GetRecommendationsAsync(Arg.Any<GetRecommendationsQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetRecommendationsResponse>.Success(Response()));

        var envelope = await this._sut.GetRecommendationsAsync("AAPL");

        Assert.Equal(2, envelope.NextActions.Count);
        Assert.Equal("get-financials-snapshot", envelope.NextActions[0].Tool);
        Assert.Equal("AAPL", envelope.NextActions[0].Args["symbol"]);
        Assert.Equal("get-peers", envelope.NextActions[1].Tool);
    }

    [Fact]
    public async Task GetRecommendationsAsync_Failure_ReturnsEmptyNextActions()
    {
        this._service.GetRecommendationsAsync(Arg.Any<GetRecommendationsQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetRecommendationsResponse>.Failure("no coverage", ResultErrorType.NotFound));

        var envelope = await this._sut.GetRecommendationsAsync("AAPL");

        Assert.Empty(envelope.NextActions);
    }

    [Fact]
    public async Task GetRecommendationsAsync_Cancelled_PropagatesOperationCanceled()
    {
        this._service.GetRecommendationsAsync(Arg.Any<GetRecommendationsQuery>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new OperationCanceledException());

        await Assert.ThrowsAsync<OperationCanceledException>(() => this._sut.GetRecommendationsAsync("AAPL"));
    }

    [Fact]
    public async Task GetRecommendationsAsync_UnexpectedFailure_PropagatesException()
    {
        this._service.GetRecommendationsAsync(Arg.Any<GetRecommendationsQuery>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new InvalidOperationException("downstream broke"));

        await Assert.ThrowsAsync<InvalidOperationException>(() => this._sut.GetRecommendationsAsync("AAPL"));
    }
}
