// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Models;
using FinnHub.MCP.Server.Application.Quotes.Features.GetQuote;

namespace FinnHub.MCP.Server.Application.Quotes.Services;

/// <summary>Application-level entry point for real-time quote lookups.</summary>
public interface IQuotesService
{
    /// <summary>Executes a quote lookup and returns a categorised result.</summary>
    Task<Result<GetQuoteResponse>> GetQuoteAsync(GetQuoteQuery query, CancellationToken cancellationToken = default);
}
