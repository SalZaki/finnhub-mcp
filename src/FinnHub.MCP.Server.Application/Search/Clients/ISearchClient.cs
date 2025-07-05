// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
//  <summary>
//    // TODO Add summary
//  </summary>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Search.Features.SearchSymbol;

namespace FinnHub.MCP.Server.Application.Search.Clients;

public interface ISearchClient : IDisposable
{
    Task<SearchSymbolResponse> SearchSymbolAsync(SearchSymbolQuery query, CancellationToken cancellationToken);
}
