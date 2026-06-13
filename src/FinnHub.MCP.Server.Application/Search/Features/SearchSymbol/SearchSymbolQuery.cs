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
/// Self-contained query following the canonical <c>{Feature}Query</c> shape used across the
/// application. It carries the common query fields directly (no shared base class) and validates
/// the length constraints specific to symbol search.
/// </remarks>
public sealed class SearchSymbolQuery
{
    /// <summary>
    /// Gets the unique identifier for this query, used for tracking, logging, and correlation.
    /// </summary>
    public required string QueryId { get; init; }

    /// <summary>
    /// Gets the search term or phrase to match against symbol names or codes.
    /// </summary>
    public required string Query { get; init; }

    /// <summary>
    /// Gets the maximum number of results to return. Defaults to 10; must be between 1 and 100 inclusive.
    /// </summary>
    public int Limit { get; init; } = 10;

    /// <summary>
    /// Gets the exchange code (e.g., "NASDAQ", "NYSE") to filter symbols by, or <c>null</c> to search all exchanges.
    /// </summary>
    public string? Exchange { get; init; }

    /// <summary>
    /// Validates the query parameters.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <see cref="Limit"/> is not between 1 and 100 inclusive.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <see cref="Query"/> is null, empty, whitespace, or exceeds 500 characters.
    /// </exception>
    public void Validate()
    {
        if (this.Limit is < 1 or > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(this.Limit), this.Limit, "Limit must be between 1 and 100.");
        }

        if (string.IsNullOrWhiteSpace(this.Query))
        {
            throw new ArgumentException("Query cannot be null or empty.", nameof(this.Query));
        }

        if (this.Query.Length > 500)
        {
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
    public static SearchSymbolQuery Create(string queryId, string query, int limit = 10) =>
        new() { QueryId = queryId, Query = query, Limit = limit };
}
