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
using FinnHub.MCP.Server.Application.Prices.Features.GetPriceSummary;
using FinnHub.MCP.Server.Application.Prices.Services;
using FinnHub.MCP.Server.Common;

namespace FinnHub.MCP.Server.Tools.Prices;

/// <summary>
/// MCP tool exposing the Finnhub <c>/stock/candle</c> endpoint as a curated
/// price summary (min/max/mean/return/vol/latest) rather than a raw OHLCV dump.
/// </summary>
[McpServerToolType]
public sealed class GetPriceSummaryTool(
    IPricesService pricesService,
    ILogger<GetPriceSummaryTool> logger)
{
    /// <summary>
    /// Returns aggregated price stats for a symbol over the requested period.
    /// <c>view = "full"</c> additionally includes the raw OHLCV arrays.
    /// </summary>
    [McpServerTool(
        Name = Constants.Tools.PriceSummary.Name,
        Title = Constants.Tools.PriceSummary.Title,
        ReadOnly = true,
        Idempotent = true,
        Destructive = false,
        OpenWorld = true)]
    [Description(Constants.Tools.PriceSummary.Description)]
    public async Task<ToolResponseEnvelope<GetPriceSummaryResponse>> GetPriceSummaryAsync(
        [Description(Constants.Tools.PriceSummary.Parameters.SymbolDescription)]
        string symbol,
        [Description(Constants.Tools.PriceSummary.Parameters.PeriodDescription)]
        string? period = null,
        [Description(Constants.Tools.PriceSummary.Parameters.ViewDescription)]
        string? view = null,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        const string ToolName = Constants.Tools.PriceSummary.Name;

        try
        {
            logger.LogTrace("Starting execution of '{Tool}'.", ToolName);

            var validatedSymbol = PricesInputValidator.ValidateSymbol(symbol);
            var validatedPeriod = PricesInputValidator.ValidatePeriod(period);
            var validatedView = PricesInputValidator.ValidateView(view);

            var query = new GetPriceSummaryQuery
            {
                QueryId = Guid.NewGuid().ToString("N"),
                Symbol = validatedSymbol,
                Period = validatedPeriod,
                IncludeCandles = validatedView == ToolView.Full
            };

            var result = await pricesService.GetSummaryAsync(query, cancellationToken);

            logger.LogInformation(
                "Price summary completed for {Symbol} period={Period} in {ElapsedMs}ms",
                validatedSymbol, validatedPeriod, stopwatch.ElapsedMilliseconds);

            return EnvelopeFactory.FromResult(
                result,
                validatedView,
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

    private static string BuildExplanation(Result<GetPriceSummaryResponse> result, string symbol)
    {
        if (!result.IsSuccess || result.Data is null)
        {
            return $"No price data for '{symbol}'.";
        }

        return string.Create(
            CultureInfo.InvariantCulture,
            $"Price summary for '{symbol}' over {result.Data.Period} ({result.Data.CandleCount} candles).");
    }
}
