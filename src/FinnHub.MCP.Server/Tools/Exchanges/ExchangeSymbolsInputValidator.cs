// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.RegularExpressions;

namespace FinnHub.MCP.Server.Tools.Exchanges;

/// <summary>
/// Input validation for the <c>get-exchange-symbols</c> tool.
/// </summary>
internal static partial class ExchangeSymbolsInputValidator
{
    [GeneratedRegex(@"^[A-Z]{1,8}$", RegexOptions.Compiled)]
    private static partial Regex ExchangeCodeRegex();

    /// <summary>
    /// Validates and normalises an exchange code to uppercase.
    /// </summary>
    /// <param name="exchange">The raw exchange code.</param>
    /// <returns>The trimmed, uppercased exchange code.</returns>
    /// <exception cref="ArgumentException">Thrown when the code is missing or malformed.</exception>
    public static string ValidateExchange(string? exchange)
    {
        if (string.IsNullOrWhiteSpace(exchange))
        {
            throw new ArgumentException("Exchange code is required.", nameof(exchange));
        }

        exchange = exchange.Trim().ToUpperInvariant();

        if (!ExchangeCodeRegex().IsMatch(exchange))
        {
            throw new ArgumentException(
                "Exchange code must be 1-8 letters, e.g. 'US' or 'L'.", nameof(exchange));
        }

        return exchange;
    }
}
