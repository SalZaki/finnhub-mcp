// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Insiders.Clients;
using FinnHub.MCP.Server.Application.Insiders.Features.GetInsiderSignal;
using FinnHub.MCP.Server.Application.Insiders.Services;
using FinnHub.MCP.Server.Application.Tests.Unit.TestDoubles;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace FinnHub.MCP.Server.Application.Tests.Unit.Application.Features.Insiders.Services;

public sealed class InsidersServiceTests
{
    private readonly IInsidersApiClient _apiClient = Substitute.For<IInsidersApiClient>();
    private readonly FakeFinnHubCache _cache = new();
    private readonly InsidersService _sut;

    public InsidersServiceTests()
    {
        this._sut = new InsidersService(this._apiClient, this._cache, NullLogger<InsidersService>.Instance);
    }

    private static GetInsiderSignalQuery Query(string symbol = "AAPL") => new()
    {
        QueryId = "q1",
        Symbol = symbol,
        From = new DateOnly(2026, 4, 27),
        To = new DateOnly(2026, 5, 27)
    };

    private static InsiderTransaction Tx(string name, long change, int dayOffset = 0, string code = "S") => new()
    {
        Name = name,
        Change = change,
        TransactionDate = new DateOnly(2026, 5, 27).AddDays(-dayOffset),
        FilingDate = new DateOnly(2026, 5, 27).AddDays(-dayOffset).AddDays(2),
        TransactionPrice = 250.0,
        TransactionCode = code
    };

