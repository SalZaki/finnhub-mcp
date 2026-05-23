// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using FinnHub.MCP.Server.Application.Financials.Features.GetFinancialsSnapshot;
using FinnHub.MCP.Server.Application.Financials.Services;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Common;

namespace FinnHub.MCP.Server.Tools.Financials;

/// <summary>
/// MCP tool exposing the Finnhub <c>/stock/metric</c> endpoint as a curated 10-KPI snapshot.
/// </summary>
[McpServerToolType]
public sealed class GetFinancialsSnapshotTool(
    IFinancialsService financialsService,
    ILogger<GetFinancialsSnapshotTool> logger)
{
    /// <summary>
    /// Returns a curated 10-KPI snapshot for a symbol. <c>view = "full"</c> additionally
    /// includes the raw upstream metric dictionary on the <c>raw</c> field.
    /// </summary>
    [McpServerTool(
        Name = Constants.Tools.FinancialsSnapshot.Name,
        Title = Constants.Tools.FinancialsSnapshot.Title,
        ReadOnly = true,
        Idempotent = true,
        Destructive = false,
        OpenWorld = true)]
    [Description(Constants.Tools.FinancialsSnapshot.Description)]
    public async Task<ToolResponseEnvelope<GetFinancialsSnapshotResponse>> GetFinancialsSnapshotAsync(
        [Description(Constants.Tools.FinancialsSnapshot.Parameters.SymbolDescription)]
        string symbol,
        [Description(Constants.Tools.FinancialsSnapshot.Parameters.ViewDescription)]
        string? view = null,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        const string ToolName = Constants.Tools.FinancialsSnapshot.Name;

        try
        {
            logger.LogTrace("Starting execution of '{Tool}'.", ToolName);

            var validatedSymbol = FinancialsInputValidator.ValidateSymbol(symbol);
            var validatedView = FinancialsInputValidator.ValidateView(view);

            var query = new GetFinancialsSnapshotQuery
            {
                QueryId = Guid.NewGuid().ToString("N"),
                Symbol = validatedSymbol,
                IncludeRaw = validatedView == ToolView.Full
            };

            var result = await financialsService.GetSnapshotAsync(query, cancellationToken);

            logger.LogInformation(
                "Financials snapshot completed for {Symbol} in {ElapsedMs}ms",
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

    private static IReadOnlyList<NextAction> BuildNextActions(Result<GetFinancialsSnapshotResponse> result, string symbol)
    {
        if (!result.IsSuccess || result.Data is null)
        {
            return [];
        }

        var args = new Dictionary<string, string>(StringComparer.Ordinal) { ["symbol"] = symbol };

        return
        [
            new NextAction("get-peers", args, "compare this symbol's valuation to industry peers"),
            new NextAction("get-price-summary", args, "check recent price action against these fundamentals")
        ];
    }

    private static string BuildExplanation(Result<GetFinancialsSnapshotResponse> result, string symbol)
    {
        if (!result.IsSuccess || result.Data is null)
        {
            return $"No financials snapshot available for '{symbol}'.";
        }

        return string.Create(
            CultureInfo.InvariantCulture,
            $"Financials snapshot for '{symbol}' (10 curated KPIs).");
    }
}
