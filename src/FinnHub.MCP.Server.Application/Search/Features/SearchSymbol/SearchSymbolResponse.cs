// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Application.Search.Features.SearchSymbol;

/// <summary>
/// Represents the response from a symbol search operation, containing a collection of matching stock symbols
/// and associated metadata about the search results. This class extends <see cref="BaseSearchResponse"/>
/// to provide symbol-specific response functionality.
/// </summary>
/// <remarks>
/// <para>
/// This response model is designed to be serialized to JSON for API responses and provides a structured
/// way to return symbol search results to clients. The class includes automatic calculation of result
/// counts and status indicators based on the symbol collection.
/// </para>
/// <para>
/// The response follows a consistent pattern for search operations while providing symbol-specific
/// data through the <see cref="Symbols"/> collection. Each symbol in the collection represents
/// a potential match for the search query with associated metadata such as confidence scores
/// and match indicators.
/// </para>
/// </remarks>
/// <example>
/// Example JSON serialization output:
/// <code>
/// {
///   "symbols": [
///     {
///       "symbol": "AAPL",
///       "display_symbol": "AAPL",
///       "description": "Apple Inc.",
///       "type": "Common Stock",
///       "confidence_score": 0.95,
///       "is_exact_match": true
///     }
///   ],
///   "total_count": 1,
///   "has_results": true
/// }
/// </code>
/// </example>
public sealed class SearchSymbolResponse : BaseSearchResponse
{
    /// <summary>
    /// Gets the collection of stock symbols that match the search criteria.
    /// Each symbol contains detailed information including the symbol identifier,
    /// display format, company description, and match quality indicators.
    /// </summary>
    /// <value>
    /// A read-only list of <see cref="StockSymbol"/> objects representing the search results.
    /// This collection is empty if no symbols match the search criteria.
    /// The collection is immutable after construction to ensure response consistency.
    /// </value>
    /// <remarks>
    /// <para>
    /// The symbols are typically ordered by relevance, with the most relevant matches
    /// appearing first in the collection. The ordering is determined by factors such as:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Exact matches vs. fuzzy matches</description></item>
    /// <item><description>Confidence scores from the search algorithm</description></item>
    /// <item><description>Symbol popularity and trading volume</description></item>
    /// <item><description>Exchange priority and market capitalization</description></item>
    /// </list>
    /// <para>
    /// Each symbol in the collection includes comprehensive metadata to help clients
    /// make informed decisions about which symbol to use for further operations.
    /// </para>
    /// </remarks>
    /// <example>
    /// Accessing symbol information:
    /// <code>
    /// foreach (var symbol in response.Symbols)
    /// {
    ///     Console.WriteLine($"Symbol: {symbol.Symbol}");
    ///     Console.WriteLine($"Company: {symbol.Description}");
    ///     Console.WriteLine($"Confidence: {symbol.ConfidenceScore:P}");
    ///     Console.WriteLine($"Exact Match: {symbol.IsExactMatch}");
    /// }
    /// </code>
    /// </example>
    [JsonPropertyName("symbols")]
    public IReadOnlyList<StockSymbol> Symbols { get; init; } = [];

    /// <summary>
    /// Gets the total number of symbols returned in the search results.
    /// This value represents the count of items in the <see cref="Symbols"/> collection
    /// and provides a quick way to determine the size of the result set.
    /// </summary>
    /// <value>
    /// An integer representing the total number of symbols in the search results.
    /// This value is automatically calculated based on the <see cref="Symbols"/> collection count
    /// and will be 0 if no symbols match the search criteria.
    /// </value>
    /// <remarks>
    /// <para>
    /// This property overrides the abstract <see cref="BaseSearchResponse.TotalCount"/> property
    /// to provide symbol-specific count calculation. The count is computed dynamically
    /// based on the current state of the <see cref="Symbols"/> collection.
    /// </para>
    /// <para>
    /// The total count is useful for:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Determining if any results were found</description></item>
    /// <item><description>Implementing pagination logic</description></item>
    /// <item><description>Displaying result statistics to users</description></item>
    /// <item><description>Logging and analytics purposes</description></item>
    /// </list>
    /// </remarks>
    [JsonPropertyName("total_count")]
    public override int TotalCount => this.Symbols.Count;

    /// <summary>
    /// Gets a value indicating whether the search operation returned any matching symbols.
    /// This property provides a convenient way to check if the search was successful
    /// without having to examine the <see cref="Symbols"/> collection directly.
    /// </summary>
    /// <value>
    /// <c>true</c> if one or more symbols were found matching the search criteria;
    /// <c>false</c> if no symbols were found or the search returned no results.
    /// </value>
    /// <remarks>
    /// <para>
    /// This property overrides the abstract <see cref="BaseSearchResponse.HasResults"/> property
    /// to provide symbol-specific result checking. The value is computed dynamically
    /// based on the <see cref="TotalCount"/> property.
    /// </para>
    /// <para>
    /// This property is particularly useful for:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Conditional logic in client applications</description></item>
    /// <item><description>User interface state management</description></item>
    /// <item><description>Error handling and user feedback</description></item>
    /// <item><description>Analytics and success rate tracking</description></item>
    /// </list>
    /// </remarks>
    /// <example>
    /// Using the HasResults property:
    /// <code>
    /// if (response.HasResults)
    /// {
    ///     // Display results to user
    ///     DisplaySymbols(response.Symbols);
    /// }
    /// else
    /// {
    ///     // Show "no results found" message
    ///     ShowNoResultsMessage();
    /// }
    /// </code>
    /// </example>
    [JsonPropertyName("has_results")]
    public override bool HasResults => this.TotalCount > 0;
}
