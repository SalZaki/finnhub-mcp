// // ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
//  <summary>
//    // TODO Add summary
//  </summary>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Search.Features.SearchSymbol;

public sealed class SearchSymbolResponse : BaseSearchResponse
{
    public IReadOnlyList<StockSymbol> Symbols { get; init; } = [];

    public override int TotalCount => this.Symbols.Count;

    public override bool HasResults => this.TotalCount > 0;
}
