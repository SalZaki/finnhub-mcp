// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using FinnHub.MCP.Server.Application.Insiders.Features.GetInsiderSignal;
using FinnHub.MCP.Server.Application.Insiders.Services;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Common;

namespace FinnHub.MCP.Server.Tools.Insiders;

/// <summary>
/// MCP tool exposing the Finnhub <c>/stock/insider-transactions</c> endpoint as an
/// aggregated buy/sell signal with the most active named insiders.
/// </summary>
[McpServerToolType]
public sealed class GetInsiderSignalTool(
    IInsidersService insidersService,
    ILogger<GetInsiderSignalTool> logger)
{
    /// <summary>
    /// Returns the aggregated insider signal for <paramref name="symbol"/> over the
    /// requested window — net buy/sell volume, notable names, total count, and the
    /// latest transaction. <c>view='full'</c> additionally returns the full transaction array.
    /// </summary>
    [McpServerTool(
        Name = Constants.Tools.InsiderSignal.Name,
        Title = Constants.Tools.InsiderSignal.Title,
        ReadOnly = true,
        Idempotent = true,
        Destructive = false,
        OpenWorld = true)]
    [Description(Constants.Tools.InsiderSignal.Description)]
    public async Task<ToolResponseEnvelope<GetInsiderSignalResponse>> GetInsiderSignalAsync(
        [Description(Constants.Tools.InsiderSignal.Parameters.SymbolDescription)]
        string symbol,
        [Description(Constants.Tools.InsiderSignal.Parameters.FromDescription)]
        string? from = null,
        [Description(Constants.Tools.InsiderSignal.Parameters.ToDescription)]
        string? to = null,
        [Description(Constants.Tools.InsiderSignal.Parameters.ViewDescription)]
        string? view = null,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        const string ToolName = Constants.Tools.InsiderSignal.Name;

        try
        {
            logger.LogTrace("Starting execution of '{Tool}'.", ToolName);

            var validatedSymbol = InsidersInputValidator.ValidateSymbol(symbol);
            var (validatedFrom, validatedTo) = InsidersInputValidator.ValidateWindow(
                from, to, DateOnly.FromDateTime(DateTime.UtcNow));
            var validatedView = InsidersInputValidator.ValidateView(view);

            var query = new GetInsiderSignalQuery
            {
                QueryId = Guid.NewGuid().ToString("N"),
                Symbol = validatedSymbol,
                From = validatedFrom,
                To = validatedTo
            };

            var result = await insidersService.GetInsiderSignalAsync(query, cancellationToken);

            var projected = ProjectForView(result, validatedView);

            logger.LogInformation(
                "Insider signal completed for {Symbol} ({From}..{To}) in {ElapsedMs}ms",
                validatedSymbol, validatedFrom, validatedTo, stopwatch.ElapsedMilliseconds);

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

    private static Result<GetInsiderSignalResponse> ProjectForView(Result<GetInsiderSignalResponse> source, ToolView view)
    {
        if (!source.IsSuccess || source.Data is null || view == ToolView.Full)
        {
            return source;
        }

        // summary / standard views drop the full transactions array — `latest` plus the
        // aggregated signal is enough to act on without dumping every row.
        if (source.Data.Transactions is null)
        {
            return source;
        }

        return Result<GetInsiderSignalResponse>.Success(new GetInsiderSignalResponse
        {
            Symbol = source.Data.Symbol,
            From = source.Data.From,
            To = source.Data.To,
            NetBuySell30d = source.Data.NetBuySell30d,
            NotableNames = source.Data.NotableNames,
            TotalCount = source.Data.TotalCount,
            Latest = source.Data.Latest,
            Transactions = null
        });
    }

    private static IReadOnlyList<NextAction> BuildNextActions(Result<GetInsiderSignalResponse> result, string symbol)
    {
        if (!result.IsSuccess || result.Data is null)
        {
            return [];
        }

        var args = new Dictionary<string, string>(StringComparer.Ordinal) { ["symbol"] = symbol };

        return
        [
            new NextAction("get-company-profile", args, "look up the company behind the insider activity"),
            new NextAction("get-quote", args, "check the current price alongside the latest insider action")
        ];
    }

    private static string BuildExplanation(Result<GetInsiderSignalResponse> result, string symbol)
    {
        if (!result.IsSuccess || result.Data is null)
        {
            return $"No insider transactions for {symbol} in the requested window.";
        }

        var data = result.Data;
        var direction = data.NetBuySell30d switch
        {
            > 0 => "net buyer",
            < 0 => "net seller",
            _ => "net neutral"
        };

        return string.Create(
            CultureInfo.InvariantCulture,
            $"{symbol} insiders {direction} ({data.NetBuySell30d:+#;-#;0} shares) across {data.TotalCount} transaction(s) in {data.From:yyyy-MM-dd}..{data.To:yyyy-MM-dd}.");
    }
}
