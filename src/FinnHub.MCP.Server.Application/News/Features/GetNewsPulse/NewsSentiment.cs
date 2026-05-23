// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.News.Features.GetNewsPulse;

/// <summary>
/// Intermediate shape returned by the client for the <c>/news-sentiment</c> endpoint.
/// </summary>
/// <param name="CompanyNewsScore">Composite sentiment score.</param>
/// <param name="BullishPercent">Share of news scored as bullish (0..1).</param>
/// <param name="BearishPercent">Share of news scored as bearish (0..1).</param>
public sealed record NewsSentiment(
    double? CompanyNewsScore,
    double? BullishPercent,
    double? BearishPercent);
