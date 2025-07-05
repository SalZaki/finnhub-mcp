// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using FinnHub.MCP.Server.Application.Search.Features.SearchSymbol;

namespace FinnHub.MCP.Server.Application.Search.Clients;

/// <summary>
/// Defines the contract for search client implementations that provide financial symbol search functionality.
/// </summary>
/// <remarks>
/// This interface abstracts the search functionality to allow for different implementations
/// (e.g., FinnHub API client, mock client for testing, cached client). Implementations should
/// handle external API communication, error handling, and data transformation.
/// </remarks>
public interface ISearchClient
{
    /// <summary>
    /// Searches for financial symbols based on the provided query parameters.
    /// </summary>
    /// <param name="query">The search query containing the search criteria and parameters.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the asynchronous operation.</param>
    /// <returns>
    /// A task that represents the asynchronous search operation. The task result contains
    /// a <see cref="SearchSymbolResponse"/> with the search results or error information.
    /// </returns>
    /// <remarks>
    /// This method should handle all aspects of the search operation including:
    /// <list type="bullet">
    /// <item><description>Validation of the query parameters</description></item>
    /// <item><description>Communication with external APIs (e.g., FinnHub)</description></item>
    /// <item><description>Error handling and appropriate error type mapping</description></item>
    /// <item><description>Data transformation and response mapping</description></item>
    /// <item><description>Proper cancellation support</description></item>
    /// </list>
    /// </remarks>
    /// <example>
    /// <code>
    /// var query = new SearchSymbolQuery
    /// {
    ///     QueryId = "search-001",
    ///     Query = "AAPL",
    ///     Exchange = "NASDAQ",
    ///     Limit = 10
    /// };
    ///
    /// var response = await searchClient.SearchSymbolAsync(query, cancellationToken);
    ///
    /// if (response.IsSuccess)
    /// {
    ///     foreach (var symbol in response.Data.Symbols)
    ///     {
    ///         Console.WriteLine($"{symbol.Symbol} - {symbol.Description}");
    ///     }
    /// }
    /// else
    /// {
    ///     Console.WriteLine($"Search failed: {response.ErrorMessage}");
    /// }
    /// </code>
    /// </example>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="query"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the query parameters are invalid (handled by query validation).
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Thrown when the operation is cancelled via the <paramref name="cancellationToken"/>.
    /// </exception>
    Task<SearchSymbolResponse> SearchSymbolAsync(SearchSymbolQuery query, CancellationToken cancellationToken);
}
