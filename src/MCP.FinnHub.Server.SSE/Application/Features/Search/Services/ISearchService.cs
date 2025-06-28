// --------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of the MCP FinnHub project and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
//  <summary>
//    Add summary.
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------

using MCP.FinnHub.Server.SSE.Application.Features.Search.Queries;
using MCP.FinnHub.Server.SSE.Models;

namespace MCP.FinnHub.Server.SSE.Application.Features.Search.Services;

public interface ISearchService
{
    Task<Result<IReadOnlyList<StockSymbol>>> SearchSymbolsAsync(
        SymbolSearchQuery query,
        CancellationToken cancellationToken = default);
}
