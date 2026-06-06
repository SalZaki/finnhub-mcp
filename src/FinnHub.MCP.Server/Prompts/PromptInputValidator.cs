// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.RegularExpressions;

namespace FinnHub.MCP.Server.Prompts;

/// <summary>
/// Shared input validation for the MCP prompts (Claude Desktop slash commands).
/// </summary>
internal static partial class PromptInputValidator
{
    [GeneratedRegex(@"^[A-Za-z0-9.\-]{1,32}$", RegexOptions.Compiled)]
    private static partial Regex SymbolRegex();

    /// <summary>
    /// Validates and normalises a prompt's <c>symbol</c> argument to uppercase.
    /// </summary>
    /// <param name="symbol">The raw symbol argument.</param>
    /// <returns>The trimmed, uppercased symbol.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the symbol is missing, longer than 32 characters, or contains disallowed characters.
    /// </exception>
    public static string ValidateSymbol(string? symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
        {
            throw new ArgumentException("Symbol is required.", nameof(symbol));
        }

        symbol = symbol.Trim().ToUpperInvariant();

        if (!SymbolRegex().IsMatch(symbol))
        {
            throw new ArgumentException(
                "Symbol must be 1-32 characters: letters, digits, dots, or dashes.", nameof(symbol));
        }

        return symbol;
    }
}
