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
using FinnHub.MCP.Server.Application.Recommendations.Features.GetRecommendations;
using FinnHub.MCP.Server.Application.Recommendations.Services;
using FinnHub.MCP.Server.Common;

namespace FinnHub.MCP.Server.Tools.Recommendations;

/// <summary>
/// MCP tool exposing the Finnhub <c>/stock/recommendation</c> endpoint as an analyst-consensus
/// snapshot with the change vs the prior period computed in-memory.
/// </summary>
[McpServerToolType]
public sealed class GetRecommendationsTool(
    IRecommendationsService recommendationsService,
    ILogger<GetRecommendationsTool> logger)
{
    /// <summary>
    /// Returns the latest analyst-consensus snapshot for <paramref name="symbol"/> plus the
    /// per-bucket delta against the prior monthly period (when Finnhub returns more than one).
    /// </summary>
    [McpServerTool(
        Name = Constants.Tools.Recommendations.Name,
        Title = Constants.Tools.Recommendations.Title,
        ReadOnly = true,
        Idempotent = true,
        Destructive = false,
        OpenWorld = true)]
    [Description(Constants.Tools.Recommendations.Description)]
    public async Task<ToolResponseEnvelope<GetRecommendationsResponse>> GetRecommendationsAsync(
        [Description(Constants.Tools.Recommendations.Parameters.SymbolDescription)]
        string symbol,
        [Description(Constants.Tools.Recommendations.Parameters.ViewDescription)]
        string? view = null,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        const string ToolName = Constants.Tools.Recommendations.Name;

        try
        {
            logger.LogTrace("Starting execution of '{Tool}'.", ToolName);

            var validatedSymbol = RecommendationsInputValidator.ValidateSymbol(symbol);
            var validatedView = RecommendationsInputValidator.ValidateView(view);

            var query = new GetRecommendationsQuery
            {
                QueryId = Guid.NewGuid().ToString("N"),
                Symbol = validatedSymbol
            };

            var result = await recommendationsService.GetRecommendationsAsync(query, cancellationToken);

            var projected = ProjectForView(result, validatedView);

            logger.LogInformation(
                "Recommendations completed for {Symbol} in {ElapsedMs}ms",
                validatedSymbol, stopwatch.ElapsedMilliseconds);

            return EnvelopeFactory.FromResult(
                projected,
                validatedView,
                nextActions: BuildNextActions(projected, validatedSymbol),
                explanation: BuildExplanation(projected, validatedSymbol));
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

    private static Result<GetRecommendationsResponse> ProjectForView(Result<GetRecommendationsResponse> source, ToolView view)
    {
        if (!source.IsSuccess || source.Data is null || view == ToolView.Full)
        {
            return source;
        }

        // summary / standard drop the per-period history — the headline snapshot + change is enough.
        if (source.Data.Snapshots is null)
        {
            return source;
        }

        return Result<GetRecommendationsResponse>.Success(new GetRecommendationsResponse
        {
            Symbol = source.Data.Symbol,
            Period = source.Data.Period,
            Consensus = source.Data.Consensus,
            StrongBuy = source.Data.StrongBuy,
            Buy = source.Data.Buy,
            Hold = source.Data.Hold,
            Sell = source.Data.Sell,
            StrongSell = source.Data.StrongSell,
            Total = source.Data.Total,
            ChangeVsPrev = source.Data.ChangeVsPrev,
            Snapshots = null
        });
    }

    private static IReadOnlyList<NextAction> BuildNextActions(Result<GetRecommendationsResponse> result, string symbol)
    {
        if (!result.IsSuccess || result.Data is null)
        {
            return [];
        }

        var args = new Dictionary<string, string>(StringComparer.Ordinal) { ["symbol"] = symbol };

        return
        [
            new NextAction("get-financials-snapshot", args, "see the KPI snapshot behind the analyst rating"),
            new NextAction("get-peers", args, "compare the rating against peers in the same industry")
        ];
    }

    private static string BuildExplanation(Result<GetRecommendationsResponse> result, string symbol)
    {
        if (!result.IsSuccess || result.Data is null)
        {
            return $"No analyst recommendations available for {symbol}.";
        }

        var data = result.Data;
        var shiftSuffix = data.ChangeVsPrev is { } change
            ? $" — {change.ConsensusShift} vs {change.PrevPeriod:yyyy-MM-dd}"
            : string.Empty;

        return string.Create(
            CultureInfo.InvariantCulture,
            $"{symbol} consensus '{data.Consensus}' ({data.Total} analyst(s) in {data.Period:yyyy-MM-dd}){shiftSuffix}.");
    }
}
