// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Collections.Frozen;
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

    /// <summary>
    /// Field allowlist for the <c>fields</c> sparse-projection parameter on
    /// <c>search-symbol</c>. Mirrors the documented response shape so unknown
    /// field names are rejected as a validation error.
    /// </summary>
    /// <remarks>
    /// P1 ships the validator only. The projection itself lands in P6 when
    /// every aggregation tool gains uniform <c>fields</c> handling
    /// (<c>SearchSymbolResponse</c> is a sealed class today, not a record,
    /// so <c>with</c>-based projection is not available here).
    /// </remarks>
    private static readonly FrozenSet<string> s_knownFields = FrozenSet.Create(
        StringComparer.Ordinal,
        "count",
        "result",
        "symbol",
        "display_symbol",
        "description",
        "type",
        "confidence_score",
        "is_exact_match");

    [GeneratedRegex(@"^[a-z_][a-z0-9_]{0,63}$", RegexOptions.Compiled)]
    private static partial Regex FieldNameRegex();

    public static IReadOnlyList<string>? ValidateFields(IReadOnlyList<string>? fields)
    {
        if (fields is null || fields.Count == 0)
        {
            return null;
        }

        foreach (var field in fields)
        {
            if (string.IsNullOrWhiteSpace(field) || !FieldNameRegex().IsMatch(field))
            {
                throw new ArgumentException(
                    $"Field name '{field}' is invalid. Field names must be snake-case (a-z, 0-9, underscore), max 64 chars.",
                    nameof(fields));
            }

            if (!s_knownFields.Contains(field))
            {
                throw new ArgumentException(
                    $"Unknown field '{field}'. Allowed: {string.Join(", ", s_knownFields)}.",
                    nameof(fields));
            }
        }

        return fields;
    }
}
