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
/// MCP prompt exposing <c>/compare-peers {symbol}</c> as a Claude Desktop slash command — a
/// deterministic, templated peer-comparison workflow over the existing tools.
/// </summary>
/// <remarks>
/// The render is a pure string template: no server-side LLM calls and no non-deterministic content,
/// so the same <c>symbol</c> always produces byte-identical text. The template instructs the client
/// to find the peer set via <c>get-peers</c> and then fan out per peer to
/// <c>get-financials-snapshot</c> to build a side-by-side comparison.
/// </remarks>
[McpServerPromptType]
public sealed class ComparePeersPrompt
{
    /// <summary>
    /// Renders the peer-comparison workflow prompt for <paramref name="symbol"/>.
    /// </summary>
    /// <param name="symbol">The ticker symbol to compare against its peers.</param>
    /// <returns>The templated comparison instructions as a single user prompt message.</returns>
    [McpServerPrompt(
        Name = Constants.Prompts.ComparePeers.Name,
        Title = Constants.Prompts.ComparePeers.Title)]
    [Description(Constants.Prompts.ComparePeers.Description)]
    public static string ComparePeers(
        [Description(Constants.Prompts.ComparePeers.Parameters.SymbolDescription)]
        string symbol)
    {
        var validated = PromptInputValidator.ValidateSymbol(symbol);

        return $"""
            You are comparing the company behind the ticker "{validated}" against its peers. Work
            through this workflow, calling each tool and summarising as you go — do not guess values
            that a tool can provide for you.

            1. Find the peer set — call `get-peers` for {validated} to get the comparable companies
               (use the default industry grouping unless a narrower set is clearly needed).
            2. Fan out across the peers — for {validated} and each peer returned by `get-peers`, call
               `get-financials-snapshot` to pull the valuation ratios and key metrics. Keep the set
               manageable (the top handful of peers) so the comparison stays focused.
            3. Build a side-by-side comparison of the key metrics (for example market cap, P/E, P/B,
               and margins) across {validated} and its peers.

            Then write a short read on where {validated} sits relative to the group — cheaper or more
            expensive, higher or lower growth and margins — and which peers are the closest
            comparables. Note any peer whose data was unavailable (for example a premium-gated
            endpoint) instead of guessing.
            """;
    }
}
