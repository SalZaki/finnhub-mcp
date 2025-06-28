// --------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of  FinnHub MCP project and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
//  <summary>
//    Add summary.
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using MCP.FinnHub.Server.SSE.Application;
using MCP.FinnHub.Server.SSE.Application.Features.Search.Queries;
using MCP.FinnHub.Server.SSE.Application.Features.Search.Services;
using MCP.FinnHub.Server.SSE.Models;
using MCP.FinnHub.Server.SSE.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace MCP.FinnHub.Server.SSE.Tests.Unit.Application.Features.Search.Services;

/// <summary>
/// Unit tests for the <see cref="SearchService"/> class, which integrates with the FinnHub API.
/// </summary>
public sealed class SearchServiceTests : IDisposable
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOptions<FinnHubOptions> _finnHubOptions;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly ILogger<SearchService> _logger;
    private HttpClient? _httpClient;

    /// <summary>
    /// Initializes test dependencies and mocks for <see cref="SearchService"/>.
    /// </summary>
    public SearchServiceTests()
    {
        this._logger = Substitute.For<ILogger<SearchService>>();
        this._httpClientFactory = Substitute.For<IHttpClientFactory>();
        this._serializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        this._finnHubOptions = Microsoft.Extensions.Options.Options.Create(new FinnHubOptions
        {
            ApiKey = "test-api-key",
            BaseUrl = "https://finnhub.io/api/v1",
            EndPoints =
            [
                new FinnHubEndPoint
                {
                    Name = "search-symbol",
                    Url = "search",
                    IsActive = true
                }
            ]
        });
    }
    /// <summary>
    /// Configures a mock <see cref="HttpClient"/> with a provided response delegate.
    /// </summary>
    /// <param name="handlerFunc">A delegate to handle HTTP requests.</param>
    private void SetupMockHttpClient(Func<HttpRequestMessage, Task<HttpResponseMessage>> handlerFunc)
    {
        using var handler = new MockHttpMessageHandler(handlerFunc);
        var client = new HttpClient(handler);
        this._httpClientFactory.CreateClient("FinnHub").Returns(client);
        this._httpClient = client;
    }

    /// <summary>
    /// Verifies that <see cref="SearchService.SearchSymbolsAsync"/> returns expected stock symbols when the FinnHub API
    /// call is successful.
    /// </summary>
    [Fact]
    public async Task SearchSymbolsAsync_ReturnsStockSymbols_WhenApiCallIsSuccessful()
    {
        // Arrange
        var query = new SymbolSearchQuery {Query = "AAPL"};

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

        this.SetupMockHttpClient(_ => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(expectedSymbols, this._serializerOptions))
        }));

        using var service = new SearchService(this._httpClientFactory, this._finnHubOptions, this._logger);

        // Act
        var actualResult = await service.SearchSymbolsAsync(query);

        // Assert
        Assert.NotNull(actualResult);
        Assert.True(actualResult.IsSuccess);
        Assert.Equal(1, actualResult.Data?.Count);
        Assert.Equal("AAPL", actualResult.Data?[0].Symbol);
        Assert.Equal("Apple Inc", actualResult.Data?[0].Description);
        Assert.Equal("USD", actualResult.Data?[0].Currency);
        Assert.Equal("AAPL", actualResult.Data?[0].DisplaySymbol);
        Assert.Equal("BBG000B9Y5X2", actualResult.Data?[0].Figi);
        Assert.Equal("XNAS", actualResult.Data?[0].Mic);
        Assert.Equal("Common Stock", actualResult.Data?[0].Type);
    }

    /// <summary>
    /// Verifies that the search includes exchange parameter when provided.
    /// </summary>
    [Fact]
    public async Task SearchSymbolsAsync_IncludesExchangeParameter_WhenExchangeIsProvided()
    {
        // Arrange
        var request = new SymbolSearchQuery
        {
            Query = "AAPL",
            Exchange = "NASDAQ"
        };

        var expectedSymbols = new List<StockSymbol>();
        Uri? capturedUri = null;

        this.SetupMockHttpClient(requestMessage =>
        {
            capturedUri = requestMessage.RequestUri;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(expectedSymbols))
            });
        });

        using var service = new SearchService(this._httpClientFactory, this._finnHubOptions, this._logger);

        // Act
        await service.SearchSymbolsAsync(request);

        // Assert
        var requestUri = capturedUri?.ToString();
        Assert.Contains("q=AAPL", requestUri);
        Assert.Contains("exchange=NASDAQ", requestUri);
    }

    /// <summary>
    /// Verifies that the search doesn't include exchange parameter when not provided.
    /// </summary>
    [Fact]
    public async Task SearchSymbolsAsync_ExcludesExchangeParameter_WhenExchangeIsNotProvided()
    {
        // Arrange
        var request = new SymbolSearchQuery{ Query = "AAPL" };

        var expectedSymbols = new List<StockSymbol>();
        Uri? capturedUri = null;

        this.SetupMockHttpClient(requestMessage =>
        {
            capturedUri = requestMessage.RequestUri;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(expectedSymbols))
            });
        });

        using var service = new SearchService(this._httpClientFactory, this._finnHubOptions, this._logger);

        // Act
        await service.SearchSymbolsAsync(request);

        // Assert
        var requestUri = capturedUri?.ToString();
        Assert.Contains("q=AAPL", requestUri);
        Assert.DoesNotContain("exchange=", requestUri);
    }

    /// <summary>
    /// Verifies that null request parameter throws ArgumentNullException.
    /// </summary>
    [Fact]
    public async Task SearchSymbolsAsync_ThrowsArgumentNullException_WhenRequestIsNull()
    {
        // Arrange
        this.SetupMockHttpClient(_ => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(new List<StockSymbol>()))
        }));

        using var service = new SearchService(this._httpClientFactory, this._finnHubOptions, this._logger);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            service.SearchSymbolsAsync(null!));
    }

    /// <summary>
    /// Verifies that empty query returns empty list.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task SearchSymbolsAsync_ReturnsEmptyList_WhenQueryIsEmpty(string query)
    {
        // Arrange
        var request = new SymbolSearchQuery { Query = query };

        this.SetupMockHttpClient(_ => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(new List<StockSymbol>()))
        }));

        using var service = new SearchService(this._httpClientFactory, this._finnHubOptions, this._logger);

        // Act
        var result = await service.SearchSymbolsAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Data?.Count);
    }

    /// <summary>
    /// Verifies that inactive endpoint throws InvalidOperationException.
    /// </summary>
    [Fact]
    public async Task SearchSymbolsAsync_ReturnsServiceUnavailable_WhenEndpointIsInactive()
    {
        // Arrange
        var inactiveOptions = Microsoft.Extensions.Options.Options.Create(new FinnHubOptions
        {
            ApiKey = "test-api-key",
            BaseUrl = "https://finnhub.io/api/v1",
            EndPoints =
            [
                new FinnHubEndPoint
                {
                    Name = "search-symbol",
                    Url = "search",
                    // Inactive endpoint
                    IsActive = false
                }
            ]
        });

        var request = new SymbolSearchQuery { Query = "AAPL" };
        using var service = new SearchService(this._httpClientFactory, inactiveOptions, this._logger);

        // Act
        var result = await service.SearchSymbolsAsync(request);

        // Assert
        Assert.IsType<Result<IReadOnlyList<StockSymbol>>>(result);
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Contains("Search service is not available", result.ErrorMessage);
        Assert.Equivalent(result.ErrorType, ResultErrorType.ServiceUnavailable);
    }

    /// <summary>
    /// Verifies that missing endpoint throws InvalidOperationException.
    /// </summary>
    [Fact]
    public async Task SearchSymbolsAsync_ReturnsServiceUnavailable_WhenEndpointIsMissing()
    {
        // Arrange
        var noEndpointOptions = Microsoft.Extensions.Options.Options.Create(new FinnHubOptions
        {
            ApiKey = "test-api-key",
            BaseUrl = "https://finnhub.io/api/v1",
            // No endpoints configured
            EndPoints = []
        });

        var request = new SymbolSearchQuery { Query = "AAPL" };

        using var service = new SearchService(this._httpClientFactory, noEndpointOptions, this._logger);

        // Act
        var result = await service.SearchSymbolsAsync(request);

        // Assert
        Assert.IsType<Result<IReadOnlyList<StockSymbol>>>(result);
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Contains("Search service is not available", result.ErrorMessage);
        Assert.Equivalent(result.ErrorType, ResultErrorType.ServiceUnavailable);
    }

    /// <summary>
    /// Verifies that HTTP request failure throws SearchServiceException.
    /// </summary>
    [Fact]
    public async Task SearchSymbolsAsync_ReturnsServiceUnavailable_WhenHttpRequestFails()
    {
        // Arrange
        var request = new SymbolSearchQuery { Query = "AAPL" };

        this.SetupMockHttpClient(_ => throw new HttpRequestException());

        using var service = new SearchService(this._httpClientFactory, this._finnHubOptions, this._logger);

        // Act
        var result = await service.SearchSymbolsAsync(request);

        // Assert
        Assert.IsType<Result<IReadOnlyList<StockSymbol>>>(result);
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Contains("Service temporarily unavailable", result.ErrorMessage);
        Assert.Equivalent(result.ErrorType, ResultErrorType.ServiceUnavailable);
    }

    /// <summary>
    /// Verifies that timeout throws SearchServiceException.
    /// </summary>
    [Fact]
    public async Task SearchSymbolsAsync_ReturnsRequestTimeout_WhenRequestTimesOut()
    {
        // Arrange
        var request = new SymbolSearchQuery {Query = "AAPL"};

        this.SetupMockHttpClient(_ => throw new TaskCanceledException("Simulated timeout", new TimeoutException()));

        using var service = new SearchService(this._httpClientFactory, this._finnHubOptions, this._logger);

        // Act
        var result = await service.SearchSymbolsAsync(request);

        // Assert
        Assert.IsType<Result<IReadOnlyList<StockSymbol>>>(result);
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Contains("Request timed out", result.ErrorMessage);
        Assert.Equivalent(result.ErrorType, ResultErrorType.Timeout);
    }

    /// <summary>
    /// Verifies that invalid JSON response throws SearchServiceException.
    /// </summary>
    [Fact]
    public async Task SearchSymbolsAsync_ReturnsInvalidResponse_WhenResponseIsInvalidResponse()
    {
        // Arrange
        var request = new SymbolSearchQuery {Query = "AAPL"};

        this.SetupMockHttpClient(_ => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("invalid json response")
        }));

        using var service = new SearchService(this._httpClientFactory, this._finnHubOptions, this._logger);

        // Act
        var result = await service.SearchSymbolsAsync(request);

        // Assert
        Assert.IsType<Result<IReadOnlyList<StockSymbol>>>(result);
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Contains("Invalid response from service", result.ErrorMessage);
        Assert.Equivalent(result.ErrorType, ResultErrorType.InvalidResponse);
    }

    /// <summary>
    /// Verifies that empty response returns empty list.
    /// </summary>
    [Fact]
    public async Task SearchSymbolsAsync_ReturnsEmptyList_WhenResponseIsEmpty()
    {
        // Arrange
        var request = new SymbolSearchQuery {Query = "AAPL"};

        this.SetupMockHttpClient(_ => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("")
        }));

        using var service = new SearchService(this._httpClientFactory, this._finnHubOptions, this._logger);

        // Act
        var result = await service.SearchSymbolsAsync(request);

        // Assert
        Assert.IsType<Result<IReadOnlyList<StockSymbol>>>(result);
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Data?.Count);
    }

    /// <summary>
    /// Verifies that API key is included in request headers.
    /// </summary>
    [Fact]
    public async Task SearchSymbolsAsync_IncludesApiKeyInHeaders()
    {
        // Arrange
        var request = new SymbolSearchQuery {Query = "AAPL"};

        HttpRequestHeaders? capturedHeaders = null;

        this.SetupMockHttpClient(requestMessage =>
        {
            capturedHeaders = requestMessage.Headers;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("[]")
            });
        });

        using var service = new SearchService(this._httpClientFactory, this._finnHubOptions, this._logger);

        // Act
        var result = await service.SearchSymbolsAsync(request);

        // Assert
        Assert.IsType<Result<IReadOnlyList<StockSymbol>>>(result);
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(capturedHeaders);
        Assert.True(capturedHeaders.Contains("X-Finnhub-Token"));
        Assert.Contains("test-api-key", capturedHeaders.GetValues("X-Finnhub-Token"));
    }

    /// <summary>
    /// Verifies that query parameters are properly URL encoded.
    /// </summary>
    [Fact]
    public async Task SearchSymbolsAsync_ContainsQueryParameters()
    {
        // Arrange
        var request = new SymbolSearchQuery
        {
            Query = "TEST & SYMBOLS",
            Exchange = "NASDAQ+NYSE"
        };

        Uri? capturedUri = null;

        this.SetupMockHttpClient(requestMessage =>
        {
            capturedUri = requestMessage.RequestUri;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("[]")
            });
        });

        using var service = new SearchService(this._httpClientFactory, this._finnHubOptions, this._logger);

        // Act
        var result = await service.SearchSymbolsAsync(request);

        // Assert
        Assert.IsType<Result<IReadOnlyList<StockSymbol>>>(result);
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        var requestQuery = capturedUri?.Query;
        Assert.Contains("TEST%20%26%20SYMBOLS", requestQuery);
        Assert.Contains("NASDAQ%2BNYSE", requestQuery);
    }

    public void Dispose()
    {
        this._httpClient?.Dispose();
    }
}
