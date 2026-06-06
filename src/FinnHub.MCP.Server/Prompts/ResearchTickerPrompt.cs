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
/// MCP prompt exposing <c>/research-ticker {symbol}</c> as a Claude Desktop slash command — a
/// deterministic, templated research workflow over the existing tools.
/// </summary>
/// <remarks>
/// The render is a pure string template: no server-side LLM calls and no non-deterministic content,
/// so the same <c>symbol</c> always produces byte-identical text. The template instructs the client
/// to resolve the symbol via <c>search-symbol</c> (the P3 symbol resolver) and then call
/// <c>get-price-summary</c>, <c>get-financials-snapshot</c>, and <c>get-news-pulse</c>.
/// </remarks>
[McpServerPromptType]
public sealed class ResearchTickerPrompt
{
    /// <summary>
    /// Renders the research-workflow prompt for <paramref name="symbol"/>.
    /// </summary>
    /// <param name="symbol">The ticker symbol to research.</param>
    /// <returns>The templated research instructions as a single user prompt message.</returns>
    [McpServerPrompt(
        Name = Constants.Prompts.ResearchTicker.Name,
        Title = Constants.Prompts.ResearchTicker.Title)]
    [Description(Constants.Prompts.ResearchTicker.Description)]
    public static string ResearchTicker(
        [Description(Constants.Prompts.ResearchTicker.Parameters.SymbolDescription)]
        string symbol)
    {
        var validated = PromptInputValidator.ValidateSymbol(symbol);

        return $"""
            You are researching the company behind the ticker "{validated}". Work through this
            workflow, calling each tool and summarising as you go — do not guess values that a tool
            can provide for you.

            1. Resolve the symbol. If "{validated}" is not a clean, unambiguous ticker, call
               `search-symbol` (the symbol resolver) to find the canonical ticker before continuing.
            2. Price action — call `get-price-summary` for {validated} to get the recent price trend,
               range, and return.
            3. Fundamentals — call `get-financials-snapshot` for {validated} to get valuation ratios
               and the key financial metrics.
            4. News and sentiment — call `get-news-pulse` for {validated} to get the latest headlines
               and the sentiment read.

            Then write a concise research brief covering what the company does, where the price is,
            how it is valued, and what the news is saying. Note any data that was unavailable (for
            example a premium-gated endpoint) instead of guessing.
            """;
    }
}
