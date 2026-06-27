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
using FinnHub.MCP.Server.Application.Peers.Features.GetPeers;
using FinnHub.MCP.Server.Application.Peers.Services;
using FinnHub.MCP.Server.Common;

namespace FinnHub.MCP.Server.Tools.Peers;

/// <summary>
/// MCP tool exposing the Finnhub <c>/stock/peers</c> endpoint.
/// </summary>
[McpServerToolType]
public sealed class GetPeersTool(
    IPeersService peersService,
    ILogger<GetPeersTool> logger)
{
    private const int SummaryPeerLimit = 10;
    private const int StandardPeerLimit = 25;

    /// <summary>
    /// Returns peer ticker symbols for a given symbol, optionally bucketed by industry, sub-industry, or sector.
    /// </summary>
    [McpServerTool(
        Name = Constants.Tools.Peers.Name,
        Title = Constants.Tools.Peers.Title,
        ReadOnly = true,
        Idempotent = true,
        Destructive = false,
        OpenWorld = true)]
    [Description(Constants.Tools.Peers.Description)]
    public async Task<ToolResponseEnvelope<GetPeersResponse>> GetPeersAsync(
        [Description(Constants.Tools.Peers.Parameters.SymbolDescription)]
        string symbol,
        [Description(Constants.Tools.Peers.Parameters.GroupingDescription)]
        string? grouping = null,
        [Description(Constants.Tools.Peers.Parameters.ViewDescription)]
        string? view = null,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        const string ToolName = Constants.Tools.Peers.Name;

        try
        {
            logger.LogTrace("Starting execution of '{Tool}'.", ToolName);

            var validatedSymbol = CommonInputValidators.ValidateSymbol(symbol);
            var validatedGrouping = PeersInputValidator.ValidateGrouping(grouping);
            var validatedView = CommonInputValidators.ValidateView(view);

            var query = new GetPeersQuery
            {
                QueryId = Guid.NewGuid().ToString("N"),
                Symbol = validatedSymbol,
                Grouping = validatedGrouping
            };

            var result = await peersService.GetPeersAsync(query, cancellationToken);

            // Project once, derive everything else from the projected result so
            // total_count, explanation, and next_actions can't disagree on the
            // peer count. Was a real bug: a 30-peer upstream result on a summary
            // call produced data.total_count=10 but explanation="Found 30 peer(s)".
            var projectedResult = ProjectByView(result, validatedView);

            logger.LogInformation(
                "Peers completed for {Symbol} in {ElapsedMs}ms: {Count} peers",
                validatedSymbol, stopwatch.ElapsedMilliseconds, projectedResult.Data?.TotalCount ?? 0);

            return EnvelopeFactory.FromResult(
                projectedResult,
                validatedView,
                nextActions: BuildNextActions(projectedResult, validatedSymbol),
                explanation: BuildExplanation(projectedResult, validatedSymbol));
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

    private static Result<GetPeersResponse> ProjectByView(Result<GetPeersResponse> result, ToolView view)
    {
        if (!result.IsSuccess || result.Data is null)
        {
            return result;
        }

        var cap = view switch
        {
            ToolView.Summary => SummaryPeerLimit,
            ToolView.Standard => StandardPeerLimit,
            _ => int.MaxValue
        };

        if (result.Data.Peers.Count <= cap)
        {
            return result;
        }

        var projected = new GetPeersResponse
        {
            Peers = result.Data.Peers.Take(cap).ToList().AsReadOnly(),
            Grouping = result.Data.Grouping
        };

        return Result<GetPeersResponse>.Success(projected);
    }

    // Emit rule: IsSuccess AND (singleton data OR non-empty collection) -- see NextAction.
    private static IReadOnlyList<NextAction> BuildNextActions(Result<GetPeersResponse> result, string symbol)
    {
        if (!result.IsSuccess || result.Data is null || result.Data.TotalCount == 0)
        {
            return [];
        }

        var args = new Dictionary<string, string>(StringComparer.Ordinal) { ["symbol"] = symbol };

        return
        [
            new NextAction("get-financials-snapshot", args, "compare this symbol's valuation to the peer set"),
            new NextAction("get-news-pulse", args, "sentiment and headlines for this symbol")
        ];
    }

    private static string BuildExplanation(Result<GetPeersResponse> result, string symbol)
    {
        if (!result.IsSuccess || result.Data is null)
        {
            return $"No peers found for '{symbol}'.";
        }

        return string.Create(
            CultureInfo.InvariantCulture,
            $"Found {result.Data.TotalCount} peer(s) for '{symbol}' ({result.Data.Grouping}).");
    }
}
