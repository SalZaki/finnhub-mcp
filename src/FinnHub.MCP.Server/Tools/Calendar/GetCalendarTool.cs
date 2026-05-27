// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using FinnHub.MCP.Server.Application.Calendar.Features.GetCalendar;
using FinnHub.MCP.Server.Application.Calendar.Services;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Common;

namespace FinnHub.MCP.Server.Tools.Calendar;

/// <summary>
/// MCP tool exposing the Finnhub <c>/calendar/*</c> endpoint family as a single
/// parameter-dispatched lookup. Ships earnings, IPO, and economic kinds.
/// </summary>
[McpServerToolType]
public sealed class GetCalendarTool(
    ICalendarService calendarService,
    ILogger<GetCalendarTool> logger)
{
    /// <summary>
    /// Dispatched calendar lookup. Returns the events for <paramref name="kind"/> within
    /// the supplied window, optionally filtered to a single ticker (earnings) or country (economic).
    /// </summary>
    [McpServerTool(
        Name = Constants.Tools.Calendar.Name,
        Title = Constants.Tools.Calendar.Title,
        ReadOnly = true,
        Idempotent = true,
        Destructive = false,
        OpenWorld = true)]
    [Description(Constants.Tools.Calendar.Description)]
    public async Task<ToolResponseEnvelope<GetCalendarResponse>> GetCalendarAsync(
        [Description(Constants.Tools.Calendar.Parameters.KindDescription)]
        string kind,
        [Description(Constants.Tools.Calendar.Parameters.SymbolDescription)]
        string? symbol = null,
        [Description(Constants.Tools.Calendar.Parameters.CountryDescription)]
        string? country = null,
        [Description(Constants.Tools.Calendar.Parameters.FromDescription)]
        string? from = null,
        [Description(Constants.Tools.Calendar.Parameters.ToDescription)]
        string? to = null,
        [Description(Constants.Tools.Calendar.Parameters.ViewDescription)]
        string? view = null,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        const string ToolName = Constants.Tools.Calendar.Name;

        try
        {
            logger.LogTrace("Starting execution of '{Tool}'.", ToolName);

            var validatedKind = CalendarInputValidator.ValidateKind(kind);
            var validatedSymbol = CalendarInputValidator.ValidateSymbolForKind(symbol, validatedKind);
            var validatedCountry = CalendarInputValidator.ValidateCountryForKind(country, validatedKind);
            var (validatedFrom, validatedTo) = CalendarInputValidator.ValidateWindow(
                from, to, DateOnly.FromDateTime(DateTime.UtcNow), validatedKind);
            var validatedView = CalendarInputValidator.ValidateView(view);

            var query = new GetCalendarQuery
            {
                QueryId = Guid.NewGuid().ToString("N"),
                Kind = validatedKind,
                Symbol = validatedSymbol,
                Country = validatedCountry,
                From = validatedFrom,
                To = validatedTo
            };

            var result = await calendarService.GetCalendarAsync(query, cancellationToken);

            var projected = ProjectForView(result, validatedView);

            logger.LogInformation(
                "Calendar completed for kind={Kind} symbol={Symbol} country={Country} ({From}..{To}) in {ElapsedMs}ms",
                validatedKind, validatedSymbol ?? "(none)", validatedCountry ?? "(none)", validatedFrom, validatedTo, stopwatch.ElapsedMilliseconds);

            return EnvelopeFactory.FromResult(
                projected,
                validatedView,
                nextActions: BuildNextActions(projected, validatedSymbol),
                explanation: BuildExplanation(projected, validatedKind, validatedFrom, validatedTo, validatedCountry));
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

    /// <summary>Cap on events returned in summary view.</summary>
    internal const int SummaryEventCap = 10;

    /// <summary>Cap on events returned in standard view.</summary>
    internal const int StandardEventCap = 25;

    private static Result<GetCalendarResponse> ProjectForView(Result<GetCalendarResponse> source, ToolView view)
    {
        if (!source.IsSuccess || source.Data is null)
        {
            return source;
        }

        var cap = view switch
        {
            ToolView.Summary => SummaryEventCap,
            ToolView.Standard => StandardEventCap,
            _ => int.MaxValue
        };

        if (source.Data.EarningsEvents is { } earnings && earnings.Count > cap)
        {
            var capped = earnings.Take(cap).ToList().AsReadOnly();
            return Result<GetCalendarResponse>.Success(new GetCalendarResponse
            {
                Kind = source.Data.Kind,
                From = source.Data.From,
                To = source.Data.To,
                Symbol = source.Data.Symbol,
                Country = source.Data.Country,
                TotalCount = capped.Count,
                EarningsEvents = capped
            });
        }

        if (source.Data.IpoEvents is { } ipos && ipos.Count > cap)
        {
            var capped = ipos.Take(cap).ToList().AsReadOnly();
            return Result<GetCalendarResponse>.Success(new GetCalendarResponse
            {
                Kind = source.Data.Kind,
                From = source.Data.From,
                To = source.Data.To,
                Symbol = source.Data.Symbol,
                Country = source.Data.Country,
                TotalCount = capped.Count,
                IpoEvents = capped
            });
        }

        if (source.Data.EconomicEvents is { } economic && economic.Count > cap)
        {
            var capped = economic.Take(cap).ToList().AsReadOnly();
            return Result<GetCalendarResponse>.Success(new GetCalendarResponse
            {
                Kind = source.Data.Kind,
                From = source.Data.From,
                To = source.Data.To,
                Symbol = source.Data.Symbol,
                Country = source.Data.Country,
                TotalCount = capped.Count,
                EconomicEvents = capped
            });
        }

        return source;
    }

    private static IReadOnlyList<NextAction> BuildNextActions(Result<GetCalendarResponse> result, string? symbol)
    {
        if (!result.IsSuccess || result.Data is null)
        {
            return [];
        }

        if (result.Data.IpoEvents is { } ipos)
        {
            // Pick the first IPO that has a tradable ticker so the LLM can pivot to
            // a follow-up tool — withdrawn / unpriced offerings have null symbols and
            // get-company-profile would reject them at validation time.
            var firstWithTicker = ipos.FirstOrDefault(e => !string.IsNullOrEmpty(e.Symbol));
            if (firstWithTicker is null)
            {
                return [];
            }

            var ipoArgs = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["symbol"] = firstWithTicker.Symbol!
            };
            return
            [
                new NextAction(
                    "get-company-profile",
                    ipoArgs,
                    $"pull the profile for the most recent IPO ({firstWithTicker.Symbol})")
            ];
        }

        // Economic events have no symbol scope — no obvious cross-link to other tools.
        if (result.Data.EconomicEvents is not null)
        {
            return [];
        }

        if (string.IsNullOrEmpty(symbol))
        {
            return [];
        }

        var args = new Dictionary<string, string>(StringComparer.Ordinal) { ["symbol"] = symbol };

        return
        [
            new NextAction("get-financials-snapshot", args, "compare upcoming estimates with the last reported KPIs"),
            new NextAction("get-news-pulse", args, "check the news pulse heading into the release date")
        ];
    }

    private static string BuildExplanation(
        Result<GetCalendarResponse> result,
        CalendarKind kind,
        DateOnly from,
        DateOnly to,
        string? country)
    {
        var label = kind switch
        {
            CalendarKind.Ipo => "IPO",
            CalendarKind.Economic => "economic",
            _ => "earnings"
        };

        var scope = kind == CalendarKind.Economic && country is not null
            ? $" for {country}"
            : string.Empty;

        if (!result.IsSuccess || result.Data is null)
        {
            return string.Create(
                CultureInfo.InvariantCulture,
                $"No {label} events{scope} in {from:yyyy-MM-dd}..{to:yyyy-MM-dd}.");
        }

        return string.Create(
            CultureInfo.InvariantCulture,
            $"Found {result.Data.TotalCount} {label} event(s){scope} in {from:yyyy-MM-dd}..{to:yyyy-MM-dd}.");
    }
}
