// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.RegularExpressions;

namespace FinnHub.MCP.Server.Tools.Search;

/// <summary>
/// Static validation helpers for symbol-search tool parameters.
/// </summary>
internal static partial class SearchInputValidator
{
    private const int DefaultLimit = 10;
    private const int MinLimit = 1;
    private const int MaxLimit = 100;

    [GeneratedRegex(@"^[a-zA-Z0-9\s\-_.]{1,500}$", RegexOptions.Compiled)]
    private static partial Regex QueryRegex();

    [GeneratedRegex(@"^[A-Z0-9\-_]{1,50}$", RegexOptions.Compiled)]
    private static partial Regex ExchangeRegex();

    public static string ValidateQuery(string? query, int minLength = 1, int maxLength = 500)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            throw new ArgumentException("Query cannot be empty or whitespace.", nameof(query));
        }

        query = query.Trim();

        if (query.Length < minLength)
        {
            throw new ArgumentException($"Query must be at least {minLength} characters long.", nameof(query));
        }

        if (query.Length > maxLength)
        {
            throw new ArgumentException($"Query must be at most {maxLength} characters long.", nameof(query));
        }

        if (!QueryRegex().IsMatch(query))
        {
            throw new ArgumentException(
                "Query contains invalid characters. Only letters, numbers, spaces, dashes (-), underscores (_) and periods (.) are allowed.",
                nameof(query));
        }

        return query;
    }

    public static string? ValidateExchange(string? exchange)
    {
        if (string.IsNullOrWhiteSpace(exchange))
        {
            return null;
        }

        exchange = exchange.Trim().ToUpperInvariant();

        if (!ExchangeRegex().IsMatch(exchange))
        {
            throw new ArgumentException(
                "Exchange must contain only A-Z, 0-9, dashes or underscores, max 50 chars.",
                nameof(exchange));
        }

        return exchange;
    }

    public static int ValidateLimit(int? limit)
    {
        var value = limit ?? DefaultLimit;

        if (value < MinLimit || value > MaxLimit)
        {
            throw new ArgumentOutOfRangeException(
                nameof(limit), value, $"Limit must be between {MinLimit} and {MaxLimit}.");
        }

        return value;
    }
}
