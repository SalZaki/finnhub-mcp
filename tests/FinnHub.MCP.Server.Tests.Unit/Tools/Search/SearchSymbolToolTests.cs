// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Search.Features.SearchSymbol;
using FinnHub.MCP.Server.Application.Search.Services;
using FinnHub.MCP.Server.Application.Symbols;
using FinnHub.MCP.Server.Tools.Search;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace FinnHub.MCP.Server.Tests.Unit.Tools.Search;

public sealed class SearchSymbolToolTests
{
    private readonly ISearchService _service = Substitute.For<ISearchService>();
    private readonly ISymbolResolver _resolver = Substitute.For<ISymbolResolver>();
    private readonly ILogger<SearchSymbolTool> _logger = Substitute.For<ILogger<SearchSymbolTool>>();

    [Fact]
    public async Task SearchSymbolAsync_WithoutView_DefaultsToSummary()
    {
        this.SetupSuccess([Stock("AAPL", "Apple Inc.", confidence: 0.5)]);
        this.SetupResolver("AAPL", 0.5);
        var tool = new SearchSymbolTool(this._service, this._resolver, this._logger);

        var envelope = await tool.SearchSymbolAsync("apple");

        Assert.Equal(ToolView.Summary, envelope.View);
    }

    [Fact]
    public async Task SearchSymbolAsync_ExplicitStandardView_Honored()
    {
        this.SetupSuccess([Stock("AAPL", "Apple Inc.", confidence: 0.5)]);
        this.SetupResolver("AAPL", 0.5);
        var tool = new SearchSymbolTool(this._service, this._resolver, this._logger);

        var envelope = await tool.SearchSymbolAsync("apple", view: "standard");

        Assert.Equal(ToolView.Standard, envelope.View);
    }

    [Fact]
    public async Task SearchSymbolAsync_InvalidView_Throws()
    {
        var tool = new SearchSymbolTool(this._service, this._resolver, this._logger);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            tool.SearchSymbolAsync("apple", view: "verbose"));
    }

    [Fact]
    public async Task SearchSymbolAsync_UnknownField_Throws()
    {
        var tool = new SearchSymbolTool(this._service, this._resolver, this._logger);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            tool.SearchSymbolAsync("apple", fields: ["bogus"]));
    }

    [Fact]
    public async Task SearchSymbolAsync_QueryEqualsSymbol_PopulatesNextActions()
    {
        this.SetupSuccess([Stock("AAPL", "Apple Inc.", confidence: 0.5)]);
        this.SetupResolver("AAPL", 1.0);
        var tool = new SearchSymbolTool(this._service, this._resolver, this._logger);

        var envelope = await tool.SearchSymbolAsync("AAPL");

        Assert.Equal(3, envelope.NextActions.Count);
        Assert.Equal("get-company-profile", envelope.NextActions[0].Tool);
        Assert.Equal("get-price-summary", envelope.NextActions[1].Tool);
        Assert.Equal("get-news-pulse", envelope.NextActions[2].Tool);
        Assert.All(envelope.NextActions, a => Assert.Equal("AAPL", a.Args["symbol"]));
    }

    [Fact]
    public async Task SearchSymbolAsync_HighConfidence_PopulatesNextActions()
    {
        this.SetupSuccess([Stock("MSFT", "Microsoft Corp.", confidence: 0.97)]);
        this.SetupResolver("MSFT", 0.97);
        var tool = new SearchSymbolTool(this._service, this._resolver, this._logger);

        var envelope = await tool.SearchSymbolAsync("microsoft");

        Assert.Equal(3, envelope.NextActions.Count);
        Assert.All(envelope.NextActions, a => Assert.Equal("MSFT", a.Args["symbol"]));
    }

    [Fact]
    public async Task SearchSymbolAsync_NoExactMatchAndLowConfidence_NextActionsEmpty()
    {
        this.SetupSuccess([Stock("AAPL", "Apple Inc.", confidence: 0.4)]);
        this.SetupResolver("AAPL", 0.4);
        var tool = new SearchSymbolTool(this._service, this._resolver, this._logger);

        var envelope = await tool.SearchSymbolAsync("appll");

        Assert.Empty(envelope.NextActions);
    }

    [Fact]
    public async Task SearchSymbolAsync_ServiceFailure_WrapsAsFailureEnvelope()
    {
        this._service.SearchSymbolAsync(Arg.Any<SearchSymbolQuery>(), Arg.Any<CancellationToken>())
            .Returns(new Result<SearchSymbolResponse>().Failure("not found", ResultErrorType.NotFound));
        this.SetupResolver("ZZZZ", 1.0);
        var tool = new SearchSymbolTool(this._service, this._resolver, this._logger);

        var envelope = await tool.SearchSymbolAsync("zzzz");

        Assert.False(envelope.IsSuccess);
        Assert.Equal("not found", envelope.ErrorMessage);
        Assert.Equal("NotFound", envelope.ErrorType);
        Assert.Null(envelope.Data);
    }

    [Fact]
    public async Task SearchSymbolAsync_Success_DefaultsSentimentSourceAndPremium()
    {
        this.SetupSuccess([Stock("AAPL", "Apple Inc.", confidence: 0.5)]);
        this.SetupResolver("AAPL", 1.0);
        var tool = new SearchSymbolTool(this._service, this._resolver, this._logger);

        var envelope = await tool.SearchSymbolAsync("AAPL");

        Assert.Null(envelope.SentimentSource);
        Assert.False(envelope.Premium);
    }

    [Fact]
    public async Task SearchSymbolAsync_ResolverFailure_StillReturnsSuccessEnvelopeButEmptyNextActions()
    {
        // Upstream search succeeds; the resolver fails (e.g. its own upstream call timed out,
        // or no canonical could be picked). The tool envelope must still surface the search
        // results, but next_actions stays empty because there's no canonical to key off.
        this.SetupSuccess([Stock("AAPL", "Apple Inc.", confidence: 0.5)]);
        this._resolver.ResolveAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new Result<ResolvedSymbol>().Failure(
                "no canonical pick", ResultErrorType.NotFound));
        var tool = new SearchSymbolTool(this._service, this._resolver, this._logger);

        var envelope = await tool.SearchSymbolAsync("AAPL");

        Assert.True(envelope.IsSuccess);
        Assert.NotNull(envelope.Data);
        Assert.Empty(envelope.NextActions);
    }

    private void SetupSuccess(IReadOnlyList<StockSymbol> symbols) =>
        this._service.SearchSymbolAsync(Arg.Any<SearchSymbolQuery>(), Arg.Any<CancellationToken>())
            .Returns(new Result<SearchSymbolResponse>().Success(
                new SearchSymbolResponse { Symbols = symbols }));

    private void SetupResolver(string canonical, double confidence) =>
        this._resolver.ResolveAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new Result<ResolvedSymbol>().Success(
                new ResolvedSymbol(canonical, canonical, Exchange: null, confidence, Candidates: [])));

    private static StockSymbol Stock(string symbol, string description, double confidence) => new()
    {
        Symbol = symbol,
        DisplaySymbol = symbol,
        Description = description,
        Type = "Common Stock",
        ConfidenceScore = confidence
    };
}
