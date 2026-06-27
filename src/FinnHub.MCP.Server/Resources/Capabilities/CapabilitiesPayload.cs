// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Resources.Capabilities;

/// <summary>
/// Wire shape for <c>finnhub://resources/capabilities</c>: the full machine-readable catalog of
/// every registered MCP tool, so a client can enumerate the surface without issuing N
/// <c>search-tools</c> calls.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class CapabilitiesPayload
{
    /// <summary>Every registered tool, in registration order.</summary>
    [JsonPropertyName("tools")]
    public required IReadOnlyList<CapabilityEntry> Tools { get; init; }

    /// <summary>Number of tools in the catalog.</summary>
    [JsonPropertyName("total_count")]
    public int TotalCount => this.Tools.Count;
}

/// <summary>
/// A single tool's entry in the capabilities catalog.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class CapabilityEntry
{
    /// <summary>The MCP tool name (e.g. <c>get-price-summary</c>).</summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>The human-readable title.</summary>
    [JsonPropertyName("title")]
    public required string Title { get; init; }

    /// <summary>The full tool description.</summary>
    [JsonPropertyName("description")]
    public required string Description { get; init; }

    /// <summary>Coarse grouping (e.g. <c>Pricing</c>, <c>Fundamentals</c>).</summary>
    [JsonPropertyName("category")]
    public string? Category { get; init; }

    /// <summary>Natural-language example intents that map to this tool.</summary>
    [JsonPropertyName("examples")]
    public IReadOnlyList<string> Examples { get; init; } = [];

    /// <summary>Whether the tool's upstream Finnhub endpoint may require a premium key.</summary>
    [JsonPropertyName("premium")]
    public bool Premium { get; init; }
}
