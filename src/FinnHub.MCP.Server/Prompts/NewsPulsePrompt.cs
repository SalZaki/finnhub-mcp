// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using FinnHub.MCP.Server.Common;

namespace FinnHub.MCP.Server.Prompts;

/// <summary>
/// MCP prompt exposing <c>/news-pulse {symbol}</c> as a Claude Desktop slash command — a
/// deterministic, templated news-sentiment workflow over the existing tools.
/// </summary>
/// <remarks>
/// The render is a pure string template: no server-side LLM calls and no non-deterministic content,
/// so the same <c>symbol</c> always produces byte-identical text. The template instructs the client
/// to call <c>get-news-pulse</c> and then frame a sentiment narrative comparing the current week
/// against the prior week.
/// </remarks>
[McpServerPromptType]
public sealed class NewsPulsePrompt
{
    /// <summary>
    /// Renders the news-pulse workflow prompt for <paramref name="symbol"/>.
    /// </summary>
    /// <param name="symbol">The ticker symbol to read the news pulse for.</param>
    /// <returns>The templated news-sentiment instructions as a single user prompt message.</returns>
    [McpServerPrompt(
        Name = Constants.Prompts.NewsPulse.Name,
        Title = Constants.Prompts.NewsPulse.Title)]
    [Description(Constants.Prompts.NewsPulse.Description)]
    public static string NewsPulse(
        [Description(Constants.Prompts.NewsPulse.Parameters.SymbolDescription)]
        string symbol)
    {
        var validated = PromptInputValidator.ValidateSymbol(symbol);

        return $"""
            You are reading the news pulse for the company behind the ticker "{validated}". Work
            through this workflow and summarise as you go — do not invent headlines or sentiment that
            the tool does not return.

            1. Pull the news pulse — call `get-news-pulse` for {validated} to get the latest
               headlines, the sentiment score, and the week-over-week change.
            2. Compare against last week — use the week-over-week delta from `get-news-pulse` to judge
               whether sentiment and news volume are rising, falling, or steady versus the prior week.

            Then write a short sentiment narrative for {validated}: what the headlines are saying,
            where sentiment sits now, and how that compares to last week (improving, deteriorating, or
            flat). If the sentiment score is unavailable (the upstream sentiment endpoint can be
            premium-gated), say so and base the read on the headlines alone instead of guessing a
            score.
            """;
    }
}
