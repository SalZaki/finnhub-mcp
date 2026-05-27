// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Options;
using FinnHub.MCP.Server.Infrastructure.Clients.Insiders;
using FinnHub.MCP.Server.Infrastructure.Tests.Unit.Fixtures;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace FinnHub.MCP.Server.Infrastructure.Tests.Unit.Clients.Insiders;

public sealed class FinnHubInsidersApiClientTests : IDisposable
{
    private readonly MockHttpMessageHandler _handler = new();
    private readonly HttpClient _httpClient;
    private readonly FinnHubInsidersApiClient _sut;

    public FinnHubInsidersApiClientTests()
    {
        this._httpClient = new HttpClient(this._handler) { BaseAddress = new Uri("https://finnhub.io/api/v1/") };

        var options = Substitute.For<IOptions<FinnHubOptions>>();
        options.Value.Returns(new FinnHubOptions
        {
            BaseUrl = "https://finnhub.io/api/v1",
            ApiKey = "test-key",
            EndPoints =
            [
                new FinnHubEndPoint { Name = "insider-transactions", Url = "stock/insider-transactions", IsActive = true }
            ]
        });

        this._sut = new FinnHubInsidersApiClient(this._httpClient, options, NullLogger<FinnHubInsidersApiClient>.Instance);
    }

    [Fact]
    public async Task GetInsiderTransactionsAsync_RealAaplFixture_MapsAllFields()
    {
        this._handler.SetResponse(HttpStatusCode.OK, Fixture.LoadFinnHub("insider-transactions-AAPL"));

        var transactions = await this._sut.GetInsiderTransactionsAsync(
            "AAPL", new DateOnly(2026, 4, 27), new DateOnly(2026, 5, 27), CancellationToken.None);

        Assert.NotEmpty(transactions);

        var first = transactions[0];
        Assert.Equal("Borders Ben", first.Name);
        Assert.Equal(-1274, first.Change);
        Assert.Equal(38713, first.Share);
        Assert.Equal(new DateOnly(2026, 5, 8), first.TransactionDate);
        Assert.Equal(new DateOnly(2026, 5, 12), first.FilingDate);
        Assert.Equal(290, first.TransactionPrice);
        Assert.Equal("S", first.TransactionCode);
        Assert.False(first.IsDerivative);

        // Fixture has a 0-price gift entry from LEVINSON.
        var gift = transactions.First(t =>
            string.Equals(t.Name, "LEVINSON ARTHUR D", StringComparison.Ordinal)
            && string.Equals(t.TransactionCode, "G", StringComparison.Ordinal));
        Assert.Equal(0, gift.TransactionPrice);
        Assert.Equal(-5000, gift.Change);
    }

