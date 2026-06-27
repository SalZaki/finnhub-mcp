// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Options;
using FinnHub.MCP.Server.Infrastructure.Clients.Exchanges;
using FinnHub.MCP.Server.Infrastructure.Tests.Unit.Fixtures;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace FinnHub.MCP.Server.Infrastructure.Tests.Unit.Clients.Exchanges;

public sealed class FinnHubExchangeSymbolsApiClientTests : IDisposable
{
    private readonly MockHttpMessageHandler _handler = new();
    private readonly HttpClient _httpClient;
    private readonly FinnHubExchangesApiClient _sut;

    public FinnHubExchangeSymbolsApiClientTests()
    {
        this._httpClient = new HttpClient(this._handler) { BaseAddress = new Uri("https://finnhub.io/api/v1/") };

        var options = Substitute.For<IOptions<FinnHubOptions>>();
        options.Value.Returns(new FinnHubOptions
        {
            BaseUrl = "https://finnhub.io/api/v1",
            ApiKey = "test-key",
            EndPoints =
            [
                new FinnHubEndPoint { Name = "exchange-symbols", Url = "stock/symbol", IsActive = true }
            ]
        });

        this._sut = new FinnHubExchangesApiClient(this._httpClient, options, NullLogger<FinnHubExchangesApiClient>.Instance);
    }

    [Fact]
    public async Task GetSymbolsAsync_RealUsFixture_MapsRealWireShape()
    {
        this._handler.SetResponse(HttpStatusCode.OK, Fixture.LoadFinnHub("stock-symbol-US"));

        var symbols = await this._sut.GetSymbolsAsync("US", CancellationToken.None);

        // Truncated real capture: 25 faithful rows (the live payload is ~30,538 rows / 7.2MB).
        Assert.Equal(25, symbols.Count);
        Assert.All(symbols, s => Assert.False(string.IsNullOrWhiteSpace(s.Symbol)));

        var first = symbols[0];
        Assert.Equal("CRRTF", first.Symbol);
        Assert.Equal("CRRTF", first.DisplaySymbol);
        Assert.Equal("CRESCITA THERAPEUTICS INC", first.Description);
        Assert.Equal("Common Stock", first.Type);

        Assert.Contains(symbols, s => s.Type == "ETP");
    }

