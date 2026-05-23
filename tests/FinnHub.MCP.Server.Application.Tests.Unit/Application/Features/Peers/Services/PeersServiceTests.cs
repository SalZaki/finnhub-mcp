// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using FinnHub.MCP.Server.Application.Caching;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Peers.Clients;
using FinnHub.MCP.Server.Application.Peers.Features.GetPeers;
using FinnHub.MCP.Server.Application.Peers.Services;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace FinnHub.MCP.Server.Application.Tests.Unit.Application.Features.Peers.Services;

public sealed class PeersServiceTests
{
    private readonly IPeersApiClient _apiClient = Substitute.For<IPeersApiClient>();
    private readonly IFinnHubCache _cache = Substitute.For<IFinnHubCache>();
    private readonly PeersService _sut;

    public PeersServiceTests()
    {
        // Cache passes through to factory by default
        this._cache
            .GetOrCreateAsync(
                Arg.Any<string>(),
                Arg.Any<CacheTier>(),
                Arg.Any<Func<CancellationToken, ValueTask<GetPeersResponse>>>(),
                Arg.Any<CancellationToken>())
            .Returns(call => call.Arg<Func<CancellationToken, ValueTask<GetPeersResponse>>>()(CancellationToken.None));

        this._sut = new PeersService(this._apiClient, this._cache, NullLogger<PeersService>.Instance);
    }

    [Fact]
    public async Task GetPeersAsync_NullQuery_Throws()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => this._sut.GetPeersAsync(null!));
    }

    [Fact]
    public async Task GetPeersAsync_WithResults_ReturnsSuccess()
    {
        var query = new GetPeersQuery { QueryId = "q1", Symbol = "AAPL" };
        var response = new GetPeersResponse { Peers = ["MSFT", "GOOG"], Grouping = "industry" };
        this._apiClient.GetPeersAsync(query, Arg.Any<CancellationToken>()).Returns(response);

        var result = await this._sut.GetPeersAsync(query);

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Data!.TotalCount);
    }

    [Fact]
    public async Task GetPeersAsync_EmptyPeers_ReturnsNotFound()
    {
        var query = new GetPeersQuery { QueryId = "q1", Symbol = "UNKN" };
        this._apiClient
            .GetPeersAsync(query, Arg.Any<CancellationToken>())
            .Returns(new GetPeersResponse { Peers = [], Grouping = "industry" });

        var result = await this._sut.GetPeersAsync(query);

        Assert.False(result.IsSuccess);
        Assert.Equal("NotFound", result.ErrorType);
    }

    [Fact]
    public async Task GetPeersAsync_PremiumRequired_MapsToPremiumRequiredResult()
    {
        var query = new GetPeersQuery { QueryId = "q1", Symbol = "AAPL" };
        this._apiClient
            .GetPeersAsync(query, Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientPremiumRequiredException("/api/v1/stock/peers"));

        var result = await this._sut.GetPeersAsync(query);

        Assert.False(result.IsSuccess);
        Assert.Equal("PremiumRequired", result.ErrorType);
    }

    [Fact]
    public async Task GetPeersAsync_HttpError_MapsToServiceUnavailable()
    {
        var query = new GetPeersQuery { QueryId = "q1", Symbol = "AAPL" };
        this._apiClient
            .GetPeersAsync(query, Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientHttpException("boom", HttpStatusCode.BadGateway));

        var result = await this._sut.GetPeersAsync(query);

        Assert.False(result.IsSuccess);
        Assert.Equal("ServiceUnavailable", result.ErrorType);
    }
}
