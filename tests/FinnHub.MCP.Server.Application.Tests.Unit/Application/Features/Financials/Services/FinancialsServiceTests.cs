// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Financials.Clients;
using FinnHub.MCP.Server.Application.Financials.Features.GetFinancialsSnapshot;
using FinnHub.MCP.Server.Application.Financials.Services;
using FinnHub.MCP.Server.Application.Tests.Unit.TestDoubles;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace FinnHub.MCP.Server.Application.Tests.Unit.Application.Features.Financials.Services;

public sealed class FinancialsServiceTests
{
    private readonly IFinancialsApiClient _apiClient = Substitute.For<IFinancialsApiClient>();
    private readonly FakeFinnHubCache _cache = new();
    private readonly FinancialsService _sut;

    public FinancialsServiceTests()
    {

        this._sut = new FinancialsService(this._apiClient, this._cache, NullLogger<FinancialsService>.Instance);
    }

    [Fact]
    public async Task GetSnapshotAsync_NullQuery_Throws()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => this._sut.GetSnapshotAsync(null!));
    }

    [Fact]
    public async Task GetSnapshotAsync_HappyPath_ReturnsSuccess()
    {
        var query = new GetFinancialsSnapshotQuery { QueryId = "q1", Symbol = "AAPL" };
        this._apiClient.GetSnapshotAsync(query, Arg.Any<CancellationToken>())
            .Returns(new GetFinancialsSnapshotResponse { Symbol = "AAPL", MarketCap = 3000000.0 });

        var result = await this._sut.GetSnapshotAsync(query);

        Assert.True(result.IsSuccess);
        Assert.Equal("AAPL", result.Data!.Symbol);
    }

    [Fact]
    public async Task GetSnapshotAsync_PremiumRequired_MapsCorrectly()
    {
        var query = new GetFinancialsSnapshotQuery { QueryId = "q1", Symbol = "AAPL" };
        this._apiClient.GetSnapshotAsync(query, Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientPremiumRequiredException("/api/v1/stock/metric"));

        var result = await this._sut.GetSnapshotAsync(query);

        Assert.False(result.IsSuccess);
        Assert.Equal("PremiumRequired", result.ErrorType);
    }

    [Fact]
    public async Task GetSnapshotAsync_HttpError_MapsToServiceUnavailable()
    {
        var query = new GetFinancialsSnapshotQuery { QueryId = "q1", Symbol = "AAPL" };
        this._apiClient.GetSnapshotAsync(query, Arg.Any<CancellationToken>())
            .ThrowsAsync(new ApiClientHttpException("boom", HttpStatusCode.BadGateway));

        var result = await this._sut.GetSnapshotAsync(query);

        Assert.False(result.IsSuccess);
        Assert.Equal("ServiceUnavailable", result.ErrorType);
    }
}
