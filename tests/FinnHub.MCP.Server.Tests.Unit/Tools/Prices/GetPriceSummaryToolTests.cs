// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Prices.Features.GetPriceSummary;
using FinnHub.MCP.Server.Application.Prices.Services;
using FinnHub.MCP.Server.Tools.Prices;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Tools.Prices;

public sealed class GetPriceSummaryToolTests
{
    private readonly IPricesService _service = Substitute.For<IPricesService>();
    private readonly GetPriceSummaryTool _sut;

    public GetPriceSummaryToolTests()
    {
        this._sut = new GetPriceSummaryTool(this._service, NullLogger<GetPriceSummaryTool>.Instance);
    }

    [Fact]
    public async Task GetPriceSummaryAsync_InvalidSymbol_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => this._sut.GetPriceSummaryAsync("!!!"));
    }

    [Fact]
    public async Task GetPriceSummaryAsync_InvalidPeriod_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            this._sut.GetPriceSummaryAsync("AAPL", period: "5h"));
    }

    [Fact]
    public async Task GetPriceSummaryAsync_FullView_SetsIncludeCandles()
    {
        this._service
            .GetSummaryAsync(Arg.Any<GetPriceSummaryQuery>(), Arg.Any<CancellationToken>())
            .Returns(new Result<GetPriceSummaryResponse>().Success(
                new GetPriceSummaryResponse { Symbol = "AAPL", Period = "30d", Resolution = "D", CandleCount = 1 }));

        await this._sut.GetPriceSummaryAsync("AAPL", view: "full");

        await this._service.Received(1).GetSummaryAsync(
            Arg.Is<GetPriceSummaryQuery>(q => q.IncludeCandles),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetPriceSummaryAsync_NinetyDaysPeriod_PassesThrough()
    {
        this._service
            .GetSummaryAsync(Arg.Any<GetPriceSummaryQuery>(), Arg.Any<CancellationToken>())
            .Returns(new Result<GetPriceSummaryResponse>().Success(
                new GetPriceSummaryResponse { Symbol = "AAPL", Period = "90d", Resolution = "D", CandleCount = 1 }));

        await this._sut.GetPriceSummaryAsync("AAPL", period: "90d");

        await this._service.Received(1).GetSummaryAsync(
            Arg.Is<GetPriceSummaryQuery>(q => q.Period == PricePeriod.NinetyDays),
            Arg.Any<CancellationToken>());
    }
}
