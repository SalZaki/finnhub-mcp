// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using FinnHub.MCP.Server.Application.Caching;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Insiders.Clients;
using FinnHub.MCP.Server.Application.Insiders.Features.GetInsiderSignal;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Symbols;
using Microsoft.Extensions.Logging;

namespace FinnHub.MCP.Server.Application.Insiders.Services;

/// <summary>
/// Default <see cref="IInsidersService"/> wrapping <see cref="IInsidersApiClient"/> with
/// hybrid caching (News tier — filings revise as supplemental Form 4s land) and
/// exception-to-result translation.
/// </summary>
public sealed class InsidersService(
    IInsidersApiClient apiClient,
    IFinnHubCache cache,
    ILogger<InsidersService> logger)
    : IInsidersService
{
    /// <summary>Maximum number of insider names returned in <see cref="GetInsiderSignalResponse.NotableNames"/>.</summary>
    internal const int NotableNamesCap = 5;

    /// <inheritdoc />
    public async Task<Result<GetInsiderSignalResponse>> GetInsiderSignalAsync(
        GetInsiderSignalQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        try
        {
            var symbol = SymbolNormalizer.Normalize(query.Symbol);
            var cacheKey = SymbolCacheKey.For(
                "insider-transactions",
                ("s", symbol),
                ("f", query.From.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)),
                ("t", query.To.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));

            var transactions = await cache.GetOrCreateAsync(
                cacheKey,
                CacheTier.News,
                async ct => await apiClient.GetInsiderTransactionsAsync(symbol, query.From, query.To, ct),
                cancellationToken);

            // Most-recent-first ordering puts the freshest activity at the top of the
            // full-view array AND lets `latest` be derived from index 0 without re-sorting.
            var ordered = transactions
                .OrderByDescending(t => t.TransactionDate)
                .ThenByDescending(t => t.FilingDate ?? DateOnly.MinValue)
                .ThenBy(t => t.Name, StringComparer.Ordinal)
                .ToList()
                .AsReadOnly();

            logger.LogInformation(
                "Insider signal for {Symbol} ({From}..{To}): {Count} transaction(s)",
                symbol, query.From, query.To, ordered.Count);

            if (ordered.Count == 0)
            {
                return Result<GetInsiderSignalResponse>.Failure(
                    $"No insider transactions for {symbol} in the requested window ({query.From}..{query.To}).",
                    ResultErrorType.NotFound);
            }

            var notableNames = ordered
                .GroupBy(t => t.Name, StringComparer.Ordinal)
                .Select(g => new { Name = g.Key, Volume = g.Sum(t => Math.Abs(t.Change)) })
                .OrderByDescending(x => x.Volume)
                .ThenBy(x => x.Name, StringComparer.Ordinal)
                .Take(NotableNamesCap)
                .Select(x => x.Name)
                .ToList()
                .AsReadOnly();

            var response = new GetInsiderSignalResponse
            {
                Symbol = symbol,
                From = query.From,
                To = query.To,
                NetBuySell30d = ordered.Sum(t => t.Change),
                NotableNames = notableNames,
                TotalCount = ordered.Count,
                Latest = ordered[0],
                Transactions = ordered
            };

            return Result<GetInsiderSignalResponse>.Success(response);
        }
        catch (ApiClientPremiumRequiredException ex)
        {
            logger.LogWarning(ex, "Premium-only insider endpoint for {Symbol}", query.Symbol);
            return Result<GetInsiderSignalResponse>.Failure(ex.Message, ResultErrorType.PremiumRequired);
        }
        catch (ApiClientHttpException ex)
        {
            logger.LogError(ex, "HTTP error fetching insider signal for {Symbol} (status: {Status})", query.Symbol, ex.StatusCode);
            return Result<GetInsiderSignalResponse>.Failure(ex.Message, ResultErrorType.ServiceUnavailable);
        }
        catch (ApiClientTimeoutException ex)
        {
            logger.LogWarning(ex, "Insider signal request timed out for {Symbol}", query.Symbol);
            return Result<GetInsiderSignalResponse>.Failure("Request timed out", ResultErrorType.Timeout);
        }
        catch (ApiClientDeserializationException ex)
        {
            logger.LogError(ex, "Failed to deserialize insider signal response for {Symbol}", query.Symbol);
            return Result<GetInsiderSignalResponse>.Failure("Invalid response from service", ResultErrorType.InvalidResponse);
        }
        catch (ApiClientCancelledException)
        {
            throw;
        }
        catch (ApiClientException ex)
        {
            logger.LogError(ex, "Unexpected insider signal failure for {Symbol}", query.Symbol);
            return Result<GetInsiderSignalResponse>.Failure("Insider signal lookup failed unexpectedly");
        }
    }
}
