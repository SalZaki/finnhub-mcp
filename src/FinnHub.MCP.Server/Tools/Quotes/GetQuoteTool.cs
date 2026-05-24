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
using FinnHub.MCP.Server.Application.Quotes.Features.GetQuote;
using FinnHub.MCP.Server.Application.Quotes.Services;
using FinnHub.MCP.Server.Common;

namespace FinnHub.MCP.Server.Tools.Quotes;

/// <summary>
/// MCP tool exposing the Finnhub <c>/quote</c> endpoint as a real-time price snapshot.
/// </summary>
[McpServerToolType]
public sealed class GetQuoteTool(
    IQuotesService quotesService,
    ILogger<GetQuoteTool> logger)
{
    /// <summary>
    /// Returns the real-time quote for a symbol — current, change, percent change,
    /// session high/low/open, previous close, and snapshot timestamp.
    /// </summary>
    [McpServerTool(
        Name = Constants.Tools.Quote.Name,
        Title = Constants.Tools.Quote.Title,
        ReadOnly = true,
        Idempotent = true,
        Destructive = false,
        OpenWorld = true)]
    [Description(Constants.Tools.Quote.Description)]
    public async Task<ToolResponseEnvelope<GetQuoteResponse>> GetQuoteAsync(
        [Description(Constants.Tools.Quote.Parameters.SymbolDescription)]
        string symbol,
        [Description(Constants.Tools.Quote.Parameters.ViewDescription)]
        string? view = null,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        const string ToolName = Constants.Tools.Quote.Name;

        try
        {
            logger.LogTrace("Starting execution of '{Tool}'.", ToolName);

            var validatedSymbol = QuotesInputValidator.ValidateSymbol(symbol);
            var validatedView = QuotesInputValidator.ValidateView(view);

            var query = new GetQuoteQuery
            {
                QueryId = Guid.NewGuid().ToString("N"),
                Symbol = validatedSymbol
            };

            var result = await quotesService.GetQuoteAsync(query, cancellationToken);

            logger.LogInformation(
                "Quote completed for {Symbol} in {ElapsedMs}ms",
                validatedSymbol, stopwatch.ElapsedMilliseconds);

            return EnvelopeFactory.FromResult(
                result,
                validatedView,
                nextActions: BuildNextActions(result, validatedSymbol),
                explanation: BuildExplanation(result, validatedSymbol));
        }
        catch (OperationCanceledException ex)
        {
            logger.LogError(ex, "'{Tool}' was cancelled.", ToolName);
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
            stopwatch.Stop();
            logger.LogTrace("Finished '{Tool}' in {ElapsedMs}ms.", ToolName, stopwatch.ElapsedMilliseconds);
        }
    }

    private static IReadOnlyList<NextAction> BuildNextActions(Result<GetQuoteResponse> result, string symbol)
    {
        if (!result.IsSuccess || result.Data is null)
        {
            return [];
        }

        var args = new Dictionary<string, string>(StringComparer.Ordinal) { ["symbol"] = symbol };

        return
        [
            new NextAction("get-news-pulse", args, "find news that may explain the current price"),
            new NextAction("get-price-summary", args, "longer-term context vs the current quote")
        ];
    }

    private static string BuildExplanation(Result<GetQuoteResponse> result, string symbol)
    {
        if (!result.IsSuccess || result.Data is null)
        {
            return $"No quote available for '{symbol}'.";
        }

        var current = result.Data.Current is { } c ? c.ToString("F2", CultureInfo.InvariantCulture) : "n/a";
        var pct = result.Data.PercentChange is { } p
            ? p.ToString("+0.00;-0.00", CultureInfo.InvariantCulture) + "%"
            : "n/a";
        var ts = result.Data.TimestampUtc is { } t ? t.ToString("u", CultureInfo.InvariantCulture) : "unknown";

        return string.Create(CultureInfo.InvariantCulture, $"{symbol} {current} ({pct}) as of {ts}.");
    }
}
