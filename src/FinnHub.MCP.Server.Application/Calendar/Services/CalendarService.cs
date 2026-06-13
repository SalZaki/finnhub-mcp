// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using FinnHub.MCP.Server.Application.Caching;
using FinnHub.MCP.Server.Application.Calendar.Clients;
using FinnHub.MCP.Server.Application.Calendar.Features.GetCalendar;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Symbols;
using Microsoft.Extensions.Logging;

namespace FinnHub.MCP.Server.Application.Calendar.Services;

/// <summary>
/// Default <see cref="ICalendarService"/> wrapping <see cref="ICalendarApiClient"/> with
/// hybrid caching (News tier — calendars revise as analysts update estimates) and
/// exception-to-result translation.
/// </summary>
public sealed class CalendarService(
    ICalendarApiClient apiClient,
    IFinnHubCache cache,
    ILogger<CalendarService> logger)
    : ICalendarService
{
    /// <inheritdoc />
    public async Task<Result<GetCalendarResponse>> GetCalendarAsync(
        GetCalendarQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        try
        {
            return query.Kind switch
            {
                CalendarKind.Earnings => await this.GetEarningsAsync(query, cancellationToken),
                CalendarKind.Ipo => await this.GetIposAsync(query, cancellationToken),
                CalendarKind.Economic => await this.GetEconomicsAsync(query, cancellationToken),
                _ => Result<GetCalendarResponse>.Failure(
                    $"Calendar kind '{query.Kind}' is not yet supported.",
                    ResultErrorType.InvalidQuery)
            };
        }
        catch (ApiClientPremiumRequiredException ex)
        {
            logger.LogWarning(ex, "Calendar endpoint is premium-locked (kind={Kind})", query.Kind);
            return Result<GetCalendarResponse>.Failure(ex.Message, ResultErrorType.PremiumRequired);
        }
        catch (ApiClientHttpException ex)
        {
            logger.LogError(ex, "HTTP error fetching calendar (kind={Kind}, status={Status})", query.Kind, ex.StatusCode);
            return Result<GetCalendarResponse>.Failure(ex.Message, ResultErrorType.ServiceUnavailable);
        }
        catch (ApiClientTimeoutException ex)
        {
            logger.LogWarning(ex, "Calendar request timed out (kind={Kind})", query.Kind);
            return Result<GetCalendarResponse>.Failure("Request timed out", ResultErrorType.Timeout);
        }
        catch (ApiClientDeserializationException ex)
        {
            logger.LogError(ex, "Failed to deserialize calendar response (kind={Kind})", query.Kind);
            return Result<GetCalendarResponse>.Failure("Invalid response from service", ResultErrorType.InvalidResponse);
        }
        catch (ApiClientCancelledException)
        {
            throw;
        }
        catch (ApiClientException ex)
        {
            logger.LogError(ex, "Unexpected calendar failure (kind={Kind})", query.Kind);
            return Result<GetCalendarResponse>.Failure("Calendar lookup failed unexpectedly");
        }
    }

    private async Task<Result<GetCalendarResponse>> GetEarningsAsync(
        GetCalendarQuery query,
        CancellationToken cancellationToken)
    {
        var symbolKey = SymbolNormalizer.NormalizeOrAll(query.Symbol);
        var cacheKey = SymbolCacheKey.For(
            "calendar-earnings",
            ("s", symbolKey),
            ("f", query.From.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)),
            ("t", query.To.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));

        var events = await cache.GetOrCreateAsync(
            cacheKey,
            CacheTier.News,
            async ct => await apiClient.GetEarningsCalendarAsync(query.From, query.To, query.Symbol, ct),
            cancellationToken);

        var ordered = events
            .OrderBy(e => e.Date)
            .ThenBy(e => e.Symbol, StringComparer.Ordinal)
            .ToList()
            .AsReadOnly();

        logger.LogInformation(
            "Earnings calendar for {Symbol} ({From}..{To}): {Count} event(s)",
            symbolKey, query.From, query.To, ordered.Count);

        var response = new GetCalendarResponse
        {
            Kind = "earnings",
            From = query.From,
            To = query.To,
            Symbol = query.Symbol,
            TotalCount = ordered.Count,
            EarningsEvents = ordered
        };

        return ordered.Count == 0
            ? Result<GetCalendarResponse>.Failure(
                $"No earnings events found for the requested window ({query.From}..{query.To}).",
                ResultErrorType.NotFound)
            : Result<GetCalendarResponse>.Success(response);
    }

    private async Task<Result<GetCalendarResponse>> GetIposAsync(
        GetCalendarQuery query,
        CancellationToken cancellationToken)
    {
        var cacheKey = SymbolCacheKey.For(
            "calendar-ipo",
            ("f", query.From.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)),
            ("t", query.To.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));

        var events = await cache.GetOrCreateAsync(
            cacheKey,
            CacheTier.News,
            async ct => await apiClient.GetIpoCalendarAsync(query.From, query.To, ct),
            cancellationToken);

        // Most-recent-first ordering matches the upstream feed's natural shape and
        // matches what an analyst asking "what just listed?" expects to see at the top.
        var ordered = events
            .OrderByDescending(e => e.Date)
            .ThenBy(e => e.Name, StringComparer.Ordinal)
            .ToList()
            .AsReadOnly();

        logger.LogInformation(
            "IPO calendar ({From}..{To}): {Count} event(s)",
            query.From, query.To, ordered.Count);

        var response = new GetCalendarResponse
        {
            Kind = "ipo",
            From = query.From,
            To = query.To,
            Symbol = null,
            TotalCount = ordered.Count,
            IpoEvents = ordered
        };

        return ordered.Count == 0
            ? Result<GetCalendarResponse>.Failure(
                $"No IPO events found for the requested window ({query.From}..{query.To}).",
                ResultErrorType.NotFound)
            : Result<GetCalendarResponse>.Success(response);
    }

    private async Task<Result<GetCalendarResponse>> GetEconomicsAsync(
        GetCalendarQuery query,
        CancellationToken cancellationToken)
    {
        // Cache the unfiltered upstream payload — a single fetch serves every
        // country filter the caller might apply on top.
        var cacheKey = SymbolCacheKey.For(
            "calendar-economic",
            ("f", query.From.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)),
            ("t", query.To.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));

        var events = await cache.GetOrCreateAsync(
            cacheKey,
            CacheTier.News,
            async ct => await apiClient.GetEconomicCalendarAsync(query.From, query.To, ct),
            cancellationToken);

        IEnumerable<EconomicEvent> filtered = events;
        if (!string.IsNullOrWhiteSpace(query.Country))
        {
            filtered = events.Where(e => string.Equals(e.Country, query.Country, StringComparison.Ordinal));
        }

        var ordered = filtered
            .OrderBy(e => e.Time)
            .ThenBy(e => e.Country, StringComparer.Ordinal)
            .ThenBy(e => e.EventName, StringComparer.Ordinal)
            .ToList()
            .AsReadOnly();

        logger.LogInformation(
            "Economic calendar for {Country} ({From}..{To}): {Count} event(s)",
            query.Country ?? "(all)", query.From, query.To, ordered.Count);

        var response = new GetCalendarResponse
        {
            Kind = "economic",
            From = query.From,
            To = query.To,
            Symbol = null,
            Country = query.Country,
            TotalCount = ordered.Count,
            EconomicEvents = ordered
        };

        return ordered.Count == 0
            ? Result<GetCalendarResponse>.Failure(
                query.Country is null
                    ? $"No economic events found for the requested window ({query.From}..{query.To})."
                    : $"No economic events found for {query.Country} in the requested window ({query.From}..{query.To}).",
                ResultErrorType.NotFound)
            : Result<GetCalendarResponse>.Success(response);
    }
}
