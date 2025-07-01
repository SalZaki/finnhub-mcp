// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
//  <summary>
//    // TODO Add summary
//  </summary>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.SSE.Models;

public sealed class SearchSymbolResult
{
    public int Count { get; init; }

    public List<StockSymbol> Result { get; init; } = [];
}
