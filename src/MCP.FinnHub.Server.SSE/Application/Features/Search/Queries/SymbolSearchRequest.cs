// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of the MCP FinnHub project and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
//  <summary>
//    Add summary.
//  </summary>
// ---------------------------------------------------------------------------------------------------------------------

namespace MCP.FinnHub.Server.SSE.Application.Features.Search.Queries;

public sealed class SymbolSearchQuery : BaseSearchQuery
{
    public required string Query { get; init; }

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

    public static SymbolSearchQuery Create(string query, int limit = 10)
    {
        return new SymbolSearchQuery {Query = query, Limit = limit};
    }

    public static SymbolSearchQuery ForExchange(string query, string exchange, int limit = 10)
    {
        return new SymbolSearchQuery {Query = query, Exchange = exchange, Limit = limit};
    }

    public static SymbolSearchQuery ForType(string query, int limit = 10)
    {
        return new SymbolSearchQuery {Query = query, Limit = limit};
    }
}
