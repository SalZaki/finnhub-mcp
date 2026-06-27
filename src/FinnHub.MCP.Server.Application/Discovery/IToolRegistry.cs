// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Discovery;

/// <summary>
/// In-memory index of MCP tool descriptors that ranks tools by a natural-language intent.
/// Lets a client discover the right tool without loading every tool schema up front.
/// </summary>
/// <remarks>
/// v1 ranking is a pure-C# BM25 keyword index over name + title + description + examples —
/// no embeddings, no external dependency (per spec §3 P7). A future v2 can swap the
/// implementation behind this interface.
/// </remarks>
public interface IToolRegistry
{
    /// <summary>All registered tool descriptors, in registration order.</summary>
    IReadOnlyList<ToolDescriptor> Descriptors { get; }

    /// <summary>
    /// Ranks the registered tools against <paramref name="intent"/> and returns the
    /// best matches, most-relevant first. Tools that share no terms with the intent are omitted.
    /// </summary>
    /// <param name="intent">The natural-language intent (e.g. <c>"is apple stock up this week"</c>).</param>
    /// <param name="topN">Maximum number of matches to return.</param>
    /// <returns>Up to <paramref name="topN"/> matches ordered by descending relevance; empty when the intent matches nothing.</returns>
    IReadOnlyList<ToolMatch> Search(string intent, int topN = 5);
}
