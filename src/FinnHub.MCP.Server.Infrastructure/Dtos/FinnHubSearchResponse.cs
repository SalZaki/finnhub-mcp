// ---------------------------------------------------------------------------------------------------------------------
//  <copyright>
//    This file is part of FinnHub MCP Server and is licensed under the MIT License.
//    See the LICENSE file in the project root for full license information.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace FinnHub.MCP.Server.Infrastructure.Dtos;

/// <summary>
/// Represents the FinnHub API response structure for symbol search.
/// </summary>
public sealed class FinnHubSearchResponse
{
    /// <summary>
    /// Gets the search results from the FinnHub API. Nullable defensively — Finnhub has been
    /// observed to omit the array entirely for some responses (see the defensive-DTO pattern from PR #167).
    /// </summary>
    [JsonPropertyName("result")]
    public List<FinnHubSymbolResult>? Result { get; init; }

    /// <summary>
    /// Gets or sets the result count from the FinnHub API.
    /// </summary>
    [JsonPropertyName("count")]
    public int Count { get; init; }
}
