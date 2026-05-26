// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Prices.Clients;
using FinnHub.MCP.Server.Application.Prices.Features.GetPriceSummary;
using FinnHub.MCP.Server.Application.Prices.Services;
using FinnHub.MCP.Server.Application.Tests.Unit.TestDoubles;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace FinnHub.MCP.Server.Application.Tests.Unit.Application.Features.Prices.Services;

public sealed class PricesServiceTests
{
    private readonly IPricesApiClient _apiClient = Substitute.For<IPricesApiClient>();
    private readonly FakeFinnHubCache _cache = new();
    private readonly PricesService _sut;

    public PricesServiceTests()
    {

        this._sut = new PricesService(this._apiClient, this._cache, NullLogger<PricesService>.Instance);
    }

    [Fact]
    public async Task GetSummaryAsync_NullQuery_Throws()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => this._sut.GetSummaryAsync(null!));
    }

    [Fact]
    public async Task GetSummaryAsync_WithCandles_ReturnsSuccess()
    {
        var query = new GetPriceSummaryQuery { QueryId = "q1", Symbol = "AAPL" };
        this._apiClient.GetSummaryAsync(query, Arg.Any<CancellationToken>())
            .Returns(new GetPriceSummaryResponse
            {
                Symbol = "AAPL",
                Period = "30d",
                Resolution = "D",
                CandleCount = 21
            });

        var result = await this._sut.GetSummaryAsync(query);

        Assert.True(result.IsSuccess);
        Assert.Equal(21, result.Data!.CandleCount);
    }

    [Fact]
    public async Task GetSummaryAsync_NoCandles_ReturnsNotFound()
    {
        var query = new GetPriceSummaryQuery { QueryId = "q1", Symbol = "UNKN" };
        this._apiClient.GetSummaryAsync(query, Arg.Any<CancellationToken>())
            .Returns(new GetPriceSummaryResponse
            {
                Symbol = "UNKN",
                Period = "30d",
                Resolution = "D",
                CandleCount = 0
            });

        var result = await this._sut.GetSummaryAsync(query);

        Assert.False(result.IsSuccess);
        Assert.Equal("NotFound", result.ErrorType);
    }

    [Fact]
    public async Task GetSummaryAsync_PremiumRequired_MapsCorrectly()
    {
        var query = new GetPriceSummaryQuery { QueryId = "q1", Symbol = "AAPL" };
        this._apiClient.GetSummaryAsync(query, Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientPremiumRequiredException("/api/v1/stock/candle"));

        var result = await this._sut.GetSummaryAsync(query);

        Assert.False(result.IsSuccess);
        Assert.Equal("PremiumRequired", result.ErrorType);
    }

    [Fact]
    public async Task GetSummaryAsync_HttpError_MapsToServiceUnavailable()
    {
        var query = new GetPriceSummaryQuery { QueryId = "q1", Symbol = "AAPL" };
        this._apiClient.GetSummaryAsync(query, Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientHttpException("boom", HttpStatusCode.BadGateway));

        var result = await this._sut.GetSummaryAsync(query);

        Assert.False(result.IsSuccess);
        Assert.Equal("ServiceUnavailable", result.ErrorType);
    }
}
