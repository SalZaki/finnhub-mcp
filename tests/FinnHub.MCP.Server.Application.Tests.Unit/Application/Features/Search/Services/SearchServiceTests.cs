// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Net;
using FinnHub.MCP.Server.Application.Search.Clients;
using FinnHub.MCP.Server.Application.Search.Features.SearchSymbol;
using FinnHub.MCP.Server.Application.Search.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace FinnHub.MCP.Server.Application.Tests.Unit.Application.Features.Search.Services;

/// <summary>
/// Unit tests for the <see cref="SearchService"/> class, which integrates with the FinnHub API.
/// </summary>
public sealed class SearchServiceTests
{
    private readonly ISearchClient _searchClient;
    private readonly ILogger<SearchService> _logger;
    private readonly SearchService _service;

    /// <summary>
    /// Initializes test dependencies and mocks for <see cref="SearchService"/>.
    /// </summary>
    public SearchServiceTests()
    {
        this._searchClient = Substitute.For<ISearchClient>();
        this._logger = Substitute.For<ILogger<SearchService>>();
        this._service = new SearchService(this._searchClient, this._logger);
    }

    /// <summary>
    /// Verifies that <see cref="SearchService.SearchSymbolAsync"/> returns expected stock symbols when the FinnHub API
    /// call is successful.
    /// </summary>
    [Fact]
    public async Task SearchSymbolsAsync_ReturnsStockSymbols_WhenApiCallIsSuccessful()
    {
        // Arrange
        var query = new SearchSymbolQuery { QueryId = "test", Query = "AAPL" };
        var expectedSymbols = new[]
        {
            new StockSymbol
            {
                Description = "Apple Inc",
                DisplaySymbol = "AAPL",
                Symbol = "AAPL",
                Type = "Common Stock"
            }
        };

        var searchSymbolResponse = new SearchSymbolResponse
        {
            Symbols = expectedSymbols,
            Query = query.Query,
            Source = "FinnHub"
        };

        this._searchClient
            .SearchSymbolAsync(Arg.Any<SearchSymbolQuery>(), Arg.Any<CancellationToken>())
            .Returns(searchSymbolResponse);

        // Act
        var actualResult = await this._service.SearchSymbolAsync(query, CancellationToken.None);

        // Assert
        Assert.NotNull(actualResult);
        Assert.True(actualResult.IsSuccess);
        Assert.NotNull(actualResult.Data);

        // Verify response structure
        Assert.Equal(1, actualResult.Data.TotalCount);
        Assert.True(actualResult.Data.HasResults);
        Assert.Equal("AAPL", actualResult.Data.Query);
        Assert.True(actualResult.Data.SearchDuration >= TimeSpan.Zero);
        Assert.True(actualResult.Data.SearchTimestamp <= DateTime.UtcNow);

        // Verify symbols data
        Assert.Single(actualResult.Data.Symbols);
        var symbol = actualResult.Data.Symbols[0];
        Assert.Equal("AAPL", symbol.Symbol);
        Assert.Equal("Apple Inc", symbol.Description);
        Assert.Equal("AAPL", symbol.DisplaySymbol);
        Assert.Equal("Common Stock", symbol.Type);

        // Verify metadata
        Assert.Equal("FinnHub", actualResult.Data.Source);

        // Verify interactions
        await this._searchClient.Received(1).SearchSymbolAsync(query, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Verifies that multiple symbols are returned correctly.
    /// </summary>
    [Fact]
    public async Task SearchSymbolsAsync_ReturnsMultipleSymbols_WhenMultipleResultsExist()
    {
        // Arrange
        var query = new SearchSymbolQuery { QueryId = "test", Query = "APP" };

        var expectedSymbols = new[]
        {
            new StockSymbol {Symbol = "AAPL", Description = "Apple Inc", DisplaySymbol = "AAPL", Type = "Common Stock"},
            new StockSymbol {Symbol = "APPN", Description = "Appian Corporation", DisplaySymbol = "APPN", Type = "Common Stock"}
        };

        var searchSymbolResponse = new SearchSymbolResponse { Symbols = expectedSymbols };
        this._searchClient
            .SearchSymbolAsync(Arg.Any<SearchSymbolQuery>(), Arg.Any<CancellationToken>())
            .Returns(searchSymbolResponse);

        // Act
        var result = await this._service.SearchSymbolAsync(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.TotalCount);
        Assert.Equal(2, result.Data.Symbols.Count);
        Assert.True(result.Data.HasResults);
    }

    /// <summary>
    /// Verifies that null request parameter throws ArgumentNullException.
    /// </summary>
    [Fact]
    public async Task SearchSymbolsAsync_ThrowsArgumentNullException_WhenRequestIsNull()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            this._service.SearchSymbolAsync(null!, CancellationToken.None));
    }

