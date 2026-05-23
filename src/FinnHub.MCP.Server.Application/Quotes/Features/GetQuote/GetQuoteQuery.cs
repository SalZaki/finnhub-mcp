// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Quotes.Features.GetQuote;

/// <summary>
/// Query parameters for the <c>get-quote</c> tool — a Finnhub <c>/quote</c> lookup
/// that returns the real-time price snapshot for a ticker.
/// </summary>
public sealed class GetQuoteQuery
{
    /// <summary>Per-invocation correlation id; passed through for logging only.</summary>
    public required string QueryId { get; init; }

    /// <summary>Uppercase ticker symbol the quote is requested for.</summary>
    public required string Symbol { get; init; }
}
