// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Discovery;

/// <summary>
/// A curated, indexable descriptor for a single MCP tool. The <see cref="IToolRegistry"/>
/// ranks these by intent using a keyword index over
/// <see cref="Name"/> + <see cref="Title"/> + <see cref="Description"/> + <see cref="Examples"/>.
/// </summary>
/// <remarks>
/// Descriptors are assembled in the Server layer from the tool definitions in
/// <c>Constants.Tools</c> so the registry stays decoupled from the host (the Application
/// project must not reference Server). <see cref="Examples"/> are natural-language intents
/// curated to make ranking match how a user actually phrases a request.
/// </remarks>
public sealed record ToolDescriptor
{
    /// <summary>The MCP tool name (e.g. <c>get-price-summary</c>).</summary>
    public required string Name { get; init; }

    /// <summary>The human-readable title (e.g. <c>Get Price Summary</c>).</summary>
    public required string Title { get; init; }

    /// <summary>The full tool description indexed for keyword ranking.</summary>
    public required string Description { get; init; }

    /// <summary>Natural-language example intents a user would type to reach this tool.</summary>
    public IReadOnlyList<string> Examples { get; init; } = [];

    /// <summary>Coarse grouping for catalog presentation (e.g. <c>Pricing</c>, <c>Fundamentals</c>).</summary>
    public string? Category { get; init; }

    /// <summary>Whether the underlying Finnhub endpoint may require a premium key.</summary>
    public bool Premium { get; init; }

    /// <summary>
    /// Whether this tool is a candidate in <see cref="IToolRegistry.Search"/> results. Defaults to
    /// <c>true</c>; the <c>search-tools</c> meta-tool sets it <c>false</c> so it never returns itself,
    /// while still appearing in <see cref="IToolRegistry.Descriptors"/> for catalog and drift purposes.
    /// </summary>
    public bool Searchable { get; init; } = true;
}
