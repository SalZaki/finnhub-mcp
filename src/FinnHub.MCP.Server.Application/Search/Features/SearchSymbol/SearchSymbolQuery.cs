// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Search.Features.SearchSymbol;

/// <summary>
/// Represents a query for searching financial symbols with optional exchange filtering.
/// </summary>
/// <remarks>
/// This class extends <see cref="BaseSearchQuery"/> to provide symbol-specific search functionality.
/// The query supports filtering by exchange and includes validation for query length constraints.
/// </remarks>
public sealed class SearchSymbolQuery : BaseSearchQuery
{
    /// <summary>
    /// Gets or initializes the exchange code to filter symbols by.
    /// </summary>
    /// <value>
    /// The exchange code (e.g., "NASDAQ", "NYSE") to filter results, or <c>null</c> to search all exchanges.
    /// </value>
    public string? Exchange { get; init; }

    /// <summary>
    /// Validates the query parameters to ensure they meet the required constraints.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Thrown when the query is null, empty, less than 1 character, or exceeds 500 characters.
    /// </exception>
    /// <remarks>
    /// This method calls the base validation and then validates the query length constraints
    /// specific to symbol search operations.
    /// </remarks>
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

    /// <summary>
    /// Creates a new instance of <see cref="SearchSymbolQuery"/> with the specified parameters.
    /// </summary>
    /// <param name="queryId">The unique identifier for the query.</param>
    /// <param name="query">The search query string to match against symbol names or codes.</param>
    /// <param name="limit">The maximum number of results to return. Defaults to 10.</param>
    /// <returns>A new <see cref="SearchSymbolQuery"/> instance.</returns>
    /// <example>
    /// <code>
    /// var query = SearchSymbolQuery.Create("query-123", "AAPL", 20);
    /// </code>
    /// </example>
    public static SearchSymbolQuery Create(string queryId, string query, int limit = 10)
    {
        return new SearchSymbolQuery { QueryId = queryId, Query = query, Limit = limit };
    }

    /// <summary>
    /// Creates a new instance of <see cref="SearchSymbolQuery"/> with exchange filtering.
    /// </summary>
    /// <param name="queryId">The unique identifier for the query.</param>
    /// <param name="query">The search query string to match against symbol names or codes.</param>
    /// <param name="exchange">The exchange code to filter results by (e.g., "NASDAQ", "NYSE").</param>
    /// <param name="limit">The maximum number of results to return. Defaults to 10.</param>
    /// <returns>A new <see cref="SearchSymbolQuery"/> instance with exchange filtering.</returns>
    /// <example>
    /// <code>
    /// var query = SearchSymbolQuery.ForExchange("query-123", "AAPL", "NASDAQ", 15);
    /// </code>
    /// </example>
    public static SearchSymbolQuery ForExchange(string queryId, string query, string exchange, int limit = 10)
    {
        return new SearchSymbolQuery { QueryId = queryId, Query = query, Exchange = exchange, Limit = limit };
    }

    /// <summary>
    /// Creates a new instance of <see cref="SearchSymbolQuery"/> for type-based searching.
    /// </summary>
    /// <param name="queryId">The unique identifier for the query.</param>
    /// <param name="query">The search query string to match against symbol names or codes.</param>
    /// <param name="limit">The maximum number of results to return. Defaults to 10.</param>
    /// <returns>A new <see cref="SearchSymbolQuery"/> instance.</returns>
    /// <remarks>
    /// This method is currently identical to <see cref="Create"/> but is provided for semantic clarity
    /// when the search intent is specifically type-based filtering.
    /// </remarks>
    /// <example>
    /// <code>
    /// var query = SearchSymbolQuery.ForType("query-123", "ETF", 25);
    /// </code>
    /// </example>
    public static SearchSymbolQuery ForType(string queryId, string query, int limit = 10)
    {
        return new SearchSymbolQuery { QueryId = queryId, Query = query, Limit = limit };
    }
}
