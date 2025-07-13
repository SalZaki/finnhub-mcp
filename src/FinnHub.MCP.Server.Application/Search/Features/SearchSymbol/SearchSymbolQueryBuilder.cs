// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Search.Features.SearchSymbol;

/// <summary>
/// Provides a fluent interface for building <see cref="SearchSymbolQuery"/> instances.
/// </summary>
/// <remarks>
/// This builder class implements the Builder pattern to allow for fluent construction of
/// <see cref="SearchSymbolQuery"/> objects. It provides a convenient way to set query parameters
/// and ensures proper validation before creating the final query object.
/// </remarks>
/// <example>
/// <code>
/// var query = new SearchSymbolQueryBuilder()
///     .WithQuery("AAPL")
///     .WithExchange("NASDAQ")
///     .WithLimit(20)
///     .Build();
/// </code>
/// </example>
public sealed class SearchSymbolQueryBuilder
{
    private string? _query;
    private string? _exchange;
    private int _limit = 10;

    /// <summary>
    /// Sets the search query string for symbol matching.
    /// </summary>
    /// <param name="query">The search query string to match against symbol names or codes.</param>
    /// <returns>The current <see cref="SearchSymbolQueryBuilder"/> instance for method chaining.</returns>
    /// <remarks>
    /// This parameter is required and must be provided before calling <see cref="Build"/>.
    /// The query will be validated for length constraints when the final query is built.
    /// </remarks>
    /// <example>
    /// <code>
    /// builder.WithQuery("AAPL");
    /// builder.WithQuery("Apple Inc");
    /// </code>
    /// </example>
    public SearchSymbolQueryBuilder WithQuery(string query)
    {
        this._query = query;

        return this;
    }

    /// <summary>
    /// Sets the exchange code to filter search results by a specific exchange.
    /// </summary>
    /// <param name="exchange">The exchange code (e.g., "NASDAQ", "NYSE") to filter results.</param>
    /// <returns>The current <see cref="SearchSymbolQueryBuilder"/> instance for method chaining.</returns>
    /// <remarks>
    /// This parameter is optional. If not set, the search will include symbols from all exchanges.
    /// </remarks>
    /// <example>
    /// <code>
    /// builder.WithExchange("NASDAQ");
    /// builder.WithExchange("NYSE");
    /// </code>
    /// </example>
    public SearchSymbolQueryBuilder WithExchange(string? exchange)
    {
        this._exchange = exchange;

        return this;
    }

    /// <summary>
    /// Sets the maximum number of results to return from the search.
    /// </summary>
    /// <param name="limit">The maximum number of search results to return.</param>
    /// <returns>The current <see cref="SearchSymbolQueryBuilder"/> instance for method chaining.</returns>
    /// <remarks>
    /// This parameter is optional and defaults to 10 if not specified.
    /// The limit should be a positive integer.
    /// </remarks>
    /// <example>
    /// <code>
    /// builder.WithLimit(25);
    /// builder.WithLimit(50);
    /// </code>
    /// </example>
    public SearchSymbolQueryBuilder WithLimit(int limit)
    {
        this._limit = limit;

        return this;
    }

    /// <summary>
    /// Builds and returns a validated <see cref="SearchSymbolQuery"/> instance.
    /// </summary>
    /// <returns>A new <see cref="SearchSymbolQuery"/> instance with the configured parameters.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the query string has not been set or is null/empty.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the query parameters fail validation (e.g., query length constraints).
    /// </exception>
    /// <remarks>
    /// This method generates a unique query ID automatically using a GUID and validates
    /// all parameters before returning the final query object. The query ID is truncated
    /// to 10 characters for brevity.
    /// </remarks>
    /// <example>
    /// <code>
    /// try
    /// {
    ///     var query = new SearchSymbolQueryBuilder()
    ///         .WithQuery("AAPL")
    ///         .WithExchange("NASDAQ")
    ///         .Build();
    /// }
    /// catch (InvalidOperationException ex)
    /// {
    ///     // Handle missing required parameters
    /// }
    /// catch (ArgumentException ex)
    /// {
    ///     // Handle validation errors
    /// }
    /// </code>
    /// </example>
    public SearchSymbolQuery Build()
    {
        if (string.IsNullOrWhiteSpace(this._query))
        {
            throw new InvalidOperationException("Query is required");
        }

        var request = new SearchSymbolQuery
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
