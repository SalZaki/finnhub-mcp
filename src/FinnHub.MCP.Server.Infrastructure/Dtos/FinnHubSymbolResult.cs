// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Infrastructure.Dtos;

/// <summary>
/// Represents a single symbol result from the FinnHub API.
/// </summary>
public sealed class FinnHubSymbolResult
{
    /// <summary>
    /// Gets or sets the symbol ticker.
    /// </summary>
    [JsonPropertyName("symbol")]
    public string? Symbol { get; set; }

    /// <summary>
    /// Gets or sets the symbol description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the display symbol.
    /// </summary>
    [JsonPropertyName("displaySymbol")]
    public string? DisplaySymbol { get; set; }

    /// <summary>
    /// Gets or sets the symbol type.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }
}
