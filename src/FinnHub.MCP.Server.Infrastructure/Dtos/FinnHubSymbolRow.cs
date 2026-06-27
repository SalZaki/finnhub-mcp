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
/// Wire DTO for a single row of the Finnhub <c>/stock/symbol</c> response.
/// </summary>
/// <remarks>
/// The endpoint returns a JSON array of these. Field names are camelCase on the wire
/// (e.g. <c>displaySymbol</c>, <c>shareClassFIGI</c>), so each property pins its
/// <see cref="JsonPropertyNameAttribute"/> explicitly to override the context's global
/// snake_case naming policy.
/// </remarks>
[ExcludeFromCodeCoverage]
public sealed class FinnHubSymbolRow
{
    /// <summary>The ticker symbol.</summary>
    [JsonPropertyName("symbol")]
    public string? Symbol { get; init; }

    /// <summary>The display symbol shown by the exchange.</summary>
    [JsonPropertyName("displaySymbol")]
    public string? DisplaySymbol { get; init; }

    /// <summary>Human-readable security description.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>Security type, e.g. <c>"Common Stock"</c>.</summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>Trading currency.</summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; init; }

    /// <summary>Market Identifier Code (ISO 10383).</summary>
    [JsonPropertyName("mic")]
    public string? Mic { get; init; }

    /// <summary>FIGI identifier.</summary>
    [JsonPropertyName("figi")]
    public string? Figi { get; init; }

    /// <summary>ISIN identifier.</summary>
    [JsonPropertyName("isin")]
    public string? Isin { get; init; }

    /// <summary>Share-class FIGI identifier.</summary>
    [JsonPropertyName("shareClassFIGI")]
    public string? ShareClassFigi { get; init; }

    /// <summary>Alternate symbol, when provided.</summary>
    [JsonPropertyName("symbol2")]
    public string? Symbol2 { get; init; }

    /// <summary>Composite FIGI identifier.</summary>
    [JsonPropertyName("figiComposite")]
    public string? FigiComposite { get; init; }
}
