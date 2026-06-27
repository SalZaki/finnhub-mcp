// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Peers.Features.GetPeers;
using FinnHub.MCP.Server.Application.Peers.Services;
using FinnHub.MCP.Server.Tools.Peers;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Tools.Peers;

public sealed class GetPeersToolTests : ToolExceptionPropagationTests<GetPeersResponse>
{
    private readonly IPeersService _service = Substitute.For<IPeersService>();
    private readonly GetPeersTool _sut;

    public GetPeersToolTests()
    {
        this._sut = new GetPeersTool(this._service, NullLogger<GetPeersTool>.Instance);
    }

    protected override void SetupServiceThrows(Exception ex)
    {
        this._service.GetPeersAsync(Arg.Any<GetPeersQuery>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(ex);
    }

    protected override void SetupServiceFailureResult()
    {
        this._service.GetPeersAsync(Arg.Any<GetPeersQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetPeersResponse>.Failure("upstream-error"));
    }

    protected override Task<ToolResponseEnvelope<GetPeersResponse>> ActAsync()
        => this._sut.GetPeersAsync("AAPL");

    [Fact]
    public async Task GetPeersAsync_InvalidSymbol_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => this._sut.GetPeersAsync("!!!"));
    }

    [Fact]
    public async Task GetPeersAsync_LowercaseSymbol_NormalisesToUppercase()
    {
        this._service
            .GetPeersAsync(Arg.Any<GetPeersQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetPeersResponse>.Success(
                new GetPeersResponse { Peers = ["MSFT"], Grouping = "industry" }));

        await this._sut.GetPeersAsync("aapl");

        await this._service.Received(1).GetPeersAsync(
            Arg.Is<GetPeersQuery>(q => q.Symbol == "AAPL"),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetPeersAsync_SummaryView_CapsAtTen()
    {
        var manyPeers = Enumerable.Range(1, 30).Select(i => $"P{i}").ToArray();
        this._service
            .GetPeersAsync(Arg.Any<GetPeersQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetPeersResponse>.Success(
                new GetPeersResponse { Peers = manyPeers, Grouping = "industry" }));

        var envelope = await this._sut.GetPeersAsync("AAPL", view: "summary");

        Assert.True(envelope.IsSuccess);
        Assert.Equal(10, envelope.Data!.TotalCount);
        Assert.Equal("P1", envelope.Data.Peers[0]);
    }

    [Fact]
    public async Task GetPeersAsync_FullView_ReturnsAll()
    {
        var manyPeers = Enumerable.Range(1, 30).Select(i => $"P{i}").ToArray();
        this._service
            .GetPeersAsync(Arg.Any<GetPeersQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetPeersResponse>.Success(
                new GetPeersResponse { Peers = manyPeers, Grouping = "industry" }));

        var envelope = await this._sut.GetPeersAsync("AAPL", view: "full");

        Assert.Equal(30, envelope.Data!.TotalCount);
    }

    [Fact]
    public async Task GetPeersAsync_InvalidGrouping_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            this._sut.GetPeersAsync("AAPL", grouping: "nonsense"));
    }

    [Fact]
    public async Task GetPeersAsync_SummaryView_ExplanationCountMatchesProjectedTotal()
    {
        // Regression: previously the tool projected `data.peers` to 10 but read
        // the original 30-peer result for the explanation string, producing an
        // envelope where data.total_count=10 but the explanation said
        // "Found 30 peer(s)". Both must derive from the same projected result.
        var manyPeers = Enumerable.Range(1, 30).Select(i => $"P{i}").ToArray();
        this._service
            .GetPeersAsync(Arg.Any<GetPeersQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetPeersResponse>.Success(
                new GetPeersResponse { Peers = manyPeers, Grouping = "industry" }));

        var envelope = await this._sut.GetPeersAsync("AAPL", view: "summary");

        Assert.Equal(10, envelope.Data!.TotalCount);
        Assert.Contains("10 peer(s)", envelope.Explanation);
        Assert.DoesNotContain("30 peer(s)", envelope.Explanation);
    }
}
