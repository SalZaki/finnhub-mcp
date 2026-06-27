// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Search.Features.SearchSymbol;
using FinnHub.MCP.Server.Application.Search.Services;
using FinnHub.MCP.Server.Application.Symbols;
using FinnHub.MCP.Server.Common;

namespace FinnHub.MCP.Server.Tools.Search;

/// <summary>
/// MCP tool for searching financial symbols via the FinnHub search service.
/// </summary>
[McpServerToolType]
public sealed class SearchSymbolTool(
    ISearchService searchService,
    ISymbolResolver symbolResolver,
    ILogger<SearchSymbolTool> logger)
{
    /// <summary>
    /// Executes a financial-symbol search against the FinnHub provider and returns
    /// the matching instruments wrapped in the standard tool response envelope.
    /// </summary>
    /// <remarks>
    /// <para>
    /// All inputs are validated and normalized via <see cref="SearchInputValidator"/>
    /// before the underlying service is called. Validation failures throw
    /// <see cref="ArgumentException"/> (or <see cref="ArgumentOutOfRangeException"/>
    /// for the limit), which the MCP runtime surfaces to the caller as a tool error.
    /// </para>
    /// <para>
    /// This is a read-only, idempotent operation: invoking it with the same arguments
    /// yields the same logical result and produces no side effects on the provider.
    /// </para>
    /// </remarks>
    /// <param name="query">The search term — ticker, company name, ISIN, or CUSIP.</param>
    /// <param name="exchange">Optional uppercase exchange code (e.g. <c>"US"</c>).</param>
    /// <param name="limit">Optional cap on the number of results (default 10, max 100).</param>
    /// <param name="view">Response detail level. See <see cref="ToolView"/>.</param>
    /// <param name="fields">Optional sparse field projection.</param>
    /// <param name="cancellationToken">Token used to cancel the in-flight search.</param>
    /// <returns>
    /// A <see cref="ToolResponseEnvelope{T}"/> wrapping the matching
    /// <see cref="SearchSymbolResponse"/>, with <c>next_actions</c> populated when
    /// the top result is an exact match.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="query"/>, <paramref name="exchange"/>,
    /// <paramref name="view"/>, or <paramref name="fields"/> fails validation.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="limit"/> is outside the valid range.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Thrown when the operation is cancelled via <paramref name="cancellationToken"/>.
    /// </exception>
    [McpServerTool(
        Name = Constants.Tools.SearchSymbols.Name,
        Title = Constants.Tools.SearchSymbols.Title,
        ReadOnly = true,
        Idempotent = true,
        Destructive = false,
        OpenWorld = true)]
    [Description(Constants.Tools.SearchSymbols.Description)]
    public async Task<ToolResponseEnvelope<SearchSymbolResponse>> SearchSymbolAsync(
        [Description(Constants.Tools.SearchSymbols.Parameters.QueryDescription)]
        string query,
        [Description(Constants.Tools.SearchSymbols.Parameters.ExchangeDescription)]
        string? exchange = null,
        [Description(Constants.Tools.SearchSymbols.Parameters.LimitDescription)]
        int? limit = null,
        [Description(Constants.Tools.SearchSymbols.Parameters.ViewDescription)]
        string? view = null,
        [Description(Constants.Tools.SearchSymbols.Parameters.FieldsDescription)]
        string[]? fields = null,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var toolName = Constants.Tools.SearchSymbols.Name;

        try
        {
            logger.LogTrace("Starting execution of '{Tool}'.", toolName);

            var validatedQuery = SearchInputValidator.ValidateQuery(query);
            var validatedExchange = SearchInputValidator.ValidateExchange(exchange);
            var validatedLimit = SearchInputValidator.ValidateLimit(limit);
            var validatedView = CommonInputValidators.ValidateView(view);
            _ = SearchInputValidator.ValidateFields(fields);

            var symbolSearchQuery = new SearchSymbolQueryBuilder()
                .WithQuery(validatedQuery)
                .WithExchange(validatedExchange)
                .WithLimit(validatedLimit)
                .Build();

            var result = await searchService.SearchSymbolAsync(symbolSearchQuery, cancellationToken);

            logger.LogInformation(
                "Search completed successfully. Found {Count} results in {ElapsedMs}ms",
                result.Data?.TotalCount, stopwatch.ElapsedMilliseconds);

            var resolved = await symbolResolver.ResolveAsync(validatedQuery, cancellationToken);

            return EnvelopeFactory.FromResult(
                result,
                validatedView,
                nextActions: BuildNextActions(result, resolved),
                explanation: BuildExplanation(result, validatedQuery));
        }
        catch (OperationCanceledException)
        {
            logger.LogDebug("'{Tool}' was cancelled.", toolName);
            throw;
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "Validation error in '{Tool}': {Message}", toolName, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred running '{Tool}'.", toolName);
            throw;
        }
        finally
        {
            logger.LogTrace("Finished executing '{Tool}' in {ElapsedMs}ms.", toolName, stopwatch.ElapsedMilliseconds);
        }
    }

    /// <summary>
    /// Builds server-suggested follow-up tool calls when the resolver has identified a
    /// high-confidence canonical (Confidence ≥ 0.95) for the user's query. Points at the
    /// P6 Wave A research workflow: pulse, fundamentals, recent price action, peers.
    /// </summary>
    /// <remarks>
    /// The resolver's three fast-paths emit Confidence = 1.0, so structurally complete
    /// inputs (<c>AAPL</c>, <c>AAPL.US</c>, <c>NASDAQ:AAPL</c>) always populate
    /// <c>next_actions</c>. Ambiguous inputs (<c>apple</c>) populate only when Finnhub's
    /// ConfidenceScore on the top match clears the threshold. The <c>args.symbol</c>
    /// value is always the resolver's <see cref="ResolvedSymbol.Canonical"/> — never
    /// the raw user input — so downstream tools receive a normalised ticker.
    /// </remarks>
    // Emit rule: IsSuccess AND (singleton data OR non-empty collection) -- see NextAction.
    private static IReadOnlyList<NextAction> BuildNextActions(
        Result<SearchSymbolResponse> result,
        Result<ResolvedSymbol> resolved)
    {
        if (!result.IsSuccess || result.Data is null)
        {
            return [];
        }

        if (!resolved.IsSuccess || resolved.Data is null || resolved.Data.Confidence < 0.95d)
        {
            return [];
        }

        var args = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["symbol"] = resolved.Data.Canonical
        };

        return
        [
            new NextAction("get-quote", args, "real-time price snapshot"),
            new NextAction("get-company-profile", args, "company name, industry, country, market cap"),
            new NextAction("get-news-pulse", args, "sentiment and top headlines from the past week"),
            new NextAction("get-financials-snapshot", args, "10 curated valuation/profitability KPIs"),
            new NextAction("get-price-summary", args, "min/max/mean/return/volatility over the last 30 days"),
            new NextAction("get-peers", args, "industry peer list for comparison")
        ];
    }

    private static string BuildExplanation(Result<SearchSymbolResponse> result, string query)
    {
        if (!result.IsSuccess || result.Data is null)
        {
            return $"No matches for '{query}'.";
        }

        return string.Create(
            CultureInfo.InvariantCulture,
            $"Found {result.Data.TotalCount} match(es) for '{query}'.");
    }
}