    /// <summary>
    /// Verifies that empty query returns appropriate failure result.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task SearchSymbolsAsync_ReturnsNotFoundResult_WhenQueryIsEmptyOrNull(string query)
    {
        // Arrange
        var searchSymbolQuery = new SearchSymbolQuery { QueryId = "test", Query = query };
        var searchSymbolResponse = new SearchSymbolResponse { Symbols = [] };

        this._searchClient
            .SearchSymbolAsync(Arg.Any<SearchSymbolQuery>(), Arg.Any<CancellationToken>())
            .Returns(searchSymbolResponse);

        // Act
        var result = await this._service.SearchSymbolAsync(searchSymbolQuery, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Null(result.Data);
        Assert.Equal("NotFound", result.ErrorType);
        Assert.Contains("No search symbol(s) found.", result.ErrorMessage);

        await this._searchClient.Received(1).SearchSymbolAsync(searchSymbolQuery, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Verifies that service handles client exceptions appropriately.
    /// </summary>
    [Fact]
    public async Task SearchSymbolsAsync_ReturnsFailureResult_WhenClientThrowsHttpRequestException()
    {
        // Arrange
        var query = new SearchSymbolQuery { QueryId = "test", Query = "AAPL" };
        this._searchClient
            .SearchSymbolAsync(Arg.Any<SearchSymbolQuery>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new SearchSymbolHttpException("Service temporarily unavailable", HttpStatusCode.ServiceUnavailable));

        // Act
        var result = await this._service.SearchSymbolAsync(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("ServiceUnavailable", result.ErrorType);
        Assert.Contains("Service temporarily unavailable", result.ErrorMessage);
    }

    /// <summary>
    /// Verifies that service handles timeout exceptions appropriately.
    /// </summary>
    [Fact]
    public async Task SearchSymbolsAsync_ReturnsTimeoutResult_WhenClientThrowsTimeoutException()
    {
        // Arrange
        var query = new SearchSymbolQuery { QueryId = "test", Query = "AAPL" };
        var timeoutException = new SearchSymbolTimeoutException("Request timed out", new TimeoutException());

        this._searchClient
            .SearchSymbolAsync(Arg.Any<SearchSymbolQuery>(), Arg.Any<CancellationToken>())
            .Throws(timeoutException);

        // Act
        var result = await this._service.SearchSymbolAsync(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Timeout", result.ErrorType);
        Assert.Contains("Request timed out", result.ErrorMessage);
    }

    /// <summary>
    /// Verifies that the service properly handles cancellation tokens.
    /// </summary>
    [Fact]
    public async Task SearchSymbolsAsync_PropagatesCancellationToken_WhenCancellationRequested()
    {
        // Arrange
        var query = new SearchSymbolQuery { QueryId = "test", Query = "AAPL" };
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        this._searchClient
            .SearchSymbolAsync(Arg.Any<SearchSymbolQuery>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(callInfo => throw new OperationCanceledException(callInfo.ArgAt<CancellationToken>(1)));

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            this._service.SearchSymbolAsync(query, cts.Token));
    }

    /// <summary>
    /// Verifies that service response includes proper timing information.
    /// </summary>
    [Fact]
    public async Task SearchSymbolsAsync_IncludesTimingInformation_InResponse()
    {
        // Arrange
        var query = new SearchSymbolQuery { QueryId = "test", Query = "AAPL" };

        var startTime = DateTime.UtcNow;

        var searchSymbolResponse = new SearchSymbolResponse
        {
            SearchDuration = TimeSpan.FromSeconds(3),
            SearchTimestamp = DateTime.UtcNow,
            Symbols =
            [
                new StockSymbol {Symbol = "AAPL", Description = "Apple Inc", DisplaySymbol = "AAPL", Type = "Common Stock"}
            ]
        };

        this._searchClient
            .SearchSymbolAsync(Arg.Any<SearchSymbolQuery>(), Arg.Any<CancellationToken>())
            .Returns(async callInfo =>
            {
                // Simulate some processing time
                await Task.Delay(10, callInfo.ArgAt<CancellationToken>(1));
                return searchSymbolResponse;
            });

        // Act
        var result = await this._service.SearchSymbolAsync(query, CancellationToken.None);

        var endTime = DateTime.UtcNow;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.True(result.Data.SearchDuration > TimeSpan.Zero);
        Assert.True(result.Data.SearchTimestamp >= startTime);
        Assert.True(result.Data.SearchTimestamp <= endTime);
    }
}
