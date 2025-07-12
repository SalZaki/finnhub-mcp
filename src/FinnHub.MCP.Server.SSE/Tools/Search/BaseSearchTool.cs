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
    /// Retrieves and validates the result limit parameter from the provided argument dictionary.
    /// Returns a default value if the parameter is missing.
    /// </summary>
    /// <param name="args">Optional dictionary of input arguments containing the limit parameter.</param>
    /// <param name="paramName">The name of the parameter to extract. Defaults to "limit".</param>
    /// <returns>The validated limit value as an integer.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the value is less than 1 or greater than 100.
    /// </exception>
    /// <remarks>
    /// Default value: 10
    /// Minimum: 1
    /// Maximum: 100
    /// </remarks>
    /// <example>
    /// var limit = ValidateAndGetLimit(args);
    /// var customLimit = ValidateAndGetLimit(args, "max_count");
    /// </example>
    protected static int ValidateAndGetLimit(IReadOnlyDictionary<string, JsonElement>? args, string paramName = "limit")
    {
        return GetIntParameter(args, paramName, DefaultLimit, MinLimit, MaxLimit);
    }

    /// <summary>
    /// Retrieves and validates a string query parameter with customizable length constraints.
    /// </summary>
    /// <param name="args">Input arguments containing the query.</param>
    /// <param name="paramName">The name of the query parameter. Defaults to "query".</param>
    /// <param name="minLength">Minimum allowed length. Defaults to 1.</param>
    /// <param name="maxLength">Maximum allowed length. Defaults to 500.</param>
    /// <returns>The validated query string.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the query is missing, empty, or outside the specified length bounds.
    /// </exception>
    /// <remarks>
    /// Useful for validating user-provided search terms such as ticker symbols or company names.
    /// </remarks>
    /// <example>
    /// var query = ValidateAndGetQuery(args); // default: 1–500 characters
    /// var symbol = ValidateAndGetQuery(args, "symbol", 1, 50);
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