    [Fact]
    public async Task GetInsiderSignalAsync_NullQuery_Throws()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => this._sut.GetInsiderSignalAsync(null!));
    }

    [Fact]
    public async Task GetInsiderSignalAsync_Empty_ReturnsNotFound()
    {
        this._apiClient
            .GetInsiderTransactionsAsync(Arg.Any<string>(), Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .Returns([]);

        var result = await this._sut.GetInsiderSignalAsync(Query());

        Assert.False(result.IsSuccess);
        Assert.Equal("NotFound", result.ErrorType);
    }

    [Fact]
    public async Task GetInsiderSignalAsync_NetBuySell_IsSignedSumOfChanges()
    {
        // Synthetic input with a known signed result: buys = +10000, sells = -6000, net = +4000.
        InsiderTransaction[] upstream =
        [
            Tx("Alice", +10000, dayOffset: 5, code: "P"),
            Tx("Bob", -1000, dayOffset: 3, code: "S"),
            Tx("Bob", -5000, dayOffset: 1, code: "S")
        ];
        this._apiClient
            .GetInsiderTransactionsAsync(Arg.Any<string>(), Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .Returns(upstream);

        var result = await this._sut.GetInsiderSignalAsync(Query());

        Assert.True(result.IsSuccess);
        Assert.Equal(4000, result.Data!.NetBuySell30d);
        Assert.Equal(3, result.Data.TotalCount);
    }

    [Fact]
    public async Task GetInsiderSignalAsync_NetBuySell_AllSellsProducesNegative()
    {
        InsiderTransaction[] upstream =
        [
            Tx("Alice", -1000, dayOffset: 5),
            Tx("Bob", -2500, dayOffset: 1)
        ];
        this._apiClient
            .GetInsiderTransactionsAsync(Arg.Any<string>(), Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .Returns(upstream);

        var result = await this._sut.GetInsiderSignalAsync(Query());

        Assert.True(result.IsSuccess);
        Assert.Equal(-3500, result.Data!.NetBuySell30d);
    }

    [Fact]
    public async Task GetInsiderSignalAsync_NotableNames_RankedByAbsoluteVolumeCapped()
    {
        // 6 distinct insiders with varying activity — top 5 by absolute volume should win.
        InsiderTransaction[] upstream =
        [
            Tx("BIG", -10000, dayOffset: 1),
            Tx("MED", -5000, dayOffset: 2),
            Tx("SMALL", -100, dayOffset: 3),
            Tx("HUGE", +20000, dayOffset: 4, code: "P"),
            Tx("TINY", -10, dayOffset: 5),
            Tx("MICRO", -5, dayOffset: 6)
        ];
        this._apiClient
            .GetInsiderTransactionsAsync(Arg.Any<string>(), Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .Returns(upstream);

        var result = await this._sut.GetInsiderSignalAsync(Query());

        Assert.True(result.IsSuccess);
        var names = result.Data!.NotableNames;
        Assert.Equal(5, names.Count);
        Assert.Equal(["HUGE", "BIG", "MED", "SMALL", "TINY"], names);
        Assert.DoesNotContain("MICRO", names);
    }

    [Fact]
    public async Task GetInsiderSignalAsync_NotableNames_AggregatesByName()
    {
        // Multiple transactions by the same insider sum to one rank entry.
        InsiderTransaction[] upstream =
        [
            Tx("Alice", -1000, dayOffset: 1),
            Tx("Alice", -2000, dayOffset: 2),
            Tx("Alice", -3000, dayOffset: 3),
            Tx("Bob", -500, dayOffset: 4)
        ];
        this._apiClient
            .GetInsiderTransactionsAsync(Arg.Any<string>(), Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .Returns(upstream);

        var result = await this._sut.GetInsiderSignalAsync(Query());

        Assert.True(result.IsSuccess);
        Assert.Equal(["Alice", "Bob"], result.Data!.NotableNames);
    }

    [Fact]
    public async Task GetInsiderSignalAsync_Latest_IsMostRecentByTransactionDate()
    {
        InsiderTransaction[] upstream =
        [
            Tx("Alice", -1000, dayOffset: 5),
            Tx("Bob", -500, dayOffset: 1),
            Tx("Carol", -200, dayOffset: 3)
        ];
        this._apiClient
            .GetInsiderTransactionsAsync(Arg.Any<string>(), Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .Returns(upstream);

        var result = await this._sut.GetInsiderSignalAsync(Query());

        Assert.True(result.IsSuccess);
        Assert.Equal("Bob", result.Data!.Latest!.Name);
    }

    [Fact]
    public async Task GetInsiderSignalAsync_Transactions_OrderedMostRecentFirst()
    {
        InsiderTransaction[] upstream =
        [
            Tx("Alice", -1000, dayOffset: 5),
            Tx("Bob", -500, dayOffset: 1),
            Tx("Carol", -200, dayOffset: 3)
        ];
        this._apiClient
            .GetInsiderTransactionsAsync(Arg.Any<string>(), Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .Returns(upstream);

        var result = await this._sut.GetInsiderSignalAsync(Query());

        Assert.True(result.IsSuccess);
        var txs = result.Data!.Transactions!;
        Assert.Equal("Bob", txs[0].Name);
        Assert.Equal("Carol", txs[1].Name);
        Assert.Equal("Alice", txs[2].Name);
    }

    [Fact]
    public async Task GetInsiderSignalAsync_TwoIdenticalCalls_HitsApiClientExactlyOnce()
    {
        this._apiClient
            .GetInsiderTransactionsAsync(Arg.Any<string>(), Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .Returns([Tx("Alice", -1000)]);

        await this._sut.GetInsiderSignalAsync(Query());
        await this._sut.GetInsiderSignalAsync(Query());

        Assert.Equal(1, this._cache.FactoryInvocationCount);
        await this._apiClient.Received(1).GetInsiderTransactionsAsync(
            Arg.Any<string>(), Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetInsiderSignalAsync_HttpError_MapsToServiceUnavailable()
    {
        this._apiClient
            .GetInsiderTransactionsAsync(Arg.Any<string>(), Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientHttpException("boom", HttpStatusCode.BadGateway));

        var result = await this._sut.GetInsiderSignalAsync(Query());

        Assert.False(result.IsSuccess);
        Assert.Equal("ServiceUnavailable", result.ErrorType);
    }

    [Fact]
    public async Task GetInsiderSignalAsync_Premium_MapsToPremiumRequired()
    {
        this._apiClient
            .GetInsiderTransactionsAsync(Arg.Any<string>(), Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientPremiumRequiredException("/stock/insider-transactions", "premium"));

        var result = await this._sut.GetInsiderSignalAsync(Query());

        Assert.False(result.IsSuccess);
        Assert.Equal("PremiumRequired", result.ErrorType);
    }

    [Fact]
    public async Task GetInsiderSignalAsync_Timeout_MapsToTimeout()
    {
        this._apiClient
            .GetInsiderTransactionsAsync(Arg.Any<string>(), Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientTimeoutException("slow"));

        var result = await this._sut.GetInsiderSignalAsync(Query());

        Assert.False(result.IsSuccess);
        Assert.Equal("Timeout", result.ErrorType);
    }

    [Fact]
    public async Task GetInsiderSignalAsync_InvalidResponse_MapsToInvalidResponse()
    {
        this._apiClient
            .GetInsiderTransactionsAsync(Arg.Any<string>(), Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientDeserializationException("bad json"));

        var result = await this._sut.GetInsiderSignalAsync(Query());

        Assert.False(result.IsSuccess);
        Assert.Equal("InvalidResponse", result.ErrorType);
    }

    [Fact]
    public async Task GetInsiderSignalAsync_Cancelled_RethrowsTypedException()
    {
        this._apiClient
            .GetInsiderTransactionsAsync(Arg.Any<string>(), Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientCancelledException("cancelled"));

        await Assert.ThrowsAsync<ApiClientCancelledException>(() => this._sut.GetInsiderSignalAsync(Query()));
    }

    [Fact]
    public async Task GetInsiderSignalAsync_UnknownApiClientException_MapsToUnknown()
    {
        this._apiClient
            .GetInsiderTransactionsAsync(Arg.Any<string>(), Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientUnexpectedException("weird"));

        var result = await this._sut.GetInsiderSignalAsync(Query());

        Assert.False(result.IsSuccess);
        Assert.Equal("Unknown", result.ErrorType);
    }
}
