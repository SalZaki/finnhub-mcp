using System.Net;
using System.Net.Http.Json;
using MCP.FinnHub.Server.SSE.Models;
using MCP.FinnHub.Server.SSE.Options;
using MCP.FinnHub.Server.SSE.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace MCP.FinnHub.Server.Sse.Tests.Unit.Services;

/// <summary>
/// Unit tests for the <see cref="FinnHubService"/> class, which integrates with the Finnhub API.
/// </summary>
public sealed class FinnHubServiceTests
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOptions<FinnHubOptions> _options;
    private readonly ILogger<IFinnHubService> _logger;

    /// <summary>
    /// Initializes test dependencies and mocks for <see cref="FinnHubService"/>.
    /// </summary>
    public FinnHubServiceTests()
    {
        this._httpClientFactory = Substitute.For<IHttpClientFactory>();
        this._logger = Substitute.For<ILogger<IFinnHubService>>();

        this._options = Options.Create(new FinnHubOptions
        {
            ApiKey = "test-api-key",
            BaseUrl = "https://finnhub.io/api/v1"
        });

        using var handler = new MockHttpMessageHandler();
        using var httpClient = new HttpClient(handler);
        httpClient.BaseAddress = new Uri(this._options.Value.BaseUrl);

        this._httpClientFactory.CreateClient("finnhub").Returns(httpClient);
    }

    /// <summary>
    /// Verifies that <see cref="FinnHubService.GetUsStockSymbolsAsync"/> returns expected stock symbols
    /// when the Finnhub API call is successful.
    /// </summary>
    [Fact]
    public async Task GetUsStockSymbolsAsync_ReturnsStockSymbols_WhenApiCallIsSuccessful()
    {
        // Arrange
        var expectedSymbols = new List<StockSymbol>
        {
            new()
            {
                Currency = "USD",
                Description = "Apple Inc",
                DisplaySymbol = "AAPL",
                Figi = "BBG000B9Y5X2",
                Mic = "XNAS",
                Symbol = "AAPL",
                Type = "Common Stock"
            }
        };

        using var mockHandler = new MockHttpMessageHandler(_ =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(expectedSymbols)
            };

            return Task.FromResult(response);
        });

        using var mockClient = new HttpClient(mockHandler);
        mockClient.BaseAddress = new Uri(this._options.Value.BaseUrl);
        this._httpClientFactory.CreateClient("finnhub").Returns(mockClient);
        var service = new FinnHubService(this._httpClientFactory, this._options, this._logger);

        // Act
        var result = await service.GetUsStockSymbolsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("AAPL", result[0].Symbol);
    }

    [Fact]
    public async Task GetUsStockSymbolsAsync_ThrowsHttpRequestException_WhenApiFails()
    {
        // Arrange
        using var handler = new MockHttpMessageHandler(_ => throw new HttpRequestException("Simulated failure"));
        using var client = new HttpClient(handler);
        client.BaseAddress = new Uri(this._options.Value.BaseUrl);

        this._httpClientFactory.CreateClient("finnhub").Returns(client);
        var service = new FinnHubService(this._httpClientFactory, this._options, this._logger);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            service.GetUsStockSymbolsAsync());
    }

    [Fact]
    public async Task GetUsStockSymbolsAsync_ThrowsTaskCanceledException_WhenRequestIsCancelled()
    {
        // Arrange
        using var handler = new MockHttpMessageHandler(_ => throw new TaskCanceledException("Simulated timeout"));
        using var client = new HttpClient(handler);
        client.BaseAddress = new Uri(this._options.Value.BaseUrl);

        this._httpClientFactory.CreateClient("finnhub").Returns(client);
        var service = new FinnHubService(this._httpClientFactory, this._options, this._logger);

        // Act & Assert
        await Assert.ThrowsAsync<TaskCanceledException>(() =>
            service.GetUsStockSymbolsAsync());
    }
}
