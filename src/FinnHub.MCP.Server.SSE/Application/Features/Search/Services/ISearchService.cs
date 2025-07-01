// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
//  <summary>
//    // TODO Add summary
//  </summary>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.SSE.Application.Features.Search.Queries;
using FinnHub.MCP.Server.SSE.Models;

namespace FinnHub.MCP.Server.SSE.Application.Features.Search.Services;

public interface ISearchService
{
    Task<Result<IReadOnlyList<StockSymbol>>> SearchSymbolsAsync(
        SymbolSearchQuery query,
        CancellationToken cancellationToken = default);
}
