// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.SSE.Tools.Search;

/// <summary>
/// Abstract base class for search-related MCP tools that provides common validation and parameter handling
/// functionality. This class extends <see cref="BaseTool"/> with search-specific operations including
/// limit validation and query string validation with configurable length constraints.
/// </summary>
/// <remarks>
/// <para>
/// This class is designed to be inherited by specific search tool implementations that need consistent
/// parameter validation behavior. It provides standardized handling for common search parameters
/// such as result limits and query strings, ensuring uniform validation across all search tools.
/// </para>
/// <para>
/// The class defines sensible defaults for search operations while allowing customization through
/// method parameters. All validation methods throw appropriate exceptions when validation fails,
/// which can be caught and converted to standardized error responses using the base class methods.
/// </para>
/// </remarks>
/// <example>
/// Example usage in a concrete search tool:
/// <code>
/// public class SymbolSearchTool : BaseSearchTool
/// {
///     public override async Task&lt;CallToolResponse&gt; CallAsync(IReadOnlyDictionary&lt;string, JsonElement&gt;? args)
///     {
///         try
///         {
///             var query = ValidateAndGetQuery(args, "query", 1, 100);
///             var limit = ValidateAndGetLimit(args, "limit");
///
///             // Perform search operation...
///             return CreateSuccessResponse(results);
///         }
///         catch (ArgumentException ex)
///         {
///             return CreateValidationErrorResponse(ex.ParamName, ex.Message);
///         }
///     }
/// }
/// </code>
/// </example>
public abstract class BaseSearchTool : BaseTool
{
    /// <summary>
    /// The default number of results to return when no limit is specified.
    /// This value provides a reasonable balance between completeness and performance.
    /// </summary>
    private const int DefaultLimit = 10;

    /// <summary>
    /// The maximum number of results that can be requested in a single search operation.
    /// This limit prevents excessive resource consumption and ensures reasonable response times.
    /// </summary>
    private const int MaxLimit = 100;

    /// <summary>
    /// The minimum number of results that can be requested in a single search operation.
    /// This ensures that at least one result is requested when a limit is specified.
    /// </summary>
    private const int MinLimit = 1;

    /// <summary>
    /// Validates and retrieves the limit parameter from the provided arguments with predefined constraints.
    /// This method ensures the limit value falls within acceptable bounds and provides a sensible default.
    /// </summary>
    /// <param name="args">The dictionary of arguments to validate against. May be null.</param>
    /// <param name="paramName">The name of the limit parameter to validate. Defaults to "limit".</param>
    /// <returns>
    /// A validated integer representing the maximum number of results to return.
    /// Returns <see cref="DefaultLimit"/> (10) if the parameter is not provided or cannot be parsed.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the limit value is less than <see cref="MinLimit"/> (1) or greater than <see cref="MaxLimit"/> (100).
    /// </exception>
    /// <remarks>
    /// <para>
    /// The limit validation enforces the following constraints:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Minimum value: 1 (at least one result must be requested)</description></item>
    /// <item><description>Maximum value: 100 (prevents excessive resource consumption)</description></item>
    /// <item><description>Default value: 10 (reasonable balance for most use cases)</description></item>
    /// </list>
    /// <para>
    /// This method is typically used in search tool implementations to ensure consistent
    /// limit parameter handling across all search operations.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Get limit with default parameter name
    /// var limit = ValidateAndGetLimit(args);
    ///
    /// // Get limit with custom parameter name
    /// var maxResults = ValidateAndGetLimit(args, "max_results");
    /// </code>
    /// </example>
    protected static int ValidateAndGetLimit(IReadOnlyDictionary<string, JsonElement>? args, string paramName = "limit")
    {
        return GetIntParameter(args, paramName, DefaultLimit, MinLimit, MaxLimit);
    }

    /// <summary>
    /// Validates and retrieves the query parameter from the provided arguments with configurable length constraints.
    /// This method ensures the query string meets the specified length requirements and is suitable for search operations.
    /// </summary>
    /// <param name="args">The dictionary of arguments to validate against. Cannot be null when query is required.</param>
    /// <param name="paramName">The name of the query parameter to validate. Defaults to "query".</param>
    /// <param name="minLength">The minimum allowed length for the query string. Defaults to 1.</param>
    /// <param name="maxLength">The maximum allowed length for the query string. Defaults to 500.</param>
    /// <returns>
    /// A validated string containing the search query that meets the specified length constraints.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown in the following scenarios:
    /// <list type="bullet">
    /// <item><description>When the query parameter is missing from the arguments</description></item>
    /// <item><description>When the query parameter is null, empty, or contains only whitespace</description></item>
    /// <item><description>When the query length is less than the specified minimum length</description></item>
    /// <item><description>When the query length exceeds the specified maximum length</description></item>
    /// </list>
    /// </exception>
    /// <remarks>
    /// <para>
    /// The query validation performs the following checks in order:
    /// </para>
    /// <list type="number">
    /// <item><description>Verifies the parameter exists and is not null/empty/whitespace</description></item>
    /// <item><description>Checks that the query length meets the minimum requirement</description></item>
    /// <item><description>Ensures the query length does not exceed the maximum limit</description></item>
    /// </list>
    /// <para>
    /// The default length constraints (1-500 characters) are suitable for most search scenarios,
    /// providing protection against both empty queries and excessively long queries that might
    /// impact performance or exceed API limits.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Basic query validation with defaults (1-500 characters)
    /// var query = ValidateAndGetQuery(args);
    ///
    /// // Custom validation for symbol search (1-50 characters)
    /// var symbol = ValidateAndGetQuery(args, "symbol", 1, 50);
    ///
    /// // Validation for company name search (3-200 characters)
    /// var companyName = ValidateAndGetQuery(args, "company_name", 3, 200);
    /// </code>
    /// </example>
    protected static string ValidateAndGetQuery(IReadOnlyDictionary<string, JsonElement>? args, string paramName = "query", int minLength = 1, int maxLength = 500)
    {
        ValidateRequiredParameter(args, paramName);

        var query = GetStringParameter(args, paramName);

        if (query.Length < minLength)
        {
            throw new ArgumentException($"Query must be at least {minLength} character(s) long.", paramName);
        }

        if (query.Length > maxLength)
        {
            throw new ArgumentException($"Query must be at most {maxLength} characters long.", paramName);
        }

        return query;
    }
}
