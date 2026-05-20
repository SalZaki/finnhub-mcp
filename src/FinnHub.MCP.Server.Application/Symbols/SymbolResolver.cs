// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.RegularExpressions;
using FinnHub.MCP.Server.Application.Caching;
using FinnHub.MCP.Server.Application.Exceptions;
using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Search.Clients;
using FinnHub.MCP.Server.Application.Search.Features.SearchSymbol;
using Microsoft.Extensions.Logging;

namespace FinnHub.MCP.Server.Application.Symbols;

/// <summary>
/// Default <see cref="ISymbolResolver"/>. Routes structurally complete inputs through
/// three syntactic short-circuits and only calls Finnhub when the input is ambiguous.
/// </summary>
/// <remarks>
/// <para>
/// Fast-path regexes intentionally exclude tickers with digits and class-share suffixes
/// (e.g. <c>BRK.A</c> matches the suffix regex and resolves to canonical <c>BRK</c> with
/// exchange <c>A</c>, which is technically wrong — the ambiguous path handles those inputs
/// correctly when the user supplies the full company name). Tightening the suffix regex to
/// a known-exchange whitelist is a future cleanup.
/// </para>
/// </remarks>
public sealed partial class SymbolResolver(
    ISearchApiClient searchApiClient,
    IFinnHubCache cache,
    ILogger<SymbolResolver> logger) : ISymbolResolver
{
    private const int MaxInputLength = 500;
    private const int MaxCandidates = 5;
    private const int UpstreamLimit = 10;

    [GeneratedRegex(@"^[A-Z]{1,8}$", RegexOptions.Compiled)]
    private static partial Regex CanonicalRegex();

    [GeneratedRegex(@"^[A-Z]{1,8}\.[A-Z]{1,4}$", RegexOptions.Compiled)]
    private static partial Regex SuffixRegex();

    [GeneratedRegex(@"^[A-Z]+:[A-Z]{1,8}$", RegexOptions.Compiled)]
    private static partial Regex ColonPrefixRegex();

    /// <inheritdoc />
    public async Task<Result<ResolvedSymbol>> ResolveAsync(
        string input,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);
        if (input.Length > MaxInputLength)
        {
            throw new ArgumentException(
                $"Input must be at most {MaxInputLength} characters.",
                nameof(input));
        }

        var trimmed = input.Trim();
        var normalised = trimmed.ToUpperInvariant();

        if (CanonicalRegex().IsMatch(normalised))
        {
            var resolved = new ResolvedSymbol(normalised, normalised, Exchange: null, Confidence: 1.0d, Candidates: []);
            this.LogResolution(input, "canonical", resolved);
            return new Result<ResolvedSymbol>().Success(resolved);
        }

        if (SuffixRegex().IsMatch(normalised))
        {
            var parts = normalised.Split('.');
            var resolved = new ResolvedSymbol(parts[0], normalised, Exchange: parts[1], Confidence: 1.0d, Candidates: []);
            this.LogResolution(input, "suffix", resolved);
            return new Result<ResolvedSymbol>().Success(resolved);
        }

        if (ColonPrefixRegex().IsMatch(normalised))
        {
            var parts = normalised.Split(':');
            var resolved = new ResolvedSymbol(parts[1], normalised, Exchange: parts[0], Confidence: 1.0d, Candidates: []);
            this.LogResolution(input, "colon", resolved);
            return new Result<ResolvedSymbol>().Success(resolved);
        }

        return await this.ResolveAmbiguousAsync(input, trimmed, cancellationToken);
    }

    private async Task<Result<ResolvedSymbol>> ResolveAmbiguousAsync(
        string originalInput,
        string trimmed,
        CancellationToken cancellationToken)
    {
        var logicalKey = $"symbol-resolve:{trimmed.ToLowerInvariant()}";

        try
        {
            var query = SearchSymbolQuery.Create(
                queryId: Guid.NewGuid().ToString("N"),
                query: trimmed,
                limit: UpstreamLimit);

            var upstream = await cache.GetOrCreateAsync(
                logicalKey,
                CacheTier.Profile,
                async ct => await searchApiClient.SearchSymbolAsync(query, ct),
                cancellationToken);

            if (upstream.Symbols.Count == 0)
            {
                logger.LogInformation(
                    "resolved input='{Input}' path=ambiguous canonical=- confidence=0.00 candidates=0",
                    originalInput);
                return new Result<ResolvedSymbol>().Failure(
                    $"No symbol match for input '{originalInput}'.",
                    ResultErrorType.NotFound);
            }

            var ordered = upstream.Symbols
                .OrderByDescending(s => s.ConfidenceScore)
                .Take(MaxCandidates)
                .ToList();

            var candidates = ordered.Select(Project).ToList();
            var top = candidates[0] with { Candidates = candidates };

            this.LogResolution(originalInput, "ambiguous", top);
            return new Result<ResolvedSymbol>().Success(top);
        }
        catch (ApiClientHttpException ex)
        {
            logger.LogError(ex, "HTTP error resolving symbol for input '{Input}'.", originalInput);
            return new Result<ResolvedSymbol>().Failure(ex.Message, ResultErrorType.ServiceUnavailable);
        }
        catch (ApiClientTimeoutException ex)
        {
            logger.LogWarning(ex, "Timeout resolving symbol for input '{Input}'.", originalInput);
            return new Result<ResolvedSymbol>().Failure("Request timed out", ResultErrorType.Timeout);
        }
        catch (ApiClientDeserializationException ex)
        {
            logger.LogError(ex, "Deserialisation error resolving symbol for input '{Input}'.", originalInput);
            return new Result<ResolvedSymbol>().Failure("Invalid response from service", ResultErrorType.InvalidResponse);
        }
        catch (ApiClientException ex)
        {
            logger.LogError(ex, "Unexpected resolver failure for input '{Input}'.", originalInput);
            return new Result<ResolvedSymbol>().Failure("Symbol resolution failed unexpectedly");
        }
    }

    private static ResolvedSymbol Project(StockSymbol s) => new(
        Canonical: s.Symbol,
        Display: string.IsNullOrWhiteSpace(s.DisplaySymbol) ? s.Symbol : s.DisplaySymbol,
        Exchange: s.Exchange,
        Confidence: Math.Clamp(s.ConfidenceScore, 0.0d, 1.0d),
        Candidates: []);

    private void LogResolution(string input, string path, ResolvedSymbol resolved) =>
        logger.LogInformation(
            "resolved input='{Input}' path={Path} canonical={Canonical} confidence={Confidence:F2} candidates={Count}",
            input,
            path,
            resolved.Canonical,
            resolved.Confidence,
            resolved.Candidates.Count);
}
