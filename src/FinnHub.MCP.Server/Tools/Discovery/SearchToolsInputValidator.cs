// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.RegularExpressions;

namespace FinnHub.MCP.Server.Tools.Discovery;

/// <summary>
/// Static validation helpers for the <c>search-tools</c> meta-tool parameters.
/// </summary>
internal static partial class SearchToolsInputValidator
{
    private const int MaxIntentLength = 200;

    // Natural-language intent: letters, digits, spaces and a small set of safe punctuation.
    [GeneratedRegex(@"^[a-zA-Z0-9\s\-_.,'?&/()]{1,200}$", RegexOptions.Compiled)]
    private static partial Regex IntentRegex();

    public static string ValidateIntent(string? intent)
    {
        if (string.IsNullOrWhiteSpace(intent))
        {
            throw new ArgumentException("Intent cannot be empty or whitespace.", nameof(intent));
        }

        intent = intent.Trim();

        if (intent.Length > MaxIntentLength)
        {
            throw new ArgumentException(
                $"Intent must be at most {MaxIntentLength} characters long.", nameof(intent));
        }

        if (!IntentRegex().IsMatch(intent))
        {
            throw new ArgumentException(
                "Intent contains invalid characters. Allowed: letters, numbers, spaces and the punctuation - _ . , ' ? & / ( ).",
                nameof(intent));
        }

        return intent;
    }
}
