// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.News.Features.GetNewsPulse;
using FinnHub.MCP.Server.Application.News.Services;
using FinnHub.MCP.Server.Tools.News;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Tools.News;

public sealed class GetNewsPulseToolTests
{
    private readonly INewsService _service = Substitute.For<INewsService>();
    private readonly GetNewsPulseTool _sut;

    public GetNewsPulseToolTests()
    {
        this._sut = new GetNewsPulseTool(this._service, NullLogger<GetNewsPulseTool>.Instance);
    }

    [Fact]
    public async Task GetNewsPulseAsync_InvalidSymbol_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => this._sut.GetNewsPulseAsync("!!!"));
    }

    [Fact]
    public async Task GetNewsPulseAsync_LowercaseSymbol_Normalises()
    {
        this._service
            .GetPulseAsync(Arg.Any<GetNewsPulseQuery>(), Arg.Any<CancellationToken>())
            .Returns(new Result<GetNewsPulseResponse>().Success(
                new GetNewsPulseResponse { Symbol = "AAPL", Count = 1, SentimentSource = "finnhub" }));

        await this._sut.GetNewsPulseAsync("aapl");

        await this._service.Received(1).GetPulseAsync(
            Arg.Is<GetNewsPulseQuery>(q => q.Symbol == "AAPL" && !q.IncludeAllHeadlines),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetNewsPulseAsync_FullView_SetsIncludeAllHeadlines()
    {
        this._service
            .GetPulseAsync(Arg.Any<GetNewsPulseQuery>(), Arg.Any<CancellationToken>())
            .Returns(new Result<GetNewsPulseResponse>().Success(
                new GetNewsPulseResponse { Symbol = "AAPL", Count = 1 }));

        await this._sut.GetNewsPulseAsync("AAPL", view: "full");

        await this._service.Received(1).GetPulseAsync(
            Arg.Is<GetNewsPulseQuery>(q => q.IncludeAllHeadlines),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetNewsPulseAsync_InvalidView_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            this._sut.GetNewsPulseAsync("AAPL", view: "nonsense"));
    }

    [Fact]
    public async Task GetNewsPulseAsync_Cancelled_PropagatesOperationCanceled()
    {
        this._service.GetPulseAsync(Arg.Any<GetNewsPulseQuery>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new OperationCanceledException());

        await Assert.ThrowsAsync<OperationCanceledException>(() => this._sut.GetNewsPulseAsync("AAPL"));
    }

    [Fact]
    public async Task GetNewsPulseAsync_UnexpectedFailure_PropagatesException()
    {
        this._service.GetPulseAsync(Arg.Any<GetNewsPulseQuery>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new InvalidOperationException("downstream broke"));

        await Assert.ThrowsAsync<InvalidOperationException>(() => this._sut.GetNewsPulseAsync("AAPL"));
    }

    [Fact]
    public async Task GetNewsPulseAsync_FailureResult_ReturnsEmptyNextActions()
    {
        this._service.GetPulseAsync(Arg.Any<GetNewsPulseQuery>(), Arg.Any<CancellationToken>())
            .Returns(new Result<GetNewsPulseResponse>().Failure("upstream-error"));

        var envelope = await this._sut.GetNewsPulseAsync("AAPL");

        Assert.Empty(envelope.NextActions);
    }
}
