// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.RegularExpressions;

namespace FinnHub.MCP.Server.Tools.Search;

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
public abstract partial class BaseSearchTool : BaseTool
{
    /// <summary>
    /// Generates the compiled regular expression for validating search queries.
    /// Allows: letters, digits, spaces, dashes, underscores, and periods. Max length: 500.
    /// </summary>
    /// <returns>A compiled regex used to validate search query strings.</returns>
    [GeneratedRegex(@"^[a-zA-Z0-9\s\-_.]{1,500}$", RegexOptions.Compiled)]
    private static partial Regex QueryRegex();

    /// <summary>
    /// Generates the compiled regular expression for validating exchange values.
    /// Allows: uppercase letters, digits, dashes, underscores. Max length: 50.
    /// </summary>
    /// <returns>A compiled regex used to validate exchange strings.</returns>
    [GeneratedRegex(@"^[A-Z0-9\-_]{1,50}$")]
    private static partial Regex ExchangeRegex();

    /// <summary>
    /// Cached compiled regex for query string validation.
    /// </summary>
    private static readonly Regex s_querySanitizerRegex = QueryRegex();

    /// <summary>
    /// Cached compiled regex for exchange string validation.
    /// </summary>
    private static readonly Regex s_exchangeSanitizerRegex = ExchangeRegex();

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
    /// Retrieves, trims, and validates a string query parameter with customizable length constraints.
    /// Input must match a safe allowlist regex: letters, numbers, spaces, dashes, underscores, and periods.
    /// </summary>
    /// <param name="args">Input arguments containing the query.</param>
    /// <param name="paramName">The name of the query parameter. Defaults to "query".</param>
    /// <param name="minLength">Minimum allowed length. Defaults to 1.</param>
    /// <param name="maxLength">Maximum allowed length. Defaults to 500.</param>
    /// <returns>The validated query string.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the query is missing, empty, outside allowed length, or contains unsafe characters.
    /// </exception>
    protected static string ValidateAndGetQuery(
        IReadOnlyDictionary<string, JsonElement>? args,
        string paramName = "query",
        int minLength = 1,
        int maxLength = 500)
    {
        ValidateRequiredParameter(args, paramName);

        var query = GetStringParameter(args, paramName).Trim();

        if (string.IsNullOrWhiteSpace(query))
        {
            throw new ArgumentException("Query cannot be empty or whitespace.", paramName);
        }

        if (query.Length < minLength)
        {
            throw new ArgumentException($"Query must be at least {minLength} characters long.", paramName);
        }

        if (query.Length > maxLength)
        {
            throw new ArgumentException($"Query must be at most {maxLength} characters long.", paramName);
        }

        if (!s_querySanitizerRegex.IsMatch(query))
        {
            throw new ArgumentException("Query contains invalid characters. Only letters, numbers, spaces, dashes (-), underscores (_) and periods (.) are allowed.", paramName);
        }

        return query;
    }

    /// <summary>
    /// Validates and sanitizes the optional exchange parameter.
    /// Converts to uppercase and ensures the value is alphanumeric with dashes/underscores only.
    /// </summary>
    /// <param name="args">Input arguments dictionary.</param>
    /// <param name="paramName">The exchange parameter name (defaults to "exchange").</param>
    /// <returns>The sanitized exchange string or null if not provided.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if exchange value is provided and contains disallowed characters or exceeds max length.
    /// </exception>
    protected static string? ValidateAndGetExchange(
        IReadOnlyDictionary<string, JsonElement>? args,
        string paramName = "exchange")
    {
        var exchange = GetStringParameter(args, paramName);

        if (string.IsNullOrWhiteSpace(exchange))
        {
            return null;
        }

        exchange = exchange.Trim().ToUpperInvariant();

        if (!s_exchangeSanitizerRegex.IsMatch(exchange))
        {
            throw new ArgumentException("Exchange must contain only A-Z, 0-9, dashes or underscores, max 50 chars.", paramName);
        }

        return exchange;
    }
}
