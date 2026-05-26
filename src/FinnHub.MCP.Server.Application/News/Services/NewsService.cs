// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Caching;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.News.Clients;
using FinnHub.MCP.Server.Application.News.Features.GetNewsPulse;
using Microsoft.Extensions.Logging;

namespace FinnHub.MCP.Server.Application.News.Services;

/// <summary>
/// Default <see cref="INewsService"/>. Orchestrates the three upstream calls
/// (news-sentiment + current week company-news + prior week company-news) and
/// gracefully degrades sentiment when the upstream endpoint is premium-locked.
/// </summary>
public sealed class NewsService(
    INewsApiClient apiClient,
    IFinnHubCache cache,
    ILogger<NewsService> logger)
    : INewsService
{
    private const int DefaultTopHeadlines = 5;

    /// <inheritdoc />
    public async Task<Result<GetNewsPulseResponse>> GetPulseAsync(
        GetNewsPulseQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var weekAgo = today.AddDays(-7);
        var twoWeeksAgo = today.AddDays(-14);

        try
        {
            // Sentiment is optional (premium-gated). Catch and degrade gracefully.
            var sentiment = await this.TryFetchSentimentAsync(query.Symbol, cancellationToken);

            var currentWeekKey = $"news-pulse:s={query.Symbol.ToUpperInvariant()}:w=current";
            var prevWeekKey = $"news-pulse:s={query.Symbol.ToUpperInvariant()}:w=prev";

            var currentWeekTask = cache.GetOrCreateAsync(
                currentWeekKey,
                CacheTier.News,
                async ct => await apiClient.GetCompanyNewsAsync(query.Symbol, weekAgo, today, ct),
                cancellationToken);

            var prevWeekTask = cache.GetOrCreateAsync(
                prevWeekKey,
                CacheTier.News,
                async ct => await apiClient.GetCompanyNewsAsync(query.Symbol, twoWeeksAgo, weekAgo, ct),
                cancellationToken);

            var currentWeek = await currentWeekTask;
            var prevWeek = await prevWeekTask;

            var topHeadlines = currentWeek
                .OrderByDescending(a => a.DatetimeUtc)
                .Take(query.IncludeAllHeadlines ? int.MaxValue : DefaultTopHeadlines)
                .Select(a => new NewsHeadline(a.Headline, a.Url, a.Source, a.DatetimeUtc))
                .ToList()
                .AsReadOnly();

            var response = new GetNewsPulseResponse
            {
                Symbol = query.Symbol,
                SentimentScore = sentiment?.CompanyNewsScore,
                BullishPercent = sentiment?.BullishPercent,
                BearishPercent = sentiment?.BearishPercent,
                SentimentSource = sentiment is null ? null : "finnhub",
                TopHeadlines = topHeadlines,
                Count = currentWeek.Count,
                DeltaVsPrevWeek = currentWeek.Count - prevWeek.Count
            };

            logger.LogInformation(
                "News pulse for {Symbol}: current={Cur} prev={Prev} sentiment={SentimentSource}",
                query.Symbol, response.Count, prevWeek.Count, response.SentimentSource ?? "none");

            return response.Count == 0
                ? new Result<GetNewsPulseResponse>().Failure(
                    $"No news found for {query.Symbol} in the past week.",
                    ResultErrorType.NotFound)
                : new Result<GetNewsPulseResponse>().Success(response);
        }
        catch (ApiClientPremiumRequiredException ex)
        {
            // Finnhub has gated /news-sentiment behind premium before; if they ever do
            // the same for /company-news, surface a typed PremiumRequired envelope so
            // the LLM sees premium=true and can hint at an upgrade. Matches the 6
            // sibling services that all carry this catch (FinancialsService.cs:48-52
            // and similar).
            logger.LogWarning(ex, "News endpoint is premium-locked for {Symbol}", query.Symbol);
            return new Result<GetNewsPulseResponse>().Failure(ex.Message, ResultErrorType.PremiumRequired);
        }
        catch (ApiClientHttpException ex)
        {
            logger.LogError(ex, "HTTP error fetching news for {Symbol} (status: {Status})", query.Symbol, ex.StatusCode);
            return new Result<GetNewsPulseResponse>().Failure(ex.Message, ResultErrorType.ServiceUnavailable);
        }
        catch (ApiClientTimeoutException ex)
        {
            logger.LogWarning(ex, "News request timed out for {Symbol}", query.Symbol);
            return new Result<GetNewsPulseResponse>().Failure("Request timed out", ResultErrorType.Timeout);
        }
        catch (ApiClientDeserializationException ex)
        {
            logger.LogError(ex, "Failed to deserialize news response for {Symbol}", query.Symbol);
            return new Result<GetNewsPulseResponse>().Failure("Invalid response from service", ResultErrorType.InvalidResponse);
        }
        catch (ApiClientCancelledException)
        {
            // Caller-initiated cancellation — surface as a typed cancel rather than
            // demoting to the catch-all "Unknown" failure that the base ApiClientException
            // arm below produces.
            throw;
        }
        catch (ApiClientException ex)
        {
            logger.LogError(ex, "Unexpected news failure for {Symbol}", query.Symbol);
            return new Result<GetNewsPulseResponse>().Failure("News pulse failed unexpectedly");
        }
    }

    private async Task<NewsSentiment?> TryFetchSentimentAsync(string symbol, CancellationToken cancellationToken)
    {
        try
        {
            return await cache.GetOrCreateAsync(
                $"news-sentiment:s={symbol.ToUpperInvariant()}",
                CacheTier.News,
                async ct => await apiClient.GetSentimentAsync(symbol, ct),
                cancellationToken);
        }
        catch (ApiClientPremiumRequiredException ex)
        {
            logger.LogInformation(ex, "Sentiment endpoint premium-locked for {Symbol}; degrading", symbol);
            return null;
        }
    }
}
