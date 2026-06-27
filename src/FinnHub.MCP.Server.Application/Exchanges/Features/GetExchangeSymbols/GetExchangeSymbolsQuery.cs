// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Exchanges.Features.GetExchangeSymbols;

/// <summary>
/// Query for the <c>get-exchange-symbols</c> feature — the symbols listed on a single exchange.
/// </summary>
public sealed class GetExchangeSymbolsQuery
{
    /// <summary>Per-invocation correlation id; passed through for logging only and excluded from the cache key.</summary>
    public required string QueryId { get; init; }

    /// <summary>Uppercase exchange code the symbol list is requested for, e.g. <c>"US"</c>.</summary>
    public required string Exchange { get; init; }
}
