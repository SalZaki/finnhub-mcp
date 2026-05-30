// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Options;
using FinnHub.MCP.Server.Infrastructure.Clients.Recommendations;
using FinnHub.MCP.Server.Infrastructure.Tests.Unit.Fixtures;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace FinnHub.MCP.Server.Infrastructure.Tests.Unit.Clients.Recommendations;

public sealed class FinnHubRecommendationsApiClientTests : IDisposable
{
    private readonly MockHttpMessageHandler _handler = new();
    private readonly HttpClient _httpClient;
    private readonly FinnHubRecommendationsApiClient _sut;

    public FinnHubRecommendationsApiClientTests()
    {
        this._httpClient = new HttpClient(this._handler) { BaseAddress = new Uri("https://finnhub.io/api/v1/") };

        var options = Substitute.For<IOptions<FinnHubOptions>>();
        options.Value.Returns(new FinnHubOptions
        {
            BaseUrl = "https://finnhub.io/api/v1",
            ApiKey = "test-key",
            EndPoints =
            [
                new FinnHubEndPoint { Name = "recommendation", Url = "stock/recommendation", IsActive = true }
            ]
        });

        this._sut = new FinnHubRecommendationsApiClient(this._httpClient, options, NullLogger<FinnHubRecommendationsApiClient>.Instance);
    }

    [Fact]
    public async Task GetRecommendationsAsync_RealAaplFixture_MapsAllFields()
    {
        this._handler.SetResponse(HttpStatusCode.OK, Fixture.LoadFinnHub("recommendation-AAPL"));

        var snapshots = await this._sut.GetRecommendationsAsync("AAPL", CancellationToken.None);

        Assert.NotEmpty(snapshots);
        // Fixture has 4 monthly snapshots, most-recent first; first entry is 2026-05-01.
        var first = snapshots[0];
        Assert.Equal(new DateOnly(2026, 5, 1), first.Period);
        Assert.Equal(15, first.StrongBuy);
        Assert.Equal(24, first.Buy);
        Assert.Equal(13, first.Hold);
        Assert.Equal(2, first.Sell);
        Assert.Equal(0, first.StrongSell);
        Assert.Equal(54, first.Total);
    }

