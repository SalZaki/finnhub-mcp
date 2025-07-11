// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Application.Search;

/// <summary>
/// Provides a base implementation for search response objects with common properties and metadata.
/// </summary>
/// <remarks>
/// This abstract class serves as the foundation for all search response types in the application.
/// It provides common metadata such as query information, timing data, result counts, and source tracking.
/// Derived classes should implement the abstract properties to provide specific result information
/// and extend this base to add result-specific data.
/// </remarks>
/// <example>
/// <code>
/// public sealed class CustomSearchResponse : BaseSearchResponse
/// {
///     [JsonPropertyName("results")]
///     public List&lt;CustomResult&gt; Results { get; init; } = new();
///
///     public override bool HasResults => Results.Any();
///
///     public override int TotalCount => Results.Count;
/// }
/// </code>
/// </example>
public abstract class BaseSearchResponse
{
    /// <summary>
    /// Gets the original search query string that was executed.
    /// </summary>
    /// <value>
    /// The search query string that was used to generate this response.
    /// Defaults to an empty string if not specified.
    /// </value>
    /// <remarks>
    /// This property allows consumers to correlate responses with their original queries,
    /// which is especially useful for debugging and logging purposes.
    /// </remarks>
    [JsonPropertyName("query")]
    public string Query { get; init; } = string.Empty;

    [JsonPropertyName("query_id")]
    public string QueryId { get; init; } = string.Empty;

    /// <summary>
    /// Gets the duration of time taken to execute the search operation.
    /// </summary>
    /// <value>
    /// A <see cref="TimeSpan"/> representing the elapsed time from search initiation to completion.
    /// </value>
    /// <remarks>
    /// This property provides performance metrics for the search operation and can be used
    /// for monitoring, optimization, and user experience improvements.
    /// </remarks>
    [JsonPropertyName("search_duration")]
    public TimeSpan SearchDuration { get; init; }

    /// <summary>
    /// Gets the timestamp when the search operation was executed.
    /// </summary>
    /// <value>
    /// A <see cref="DateTime"/> indicating when the search was performed.
    /// </value>
    /// <remarks>
    /// This property provides temporal context for the search results and can be used
    /// for cache invalidation, audit trails, and data freshness validation.
    /// </remarks>
    [JsonPropertyName("search_timestamp")]
    public DateTime SearchTimestamp { get; init; }

    /// <summary>
    /// Gets a value indicating whether the search operation returned any results.
    /// </summary>
    /// <value>
    /// <c>true</c> if the search returned one or more results; otherwise, <c>false</c>.
    /// </value>
    /// <remarks>
    /// This abstract property must be implemented by derived classes to indicate
    /// whether the specific search operation yielded any results. The implementation
    /// should check the actual result collection or data structure.
    /// </remarks>
    [JsonPropertyName("has_results")]
    public abstract bool HasResults { get; }

    /// <summary>
    /// Gets the total number of results returned by the search operation.
    /// </summary>
    /// <value>
    /// The count of results included in this response.
    /// </value>
    /// <remarks>
    /// This abstract property must be implemented by derived classes to provide
    /// the actual count of results. This may be different from the requested limit
    /// if fewer results were available or if the search returned no matches.
    /// </remarks>
    [JsonPropertyName("total_count")]
    public abstract int TotalCount { get; }

    /// <summary>
    /// Gets the source identifier indicating where the search results originated.
    /// </summary>
    /// <value>
    /// A string identifying the data source (e.g., "FinnHub", "Cache", "MockData").
    /// Defaults to an empty string if not specified.
    /// </value>
    /// <remarks>
    /// This property helps track the origin of search results, which is useful for
    /// debugging, data quality assessment, and understanding the data flow through
    /// different layers of the application.
    /// </remarks>
    [JsonPropertyName("source")]
    public string Source { get; init; } = string.Empty;

    /// <summary>
    /// Gets a value indicating whether the search results were retrieved from cache.
    /// </summary>
    /// <value>
    /// <c>true</c> if the results were served from cache; <c>false</c> if they were
    /// retrieved from the original data source.
    /// </value>
    /// <remarks>
    /// This property provides transparency about data freshness and can be used
    /// for cache performance monitoring and troubleshooting. Cached results may
    /// be faster but potentially less current than fresh data.
    /// </remarks>
    [JsonPropertyName("is_from_cache")]
    public bool IsFromCache { get; init; }
}
