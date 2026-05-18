// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Application.Models;

/// <summary>
/// A server-suggested follow-up tool call attached to a response envelope.
/// </summary>
/// <param name="Tool">The MCP tool name the client should invoke next (snake-case).</param>
/// <param name="Args">Argument map for that tool call.</param>
/// <param name="Why">Short rationale describing why this follow-up is useful for the current context.</param>
/// <remarks>
/// Forward-compatible: <see cref="Tool"/> may name a tool that is not yet registered on the server.
/// Clients are expected to ignore unknown tool names.
/// </remarks>
[ExcludeFromCodeCoverage]
public sealed record NextAction(
    [property: JsonPropertyName("tool")] string Tool,
    [property: JsonPropertyName("args")] IReadOnlyDictionary<string, string> Args,
    [property: JsonPropertyName("why")] string Why);
