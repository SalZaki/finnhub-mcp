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
using FinnHub.MCP.Server.Application.News.Features.GetNewsPulse;
using FinnHub.MCP.Server.Application.News.Services;
using FinnHub.MCP.Server.Common;

namespace FinnHub.MCP.Server.Tools.News;

/// <summary>
/// MCP tool exposing an aggregated news pulse for a symbol — sentiment (when
/// available), top headlines, and a delta vs the prior 7 days.
/// </summary>
[McpServerToolType]
public sealed class GetNewsPulseTool(
    INewsService newsService,
    ILogger<GetNewsPulseTool> logger)
{
    /// <summary>
    /// Returns the news pulse for a symbol — sentiment score, top headlines, and
    /// article-count delta vs the prior 7-day window. <c>view = "full"</c>
    /// returns the complete headline list rather than the top 5.
    /// </summary>
    [McpServerTool(
        Name = Constants.Tools.NewsPulse.Name,
        Title = Constants.Tools.NewsPulse.Title,
        ReadOnly = true,
        Idempotent = true,
        Destructive = false,
        OpenWorld = true)]
    [Description(Constants.Tools.NewsPulse.Description)]
    public async Task<ToolResponseEnvelope<GetNewsPulseResponse>> GetNewsPulseAsync(
        [Description(Constants.Tools.NewsPulse.Parameters.SymbolDescription)]
        string symbol,
        [Description(Constants.Tools.NewsPulse.Parameters.ViewDescription)]
        string? view = null,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        const string ToolName = Constants.Tools.NewsPulse.Name;

        try
        {
            logger.LogTrace("Starting execution of '{Tool}'.", ToolName);

            var validatedSymbol = NewsInputValidator.ValidateSymbol(symbol);
            var validatedView = NewsInputValidator.ValidateView(view);

            var query = new GetNewsPulseQuery
            {
                QueryId = Guid.NewGuid().ToString("N"),
                Symbol = validatedSymbol,
                IncludeAllHeadlines = validatedView == ToolView.Full
            };

            var result = await newsService.GetPulseAsync(query, cancellationToken);

            logger.LogInformation(
                "News pulse completed for {Symbol} in {ElapsedMs}ms",
                validatedSymbol, stopwatch.ElapsedMilliseconds);

            return EnvelopeFactory.FromResult(
                result,
                validatedView,
                nextActions: BuildNextActions(result, validatedSymbol),
                explanation: BuildExplanation(result, validatedSymbol),
                sentimentSource: result.Data?.SentimentSource);
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

    private static IReadOnlyList<NextAction> BuildNextActions(Result<GetNewsPulseResponse> result, string symbol)
    {
        if (!result.IsSuccess || result.Data is null || result.Data.Count == 0)
        {
            return [];
        }

        var args = new Dictionary<string, string>(StringComparer.Ordinal) { ["symbol"] = symbol };

        return
        [
            new NextAction("get-price-summary", args, "correlate sentiment with recent price action"),
            new NextAction("get-financials-snapshot", args, "check fundamentals against the news flow")
        ];
    }

    private static string BuildExplanation(Result<GetNewsPulseResponse> result, string symbol)
    {
        if (!result.IsSuccess || result.Data is null)
        {
            return $"No news pulse available for '{symbol}'.";
        }

        var deltaWord = result.Data.DeltaVsPrevWeek switch
        {
            > 0 => $"+{result.Data.DeltaVsPrevWeek}",
            _ => result.Data.DeltaVsPrevWeek.ToString(CultureInfo.InvariantCulture)
        };

        return string.Create(
            CultureInfo.InvariantCulture,
            $"News pulse for '{symbol}': {result.Data.Count} articles in past 7d ({deltaWord} vs prior week).");
    }
}