    /// <summary>
    /// Regression: catches future URL-construction drift. Pinning the full URL guards against the
    /// `/api/v1` → `/api` trailing-slash bug class (PR #169). Auth is the X-Finnhub-Token header
    /// (added only in production DI), so the pinned URL has no token query; the live 302 → signed
    /// CDN redirect is followed by the real handler but never by the mock, so the pin stays clean.
    /// </summary>
    [Fact]
    public async Task GetSymbolsAsync_HitsApiV1StockSymbolEndpoint()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "[]");

        await this._sut.GetSymbolsAsync("US", CancellationToken.None);

        Assert.Equal(
            "https://finnhub.io/api/v1/stock/symbol?exchange=US",
            this._handler.LastRequest!.RequestUri!.AbsoluteUri);
    }

    [Fact]
    public async Task GetSymbolsAsync_EmptyArray_ReturnsEmptyList()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "[]");

        var symbols = await this._sut.GetSymbolsAsync("US", CancellationToken.None);

        Assert.Empty(symbols);
    }

    [Fact]
    public async Task GetSymbolsAsync_RowWithBlankSymbol_IsSkipped()
    {
        this._handler.SetResponse(
            HttpStatusCode.OK,
            "[{\"symbol\":\"\",\"description\":\"blank\",\"type\":\"Common Stock\"},{\"symbol\":\"AAPL\",\"displaySymbol\":\"AAPL\",\"description\":\"APPLE INC\",\"type\":\"Common Stock\"}]");

        var symbols = await this._sut.GetSymbolsAsync("US", CancellationToken.None);

        Assert.Single(symbols);
        Assert.Equal("AAPL", symbols[0].Symbol);
    }

    [Fact]
    public async Task GetSymbolsAsync_Forbidden_ThrowsPremiumRequired()
    {
        this._handler.SetResponse(HttpStatusCode.Forbidden, "premium");

        await Assert.ThrowsAsync<ApiClientPremiumRequiredException>(() => this._sut.GetSymbolsAsync(
            "US", CancellationToken.None));
    }

    /// <summary>
    /// Finnhub gates premium-locked exchanges on /stock/symbol with HTTP 401 (not 403). The
    /// exchanges client maps 401 to PremiumRequired so the envelope surfaces premium=true rather
    /// than a misleading ServiceUnavailable.
    /// </summary>
    [Fact]
    public async Task GetSymbolsAsync_Unauthorized_ThrowsPremiumRequired()
    {
        this._handler.SetResponse(HttpStatusCode.Unauthorized, "{\"error\":\"You don't have access to this resource.\"}");

        await Assert.ThrowsAsync<ApiClientPremiumRequiredException>(() => this._sut.GetSymbolsAsync(
            "L", CancellationToken.None));
    }

    [Fact]
    public async Task GetSymbolsAsync_ServerError_ThrowsHttpExceptionWithStatus()
    {
        this._handler.SetResponse(HttpStatusCode.InternalServerError, "boom");

        var ex = await Assert.ThrowsAsync<ApiClientHttpException>(() => this._sut.GetSymbolsAsync(
            "US", CancellationToken.None));

        Assert.Equal(HttpStatusCode.InternalServerError, ex.StatusCode);
    }

    [Fact]
    public async Task GetSymbolsAsync_InvalidJson_ThrowsDeserialization()
    {
        this._handler.SetResponse(HttpStatusCode.OK, "not-json");

        await Assert.ThrowsAsync<ApiClientDeserializationException>(() => this._sut.GetSymbolsAsync(
            "US", CancellationToken.None));
    }

    [Fact]
    public async Task GetSymbolsAsync_HttpRequestException_PreservesInnerExceptionAndUsesServiceUnavailable()
    {
        var network = new HttpRequestException("DNS resolution failed");
        this._handler.SetException(network);

        var ex = await Assert.ThrowsAsync<ApiClientHttpException>(() => this._sut.GetSymbolsAsync(
            "US", CancellationToken.None));

        Assert.Same(network, ex.InnerException);
        Assert.Equal(HttpStatusCode.ServiceUnavailable, ex.StatusCode);
    }

    [Fact]
    public async Task GetSymbolsAsync_TaskCanceledWithTimeout_ThrowsTimeoutException()
    {
        this._handler.SetException(new TaskCanceledException("slow", new TimeoutException()));

        await Assert.ThrowsAsync<ApiClientTimeoutException>(() => this._sut.GetSymbolsAsync(
            "US", CancellationToken.None));
    }

    [Fact]
    public async Task GetSymbolsAsync_TokenAlreadyCancelled_ThrowsCancelledException()
    {
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        await Assert.ThrowsAsync<ApiClientCancelledException>(() => this._sut.GetSymbolsAsync(
            "US", cts.Token));
    }

    [Fact]
    public async Task GetSymbolsAsync_NoConfiguredEndpoint_Throws()
    {
        var options = Substitute.For<IOptions<FinnHubOptions>>();
        options.Value.Returns(new FinnHubOptions
        {
            BaseUrl = "https://finnhub.io/api/v1/",
            ApiKey = "test-key",
            EndPoints = []
        });

        var client = new FinnHubExchangesApiClient(this._httpClient, options, NullLogger<FinnHubExchangesApiClient>.Instance);

        await Assert.ThrowsAsync<ArgumentException>(() => client.GetSymbolsAsync(
            "US", CancellationToken.None));
    }

    public void Dispose()
    {
        this._handler.Dispose();
        this._httpClient.Dispose();
    }
}
