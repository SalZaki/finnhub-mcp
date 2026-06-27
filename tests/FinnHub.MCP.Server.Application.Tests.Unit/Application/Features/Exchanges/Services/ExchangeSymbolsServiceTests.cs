// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Exchanges.Clients;
using FinnHub.MCP.Server.Application.Exchanges.Features.GetExchangeSymbols;
using FinnHub.MCP.Server.Application.Exchanges.Services;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Tests.Unit.TestDoubles;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace FinnHub.MCP.Server.Application.Tests.Unit.Application.Features.Exchanges.Services;

public sealed class ExchangeSymbolsServiceTests
{
    private readonly IExchangesApiClient _apiClient = Substitute.For<IExchangesApiClient>();
    private readonly FakeFinnHubCache _cache = new();
    private readonly ExchangeSymbolsService _sut;

    public ExchangeSymbolsServiceTests()
    {
        this._sut = new ExchangeSymbolsService(this._apiClient, this._cache, NullLogger<ExchangeSymbolsService>.Instance);
    }

    [Fact]
    public async Task GetExchangeSymbolsAsync_NullQuery_Throws()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => this._sut.GetExchangeSymbolsAsync(null!));
    }

    [Fact]
    public async Task GetExchangeSymbolsAsync_Success_AggregatesCountAndTypeBreakdown()
    {
        var query = new GetExchangeSymbolsQuery { QueryId = "q1", Exchange = "US" };
        this._apiClient.GetSymbolsAsync("US", Arg.Any<CancellationToken>())
            .Returns(BuildSymbols(commonStock: 3, etp: 2));

        var result = await this._sut.GetExchangeSymbolsAsync(query);

        Assert.True(result.IsSuccess);
        Assert.Equal("US", result.Data!.Exchange);
        Assert.Equal(5, result.Data.TotalCount);
        Assert.Equal(3, result.Data.TypeBreakdown["Common Stock"]);
        Assert.Equal(2, result.Data.TypeBreakdown["ETP"]);
        Assert.NotNull(result.Data.Symbols);
        Assert.Equal(5, result.Data.Symbols!.Count);
    }

    [Fact]
    public async Task GetExchangeSymbolsAsync_TypeBreakdown_OrdersMostCommonFirst()
    {
        var query = new GetExchangeSymbolsQuery { QueryId = "q1", Exchange = "US" };
        this._apiClient.GetSymbolsAsync("US", Arg.Any<CancellationToken>())
            .Returns(BuildSymbols(commonStock: 2, etp: 5));

        var result = await this._sut.GetExchangeSymbolsAsync(query);

        Assert.Equal("ETP", result.Data!.TypeBreakdown.Keys.First());
    }

    [Fact]
    public async Task GetExchangeSymbolsAsync_LargeUpstream_CapsCachedSampleButKeepsTrueCount()
    {
        var query = new GetExchangeSymbolsQuery { QueryId = "q1", Exchange = "US" };
        this._apiClient.GetSymbolsAsync("US", Arg.Any<CancellationToken>())
            .Returns(BuildSymbols(commonStock: 150, etp: 0));

        var result = await this._sut.GetExchangeSymbolsAsync(query);

        Assert.Equal(150, result.Data!.TotalCount);
        Assert.Equal(100, result.Data.Symbols!.Count);
    }

    [Fact]
    public async Task GetExchangeSymbolsAsync_TwoIdenticalCalls_HitsApiClientExactlyOnce()
    {
        var query = new GetExchangeSymbolsQuery { QueryId = "q1", Exchange = "US" };
        this._apiClient.GetSymbolsAsync("US", Arg.Any<CancellationToken>())
            .Returns(BuildSymbols(commonStock: 3, etp: 1));

        await this._sut.GetExchangeSymbolsAsync(query);
        await this._sut.GetExchangeSymbolsAsync(query);

        Assert.Equal(1, this._cache.FactoryInvocationCount);
        await this._apiClient.Received(1).GetSymbolsAsync("US", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetExchangeSymbolsAsync_EmptyUpstream_ReturnsNotFound()
    {
        var query = new GetExchangeSymbolsQuery { QueryId = "q1", Exchange = "US" };
        this._apiClient.GetSymbolsAsync("US", Arg.Any<CancellationToken>())
            .Returns(Array.Empty<ExchangeSymbol>());

        var result = await this._sut.GetExchangeSymbolsAsync(query);

        Assert.False(result.IsSuccess);
        Assert.Equal(nameof(ResultErrorType.NotFound), result.ErrorType);
    }

    [Fact]
    public async Task GetExchangeSymbolsAsync_PremiumLocked_MapsToPremiumRequired()
    {
        var query = new GetExchangeSymbolsQuery { QueryId = "q1", Exchange = "L" };
        this._apiClient.GetSymbolsAsync("L", Arg.Any<CancellationToken>())
            .Throws(new ApiClientPremiumRequiredException("/stock/symbol", "premium"));

        var result = await this._sut.GetExchangeSymbolsAsync(query);

        Assert.False(result.IsSuccess);
        Assert.Equal(nameof(ResultErrorType.PremiumRequired), result.ErrorType);
    }

    [Fact]
    public async Task GetExchangeSymbolsAsync_HttpError_MapsToServiceUnavailable()
    {
        var query = new GetExchangeSymbolsQuery { QueryId = "q1", Exchange = "US" };
        this._apiClient.GetSymbolsAsync("US", Arg.Any<CancellationToken>())
            .Throws(new ApiClientHttpException("boom", HttpStatusCode.ServiceUnavailable));

        var result = await this._sut.GetExchangeSymbolsAsync(query);

        Assert.False(result.IsSuccess);
        Assert.Equal(nameof(ResultErrorType.ServiceUnavailable), result.ErrorType);
    }

    [Fact]
    public async Task GetExchangeSymbolsAsync_Timeout_MapsToTimeout()
    {
        var query = new GetExchangeSymbolsQuery { QueryId = "q1", Exchange = "US" };
        this._apiClient.GetSymbolsAsync("US", Arg.Any<CancellationToken>())
            .Throws(new ApiClientTimeoutException("timed out"));

        var result = await this._sut.GetExchangeSymbolsAsync(query);

        Assert.False(result.IsSuccess);
        Assert.Equal(nameof(ResultErrorType.Timeout), result.ErrorType);
    }

    [Fact]
    public async Task GetExchangeSymbolsAsync_Deserialization_MapsToInvalidResponse()
    {
        var query = new GetExchangeSymbolsQuery { QueryId = "q1", Exchange = "US" };
        this._apiClient.GetSymbolsAsync("US", Arg.Any<CancellationToken>())
            .Throws(new ApiClientDeserializationException("bad payload"));

        var result = await this._sut.GetExchangeSymbolsAsync(query);

        Assert.False(result.IsSuccess);
        Assert.Equal(nameof(ResultErrorType.InvalidResponse), result.ErrorType);
    }

    [Fact]
    public async Task GetExchangeSymbolsAsync_Cancelled_Rethrows()
    {
        var query = new GetExchangeSymbolsQuery { QueryId = "q1", Exchange = "US" };
        this._apiClient.GetSymbolsAsync("US", Arg.Any<CancellationToken>())
            .Throws(new ApiClientCancelledException("cancelled"));

        await Assert.ThrowsAsync<ApiClientCancelledException>(() => this._sut.GetExchangeSymbolsAsync(query));
    }

    [Fact]
    public async Task GetExchangeSymbolsAsync_UnexpectedApiException_MapsToUnknown()
    {
        var query = new GetExchangeSymbolsQuery { QueryId = "q1", Exchange = "US" };
        this._apiClient.GetSymbolsAsync("US", Arg.Any<CancellationToken>())
            .Throws(new ApiClientUnexpectedException("weird"));

        var result = await this._sut.GetExchangeSymbolsAsync(query);

        Assert.False(result.IsSuccess);
        Assert.Equal(nameof(ResultErrorType.Unknown), result.ErrorType);
    }

    private static List<ExchangeSymbol> BuildSymbols(int commonStock, int etp)
    {
        var list = new List<ExchangeSymbol>();

        for (var i = 0; i < commonStock; i++)
        {
            list.Add(new ExchangeSymbol { Symbol = $"CS{i}", Type = "Common Stock" });
        }

        for (var i = 0; i < etp; i++)
        {
            list.Add(new ExchangeSymbol { Symbol = $"ETP{i}", Type = "ETP" });
        }

        return list;
    }
}
