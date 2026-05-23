// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.News.Features.GetNewsPulse;

/// <summary>
/// Intermediate shape returned by the client for an individual <c>/company-news</c> article.
/// </summary>
/// <param name="Headline">Article headline.</param>
/// <param name="Url">Public URL.</param>
/// <param name="Source">Publisher name.</param>
/// <param name="DatetimeUtc">UTC publication timestamp.</param>
public sealed record CompanyNewsArticle(
    string Headline,
    string Url,
    string Source,
    DateTimeOffset DatetimeUtc);
