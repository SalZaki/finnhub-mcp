// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Quotes.Features.GetQuote;
using FinnHub.MCP.Server.Application.Quotes.Services;
using FinnHub.MCP.Server.Tools.Quotes;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Tools.Quotes;

public sealed class GetQuoteToolTests
{
    private readonly IQuotesService _service = Substitute.For<IQuotesService>();
    private readonly GetQuoteTool _sut;

    public GetQuoteToolTests()
    {
        this._sut = new GetQuoteTool(this._service, NullLogger<GetQuoteTool>.Instance);
    }

    [Fact]
    public async Task GetQuoteAsync_InvalidSymbol_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => this._sut.GetQuoteAsync("!!!"));
    }

    [Fact]
    public async Task GetQuoteAsync_LowercaseSymbol_Normalises()
    {
        this._service.GetQuoteAsync(Arg.Any<GetQuoteQuery>(), Arg.Any<CancellationToken>())
            .Returns(new Result<GetQuoteResponse>().Success(new GetQuoteResponse { Symbol = "AAPL", Current = 100 }));

        await this._sut.GetQuoteAsync("aapl");

        await this._service.Received(1).GetQuoteAsync(
            Arg.Is<GetQuoteQuery>(q => q.Symbol == "AAPL"),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetQuoteAsync_Success_PopulatesNextActions()
    {
        this._service.GetQuoteAsync(Arg.Any<GetQuoteQuery>(), Arg.Any<CancellationToken>())
            .Returns(new Result<GetQuoteResponse>().Success(new GetQuoteResponse { Symbol = "AAPL", Current = 100 }));

        var envelope = await this._sut.GetQuoteAsync("AAPL");

        Assert.Equal(2, envelope.NextActions.Count);
        Assert.Equal("get-news-pulse", envelope.NextActions[0].Tool);
        Assert.Equal("get-price-summary", envelope.NextActions[1].Tool);
    }

    [Fact]
    public async Task GetQuoteAsync_Cancelled_PropagatesOperationCanceled()
    {
        this._service.GetQuoteAsync(Arg.Any<GetQuoteQuery>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new OperationCanceledException());

        await Assert.ThrowsAsync<OperationCanceledException>(() => this._sut.GetQuoteAsync("AAPL"));
    }

    [Fact]
    public async Task GetQuoteAsync_UnexpectedFailure_PropagatesException()
    {
        this._service.GetQuoteAsync(Arg.Any<GetQuoteQuery>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new InvalidOperationException("downstream broke"));

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => this._sut.GetQuoteAsync("AAPL"));
        Assert.Equal("downstream broke", ex.Message);
    }

    [Fact]
    public async Task GetQuoteAsync_FailureResult_ReturnsEmptyNextActions()
    {
        this._service.GetQuoteAsync(Arg.Any<GetQuoteQuery>(), Arg.Any<CancellationToken>())
            .Returns(new Result<GetQuoteResponse>().Failure("upstream-error", ResultErrorType.ServiceUnavailable));

        var envelope = await this._sut.GetQuoteAsync("AAPL");

        Assert.Empty(envelope.NextActions);
    }

    [Fact]
    public async Task GetQuoteAsync_SuccessWithAllNullFields_BuildsFallbackExplanation()
    {
        // Covers the "n/a" / "unknown" branches in BuildExplanation when the upstream
        // response is shape-valid but has null Current / PercentChange / TimestampUtc
        // (the unknown-symbol case Finnhub returns — see Fixtures/finnhub/quote-unknown.json).
        this._service.GetQuoteAsync(Arg.Any<GetQuoteQuery>(), Arg.Any<CancellationToken>())
            .Returns(new Result<GetQuoteResponse>().Success(
                new GetQuoteResponse { Symbol = "ZZZZ" }));

        var envelope = await this._sut.GetQuoteAsync("ZZZZ");

        Assert.Equal(2, envelope.NextActions.Count);
    }
}
