// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Calendar.Features.GetCalendar;
using FinnHub.MCP.Server.Application.Calendar.Services;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Tools.Calendar;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Tools.Calendar;

public sealed class GetCalendarToolTests
{
    private readonly ICalendarService _service = Substitute.For<ICalendarService>();
    private readonly GetCalendarTool _sut;

    public GetCalendarToolTests()
    {
        this._sut = new GetCalendarTool(this._service, NullLogger<GetCalendarTool>.Instance);
    }

    private static GetCalendarResponse Response(int events, string? symbol = "AAPL") => new()
    {
        Kind = "earnings",
        From = new DateOnly(2026, 5, 1),
        To = new DateOnly(2026, 6, 1),
        Symbol = symbol,
        TotalCount = events,
        EarningsEvents = Enumerable.Range(1, events)
            .Select(i => new EarningsEvent
            {
                Symbol = symbol ?? "AAPL",
                Date = new DateOnly(2026, 5, 1).AddDays(i),
                EpsEstimate = i
            })
            .ToList()
            .AsReadOnly()
    };

    [Fact]
    public async Task GetCalendarAsync_InvalidKind_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => this._sut.GetCalendarAsync("dividends"));
    }

    [Fact]
    public async Task GetCalendarAsync_UnsupportedKind_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => this._sut.GetCalendarAsync("ipo"));
    }

    [Fact]
    public async Task GetCalendarAsync_InvalidSymbol_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => this._sut.GetCalendarAsync("earnings", symbol: "!!!"));
    }

    [Fact]
    public async Task GetCalendarAsync_InvalidView_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            this._sut.GetCalendarAsync("earnings", view: "brief"));
    }

    [Fact]
    public async Task GetCalendarAsync_ToBeforeFrom_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            this._sut.GetCalendarAsync("earnings", from: "2026-06-01", to: "2026-05-01"));
    }

    [Fact]
    public async Task GetCalendarAsync_LowercaseSymbol_NormalisesToUppercase()
    {
        this._service.GetCalendarAsync(Arg.Any<GetCalendarQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetCalendarResponse>.Success(Response(1)));

        await this._sut.GetCalendarAsync("earnings", symbol: "aapl", from: "2026-05-01", to: "2026-06-01");

        await this._service.Received(1).GetCalendarAsync(
            Arg.Is<GetCalendarQuery>(q => q.Symbol == "AAPL"),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetCalendarAsync_OmittedSymbol_PassesNullToService()
    {
        this._service.GetCalendarAsync(Arg.Any<GetCalendarQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetCalendarResponse>.Success(Response(1, symbol: null)));

        await this._sut.GetCalendarAsync("earnings", from: "2026-05-01", to: "2026-06-01");

        await this._service.Received(1).GetCalendarAsync(
            Arg.Is<GetCalendarQuery>(q => q.Symbol == null),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetCalendarAsync_SummaryView_CapsAtTenEvents()
    {
        this._service.GetCalendarAsync(Arg.Any<GetCalendarQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetCalendarResponse>.Success(Response(30)));

        var envelope = await this._sut.GetCalendarAsync(
            "earnings", symbol: "AAPL", from: "2026-05-01", to: "2026-06-01", view: "summary");

        Assert.True(envelope.IsSuccess);
        Assert.Equal(GetCalendarTool.SummaryEventCap, envelope.Data!.TotalCount);
        Assert.Equal(GetCalendarTool.SummaryEventCap, envelope.Data.EarningsEvents!.Count);
    }

    [Fact]
    public async Task GetCalendarAsync_StandardView_CapsAtTwentyFiveEvents()
    {
        this._service.GetCalendarAsync(Arg.Any<GetCalendarQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetCalendarResponse>.Success(Response(40)));

        var envelope = await this._sut.GetCalendarAsync(
            "earnings", symbol: "AAPL", from: "2026-05-01", to: "2026-06-01", view: "standard");

        Assert.Equal(GetCalendarTool.StandardEventCap, envelope.Data!.TotalCount);
    }

    [Fact]
    public async Task GetCalendarAsync_FullView_ReturnsAll()
    {
        this._service.GetCalendarAsync(Arg.Any<GetCalendarQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetCalendarResponse>.Success(Response(40)));

        var envelope = await this._sut.GetCalendarAsync(
            "earnings", symbol: "AAPL", from: "2026-05-01", to: "2026-06-01", view: "full");

        Assert.Equal(40, envelope.Data!.TotalCount);
    }

    [Fact]
    public async Task GetCalendarAsync_SuccessWithSymbol_PopulatesNextActions()
    {
        this._service.GetCalendarAsync(Arg.Any<GetCalendarQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetCalendarResponse>.Success(Response(1)));

        var envelope = await this._sut.GetCalendarAsync("earnings", symbol: "AAPL");

        Assert.Equal(2, envelope.NextActions.Count);
        Assert.Equal("get-financials-snapshot", envelope.NextActions[0].Tool);
        Assert.Equal("get-news-pulse", envelope.NextActions[1].Tool);
    }

    [Fact]
    public async Task GetCalendarAsync_SuccessWithoutSymbol_ReturnsEmptyNextActions()
    {
        this._service.GetCalendarAsync(Arg.Any<GetCalendarQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetCalendarResponse>.Success(Response(1, symbol: null)));

        var envelope = await this._sut.GetCalendarAsync("earnings");

        Assert.Empty(envelope.NextActions);
    }

    [Fact]
    public async Task GetCalendarAsync_FailureResult_ReturnsEmptyNextActions()
    {
        this._service.GetCalendarAsync(Arg.Any<GetCalendarQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetCalendarResponse>.Failure("upstream-error"));

        var envelope = await this._sut.GetCalendarAsync("earnings", symbol: "AAPL");

        Assert.Empty(envelope.NextActions);
    }

    [Fact]
    public async Task GetCalendarAsync_Cancelled_PropagatesOperationCanceled()
    {
        this._service.GetCalendarAsync(Arg.Any<GetCalendarQuery>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new OperationCanceledException());

        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            this._sut.GetCalendarAsync("earnings", symbol: "AAPL"));
    }

    [Fact]
    public async Task GetCalendarAsync_UnexpectedFailure_PropagatesException()
    {
        this._service.GetCalendarAsync(Arg.Any<GetCalendarQuery>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new InvalidOperationException("downstream broke"));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            this._sut.GetCalendarAsync("earnings", symbol: "AAPL"));
    }
}
