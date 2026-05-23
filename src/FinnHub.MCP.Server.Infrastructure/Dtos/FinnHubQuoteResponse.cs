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
/// Wire DTO for the Finnhub <c>/quote</c> endpoint. All numeric fields are nullable
/// because Finnhub returns <c>null</c> for change/percent-change on freshly listed
/// symbols (no previous close) and crypto/FX symbols outside their session, and
/// returns <c>0</c> or <c>null</c> for unknown symbols.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class FinnHubQuoteResponse
{
    /// <summary>Current price; <c>null</c> for unknown symbols.</summary>
    [JsonPropertyName("c")]
    public double? Current { get; init; }

    /// <summary>Absolute change vs previous close; <c>null</c> when no previous close exists.</summary>
    [JsonPropertyName("d")]
    public double? Change { get; init; }

    /// <summary>Percentage change vs previous close; <c>null</c> when no previous close exists.</summary>
    [JsonPropertyName("dp")]
    public double? PercentChange { get; init; }

    /// <summary>Session high.</summary>
    [JsonPropertyName("h")]
    public double? High { get; init; }

    /// <summary>Session low.</summary>
    [JsonPropertyName("l")]
    public double? Low { get; init; }

    /// <summary>Session open.</summary>
    [JsonPropertyName("o")]
    public double? Open { get; init; }

    /// <summary>Previous close.</summary>
    [JsonPropertyName("pc")]
    public double? PrevClose { get; init; }

    /// <summary>Unix-epoch (seconds) snapshot timestamp; <c>null</c> or <c>0</c> for unknown symbols.</summary>
    [JsonPropertyName("t")]
    public long? Timestamp { get; init; }
}
