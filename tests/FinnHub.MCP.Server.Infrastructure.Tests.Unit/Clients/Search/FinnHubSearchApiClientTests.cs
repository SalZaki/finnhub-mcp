
// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using System.Text.Json;
using FinnHub.MCP.Server.Application.Options;
using FinnHub.MCP.Server.Application.Search.Features.SearchSymbol;
using FinnHub.MCP.Server.Infrastructure.Clients.Search;
using FinnHub.MCP.Server.Infrastructure.Dtos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace FinnHub.MCP.Server.Infrastructure.Tests.Unit.Clients.Search;

/// <summary>
/// Unit tests for the <see cref="FinnHubSearchApiClient"/> class.
/// </summary>
public sealed class FinnHubSearchApiClientTests : IDisposable
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOptions<FinnHubOptions> _options;
    private readonly ILogger<FinnHubSearchApiClient> _logger;
    private readonly FinnHubOptions _finnHubOptions;
    private FinnHubSearchApiClient _sut;
    private readonly HttpClient _httpClient;
    private readonly MockHttpMessageHandler _messageHandler;

    public FinnHubSearchApiClientTests()
    {
        this._httpClientFactory = Substitute.For<IHttpClientFactory>();
        this._options = Substitute.For<IOptions<FinnHubOptions>>();
        this._logger = Substitute.For<ILogger<FinnHubSearchApiClient>>();
        this._messageHandler = new MockHttpMessageHandler();
        this._httpClient = new HttpClient(this._messageHandler);

        this._finnHubOptions = new FinnHubOptions
        {
            BaseUrl = "https://finnhub.io/api/v1",
            ApiKey = "test-api-key",
            EndPoints =
            [
                new FinnHubEndPoint
                {
                    Name = "search-symbol",
                    Url = "search",
                    IsActive = true
                }
            ]
        };

        this._options.Value.Returns(this._finnHubOptions);
        this._httpClientFactory.CreateClient("FinnHub").Returns(this._httpClient);
        this._sut = new FinnHubSearchApiClient(this._httpClientFactory, this._options, this._logger);
    }

    [Fact]
    public void Constructor_WithNullHttpClientFactory_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new FinnHubSearchApiClient(null!, this._options, this._logger));
    }

    [Fact]
    public void Constructor_WithNullOptions_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new FinnHubSearchApiClient(this._httpClientFactory, null!, this._logger));
    }

    [Fact]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new FinnHubSearchApiClient(this._httpClientFactory, this._options, null!));
    }

    [Fact]
    public void Constructor_WithNullOptionsValue_ThrowsArgumentException()
    {
        // Arrange
        var nullOptions = Substitute.For<IOptions<FinnHubOptions>>();
        nullOptions.Value.Returns((FinnHubOptions)null!);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new FinnHubSearchApiClient(this._httpClientFactory, nullOptions, this._logger));

        Assert.Equal("FinnHub options cannot be null. (Parameter 'options')", exception.Message);
    }

    [Fact]
    public async Task SearchSymbolAsync_WithNullQuery_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            this._sut.SearchSymbolAsync(null!, CancellationToken.None));
    }

    [Fact]
    public async Task SearchSymbolAsync_WithDisposedClient_ThrowsObjectDisposedException()
    {
        // Arrange
        var query = new SearchSymbolQuery { Query = "AAPL", QueryId = Guid.NewGuid().ToString() };
        this._sut.Dispose();

        // Act & Assert
        await Assert.ThrowsAsync<ObjectDisposedException>(() =>
            this._sut.SearchSymbolAsync(query, CancellationToken.None));
    }

    [Fact]
    public async Task SearchSymbolAsync_WithNoActiveEndpoint_ThrowsArgumentException()
    {
        // Arrange
        var query = new SearchSymbolQuery { Query = "AAPL", QueryId = Guid.NewGuid().ToString() };
        this._finnHubOptions.EndPoints.Clear();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            this._sut.SearchSymbolAsync(query, CancellationToken.None));

        Assert.Equal("Search symbol endpoint is not configured or inactive", exception.Message);
    }

    [Fact]
    public async Task SearchSymbolAsync_WithInactiveEndpoint_ThrowsArgumentException()
    {
        // Arrange
        var query = new SearchSymbolQuery { Query = "AAPL", QueryId = Guid.NewGuid().ToString() };

        var finnHubOptions = new FinnHubOptions
        {
            BaseUrl = "https://finnhub.io/api/v1",
            ApiKey = "test-api-key",
            EndPoints =
            [
                new FinnHubEndPoint
                {
                    Name = "search-symbol",
                    Url = "search",
                    IsActive = false
                }
            ]
        };

        this._options.Value.Returns(finnHubOptions);
        this._httpClientFactory.CreateClient("FinnHub").Returns(this._httpClient);
        this._sut = new FinnHubSearchApiClient(this._httpClientFactory, this._options, this._logger);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            this._sut.SearchSymbolAsync(query, CancellationToken.None));

        Assert.Equal("Search symbol endpoint is not configured or inactive", exception.Message);
    }

    [Fact]
    public async Task SearchSymbolAsync_WithValidQuery_ReturnsSuccessfulResponse()
    {
        // Arrange
        var query = new SearchSymbolQuery { Query = "AAPL", QueryId = Guid.NewGuid().ToString() };
        var expectedResponse = new FinnHubSearchResponse
        {
            Count = 1,
            Result =
            [
                new FinnHubSymbolResult
                {
                    Symbol = "AAPL",
                    Description = "Apple Inc",
                    DisplaySymbol = "AAPL",
                    Type = "Common Stock"
                }
            ]
        };

        var jsonResponse = JsonSerializer.Serialize(expectedResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        });

        this._messageHandler.SetResponse(HttpStatusCode.OK, jsonResponse);

        // Act
        var result = await this._sut.SearchSymbolAsync(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("AAPL", result.Query);
        Assert.NotNull(result.QueryId);
        Assert.Equal("finnhub-api", result.Source);
        Assert.False(result.IsFromCache);
        Assert.Single(result.Symbols);
        Assert.Equal("AAPL", result.Symbols[0].Symbol);
        Assert.Equal("Apple Inc", result.Symbols[0].Description);
        Assert.Equal("AAPL", result.Symbols[0].DisplaySymbol);
        Assert.Equal("Common Stock", result.Symbols[0].Type);
    }

    [Fact]
    public async Task SearchSymbolAsync_WithQueryAndExchange_BuildsCorrectUri()
    {
        // Arrange
        var query = new SearchSymbolQuery
        {
            Query = "AAPL",
            Exchange = "NASDAQ",
            QueryId = Guid.NewGuid().ToString()
        };

        var expectedResponse = new FinnHubSearchResponse
        {
            Count = 0,
            Result = []
        };

        var jsonResponse = JsonSerializer.Serialize(expectedResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        });

        this._messageHandler.SetResponse(HttpStatusCode.OK, jsonResponse);

        // Act
        await this._sut.SearchSymbolAsync(query, CancellationToken.None);

        // Assert
        var requestUri = this._messageHandler.LastRequest?.RequestUri?.ToString();
        Assert.NotNull(requestUri);
        Assert.Contains("q=AAPL", requestUri);
        Assert.Contains("exchange=NASDAQ", requestUri);
    }

    [Fact]
    public async Task SearchSymbolAsync_WithEmptyResponse_ReturnsEmptySymbolsList()
    {
        // Arrange
        var query = new SearchSymbolQuery { Query = "INVALID", QueryId = Guid.NewGuid().ToString() };
        var expectedResponse = new FinnHubSearchResponse
        {
            Count = 0,
            Result = []
        };

        var jsonResponse =  JsonSerializer.Serialize(expectedResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        });

        this._messageHandler.SetResponse(HttpStatusCode.OK, jsonResponse);

        // Act
        var result = await this._sut.SearchSymbolAsync(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Symbols);
    }

    [Fact]
    public async Task SearchSymbolAsync_WithHttpRequestException_ThrowsHttpRequestException()
    {
        // Arrange
        var query = new SearchSymbolQuery { Query = "AAPL", QueryId = Guid.NewGuid().ToString() };
        this._messageHandler.SetException(new HttpRequestException("Network error"));

        // Act & Assert
        await Assert.ThrowsAsync<SearchSymbolHttpException>(() =>
            this._sut.SearchSymbolAsync(query, CancellationToken.None));
    }

    [Fact]
    public async Task SearchSymbolAsync_WithTaskCancelledException_ThrowsSearchSymbolTimeoutException()
    {
        // Arrange
        var query = new SearchSymbolQuery { Query = "AAPL", QueryId = Guid.NewGuid().ToString() };
        this._messageHandler.SetException(new TaskCanceledException("Request timed out", new TimeoutException()));

        // Act & Assert
        await Assert.ThrowsAsync<SearchSymbolTimeoutException>(() =>
            this._sut.SearchSymbolAsync(query, CancellationToken.None));
    }

    [Fact]
    public async Task SearchSymbolAsync_WithCancellationToken_ThrowsOperationCanceledException()
    {
        // Arrange
        var query = new SearchSymbolQuery { Query = "AAPL", QueryId = Guid.NewGuid().ToString() };
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        // Act & Assert
        await Assert.ThrowsAsync<SearchSymbolCancelledException>(() =>
            this._sut.SearchSymbolAsync(query, cts.Token));
    }

    [Fact]
    public async Task SearchSymbolAsync_WithUnauthorizedResponse_ThrowsHttpRequestException()
    {
        // Arrange
        var query = new SearchSymbolQuery {Query = "AAPL", QueryId = Guid.NewGuid().ToString()};
        this._messageHandler.SetResponse(HttpStatusCode.Unauthorized, "Unauthorized");

        // Act & Assert
        await Assert.ThrowsAsync<SearchSymbolHttpException>(() =>
            this._sut.SearchSymbolAsync(query, CancellationToken.None));
    }

    [Fact]
    public async Task SearchSymbolAsync_WithInvalidJson_ThrowsJsonException()
    {
        // Arrange
        var query = new SearchSymbolQuery {Query = "AAPL", QueryId = Guid.NewGuid().ToString()};
        this._messageHandler.SetResponse(HttpStatusCode.OK, "invalid_json");

        // Act & Assert
        await Assert.ThrowsAsync<SearchSymbolDeserializationException>(() =>
            this._sut.SearchSymbolAsync(query, CancellationToken.None));
    }

    [Fact]
    public async Task SearchSymbolAsync_WithNullSymbolFields_HandlesNullValues()
    {
        // Arrange
        var query = new SearchSymbolQuery {Query = "TEST", QueryId = Guid.NewGuid().ToString()};
        var expectedResponse = new FinnHubSearchResponse
        {
            Count = 1,
            Result =
            [
                new FinnHubSymbolResult
                {
                    Symbol = null,
                    Description = null,
                    DisplaySymbol = null,
                    Type = null
                }
            ]
        };

        var jsonResponse = JsonSerializer.Serialize(expectedResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        });

        this._messageHandler.SetResponse(HttpStatusCode.OK, jsonResponse);

        // Act
        var result = await this._sut.SearchSymbolAsync(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Symbols);
        Assert.Equal(string.Empty, result.Symbols[0].Symbol);
        Assert.Equal(string.Empty, result.Symbols[0].Description);
        Assert.Equal(string.Empty, result.Symbols[0].DisplaySymbol);
        Assert.Equal(string.Empty, result.Symbols[0].Type);
    }

    [Fact]
    public void Dispose_CalledOnce_DisposesResourcesAndLogsDebug()
    {
        // Act
        this._sut.Dispose();

        // Assert
        this._logger.Received(1).Log(
            LogLevel.Debug,
            Arg.Any<EventId>(),
            Arg.Is<object>(v => v.ToString() == "FinnHubSearchApiClient disposed."),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception, string>>()!);
    }

    [Fact]
    public void Dispose_CalledMultipleTimes_DoesNotThrow()
    {
        // Act & Assert
        this._sut.Dispose();
        this._sut.Dispose();
    }

    public void Dispose()
    {
        this._sut.Dispose();
        this._httpClient.Dispose();
        this._messageHandler.Dispose();
    }
}
