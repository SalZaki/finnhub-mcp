// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Options;
using FinnHub.MCP.Server.Application.Peers.Features.GetPeers;
using FinnHub.MCP.Server.Infrastructure.Clients.Peers;
using FinnHub.MCP.Server.Infrastructure.Tests.Unit.Fixtures;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace FinnHub.MCP.Server.Infrastructure.Tests.Unit.Clients.Peers;

public sealed class FinnHubPeersApiClientTests : IDisposable
{
    private readonly MockHttpMessageHandler _handler = new();
    private readonly HttpClient _httpClient;
    private readonly FinnHubPeersApiClient _sut;

    public FinnHubPeersApiClientTests()
    {
        this._httpClient = new HttpClient(this._handler) { BaseAddress = new Uri("https://finnhub.io/api/v1/") };

        var options = Substitute.For<IOptions<FinnHubOptions>>();
        options.Value.Returns(new FinnHubOptions
        {
            BaseUrl = "https://finnhub.io/api/v1",
            ApiKey = "test-key",
            EndPoints = [new FinnHubEndPoint { Name = "peers", Url = "stock/peers", IsActive = true }]
        });

        this._sut = new FinnHubPeersApiClient(
            this._httpClient,
            options,
            NullLogger<FinnHubPeersApiClient>.Instance);
    }

    [Fact]
    public async Task GetPeersAsync_Success_ReturnsMappedResponse()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "[\"MSFT\",\"GOOG\",\"AMZN\"]");

        var result = await this._sut.GetPeersAsync(
            new GetPeersQuery { QueryId = "q1", Symbol = "AAPL", Grouping = PeersGrouping.Industry },
            CancellationToken.None);

        Assert.Equal(3, result.TotalCount);
        Assert.Equal("MSFT", result.Peers[0]);
        Assert.Equal("industry", result.Grouping);
    }

    [Fact]
    public async Task GetPeersAsync_SubIndustryGrouping_MapsCorrectlyInUri()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "[]");

        await this._sut.GetPeersAsync(
            new GetPeersQuery { QueryId = "q1", Symbol = "AAPL", Grouping = PeersGrouping.SubIndustry },
            CancellationToken.None);

        Assert.NotNull(this._handler.LastRequest?.RequestUri);
        Assert.Contains("grouping=subIndustry", this._handler.LastRequest!.RequestUri!.Query, StringComparison.Ordinal);
    }

    /// <summary>
    /// Regression: catches future URL-construction drift. See FinnHubProfilesApiClientTests
    /// for the bug class this protects against.
    /// </summary>
    [Fact]
    public async Task GetPeersAsync_HitsApiV1StockPeersEndpoint()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "[]");

        await this._sut.GetPeersAsync(
            new GetPeersQuery { QueryId = "q1", Symbol = "AAPL", Grouping = PeersGrouping.Industry },
            CancellationToken.None);

        Assert.NotNull(this._handler.LastRequest?.RequestUri);
        Assert.Equal(
            "https://finnhub.io/api/v1/stock/peers?symbol=AAPL&grouping=industry",
            this._handler.LastRequest!.RequestUri!.AbsoluteUri);
    }

    [Fact]
    public async Task GetPeersAsync_Forbidden_ThrowsPremiumRequired()
    {
        this._handler.SetResponse(HttpStatusCode.Forbidden, "{\"error\":\"premium\"}");

        await Assert.ThrowsAsync<ApiClientPremiumRequiredException>(() => this._sut.GetPeersAsync(
            new GetPeersQuery { QueryId = "q1", Symbol = "AAPL" },
            CancellationToken.None));
    }

    [Fact]
    public async Task GetPeersAsync_ServerError_ThrowsHttpException()
    {
        this._handler.SetResponse(HttpStatusCode.BadGateway, "upstream down");

        await Assert.ThrowsAsync<ApiClientHttpException>(() => this._sut.GetPeersAsync(
            new GetPeersQuery { QueryId = "q1", Symbol = "AAPL" },
            CancellationToken.None));
    }

    [Fact]
    public async Task GetPeersAsync_InvalidJson_ThrowsDeserialization()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "not-json");

        await Assert.ThrowsAsync<ApiClientDeserializationException>(() => this._sut.GetPeersAsync(
            new GetPeersQuery { QueryId = "q1", Symbol = "AAPL" },
            CancellationToken.None));
    }

    [Fact]
    public async Task GetPeersAsync_HttpRequestException_ThrowsHttpException()
    {
        this._handler.SetException(new HttpRequestException("network unreachable"));

        await Assert.ThrowsAsync<ApiClientHttpException>(() => this._sut.GetPeersAsync(
            new GetPeersQuery { QueryId = "q1", Symbol = "AAPL" },
            CancellationToken.None));
    }

    [Fact]
    public async Task GetPeersAsync_TaskCanceledWithTimeout_ThrowsTimeoutException()
    {
        this._handler.SetException(new TaskCanceledException("slow", new TimeoutException()));

        await Assert.ThrowsAsync<ApiClientTimeoutException>(() => this._sut.GetPeersAsync(
            new GetPeersQuery { QueryId = "q1", Symbol = "AAPL" },
            CancellationToken.None));
    }

    [Fact]
    public async Task GetPeersAsync_TokenAlreadyCancelled_ThrowsCancelledException()
    {
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        await Assert.ThrowsAsync<ApiClientCancelledException>(() => this._sut.GetPeersAsync(
            new GetPeersQuery { QueryId = "q1", Symbol = "AAPL" },
            cts.Token));
    }

    /// <summary>
    /// Real captured Finnhub /stock/peers response for AAPL (industry grouping).
    /// Catches Finnhub shape drift on the next fixture refresh — see CLAUDE.md
    /// "Don't ship synthetic-payload-only client tests".
    /// </summary>
    [Fact]
    public async Task GetPeersAsync_RealAaplFixture_ParsesPeerArray()
    {
        this._handler.SetResponse(HttpStatusCode.OK, Fixture.LoadFinnHub("peers-AAPL-industry"));

        var result = await this._sut.GetPeersAsync(
            new GetPeersQuery { QueryId = "q1", Symbol = "AAPL" },
            CancellationToken.None);

        Assert.NotEmpty(result.Peers);
        Assert.Contains("AAPL", result.Peers);
    }

    public void Dispose()
    {
        this._handler.Dispose();
        this._httpClient.Dispose();
    }
}
