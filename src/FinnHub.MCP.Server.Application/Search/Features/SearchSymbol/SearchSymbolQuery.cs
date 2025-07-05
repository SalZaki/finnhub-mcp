// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
//  <summary>
//    // TODO Add summary
//  </summary>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Search.Features.SearchSymbol;

public sealed class SearchSymbolQuery : BaseSearchQuery
{
    public string? Exchange { get; init; }

    public override void Validate()
    {
        base.Validate();

        if (string.IsNullOrWhiteSpace(this.Query))
        {
            throw new ArgumentException("Query cannot be null or empty.", nameof(this.Query));
        }

        switch (this.Query.Length)
        {
            case < 1:
                throw new ArgumentException("Query must be at least 1 character long.", nameof(this.Query));
            case > 500:
                throw new ArgumentException("Query must be at most 500 characters long.", nameof(this.Query));
        }
    }

    public static SearchSymbolQuery Create(string queryId, string query, int limit = 10)
    {
        return new SearchSymbolQuery { QueryId = queryId, Query = query, Limit = limit };
    }

    public static SearchSymbolQuery ForExchange(string queryId, string query, string exchange, int limit = 10)
    {
        return new SearchSymbolQuery { QueryId = queryId, Query = query, Exchange = exchange, Limit = limit };
    }

    public static SearchSymbolQuery ForType(string queryId, string query, int limit = 10)
    {
        return new SearchSymbolQuery { QueryId = queryId, Query = query, Limit = limit };
    }
}
