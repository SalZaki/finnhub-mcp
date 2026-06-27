// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using FinnHub.MCP.Server.Application.Exchanges.Features.GetExchangeSymbols;
using FinnHub.MCP.Server.Application.Exchanges.Services;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Common;

namespace FinnHub.MCP.Server.Tools.Exchanges;

/// <summary>
/// MCP tool exposing the Finnhub <c>/stock/symbol</c> endpoint as an aggregated, token-conscious
/// summary of the symbols listed on an exchange.
/// </summary>
[McpServerToolType]
public sealed class GetExchangeSymbolsTool(
    IExchangeSymbolsService exchangeSymbolsService,
    ILogger<GetExchangeSymbolsTool> logger)
{
    private const int StandardSampleLimit = 25;
    private const int FullSampleLimit = 100;

    /// <summary>
    /// Returns the aggregated symbol listing for <paramref name="exchange"/> — total count, a
    /// breakdown by security type, and a capped sample of symbols.
    /// </summary>
    [McpServerTool(
        Name = Constants.Tools.ExchangeSymbols.Name,
        Title = Constants.Tools.ExchangeSymbols.Title,
        ReadOnly = true,
        Idempotent = true,
        Destructive = false,
        OpenWorld = true)]
    [Description(Constants.Tools.ExchangeSymbols.Description)]
    public async Task<ToolResponseEnvelope<GetExchangeSymbolsResponse>> GetExchangeSymbolsAsync(
        [Description(Constants.Tools.ExchangeSymbols.Parameters.ExchangeDescription)]
        string exchange,
        [Description(Constants.Tools.ExchangeSymbols.Parameters.ViewDescription)]
        string? view = null,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        const string ToolName = Constants.Tools.ExchangeSymbols.Name;

        try
        {
            logger.LogTrace("Starting execution of '{Tool}'.", ToolName);

            var validatedExchange = ExchangeSymbolsInputValidator.ValidateExchange(exchange);
            var validatedView = CommonInputValidators.ValidateView(view);

            var query = new GetExchangeSymbolsQuery
            {
                QueryId = Guid.NewGuid().ToString("N"),
                Exchange = validatedExchange
            };

            var result = await exchangeSymbolsService.GetExchangeSymbolsAsync(query, cancellationToken);

            // Project once, then derive explanation/next_actions from the projected result so the
            // sample shown and the metadata can't disagree (same discipline as get-peers).
            var projected = ProjectForView(result, validatedView);

            logger.LogInformation(
                "Exchange symbols completed for {Exchange} in {ElapsedMs}ms: {Count} symbol(s)",
                validatedExchange, stopwatch.ElapsedMilliseconds, projected.Data?.TotalCount ?? 0);

            return EnvelopeFactory.FromResult(
                projected,
                validatedView,
                nextActions: BuildNextActions(projected, validatedExchange),
                explanation: BuildExplanation(projected, validatedExchange));
        }
        catch (OperationCanceledException)
        {
            logger.LogDebug("'{Tool}' was cancelled.", ToolName);
            throw;
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "Validation error in '{Tool}': {Message}", ToolName, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred running '{Tool}'.", ToolName);
            throw;
        }
        finally
        {
            logger.LogTrace("Finished '{Tool}' in {ElapsedMs}ms.", ToolName, stopwatch.ElapsedMilliseconds);
        }
    }

    private static Result<GetExchangeSymbolsResponse> ProjectForView(Result<GetExchangeSymbolsResponse> source, ToolView view)
    {
        if (!source.IsSuccess || source.Data is null)
        {
            return source;
        }

        var data = source.Data;

        // summary drops the sample entirely (count + type_breakdown is enough to act on);
        // standard keeps 25 rows; full keeps the full cached sample (up to 100).
        var symbols = view switch
        {
            ToolView.Summary => null,
            ToolView.Standard => Cap(data.Symbols, StandardSampleLimit),
            _ => Cap(data.Symbols, FullSampleLimit)
        };

        return Result<GetExchangeSymbolsResponse>.Success(new GetExchangeSymbolsResponse
        {
            Exchange = data.Exchange,
            TotalCount = data.TotalCount,
            TypeBreakdown = data.TypeBreakdown,
            Symbols = symbols
        });
    }

    private static IReadOnlyList<ExchangeSymbol>? Cap(IReadOnlyList<ExchangeSymbol>? symbols, int cap)
    {
        if (symbols is null)
        {
            return null;
        }

        return symbols.Count <= cap ? symbols : symbols.Take(cap).ToList().AsReadOnly();
    }

    // Emit rule: IsSuccess AND (singleton data OR non-empty collection) -- see NextAction.
    private static IReadOnlyList<NextAction> BuildNextActions(Result<GetExchangeSymbolsResponse> result, string exchange)
    {
        if (!result.IsSuccess || result.Data is null || result.Data.TotalCount == 0)
        {
            return [];
        }

        var args = new Dictionary<string, string>(StringComparer.Ordinal) { ["exchange"] = exchange };

        return
        [
            new NextAction("search-symbol", args, "find a specific symbol on this exchange by name or ticker")
        ];
    }

    private static string BuildExplanation(Result<GetExchangeSymbolsResponse> result, string exchange)
    {
        if (!result.IsSuccess || result.Data is null)
        {
            return $"No symbols found for exchange '{exchange}'.";
        }

        return string.Create(
            CultureInfo.InvariantCulture,
            $"Exchange '{exchange}' lists {result.Data.TotalCount} symbol(s) across {result.Data.TypeBreakdown.Count} type(s).");
    }
}