    /// <summary>
    /// Regression: catches future URL-construction drift. Pinning the full URL
    /// guards against the `/api/v1` → `/api` trailing-slash bug class (PR #169).
    /// </summary>
    [Fact]
    public async Task GetInsiderTransactionsAsync_HitsApiV1InsiderTransactionsEndpoint()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "{\"data\":[]}");

        await this._sut.GetInsiderTransactionsAsync(
            "AAPL", new DateOnly(2026, 4, 27), new DateOnly(2026, 5, 27), CancellationToken.None);

        Assert.Equal(
            "https://finnhub.io/api/v1/stock/insider-transactions?symbol=AAPL&from=2026-04-27&to=2026-05-27",
            this._handler.LastRequest!.RequestUri!.AbsoluteUri);
    }

    [Fact]
    public async Task GetInsiderTransactionsAsync_EmptyEnvelope_ReturnsEmptyList()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "{\"data\":[]}");

        var transactions = await this._sut.GetInsiderTransactionsAsync(
            "AAPL", new DateOnly(2026, 4, 27), new DateOnly(2026, 5, 27), CancellationToken.None);

        Assert.Empty(transactions);
    }

    [Fact]
    public async Task GetInsiderTransactionsAsync_NullData_ReturnsEmptyList()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "{}");

        var transactions = await this._sut.GetInsiderTransactionsAsync(
            "AAPL", new DateOnly(2026, 4, 27), new DateOnly(2026, 5, 27), CancellationToken.None);

        Assert.Empty(transactions);
    }

    [Fact]
    public async Task GetInsiderTransactionsAsync_EntryWithMissingName_IsSkipped()
    {
        this._handler.SetResponse(
            HttpStatusCode.OK,
            "{\"data\":[{\"change\":100,\"transactionDate\":\"2026-05-08\"},{\"name\":\"Alice\",\"change\":-100,\"transactionDate\":\"2026-05-09\"}]}");

        var transactions = await this._sut.GetInsiderTransactionsAsync(
            "AAPL", new DateOnly(2026, 4, 27), new DateOnly(2026, 5, 27), CancellationToken.None);

        Assert.Single(transactions);
        Assert.Equal("Alice", transactions[0].Name);
    }

    [Fact]
    public async Task GetInsiderTransactionsAsync_EntryWithNullChange_IsSkipped()
    {
        this._handler.SetResponse(
            HttpStatusCode.OK,
            "{\"data\":[{\"name\":\"Alice\",\"change\":null,\"transactionDate\":\"2026-05-08\"},{\"name\":\"Bob\",\"change\":-100,\"transactionDate\":\"2026-05-09\"}]}");

        var transactions = await this._sut.GetInsiderTransactionsAsync(
            "AAPL", new DateOnly(2026, 4, 27), new DateOnly(2026, 5, 27), CancellationToken.None);

        Assert.Single(transactions);
        Assert.Equal("Bob", transactions[0].Name);
    }

    [Fact]
    public async Task GetInsiderTransactionsAsync_EntryWithMalformedDate_IsSkipped()
    {
        this._handler.SetResponse(
            HttpStatusCode.OK,
            "{\"data\":[{\"name\":\"Alice\",\"change\":-100,\"transactionDate\":\"not-a-date\"},{\"name\":\"Bob\",\"change\":-50,\"transactionDate\":\"2026-05-09\"}]}");

        var transactions = await this._sut.GetInsiderTransactionsAsync(
            "AAPL", new DateOnly(2026, 4, 27), new DateOnly(2026, 5, 27), CancellationToken.None);

        Assert.Single(transactions);
        Assert.Equal("Bob", transactions[0].Name);
    }

    [Fact]
    public async Task GetInsiderTransactionsAsync_EntryWithMissingFilingDate_ParsesAsNull()
    {
        this._handler.SetResponse(
            HttpStatusCode.OK,
            "{\"data\":[{\"name\":\"Alice\",\"change\":-100,\"transactionDate\":\"2026-05-09\"}]}");

        var transactions = await this._sut.GetInsiderTransactionsAsync(
            "AAPL", new DateOnly(2026, 4, 27), new DateOnly(2026, 5, 27), CancellationToken.None);

        Assert.Single(transactions);
        Assert.Null(transactions[0].FilingDate);
    }

    [Fact]
    public async Task GetInsiderTransactionsAsync_Forbidden_ThrowsPremiumRequired()
    {
        this._handler.SetResponse(HttpStatusCode.Forbidden, "premium");

        await Assert.ThrowsAsync<ApiClientPremiumRequiredException>(() => this._sut.GetInsiderTransactionsAsync(
            "AAPL", new DateOnly(2026, 4, 27), new DateOnly(2026, 5, 27), CancellationToken.None));
    }

    [Fact]
    public async Task GetInsiderTransactionsAsync_InvalidJson_ThrowsDeserialization()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "not-json");

        await Assert.ThrowsAsync<ApiClientDeserializationException>(() => this._sut.GetInsiderTransactionsAsync(
            "AAPL", new DateOnly(2026, 4, 27), new DateOnly(2026, 5, 27), CancellationToken.None));
    }

    [Fact]
    public async Task GetInsiderTransactionsAsync_HttpRequestException_PreservesInnerExceptionAndUsesServiceUnavailable()
    {
        var network = new HttpRequestException("DNS resolution failed");
        this._handler.SetException(network);

        var ex = await Assert.ThrowsAsync<ApiClientHttpException>(() => this._sut.GetInsiderTransactionsAsync(
            "AAPL", new DateOnly(2026, 4, 27), new DateOnly(2026, 5, 27), CancellationToken.None));

        Assert.Same(network, ex.InnerException);
        Assert.Equal(HttpStatusCode.ServiceUnavailable, ex.StatusCode);
    }

    [Fact]
    public async Task GetInsiderTransactionsAsync_TaskCanceledWithTimeout_ThrowsTimeoutException()
    {
        this._handler.SetException(new TaskCanceledException("slow", new TimeoutException()));

        await Assert.ThrowsAsync<ApiClientTimeoutException>(() => this._sut.GetInsiderTransactionsAsync(
            "AAPL", new DateOnly(2026, 4, 27), new DateOnly(2026, 5, 27), CancellationToken.None));
    }

    [Fact]
    public async Task GetInsiderTransactionsAsync_TokenAlreadyCancelled_ThrowsCancelledException()
    {
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        await Assert.ThrowsAsync<ApiClientCancelledException>(() => this._sut.GetInsiderTransactionsAsync(
            "AAPL", new DateOnly(2026, 4, 27), new DateOnly(2026, 5, 27), cts.Token));
    }

    [Fact]
    public async Task GetInsiderTransactionsAsync_NoConfiguredEndpoint_Throws()
    {
        var options = Substitute.For<IOptions<FinnHubOptions>>();
        options.Value.Returns(new FinnHubOptions
        {
            BaseUrl = "https://finnhub.io/api/v1/",
            ApiKey = "test-key",
            EndPoints = []
        });

        var client = new FinnHubInsidersApiClient(this._httpClient, options, NullLogger<FinnHubInsidersApiClient>.Instance);

        await Assert.ThrowsAsync<ArgumentException>(() => client.GetInsiderTransactionsAsync(
            "AAPL", new DateOnly(2026, 4, 27), new DateOnly(2026, 5, 27), CancellationToken.None));
    }

    public void Dispose()
    {
        this._handler.Dispose();
        this._httpClient.Dispose();
    }
}
