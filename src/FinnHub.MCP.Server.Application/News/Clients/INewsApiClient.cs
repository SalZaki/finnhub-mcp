// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.News.Features.GetNewsPulse;

namespace FinnHub.MCP.Server.Application.News.Clients;

/// <summary>
/// Infrastructure contract for Finnhub's news endpoints. Two separate methods so the
/// service layer can fan them out (and gracefully degrade sentiment when premium-locked).
/// </summary>
public interface INewsApiClient
{
    /// <summary>Fetches company news articles for a symbol between the supplied UTC dates (inclusive).</summary>
    Task<IReadOnlyList<CompanyNewsArticle>> GetCompanyNewsAsync(
        string symbol,
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken);

    /// <summary>Fetches the composite news-sentiment snapshot for a symbol. Throws <c>ApiClientPremiumRequiredException</c> on 403.</summary>
    Task<NewsSentiment> GetSentimentAsync(string symbol, CancellationToken cancellationToken);
}
