// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Caching;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Recommendations.Clients;
using FinnHub.MCP.Server.Application.Recommendations.Features.GetRecommendations;
using FinnHub.MCP.Server.Application.Symbols;
using Microsoft.Extensions.Logging;

namespace FinnHub.MCP.Server.Application.Recommendations.Services;

/// <summary>
/// Default <see cref="IRecommendationsService"/> wrapping <see cref="IRecommendationsApiClient"/>
/// with hybrid caching (Profile tier — analyst consensus revises monthly) and exception-to-result
/// translation.
/// </summary>
public sealed class RecommendationsService(
    IRecommendationsApiClient apiClient,
    IFinnHubCache cache,
    ILogger<RecommendationsService> logger)
    : IRecommendationsService
{
    /// <summary>Threshold above which a score delta counts as a sentiment shift.</summary>
    internal const double ConsensusShiftThreshold = 0.1;

    /// <inheritdoc />
    public async Task<Result<GetRecommendationsResponse>> GetRecommendationsAsync(
        GetRecommendationsQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        try
        {
            var symbol = SymbolNormalizer.Normalize(query.Symbol);
            var cacheKey = SymbolCacheKey.For("recommendations", ("s", symbol));

            var snapshots = await cache.GetOrCreateAsync(
                cacheKey,
                CacheTier.Profile,
                async ct => await apiClient.GetRecommendationsAsync(symbol, ct),
                cancellationToken);

            // Finnhub returns most-recent first. Re-order defensively so [0] is always
            // the current period regardless of upstream ordering drift.
            var ordered = snapshots
                .OrderByDescending(s => s.Period)
                .ToList()
                .AsReadOnly();

            logger.LogInformation(
                "Recommendations for {Symbol}: {Count} period(s)",
                symbol, ordered.Count);

            if (ordered.Count == 0)
            {
                return Result<GetRecommendationsResponse>.Failure(
                    $"No analyst recommendations available for {symbol}.",
                    ResultErrorType.NotFound);
            }

            var current = ordered[0];
            var previous = ordered.Count > 1 ? ordered[1] : null;

            var response = new GetRecommendationsResponse
            {
                Symbol = symbol,
                Period = current.Period,
                Consensus = DeriveConsensus(current),
                StrongBuy = current.StrongBuy,
                Buy = current.Buy,
                Hold = current.Hold,
                Sell = current.Sell,
                StrongSell = current.StrongSell,
                Total = current.Total,
                ChangeVsPrev = previous is null ? null : BuildChange(current, previous),
                Snapshots = ordered
            };

            return Result<GetRecommendationsResponse>.Success(response);
        }
        catch (ApiClientPremiumRequiredException ex)
        {
            logger.LogWarning(ex, "Premium-only recommendations endpoint for {Symbol}", query.Symbol);
            return Result<GetRecommendationsResponse>.Failure(ex.Message, ResultErrorType.PremiumRequired);
        }
        catch (ApiClientHttpException ex)
        {
            logger.LogError(ex, "HTTP error fetching recommendations for {Symbol} (status: {Status})", query.Symbol, ex.StatusCode);
            return Result<GetRecommendationsResponse>.Failure(ex.Message, ResultErrorType.ServiceUnavailable);
        }
        catch (ApiClientTimeoutException ex)
        {
            logger.LogWarning(ex, "Recommendations request timed out for {Symbol}", query.Symbol);
            return Result<GetRecommendationsResponse>.Failure("Request timed out", ResultErrorType.Timeout);
        }
        catch (ApiClientDeserializationException ex)
        {
            logger.LogError(ex, "Failed to deserialize recommendations response for {Symbol}", query.Symbol);
            return Result<GetRecommendationsResponse>.Failure("Invalid response from service", ResultErrorType.InvalidResponse);
        }
        catch (ApiClientCancelledException)
        {
            throw;
        }
        catch (ApiClientException ex)
        {
            logger.LogError(ex, "Unexpected recommendations failure for {Symbol}", query.Symbol);
            return Result<GetRecommendationsResponse>.Failure("Recommendations lookup failed unexpectedly");
        }
    }

    /// <summary>
    /// Maps a snapshot to a 5-tier consensus label using the weighted mean score
    /// (Strong Buy = +2, Buy = +1, Hold = 0, Sell = −1, Strong Sell = −2).
    /// </summary>
    internal static string DeriveConsensus(RecommendationSnapshot s)
    {
        var score = WeightedScore(s);
        return score switch
        {
            >= 1.5 => "Strong Buy",
            >= 0.5 => "Buy",
            >= -0.5 => "Hold",
            >= -1.5 => "Sell",
            _ => "Strong Sell"
        };
    }

    private static RecommendationChange BuildChange(RecommendationSnapshot current, RecommendationSnapshot previous)
    {
        var currentScore = WeightedScore(current);
        var previousScore = WeightedScore(previous);
        var shift = currentScore - previousScore;

        var label = shift switch
        {
            > ConsensusShiftThreshold => "more bullish",
            < -ConsensusShiftThreshold => "more bearish",
            _ => "no change"
        };

        return new RecommendationChange
        {
            PrevPeriod = previous.Period,
            StrongBuyDelta = current.StrongBuy - previous.StrongBuy,
            BuyDelta = current.Buy - previous.Buy,
            HoldDelta = current.Hold - previous.Hold,
            SellDelta = current.Sell - previous.Sell,
            StrongSellDelta = current.StrongSell - previous.StrongSell,
            ConsensusShift = label
        };
    }

    private static double WeightedScore(RecommendationSnapshot s)
    {
        if (s.Total == 0)
        {
            return 0;
        }

        var weighted = (s.StrongBuy * 2) + s.Buy + (s.Sell * -1) + (s.StrongSell * -2);
        return (double)weighted / s.Total;
    }
}
