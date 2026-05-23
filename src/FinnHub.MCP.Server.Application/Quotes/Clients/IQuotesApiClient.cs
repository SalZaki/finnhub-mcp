// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Quotes.Features.GetQuote;

namespace FinnHub.MCP.Server.Application.Quotes.Clients;

/// <summary>
/// Infrastructure contract for the Finnhub <c>/quote</c> endpoint.
/// </summary>
public interface IQuotesApiClient
{
    /// <summary>Fetches the real-time quote snapshot for a symbol.</summary>
    Task<GetQuoteResponse> GetQuoteAsync(GetQuoteQuery query, CancellationToken cancellationToken);
}
