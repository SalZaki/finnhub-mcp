// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
//  <summary>
//    // TODO Add summary
//  </summary>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.SSE.Application.Features.Search.Queries;

public sealed class SymbolSearchQueryBuilder
{
    private string? _query;
    private string? _exchange;
    private int _limit = 10;

    public SymbolSearchQueryBuilder WithQuery(string query)
    {
        this._query = query;

        return this;
    }

    public SymbolSearchQueryBuilder WithExchange(string exchange)
    {
        this._exchange = exchange;

        return this;
    }

    public SymbolSearchQueryBuilder WithLimit(int limit)
    {
        this._limit = limit;

        return this;
    }

    public SymbolSearchQuery Build()
    {
        if (string.IsNullOrWhiteSpace(this._query))
        {
            throw new InvalidOperationException("Query is required");
        }

        var request = new SymbolSearchQuery
        {
            QueryId = Guid.NewGuid().ToString("N")[..10],
            Query = this._query,
            Exchange = this._exchange,
            Limit = this._limit
        };

        request.Validate();

        return request;
    }
}
