// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Search;

/// <summary>
/// Provides a base implementation for search query objects with common properties and validation.
/// </summary>
/// <remarks>
/// This abstract class serves as the foundation for all search query types in the application.
/// It provides common properties like query ID, search query string, and result limit,
/// along with basic validation logic. Derived classes should extend this base to add
/// specific functionality and additional validation as needed.
/// </remarks>
/// <example>
/// <code>
/// public sealed class CustomSearchQuery : BaseSearchQuery
/// {
///     public string? Category { get; init; }
///
///     public override void Validate()
///     {
///         base.Validate(); // Call base validation first
///
///         if (string.IsNullOrWhiteSpace(this.Query))
///         {
///             throw new ArgumentException("Query cannot be empty.", nameof(this.Query));
///         }
///     }
/// }
/// </code>
/// </example>
public abstract class BaseSearchQuery
{
    /// <summary>
    /// Gets the unique identifier for this search query.
    /// </summary>
    /// <value>
    /// A string that uniquely identifies this search query instance.
    /// This property is required and must be set during object initialization.
    /// </value>
    /// <remarks>
    /// The query ID is used for tracking, logging, and correlation purposes.
    /// It should be unique across all search operations and is typically
    /// generated automatically by query builders or factory methods.
    /// </remarks>
    public required string QueryId { get; init; }

    /// <summary>
    /// Gets the search query string used for matching.
    /// </summary>
    /// <value>
    /// The search term or phrase to match against the target data.
    /// This property is required and must be set during object initialization.
    /// </value>
    /// <remarks>
    /// The query string is the primary search criteria and will be used to match
    /// against relevant fields in the target data source. Specific validation
    /// rules for the query content should be implemented in derived classes.
    /// </remarks>
    public required string Query { get; init; }

    /// <summary>
    /// Gets the maximum number of results to return from the search.
    /// </summary>
    /// <value>
    /// The maximum number of search results to return. Defaults to 10 if not specified.
    /// Must be between 1 and 100 inclusive.
    /// </value>
    /// <remarks>
    /// This property controls the size of the result set to prevent excessive
    /// resource usage and improve response times. The valid range is enforced
    /// by the <see cref="Validate"/> method.
    /// </remarks>
    public int Limit { get; init; } = 10;

    /// <summary>
    /// Validates the search query parameters to ensure they meet the required constraints.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <see cref="Limit"/> is not between 1 and 100 inclusive.
    /// </exception>
    /// <remarks>
    /// This method provides base validation for common properties. Derived classes
    /// should override this method to add specific validation logic while calling
    /// the base implementation to ensure all common validations are performed.
    /// </remarks>
    /// <example>
    /// <code>
    /// public override void Validate()
    /// {
    ///     base.Validate(); // Always call base validation first
    ///
    ///     // Add specific validation logic
    ///     if (string.IsNullOrWhiteSpace(this.Query))
    ///     {
    ///         throw new ArgumentException("Query cannot be empty.", nameof(this.Query));
    ///     }
    /// }
    /// </code>
    /// </example>
    public virtual void Validate()
    {
        if (this.Limit is < 1 or > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(this.Limit), this.Limit, "Limit must be between 1 and 100.");
        }
    }
}
