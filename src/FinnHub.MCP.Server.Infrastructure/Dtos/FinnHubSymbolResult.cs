// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Infrastructure.Dtos;

/// <summary>
/// Represents a single symbol result from the FinnHub API.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class FinnHubSymbolResult
{
    /// <summary>Gets the symbol ticker.</summary>
    [JsonPropertyName("symbol")]
    public string? Symbol { get; init; }

    /// <summary>Gets the symbol description.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>Gets the display symbol.</summary>
    [JsonPropertyName("displaySymbol")]
    public string? DisplaySymbol { get; init; }

    /// <summary>Gets the symbol type.</summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }
}
