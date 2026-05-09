// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Diagnostics;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Search.Features.SearchSymbol;
using FinnHub.MCP.Server.Application.Search.Services;
using FinnHub.MCP.Server.Common;

namespace FinnHub.MCP.Server.Tools.Search;

/// <summary>
/// MCP tool for searching financial symbols via the FinnHub search service.
/// </summary>
[McpServerToolType]
public sealed class SearchSymbolTool(
    ISearchService searchService,
    ILogger<SearchSymbolTool> logger)
{
    /// <summary>
    /// Executes a financial-symbol search against the FinnHub provider and returns
    /// the matching instruments wrapped in an application-level <see cref="Result{T}"/>.
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
    /// <param name="query">
    /// The search term — typically a ticker, company name, ISIN, or CUSIP. Must be
    /// non-empty after trimming and contain only letters, digits, spaces, dashes,
    /// underscores, or periods (max 500 chars).
    /// </param>
    /// <param name="exchange">
    /// Optional uppercase exchange code (e.g. <c>"US"</c>, <c>"L"</c>). When supplied,
    /// results are restricted to that venue. Must match <c>[A-Z0-9\-_]{1,50}</c>.
    /// </param>
    /// <param name="limit">
    /// Optional cap on the number of results to return. Defaults to <c>10</c>; valid
    /// range is <c>1..100</c> inclusive.
    /// </param>
    /// <param name="cancellationToken">
    /// Token used to cancel the in-flight search. Cancellation is propagated to the
    /// HTTP layer and re-thrown to the caller.
    /// </param>
    /// <returns>
    /// A <see cref="Result{T}"/> wrapping the matching <see cref="SearchSymbolResponse"/>.
    /// On provider-level failures (timeout, transport, deserialization) the result
    /// reports an error category instead of throwing.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="query"/> or <paramref name="exchange"/> fails
    /// validation.
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
    public async Task<Result<SearchSymbolResponse>> SearchSymbolAsync(
        [Description(Constants.Tools.SearchSymbols.Parameters.QueryDescription)]
        string query,
        [Description(Constants.Tools.SearchSymbols.Parameters.ExchangeDescription)]
        string? exchange = null,
        [Description(Constants.Tools.SearchSymbols.Parameters.LimitDescription)]
        int? limit = null,
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

            logger.LogDebug(
                "Executing search with query: '{Query}', exchange: '{Exchange}', limit: {Limit}",
                validatedQuery, validatedExchange, validatedLimit);

            var symbolSearchQuery = new SearchSymbolQueryBuilder()
                .WithQuery(validatedQuery)
                .WithExchange(validatedExchange)
                .WithLimit(validatedLimit)
                .Build();

            var results = await searchService.SearchSymbolAsync(symbolSearchQuery, cancellationToken);

            logger.LogInformation(
                "Search completed successfully. Found {Count} results in {ElapsedMs}ms",
                results.Data?.TotalCount, stopwatch.ElapsedMilliseconds);

            return results;
        }
        catch (OperationCanceledException ex)
        {
            logger.LogError(ex, "Search operation was canceled for '{Tool}'.", toolName);
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
            stopwatch.Stop();
            logger.LogTrace("Finished executing '{Tool}' in {ElapsedMs}ms.", toolName, stopwatch.ElapsedMilliseconds);
        }
    }
}
