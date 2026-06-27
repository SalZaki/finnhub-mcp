// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace FinnHub.MCP.Server.Application.Discovery;

/// <summary>
/// A single ranked search hit: the matched <see cref="ToolDescriptor"/> and its
/// BM25 relevance <see cref="Score"/> (higher is more relevant).
/// </summary>
public sealed record ToolMatch(ToolDescriptor Tool, double Score);