    /// <summary>
    /// Regression: catches future URL-construction drift. Pinning the full URL
    /// guards against the `/api/v1` → `/api` trailing-slash bug class (PR #169).
    /// </summary>
    [Fact]
    public async Task GetRecommendationsAsync_HitsApiV1RecommendationEndpoint()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "[]");

        await this._sut.GetRecommendationsAsync("AAPL", CancellationToken.None);

        Assert.Equal(
            "https://finnhub.io/api/v1/stock/recommendation?symbol=AAPL",
            this._handler.LastRequest!.RequestUri!.AbsoluteUri);
    }

    [Fact]
    public async Task GetRecommendationsAsync_EmptyArray_ReturnsEmptyList()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "[]");

        var snapshots = await this._sut.GetRecommendationsAsync("AAPL", CancellationToken.None);

        Assert.Empty(snapshots);
    }

    [Fact]
    public async Task GetRecommendationsAsync_EntryWithMissingPeriod_IsSkipped()
    {
        this._handler.SetResponse(
            HttpStatusCode.OK,
            "[{\"symbol\":\"AAPL\",\"strongBuy\":10,\"buy\":5,\"hold\":2,\"sell\":1,\"strongSell\":0},{\"symbol\":\"AAPL\",\"period\":\"2026-05-01\",\"strongBuy\":15,\"buy\":24,\"hold\":13,\"sell\":2,\"strongSell\":0}]");

        var snapshots = await this._sut.GetRecommendationsAsync("AAPL", CancellationToken.None);

        Assert.Single(snapshots);
        Assert.Equal(new DateOnly(2026, 5, 1), snapshots[0].Period);
    }

    [Fact]
    public async Task GetRecommendationsAsync_EntryWithMalformedPeriod_IsSkipped()
    {
        this._handler.SetResponse(
            HttpStatusCode.OK,
            "[{\"symbol\":\"AAPL\",\"period\":\"not-a-date\",\"strongBuy\":10},{\"symbol\":\"AAPL\",\"period\":\"2026-05-01\",\"strongBuy\":15}]");

        var snapshots = await this._sut.GetRecommendationsAsync("AAPL", CancellationToken.None);

        Assert.Single(snapshots);
        Assert.Equal(new DateOnly(2026, 5, 1), snapshots[0].Period);
    }

    [Fact]
    public async Task GetRecommendationsAsync_EntryWithNullBuckets_DefaultsToZero()
    {
        this._handler.SetResponse(
            HttpStatusCode.OK,
            "[{\"symbol\":\"AAPL\",\"period\":\"2026-05-01\",\"strongBuy\":null,\"buy\":null,\"hold\":null,\"sell\":null,\"strongSell\":null}]");

        var snapshots = await this._sut.GetRecommendationsAsync("AAPL", CancellationToken.None);

        Assert.Single(snapshots);
        Assert.Equal(0, snapshots[0].Total);
    }

    [Fact]
    public async Task GetRecommendationsAsync_Forbidden_ThrowsPremiumRequired()
    {
        this._handler.SetResponse(HttpStatusCode.Forbidden, "premium");

        await Assert.ThrowsAsync<ApiClientPremiumRequiredException>(() => this._sut.GetRecommendationsAsync(
            "AAPL", CancellationToken.None));
    }

    [Fact]
    public async Task GetRecommendationsAsync_InvalidJson_ThrowsDeserialization()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "not-json");

        await Assert.ThrowsAsync<ApiClientDeserializationException>(() => this._sut.GetRecommendationsAsync(
            "AAPL", CancellationToken.None));
    }

    [Fact]
    public async Task GetRecommendationsAsync_HttpRequestException_PreservesInnerExceptionAndUsesServiceUnavailable()
    {
        var network = new HttpRequestException("DNS resolution failed");
        this._handler.SetException(network);

        var ex = await Assert.ThrowsAsync<ApiClientHttpException>(() => this._sut.GetRecommendationsAsync(
            "AAPL", CancellationToken.None));

        Assert.Same(network, ex.InnerException);
        Assert.Equal(HttpStatusCode.ServiceUnavailable, ex.StatusCode);
    }

    [Fact]
    public async Task GetRecommendationsAsync_TaskCanceledWithTimeout_ThrowsTimeoutException()
    {
        this._handler.SetException(new TaskCanceledException("slow", new TimeoutException()));

        await Assert.ThrowsAsync<ApiClientTimeoutException>(() => this._sut.GetRecommendationsAsync(
            "AAPL", CancellationToken.None));
    }

    [Fact]
    public async Task GetRecommendationsAsync_TokenAlreadyCancelled_ThrowsCancelledException()
    {
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        await Assert.ThrowsAsync<ApiClientCancelledException>(() => this._sut.GetRecommendationsAsync(
            "AAPL", cts.Token));
    }

    [Fact]
    public async Task GetRecommendationsAsync_NoConfiguredEndpoint_Throws()
    {
        var options = Substitute.For<IOptions<FinnHubOptions>>();
        options.Value.Returns(new FinnHubOptions
        {
            BaseUrl = "https://finnhub.io/api/v1/",
            ApiKey = "test-key",
            EndPoints = []
        });

        var client = new FinnHubRecommendationsApiClient(this._httpClient, options, NullLogger<FinnHubRecommendationsApiClient>.Instance);

        await Assert.ThrowsAsync<ArgumentException>(() => client.GetRecommendationsAsync(
            "AAPL", CancellationToken.None));
    }

    public void Dispose()
    {
        this._handler.Dispose();
        this._httpClient.Dispose();
    }
}
