// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Exchanges.Features.GetExchangeSymbols;
using FinnHub.MCP.Server.Application.Exchanges.Services;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Tools.Exchanges;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Tools.Exchanges;

public sealed class GetExchangeSymbolsToolTests
{
    private readonly IExchangeSymbolsService _service = Substitute.For<IExchangeSymbolsService>();
    private readonly GetExchangeSymbolsTool _sut;

    public GetExchangeSymbolsToolTests()
    {
        this._sut = new GetExchangeSymbolsTool(this._service, NullLogger<GetExchangeSymbolsTool>.Instance);
    }

    [Fact]
    public async Task GetExchangeSymbolsAsync_InvalidExchange_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => this._sut.GetExchangeSymbolsAsync("123"));
    }

    [Fact]
    public async Task GetExchangeSymbolsAsync_InvalidView_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => this._sut.GetExchangeSymbolsAsync("US", view: "brief"));
    }

    [Fact]
    public async Task GetExchangeSymbolsAsync_LowercaseExchange_NormalisesToUppercase()
    {
        this._service.GetExchangeSymbolsAsync(Arg.Any<GetExchangeSymbolsQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetExchangeSymbolsResponse>.Success(Response(5)));

        await this._sut.GetExchangeSymbolsAsync("us");

        await this._service.Received(1).GetExchangeSymbolsAsync(
            Arg.Is<GetExchangeSymbolsQuery>(q => q.Exchange == "US"),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetExchangeSymbolsAsync_SummaryView_DropsSymbolsButKeepsCount()
    {
        this._service.GetExchangeSymbolsAsync(Arg.Any<GetExchangeSymbolsQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetExchangeSymbolsResponse>.Success(Response(60)));

        var envelope = await this._sut.GetExchangeSymbolsAsync("US", view: "summary");

        Assert.True(envelope.IsSuccess);
        Assert.Null(envelope.Data!.Symbols);
        Assert.Equal(60, envelope.Data.TotalCount);
    }

    [Fact]
    public async Task GetExchangeSymbolsAsync_StandardView_Caps25()
    {
        this._service.GetExchangeSymbolsAsync(Arg.Any<GetExchangeSymbolsQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetExchangeSymbolsResponse>.Success(Response(60)));

        var envelope = await this._sut.GetExchangeSymbolsAsync("US", view: "standard");

        Assert.NotNull(envelope.Data!.Symbols);
        Assert.Equal(25, envelope.Data.Symbols!.Count);
    }

    [Fact]
    public async Task GetExchangeSymbolsAsync_FullView_KeepsCachedSample()
    {
        this._service.GetExchangeSymbolsAsync(Arg.Any<GetExchangeSymbolsQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetExchangeSymbolsResponse>.Success(Response(60)));

        var envelope = await this._sut.GetExchangeSymbolsAsync("US", view: "full");

        Assert.NotNull(envelope.Data!.Symbols);
        Assert.Equal(60, envelope.Data.Symbols!.Count);
    }

    [Fact]
    public async Task GetExchangeSymbolsAsync_Success_SuggestsSearchSymbol()
    {
        this._service.GetExchangeSymbolsAsync(Arg.Any<GetExchangeSymbolsQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetExchangeSymbolsResponse>.Success(Response(5)));

        var envelope = await this._sut.GetExchangeSymbolsAsync("US");

        var next = Assert.Single(envelope.NextActions);
        Assert.Equal("search-symbol", next.Tool);
        Assert.Equal("US", next.Args["exchange"]);
    }

    [Fact]
    public async Task GetExchangeSymbolsAsync_NotFound_PassesThroughWithoutNextActions()
    {
        this._service.GetExchangeSymbolsAsync(Arg.Any<GetExchangeSymbolsQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetExchangeSymbolsResponse>.Failure("none", ResultErrorType.NotFound));

        var envelope = await this._sut.GetExchangeSymbolsAsync("US");

        Assert.False(envelope.IsSuccess);
        Assert.Equal(nameof(ResultErrorType.NotFound), envelope.ErrorType);
        Assert.Empty(envelope.NextActions);
    }

    [Fact]
    public async Task GetExchangeSymbolsAsync_PremiumRequired_FlagsPremium()
    {
        this._service.GetExchangeSymbolsAsync(Arg.Any<GetExchangeSymbolsQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetExchangeSymbolsResponse>.Failure("premium", ResultErrorType.PremiumRequired));

        var envelope = await this._sut.GetExchangeSymbolsAsync("L");

        Assert.False(envelope.IsSuccess);
        Assert.True(envelope.Premium);
    }

    [Fact]
    public async Task GetExchangeSymbolsAsync_Cancelled_Rethrows()
    {
        this._service.GetExchangeSymbolsAsync(Arg.Any<GetExchangeSymbolsQuery>(), Arg.Any<CancellationToken>())
            .Throws(new OperationCanceledException());

        await Assert.ThrowsAsync<OperationCanceledException>(() => this._sut.GetExchangeSymbolsAsync("US"));
    }

    [Fact]
    public async Task GetExchangeSymbolsAsync_UnexpectedException_Propagates()
    {
        this._service.GetExchangeSymbolsAsync(Arg.Any<GetExchangeSymbolsQuery>(), Arg.Any<CancellationToken>())
            .Throws(new InvalidOperationException("boom"));

        await Assert.ThrowsAsync<InvalidOperationException>(() => this._sut.GetExchangeSymbolsAsync("US"));
    }

    private static GetExchangeSymbolsResponse Response(int sampleCount)
    {
        var symbols = new List<ExchangeSymbol>();

        for (var i = 0; i < sampleCount; i++)
        {
            symbols.Add(new ExchangeSymbol { Symbol = $"S{i}", Type = "Common Stock" });
        }

        return new GetExchangeSymbolsResponse
        {
            Exchange = "US",
            TotalCount = sampleCount,
            TypeBreakdown = new Dictionary<string, int>(StringComparer.Ordinal) { ["Common Stock"] = sampleCount },
            Symbols = symbols
        };
    }
}
