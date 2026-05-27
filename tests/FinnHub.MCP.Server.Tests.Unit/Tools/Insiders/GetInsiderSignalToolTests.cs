// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Insiders.Features.GetInsiderSignal;
using FinnHub.MCP.Server.Application.Insiders.Services;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Tools.Insiders;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Tools.Insiders;

public sealed class GetInsiderSignalToolTests
{
    private readonly IInsidersService _service = Substitute.For<IInsidersService>();
    private readonly GetInsiderSignalTool _sut;

    public GetInsiderSignalToolTests()
    {
        this._sut = new GetInsiderSignalTool(this._service, NullLogger<GetInsiderSignalTool>.Instance);
    }

    private static InsiderTransaction Tx(string name, long change, int dayOffset = 0) => new()
    {
        Name = name,
        Change = change,
        TransactionDate = new DateOnly(2026, 5, 27).AddDays(-dayOffset),
        TransactionCode = change >= 0 ? "P" : "S"
    };

    private static GetInsiderSignalResponse Response(int txCount, long net = -5000) => new()
    {
        Symbol = "AAPL",
        From = new DateOnly(2026, 4, 27),
        To = new DateOnly(2026, 5, 27),
        NetBuySell30d = net,
        NotableNames = ["LEVINSON ARTHUR D", "Borders Ben"],
        TotalCount = txCount,
        Latest = txCount > 0 ? Tx("LEVINSON ARTHUR D", -1000) : null,
        Transactions = Enumerable.Range(0, txCount)
            .Select(i => Tx(i % 2 == 0 ? "LEVINSON ARTHUR D" : "Borders Ben", i % 2 == 0 ? -1000 : -500, i))
            .ToList()
            .AsReadOnly()
    };

    [Fact]
    public async Task GetInsiderSignalAsync_InvalidSymbol_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => this._sut.GetInsiderSignalAsync("!!!"));
    }

    [Fact]
    public async Task GetInsiderSignalAsync_EmptySymbol_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => this._sut.GetInsiderSignalAsync(""));
    }

    [Fact]
    public async Task GetInsiderSignalAsync_ToBeforeFrom_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            this._sut.GetInsiderSignalAsync("AAPL", from: "2026-05-15", to: "2026-05-01"));
    }

    [Fact]
    public async Task GetInsiderSignalAsync_InvalidView_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            this._sut.GetInsiderSignalAsync("AAPL", view: "brief"));
    }

    [Fact]
    public async Task GetInsiderSignalAsync_LowercaseSymbol_NormalisesToUppercase()
    {
        this._service.GetInsiderSignalAsync(Arg.Any<GetInsiderSignalQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetInsiderSignalResponse>.Success(Response(3)));

        await this._sut.GetInsiderSignalAsync("aapl");

        await this._service.Received(1).GetInsiderSignalAsync(
            Arg.Is<GetInsiderSignalQuery>(q => q.Symbol == "AAPL"),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetInsiderSignalAsync_SummaryView_DropsTransactionsArray()
    {
        this._service.GetInsiderSignalAsync(Arg.Any<GetInsiderSignalQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetInsiderSignalResponse>.Success(Response(10)));

        var envelope = await this._sut.GetInsiderSignalAsync("AAPL", view: "summary");

        Assert.True(envelope.IsSuccess);
        Assert.Null(envelope.Data!.Transactions);
        // Aggregated signal + latest are preserved.
        Assert.Equal(10, envelope.Data.TotalCount);
        Assert.NotNull(envelope.Data.Latest);
        Assert.NotEmpty(envelope.Data.NotableNames);
    }

    [Fact]
    public async Task GetInsiderSignalAsync_StandardView_DropsTransactionsArray()
    {
        this._service.GetInsiderSignalAsync(Arg.Any<GetInsiderSignalQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetInsiderSignalResponse>.Success(Response(10)));

        var envelope = await this._sut.GetInsiderSignalAsync("AAPL", view: "standard");

        Assert.Null(envelope.Data!.Transactions);
    }

    [Fact]
    public async Task GetInsiderSignalAsync_FullView_KeepsTransactionsArray()
    {
        this._service.GetInsiderSignalAsync(Arg.Any<GetInsiderSignalQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetInsiderSignalResponse>.Success(Response(10)));

        var envelope = await this._sut.GetInsiderSignalAsync("AAPL", view: "full");

        Assert.NotNull(envelope.Data!.Transactions);
        Assert.Equal(10, envelope.Data.Transactions!.Count);
    }

    [Fact]
    public async Task GetInsiderSignalAsync_Success_PopulatesNextActions()
    {
        this._service.GetInsiderSignalAsync(Arg.Any<GetInsiderSignalQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetInsiderSignalResponse>.Success(Response(1)));

        var envelope = await this._sut.GetInsiderSignalAsync("AAPL");

        Assert.Equal(2, envelope.NextActions.Count);
        Assert.Equal("get-company-profile", envelope.NextActions[0].Tool);
        Assert.Equal("AAPL", envelope.NextActions[0].Args["symbol"]);
        Assert.Equal("get-quote", envelope.NextActions[1].Tool);
    }

    [Fact]
    public async Task GetInsiderSignalAsync_Failure_ReturnsEmptyNextActions()
    {
        this._service.GetInsiderSignalAsync(Arg.Any<GetInsiderSignalQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetInsiderSignalResponse>.Failure("no data", ResultErrorType.NotFound));

        var envelope = await this._sut.GetInsiderSignalAsync("AAPL");

        Assert.Empty(envelope.NextActions);
    }

    [Fact]
    public async Task GetInsiderSignalAsync_PassesWindowToService()
    {
        this._service.GetInsiderSignalAsync(Arg.Any<GetInsiderSignalQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetInsiderSignalResponse>.Success(Response(1)));

        await this._sut.GetInsiderSignalAsync("AAPL", from: "2026-04-27", to: "2026-05-27");

        await this._service.Received(1).GetInsiderSignalAsync(
            Arg.Is<GetInsiderSignalQuery>(q =>
                q.From == new DateOnly(2026, 4, 27) && q.To == new DateOnly(2026, 5, 27)),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetInsiderSignalAsync_Cancelled_PropagatesOperationCanceled()
    {
        this._service.GetInsiderSignalAsync(Arg.Any<GetInsiderSignalQuery>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new OperationCanceledException());

        await Assert.ThrowsAsync<OperationCanceledException>(() => this._sut.GetInsiderSignalAsync("AAPL"));
    }

    [Fact]
    public async Task GetInsiderSignalAsync_UnexpectedFailure_PropagatesException()
    {
        this._service.GetInsiderSignalAsync(Arg.Any<GetInsiderSignalQuery>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new InvalidOperationException("downstream broke"));

        await Assert.ThrowsAsync<InvalidOperationException>(() => this._sut.GetInsiderSignalAsync("AAPL"));
    }
}
