// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Financials.Features.GetFinancialsSnapshot;
using FinnHub.MCP.Server.Application.Financials.Services;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Tools.Financials;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Tools.Financials;

public sealed class GetFinancialsSnapshotToolTests : ToolExceptionPropagationTests<GetFinancialsSnapshotResponse>
{
    private readonly IFinancialsService _service = Substitute.For<IFinancialsService>();
    private readonly GetFinancialsSnapshotTool _sut;

    public GetFinancialsSnapshotToolTests()
    {
        this._sut = new GetFinancialsSnapshotTool(this._service, NullLogger<GetFinancialsSnapshotTool>.Instance);
    }

    protected override void SetupServiceThrows(Exception ex)
    {
        this._service.GetSnapshotAsync(Arg.Any<GetFinancialsSnapshotQuery>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(ex);
    }

    protected override void SetupServiceFailureResult()
    {
        this._service.GetSnapshotAsync(Arg.Any<GetFinancialsSnapshotQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetFinancialsSnapshotResponse>.Failure("upstream-error"));
    }

    protected override Task<ToolResponseEnvelope<GetFinancialsSnapshotResponse>> ActAsync()
        => this._sut.GetFinancialsSnapshotAsync("AAPL");

    [Fact]
    public async Task GetFinancialsSnapshotAsync_InvalidSymbol_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => this._sut.GetFinancialsSnapshotAsync("!!!"));
    }

    [Fact]
    public async Task GetFinancialsSnapshotAsync_LowercaseSymbol_Normalises()
    {
        this._service
            .GetSnapshotAsync(Arg.Any<GetFinancialsSnapshotQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetFinancialsSnapshotResponse>.Success(
                new GetFinancialsSnapshotResponse { Symbol = "AAPL" }));

        await this._sut.GetFinancialsSnapshotAsync("aapl");

        await this._service.Received(1).GetSnapshotAsync(
            Arg.Is<GetFinancialsSnapshotQuery>(q => q.Symbol == "AAPL" && !q.IncludeRaw),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetFinancialsSnapshotAsync_FullView_SetsIncludeRaw()
    {
        this._service
            .GetSnapshotAsync(Arg.Any<GetFinancialsSnapshotQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result<GetFinancialsSnapshotResponse>.Success(
                new GetFinancialsSnapshotResponse { Symbol = "AAPL" }));

        await this._sut.GetFinancialsSnapshotAsync("AAPL", view: "full");

        await this._service.Received(1).GetSnapshotAsync(
            Arg.Is<GetFinancialsSnapshotQuery>(q => q.IncludeRaw),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetFinancialsSnapshotAsync_InvalidView_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            this._sut.GetFinancialsSnapshotAsync("AAPL", view: "nonsense"));
    }
}
