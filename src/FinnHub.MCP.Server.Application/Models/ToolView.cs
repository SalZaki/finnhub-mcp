// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Application.Models;

/// <summary>
/// Controls the verbosity of a tool response.
/// </summary>
/// <remarks>
/// Wire form is the lowercase string (<c>summary</c>, <c>standard</c>, <c>full</c>). Each level
/// has an associated token ceiling that the tool invocation middleware enforces; responses that
/// exceed their declared budget are downgraded to a summary-view failure envelope.
/// </remarks>
[JsonConverter(typeof(JsonStringEnumConverter<ToolView>))]
public enum ToolView
{
    /// <summary>
    /// Curated, aggregated payload. Hard ceiling: 500 tokens. Default for every tool.
    /// </summary>
    [JsonStringEnumMemberName("summary")]
    Summary,

    /// <summary>
    /// Curated but broader payload. Hard ceiling: 2000 tokens.
    /// </summary>
    [JsonStringEnumMemberName("standard")]
    Standard,

    /// <summary>
    /// Raw provider payload after DTO mapping. No hard ceiling; soft-budgeted.
    /// </summary>
    [JsonStringEnumMemberName("full")]
    Full
}
