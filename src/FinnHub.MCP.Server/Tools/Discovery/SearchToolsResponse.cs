// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Tools.Discovery;

/// <summary>
/// Wire payload for the <c>search-tools</c> meta-tool: the echoed intent and the ranked matches.
/// </summary>
public sealed class SearchToolsResponse
{
    /// <summary>The natural-language intent that was searched.</summary>
    [JsonPropertyName("intent")]
    public required string Intent { get; init; }

    /// <summary>Matching tools, most-relevant first.</summary>
    [JsonPropertyName("matches")]
    public required IReadOnlyList<ToolMatchView> Matches { get; init; }

    /// <summary>Number of matches returned.</summary>
    [JsonPropertyName("total_matches")]
    public int TotalMatches => this.Matches.Count;
}

/// <summary>
/// A single ranked tool match projected for the wire. <see cref="Description"/> is populated
/// only in the <c>standard</c> and <c>full</c> views to keep the <c>summary</c> view token-light.
/// </summary>
public sealed class ToolMatchView
{
    /// <summary>The MCP tool name to invoke next.</summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>The human-readable title.</summary>
    [JsonPropertyName("title")]
    public required string Title { get; init; }

    /// <summary>BM25 relevance score (higher is more relevant), rounded for the wire.</summary>
    [JsonPropertyName("score")]
    public required double Score { get; init; }

    /// <summary>Coarse grouping for presentation.</summary>
    [JsonPropertyName("category")]
    public string? Category { get; init; }

    /// <summary>Whether the tool's upstream Finnhub endpoint may require a premium key.</summary>
    [JsonPropertyName("premium")]
    public bool Premium { get; init; }

    /// <summary>Full tool description; <c>null</c> in the summary view.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }
}
